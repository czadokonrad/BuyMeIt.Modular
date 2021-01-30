using Autofac;
using BuyMeIt.Modules.UserAccess.Domain.UserRegistrations;
using BuyMeIt.Modules.UserAccess.Application.UserRegistrations;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration.Domain
{
    internal sealed class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserUniqueness>()
                .As<IUserUniqueness>()
                .InstancePerLifetimeScope();
        }
    }
}
