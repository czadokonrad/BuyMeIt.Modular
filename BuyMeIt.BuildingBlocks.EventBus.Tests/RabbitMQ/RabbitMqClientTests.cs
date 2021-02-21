using System;
using System.Threading.Tasks;
using BuyMeIt.BuildingBlocks.EventBus.InMemory;
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
        private ConnectionFactory _connectionFactory;
        private Mock<ILogger> _mockLogger;
        private const string queueName = "RabbitMQEventBusClient_test_queue";

        [SetUp]
        public void Initialize()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                DispatchConsumersAsync = true
            };

            _mockLogger = new Mock<ILogger>();

            _rabbitMqConnection = new RabbitMqPersistentConnection(
                _connectionFactory,
                _mockLogger.Object,
                5);
        }

        [TearDown]
        public void ClearBeforeNextTestRun()
        {
            InMemoryEventBus.Instance.ClearHandlers();

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDelete(queueName, false, false);
                }
            }
        }

        [Test]
        public async Task When_PublishInvoked_And_1_SubscriberForPublishedEvent_ThenEventShouldBeHandled_Once()
        {
            var rabbitMqClient = new RabbitMQEventBusClient(
                _rabbitMqConnection, 
                _mockLogger.Object, 
                queueName, 
                5);

            int timesCalled = 0;

            InMemoryEventBus.Instance.OnEventPublished += (s, e) =>
            {
                ++timesCalled;
            };

            rabbitMqClient.Subscribe(new FourthTestEventHandler());
            
            await rabbitMqClient.Publish(new FourthTestEvent(Guid.NewGuid(), DateTimeOffset.UtcNow));

            await Task.Delay(500);

            Assert.AreEqual(1, timesCalled);

        }

        [Test]
        public async Task When_PublishInvoked_Twice_Then_EventsShouldBeHandledTwice()
        {
            var rabbitMqClient = new RabbitMQEventBusClient(
                _rabbitMqConnection, 
                _mockLogger.Object, 
                queueName, 
                5);

            int timesCalled = 0;

            InMemoryEventBus.Instance.OnEventPublished += (s, e) =>
            {
                ++timesCalled;
            };
            
            rabbitMqClient.Subscribe(new FirstTestEventHandler());
            rabbitMqClient.Subscribe(new SecondTestEventHandler());

            await rabbitMqClient.Publish(new FirstTestEvent(Guid.NewGuid(), DateTimeOffset.UtcNow));
            await rabbitMqClient.Publish(new SecondTestEvent(Guid.NewGuid(), DateTimeOffset.UtcNow));

            await Task.Delay(500);
            
            Assert.That(timesCalled, Is.EqualTo(2));
        }
    }
}