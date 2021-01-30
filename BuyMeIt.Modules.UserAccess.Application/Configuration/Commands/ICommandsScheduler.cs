using System.Threading.Tasks;
using BuyMeIt.Modules.UserAccess.Application.Contracts;

namespace BuyMeIt.Modules.UserAccess.Application.Configuration.Commands
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync(ICommand command);

        Task EnqueueAsync<T>(ICommand<T> command);
    }
}