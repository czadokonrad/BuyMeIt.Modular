using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BuyMeIt.BuildingBlocks.EventBus.InMemory;
using BuyMeIt.BuildingBlocks.Infrastructure.EventBus;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Serilog;

namespace BuyMeIt.BuildingBlocks.EventBus.RabbitMQ
{
    public class RabbitMQEventBusClient : IEventsBus, IDisposable
    {
        private readonly IRabbitMqPersistentConnection _rabbitMqPersistentConnection;
        private readonly ILogger _logger;
        private readonly int _retryCount;
        
        private string _queueName;
        private IModel _consumerChannel;
        private readonly DirectExchangeRabbitMqManager _directExchangeRabbitMQManager;

        private const string ExchangeName = "BuyMeIt_Modular_Event_Bus";
        private const string DirectExchangeType = "direct";
        
        private const int DefaultRetryCount = 5;
        private const int PersistentDeliveryMode = 2;
        
        public RabbitMQEventBusClient(
            IRabbitMqPersistentConnection rabbitMqPersistentConnection, 
            ILogger logger,
            string queueName,
            int retryCount)
        {
            _directExchangeRabbitMQManager = new DirectExchangeRabbitMqManager();
            _rabbitMqPersistentConnection = rabbitMqPersistentConnection;
            _logger = logger;
            _queueName = queueName;
            _retryCount = retryCount == default ? DefaultRetryCount : retryCount;
            _consumerChannel = CreateConsumerChannel();
        }
        
        public Task Publish<TEvent>(TEvent @event) where TEvent : IntegrationEvent
        {
            RenewConnectionIfNeeded();

            var policy = CreateRabbitMqConnectRetryPolicy(@event);

            var eventName = @event.GetType().FullName;
            
            _logger.Information("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

            using (var channel = _rabbitMqPersistentConnection.CreateModel())
            {
                _logger.Information("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);
                
                _directExchangeRabbitMQManager.DeclareDurableDirectExchange(channel, ExchangeName);
                
                string message = JsonConvert.SerializeObject(@event);
                byte[] body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = PersistentDeliveryMode;
                    
                    _logger.Information("Publishing event to RabbitMQ: {EventId}", @event.Id);
                    
                    channel.BasicPublish(exchange: ExchangeName,
                                         routingKey: eventName,
                                         basicProperties: properties,
                                         body: body);
                });
            }

            return Task.CompletedTask;
        }

        public void Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler) where TEvent : IntegrationEvent
        {
            string handlerName = handler.GetType().Name;
            string eventName = typeof(TEvent).FullName;
            
            
            _logger.Information("Handler {HandlerName} is subscribing to event {EventName}",
                handlerName, eventName);

            RenewConnectionIfNeeded();

            using (var channel = _rabbitMqPersistentConnection.CreateModel())
            {
                channel.QueueBind(queue: _queueName ?? string.Empty,
                                  exchange: ExchangeName,
                                  routingKey: eventName,
                                  arguments: null);
            }
            
            InMemoryEventBus.Instance.Subscribe(handler);

            StartBasicConsume();
        }

        private void RenewConnectionIfNeeded()
        {
            if (!_rabbitMqPersistentConnection.IsConnected)
            {
                _rabbitMqPersistentConnection.TryConnect();
            }
        }

        private void StartBasicConsume()
        {
            _logger.Information("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                
                consumer.Received += OnBasicConsumeReceived;

                _consumerChannel.BasicConsume(queue: _queueName, 
                                              autoAck: false,
                                              consumer: consumer);
            }
            else
            {
                _logger.Error("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task OnBasicConsumeReceived(object sender, BasicDeliverEventArgs @event)
        {
            string eventName = @event.RoutingKey;
            string message = Encoding.UTF8.GetString(@event.Body.ToArray());
            
            try
            {
                dynamic integrationEvent = JsonConvert.DeserializeObject(message);
                await ProcessEventAsync(eventName, integrationEvent);
                
                _consumerChannel.BasicAck(@event.DeliveryTag, multiple: false);
            }
            catch (Exception e)
            {
                _directExchangeRabbitMQManager.DeadLetter(_consumerChannel, @event.DeliveryTag);
                _logger.Error(e, "----- ERROR Processing message \"{Message}\"", message);
            }
            
        }

        private async Task ProcessEventAsync(string eventName, dynamic @event)
        {
            _logger.Information("Processing RabbitMQ event: {EventName}", @event.GetType().Name);

            await InMemoryEventBus.Instance.Publish(eventName, @event);
        }

        private IModel CreateConsumerChannel()
        {
            if (!_rabbitMqPersistentConnection.IsConnected)
            {
                _rabbitMqPersistentConnection.TryConnect(); 
            }
            
            _logger.Information("Creating RabbitMQ consumer channel");

            var channel = _rabbitMqPersistentConnection.CreateModel();
            
            _directExchangeRabbitMQManager.DeclareDurableDirectExchange(channel, ExchangeName);

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            
            channel.CallbackException += OnConsumerChannelException;

            return channel;
        }

        private void OnConsumerChannelException(object sender, CallbackExceptionEventArgs e)
        {
            _logger.Warning(e.Exception, "Recreating RabbitMQ consumer channel");
            
            _consumerChannel.Dispose();

            _consumerChannel = CreateConsumerChannel();
            StartBasicConsume();
        }

        private RetryPolicy CreateRabbitMqConnectRetryPolicy<TEvent>(TEvent @event) where TEvent : IntegrationEvent =>
            Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(
                    _retryCount,
                    GetRetryAttemptExponentialBackoff,
                    (ex, time) =>
                    {
                        _logger.Warning(ex, "RabbitMQ Client could not publish event: {EventId} after {TimeOut}s ({ExceptionMessage})",
                            $"{@event.Id}-{@event.GetType().Name} {time.TotalSeconds:n1}", ex.Message);
                    });
        private TimeSpan GetRetryAttemptExponentialBackoff(int retryAttemptNumber) =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttemptNumber));
        
        public void Dispose()
        {
            _consumerChannel?.Dispose();
        }
    }
}