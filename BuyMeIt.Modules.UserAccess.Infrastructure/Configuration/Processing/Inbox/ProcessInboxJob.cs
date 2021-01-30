using System.Threading.Tasks;
using Quartz;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration.Processing.Inbox
{
    [DisallowConcurrentExecution]
    public class ProcessInboxJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await CommandsExecutor.ExecuteAsync(new ProcessInboxCommand());
        }
    }
}