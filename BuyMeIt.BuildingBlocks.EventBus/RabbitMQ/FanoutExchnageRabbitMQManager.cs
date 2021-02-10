using RabbitMQ.Client;

namespace BuyMeIt.BuildingBlocks.EventBus.RabbitMQ
{
    public class FanoutExchnageRabbitMQManager : RabbitMQManager
    {
        /// <summary>
        /// Declares non-durable, non-autodelete fanout exchange without setting any additional parameters explicitly
        /// </summary>
        /// <param name="channel">Channel on which exchange should be declared</param>
        /// <param name="exchangeName">Exchange name</param>
        public void DeclareDefaultFanoutExchange(IModel channel, string exchangeName) =>
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
        
        public void CreateFanoutExchangeWithQueues(IModel channel, string[] queueNames, string exchangeName,
            string routingKey)
        {
            DeclareDefaultFanoutExchange(channel, exchangeName);

            for (int i = 0; i < queueNames.Length; i++)
            {
                DeclareDefaultQueue(channel, queueNames[i]);
                DefaultQueueBind(channel, queueNames[i], exchangeName, routingKey);
            }
        }
    }
}