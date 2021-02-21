using System.Reflection;
using BuyMeIt.Modules.UserAccess.Application.Configuration.Commands;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;
    }
}
