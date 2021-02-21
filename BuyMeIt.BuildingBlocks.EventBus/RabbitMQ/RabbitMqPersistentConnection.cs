using System;
using System.IO;
using System.Net.Sockets;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Serilog;

namespace BuyMeIt.BuildingBlocks.EventBus.RabbitMQ
{
    public class RabbitMqPersistentConnection : IRabbitMqPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger _logger;
        
        private int _retryCount;
        private bool _disposed;
        private IConnection _connection;
        
        private const int DefaultRetryCount = 5;

        private object sync_root = new object();
        
        public RabbitMqPersistentConnection(IConnectionFactory connectionFactory, ILogger logger, int retryCount)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _retryCount = retryCount == default ? DefaultRetryCount : retryCount;
        }


        public bool IsConnected =>
            _connection != null && _connection.IsOpen && !_disposed;
        
        public bool TryConnect()
        {
            _logger.Information("Trying to connect to RabbitMQ using synchronous TryConnect");

            lock (sync_root)
            {
                if (IsConnected) return true;

                var policy = CreateRabbitMqConnectRetryPolicy();

                policy.Execute(() =>
                {
                    _connection = _connectionFactory.CreateConnection();
                });

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;
                    
                    _logger.Information("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);

                    return true;
                }
                else
                {
                    _logger.Fatal("FATAL ERROR: RabbitMQ connections could not be created and opened");

                    return false;
                }
            }
        }
        

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        private RetryPolicy CreateRabbitMqConnectRetryPolicy() =>
            Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(
                    _retryCount,
                    GetRetryAttemptExponentialBackoff,
                    (ex, time) =>
                    {
                        _logger.Warning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})",
                            $"{time.TotalSeconds:n1}", ex.Message);
                    });

        private TimeSpan GetRetryAttemptExponentialBackoff(int retryAttemptNumber) =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttemptNumber));
        
        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.Warning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.Warning("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }
        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if(_disposed) return;
            
            _logger.Warning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }
        
        
        public void Dispose()
        {
            if (_disposed) return;
            
            _disposed = true;

            try
            {
                _connection?.Dispose();
            }
            catch (IOException e)
            {
                _logger.Fatal(e, "Error in Dispose of RabbitMqPersistentConnection");
            }
        }
    }
}