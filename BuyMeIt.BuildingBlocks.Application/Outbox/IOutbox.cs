using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.Application.Outbox
{
    public interface IOutbox
    {
        void Add(OutboxMessage message);

        Task SaveAsync();
    }
}
