using System;
using System.Threading.Tasks;
using BuyMeIt.BuildingBlocks.EventBus.RabbitMQ;
using Moq;
using NUnit.Framework;
using RabbitMQ.Client;
using Serilog;

namespace BuyMeIt.BuildingBlocks.EventBus.Tests.RabbitMQ
{
    [TestFixture]
    public class RabbitMqClientTests : EventBusTestsBase
    {
        private RabbitMqPersistentConnection _rabbitMqConnection;
        private Mock<ILogger> _mockLogger;

        [SetUp]
        public void Initialize()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _mockLogger = new Mock<ILogger>();

            _rabbitMqConnection = new RabbitMqPersistentConnection(
                connectionFactory,
                _mockLogger.Object,
                5);
        }

        //[TearDown]
        //public void DisposeRabbitMqConnection()
        //{
        //    _rabbitMqConnection.Dispose();
        //}

        [Test]
        public async Task WhenPublishInvoked_AndNoSubscribers_Then()
        {
            string queueName = "RabbitMQEventBusClient_test_queue";
            var rabbitMqClient = new RabbitMQEventBusClient(
                _rabbitMqConnection, 
                _mockLogger.Object, 
                queueName, 5);

            await rabbitMqClient.Publish(new FirstTestEvent(Guid.NewGuid(), DateTimeOffset.UtcNow));

            rabbitMqClient.Subscribe(new FirstTestEventHandler());

            await Task.Delay(300000);
        }
    }
}