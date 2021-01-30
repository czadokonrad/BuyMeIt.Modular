using Autofac;
using Serilog;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration.Logging
{
    public class LoggingModule : Module
    {
        private readonly ILogger _logger;

        internal LoggingModule(ILogger logger)
        {
            _logger = logger;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_logger)
                .As<ILogger>()
                .SingleInstance();
        }
    }
}