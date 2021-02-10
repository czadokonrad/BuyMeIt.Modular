using System.Threading.Tasks;
using Quartz;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands
{
    [DisallowConcurrentExecution]
    public class ProcessInternalCommandsJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await CommandsExecutor.ExecuteAsync(new ProcessInternalCommandsCommand());
        }
    }
}