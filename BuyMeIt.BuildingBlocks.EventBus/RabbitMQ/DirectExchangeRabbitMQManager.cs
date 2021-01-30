using RabbitMQ.Client;

namespace BuyMeIt.BuildingBlocks.EventBus.RabbitMQ
{
    public class DirectExchangeRabbitMQManager : RabbitMQManager
    {
        /// <summary>
        /// Declares non-durable, non-autodelete direct exchange without setting any additional parameters explicitly
        /// </summary>
        /// <param name="channel">Channel on which exchange should be declared</param>
        /// <param name="exchangeName">Exchange name</param>
        public void DeclareDefaultDirectExchange(IModel channel, string exchangeName) =>
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
        
        protected override void CreateDefaultExchangeAndDefaultQueue(IModel channel, string queueName, string exchangeName,
            string routingKey)
        {
            DeclareDefaultDirectExchange(channel, exchangeName);
            DeclareDefaultQueue(channel, queueName);
            DefaultQueueBind(channel, queueName, exchangeName, routingKey);
        }
    }
}