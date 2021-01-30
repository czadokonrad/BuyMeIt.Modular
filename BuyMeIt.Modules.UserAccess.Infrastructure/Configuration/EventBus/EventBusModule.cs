using Autofac;
using BuyMeIt.BuildingBlocks.EventBus.InMemory;
using BuyMeIt.BuildingBlocks.Infrastructure.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration.EventBus
{
    internal sealed class EventBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryEventBusClient>()
                .As<IEventsBus>()
                .InstancePerLifetimeScope();
        }
    }
}
