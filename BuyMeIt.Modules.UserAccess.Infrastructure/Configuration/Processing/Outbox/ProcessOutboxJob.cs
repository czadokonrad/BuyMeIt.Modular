using System.Threading.Tasks;
using Quartz;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox
{
    [DisallowConcurrentExecution]
    public class ProcessOutboxJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await CommandsExecutor.ExecuteAsync(new ProcessOutboxCommand());
        }
    }
}