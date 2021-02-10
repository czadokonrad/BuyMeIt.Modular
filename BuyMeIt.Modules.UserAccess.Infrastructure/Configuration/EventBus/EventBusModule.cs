using Autofac;
using BuyMeIt.BuildingBlocks.EventBus.InMemory;
using BuyMeIt.BuildingBlocks.Infrastructure.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuyMeIt.BuildingBlocks.EventBus.RabbitMQ;
using RabbitMQ.Client;
using Serilog;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration.EventBus
{
    internal sealed class EventBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<RabbitMqPersistentConnection>()
                .As<IRabbitMqPersistentConnection>()
                .WithParameter("retryCount", 5)
                .SingleInstance();
            
            
            builder.Register(component =>
                {
                    var factory = new ConnectionFactory
                    {
                        UserName = "guest",
                        Password = "guest",
                        HostName = "localhost",
                        DispatchConsumersAsync = true
                    };

                    var logger = component.Resolve<ILogger>();
                    
                    return new RabbitMqPersistentConnection(factory, logger, 5);
                    
                })
                .AsSelf()
                .As<IRabbitMqPersistentConnection>()
                .SingleInstance();
            
            builder.RegisterType<InMemoryEventBusClient>()
                .As<IEventsBus>()
                .InstancePerLifetimeScope();


        }
    }
}
