using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

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
        public IModel CreateNewChannel(IConnection connection) =>
            connection.CreateModel();

        /// <summary>
        /// Declares non durable, non-autodelete, non-exclusive queue 
        /// </summary>
        /// <param name="channel">Channel on which queue should be declared</param>
        /// <param name="queueName">Queue name</param>
        public void DeclareDefaultQueue(IModel channel, string queueName) =>
            channel.QueueDeclare(queueName, false, false, false);

        /// <summary>
        /// Creates a queue which will be re-created after crash or restart of rabbit server
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="queueName"></param>
        public void DeclareDurableQueue(IModel channel, string queueName) =>
            channel.QueueDeclare(queueName, true, false, false);

        /// <summary>
        /// Declares queue
        /// </summary>
        /// <param name="channel">Channel on which queue should be declared</param>
        /// <param name="queueName">Queue name</param>
        /// <param name="autoDelete">If true queue is automatically removed when last consumer unsubsribes</param>
        /// <param name="exclusive">If true queue becomes private and can only be consumed by your app.
        /// This is useful when you need to limit queue to only one consumer.</param>
        public void DeclareQueue(IModel channel, string queueName, bool autoDelete, bool exclusive) =>
            channel.QueueDeclare(queueName, false, exclusive, autoDelete);


        public void DefaultQueueBind(IModel channel, string queueName, string exchangeName, string routingKey) =>
            channel.QueueBind(queueName, exchangeName, routingKey);

        public void DeleteQueue(IModel channel, string queueName) =>
            channel.QueueDelete(queueName, false, false);

        public void DeleteQueueIfEmpty(IModel channel, string queueName) =>
            channel.QueueDelete(queueName, false, true);

        /// <summary>
        /// Message that has to be persistent must be published to durable exchange and durable queue
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="queueName"></param>
        public void PublishPersistentMessage(IModel channel, string exchange, string routingKey,
            string contentType, ReadOnlyMemory<byte> data)
        {
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.DeliveryMode = DeliveryMode.Persistent;
            properties.ContentType = contentType;
            
            channel.BasicPublish(exchange, routingKey, properties, data);
        }

        struct DeliveryMode
        {
            public const byte NonPersistent = 1;
            public const byte Persistent = 2;
        }
        struct ContentType
        {
            public const string TextPlain = "text/plain";
            public const string ApplicationJson = "application/json";
        }
        
        /// <summary>
        /// Subscribes to queue to fetch only single message and then unsubscribes.
        /// Should not be used in a loop for performance reasons, better use BasicConsumer
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="queueName"></param>
        /// <param name="autoAck"></param>
        /// <returns></returns>
        public BasicGetResult GetOnlyNextMessage(IModel channel, string queueName, bool autoAck = false) =>
            channel.BasicGet(queueName, autoAck);

        public void DeadLetter(IModel channel, ulong deliveryTag) =>
            channel.BasicReject(deliveryTag, false);
        
        public async Task CreateNewVHostAsync(string rabbitmqctlPath, string vHostName)
        {
            var processStartInfo = createProcessStartInfoForRabbitMQCtl(rabbitmqctlPath, $"add_vhost {vHostName}");

            using (var rabbitMqCtl = Process.Start(processStartInfo))
            {
                rabbitMqCtl.Start();
                await rabbitMqCtl.WaitForExitAsync();

                var result = await rabbitMqCtl.StandardOutput.ReadToEndAsync();
            }
        }
        
        public async Task RemoveVHostAsync(string rabbitmqctlPath, string vHostName)
        {
            var processStartInfo = createProcessStartInfoForRabbitMQCtl(rabbitmqctlPath, $"delete_vhost {vHostName}");

            using (var rabbitMqCtl = Process.Start(processStartInfo))
            {
                rabbitMqCtl.Start();
                await rabbitMqCtl.WaitForExitAsync();

                var result = await rabbitMqCtl.StandardOutput.ReadToEndAsync();
            }
        }
        
        public async Task<IEnumerable<string>> GetAllHostsAsync(string rabbitmqctlPath)
        {
            var processStartInfo = createProcessStartInfoForRabbitMQCtl(rabbitmqctlPath, $"list_vhosts");

            using (var rabbitMqCtl = Process.Start(processStartInfo))
            {
                rabbitMqCtl.Start();
                await rabbitMqCtl.WaitForExitAsync();

                var result = await rabbitMqCtl.StandardOutput.ReadToEndAsync();

                return result.Split("\n");
            }
        }
        
        private ProcessStartInfo createProcessStartInfoForRabbitMQCtl(string rabbitmqctlPath, string argument)
        {
            rabbitmqctlPath ??= @"C:\Program Files\RabbitMQ Server\rabbitmq_server-3.8.11\sbin\rabbitmqctl.bat";

            rabbitmqctlPath = rabbitmqctlPath.EndsWith(@"\sbin")
                ? rabbitmqctlPath + @"\rabbitmqctl.bat"
                : rabbitmqctlPath;

            rabbitmqctlPath = rabbitmqctlPath.EndsWith("rabbitmqctl")
                ? rabbitmqctlPath + ".bat"
                : rabbitmqctlPath;

            var processStartInfo = new ProcessStartInfo(
                rabbitmqctlPath,
                argument);

            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            
            return processStartInfo;
        }
    }
}