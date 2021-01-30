using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BuyMeIt.Modules.UserAccess.Application.Configuration.Commands;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;
    }
}
