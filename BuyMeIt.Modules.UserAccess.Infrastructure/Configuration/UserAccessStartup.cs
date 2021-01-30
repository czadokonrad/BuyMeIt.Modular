using Autofac;
using BuyMeIt.BuildingBlocks.Application;
using BuyMeIt.BuildingBlocks.Application.Emails;
using BuyMeIt.BuildingBlocks.Infrastructure.Emails;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration
{
    public static class UserAccessStartup
    {
        private static IContainer _container;

        public static void Initialize(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            EmailsConfiguration emailsConfiguration,
            string textEncryptionKey,
            IEmailSender emailSender)
        {

        }
    }
}
