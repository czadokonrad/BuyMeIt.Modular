using System;
using System.Threading.Tasks;
using BuyMeIt.BuildingBlocks.EventBus.InMemory;
using NUnit.Framework;

namespace BuyMeIt.BuildingBlocks.EventBus.Tests.InMemory
{
    [TestFixture]
    public class InMemoryEventBusTests : EventBusTestsBase
    {
        private InMemoryEventBus _eventBus;

        [SetUp]
        public void Initialize()
        {
            _eventBus = InMemoryEventBus.Instance;
            _eventBus.ClearHandlers();
        }
        
        [Test]
        public void When_3_Handlers_AreSubscribed_Then_HandlersCollection_ShouldHave_3_Items()
        {
            
            _eventBus.Subscribe(new FirstTestEventHandler());
            _eventBus.Subscribe(new SecondTestEventHandler());
            _eventBus.Subscribe(new ThirdTestEventHandler());

            Assert.AreEqual(3, _eventBus.Handlers.Count);
        }

        [Test]
        public async Task When_3_Handlers_AreSubscribed_And_Each_PublishedOnce_Then_Publish_ShouldFire_Exactly_3_Times()
        {
            
            _eventBus.Subscribe(new FirstTestEventHandler());
            _eventBus.Subscribe(new SecondTestEventHandler());
            _eventBus.Subscribe(new ThirdTestEventHandler());

            int firedCount = 0;

            _eventBus.OnEventPublished += (sender, args) => ++firedCount; 
            
            await _eventBus.Publish(new FirstTestEvent(Guid.NewGuid(), DateTimeOffset.UtcNow));
            await _eventBus.Publish(new SecondTestEvent(Guid.NewGuid(), DateTimeOffset.UtcNow));
            await _eventBus.Publish(new ThirdTestEvent(Guid.NewGuid(), DateTimeOffset.UtcNow));
 
            Assert.AreEqual(3, firedCount);
            
        }
    }
}