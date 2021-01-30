using RabbitMQ.Client;

namespace BuyMeIt.BuildingBlocks.EventBus.RabbitMQ
{
    public abstract class RabbitMQManager
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
        protected IModel CreateNewChannel(IConnection connection) =>
            connection.CreateModel();
        
        /// <summary>
        /// Declares non durable, non-autodelete, non-exclusive queue 
        /// </summary>
        /// <param name="channel">Channel on which queue should be declared</param>
        /// <param name="queueName">Queue name</param>
        protected void DeclareDefaultQueue(IModel channel, string queueName) =>
            channel.QueueDeclare(queueName, false, false, false);

        protected void DefaultQueueBind(IModel channel, string queueName, string exchangeName, string routingKey) =>
            channel.QueueBind(queueName, exchangeName, routingKey);

        protected abstract void CreateDefaultExchangeAndDefaultQueue(IModel channel, string queueName, string exchangeName,
            string routingKey);
        
        protected void DeleteQueue(IModel channel, string queueName) =>
            channel.QueueDelete(queueName, false, false);

        protected void DeleteQueueIfEmpty(IModel channel, string queueName) =>
            channel.QueueDelete(queueName, false, true);

    }
}