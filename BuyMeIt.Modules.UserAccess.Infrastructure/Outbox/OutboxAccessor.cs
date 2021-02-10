using System.Threading.Tasks;
using BuyMeIt.BuildingBlocks.Application.Outbox;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Outbox
{
    public class OutboxAccessor : IOutbox
    {
        private readonly UserAccessContext _userAccessContext;

        public OutboxAccessor(UserAccessContext userAccessContext)
        {
            _userAccessContext = userAccessContext;
        }

        public void Add(OutboxMessage message)
        {
            //_userAccessContext.OutboxMessages.Add(message);
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask; // Save is done automatically using EF Core Change Tracking mechanism during SaveChanges.
        }
    }
}