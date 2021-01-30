using System.Threading.Tasks;
using Autofac;
using BuyMeIt.Modules.UserAccess.Application.Contracts;
using MediatR;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration.Processing
{
    internal static class CommandsExecutor
    {
        internal static async Task ExecuteAsync(ICommand command)
        {
            await using var scope = UserAccessCompositionRoot.BeginLifetimeScope();

            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(command);
        }
    }
}