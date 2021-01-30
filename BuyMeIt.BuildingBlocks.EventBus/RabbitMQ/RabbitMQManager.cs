using RabbitMQ.Client;

namespace BuyMeIt.BuildingBlocks.EventBus.RabbitMQ
{
    public interface IRabbitMQManager
    {
        /// <summary>
        /// Returns new channel.
        /// <para></para>
        /// Channel is a virtual connection inside the "real" TCP connection over which we are issuing AMQP command.
        /// Every channel has a unique id assigned to it.
        /// All actions (publishing, subscribing or receiving) are all done over a channel
        /// <para></para>
        /// For performance reasons channels are using the same AMQP/TCP connection
        /// </summary>
        /// <param name="connection">AMQP connection to RabbitMQ</param>
        /// <returns></returns>
        IModel CreateNewChannel(IConnection connection);

        /// <summary>
        /// Declares non-durable, non-autodelete direct exchange without setting any additional parameters explicitly
        /// </summary>
        /// <param name="channel">Channel on which exchange should be declared</param>
        /// <param name="exchangeName">Exchange name</param>
        void DeclareDefaultDirectExchange(IModel channel, string exchangeName);

        /// <summary>
        /// Declares non durable, non-autodelete, non-exclusive queue 
        /// </summary>
        /// <param name="channel">Channel on which queue should be declared</param>
        /// <param name="queueName">Queue name</param>
        void DeclareDefaultQueue(IModel channel, string queueName);


        /// <summary>
        /// Binds queue to exchange using passed routingKey without extra arguments
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="queueName"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        void DefaultQueueBind(IModel channel, string queueName, string exchangeName, string routingKey);
    }

    public class RabbitMQManager : IRabbitMQManager
    {
        public IModel CreateNewChannel(IConnection connection) =>
            connection.CreateModel();

        public void DeclareDefaultDirectExchange(IModel channel, string exchangeName) =>
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

        public void DeclareDefaultQueue(IModel channel, string queueName) =>
            channel.QueueDeclare(queueName, false, false, false);

        public void DefaultQueueBind(IModel channel, string queueName, string exchangeName, string routingKey) =>
            channel.QueueBind(queueName, exchangeName, routingKey);

        public void CreateDirectExchangeWithNonDurableQueueAndBindThem(IModel channel, string queueName, string exchangeName,
            string routingKey)
        {
            DeclareDefaultDirectExchange(channel, exchangeName);
            DeclareDefaultQueue(channel, queueName);
            DefaultQueueBind(channel, queueName, exchangeName, routingKey);
        }


        public void DeleteQueue(IModel channel, string queueName) =>
            channel.QueueDelete(queueName, false, false);

        public void DeleteQueueIfEmpty(IModel channel, string queueName) =>
            channel.QueueDelete(queueName, false, true);

    }
}