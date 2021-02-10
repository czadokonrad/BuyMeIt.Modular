using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.Domain
{
    public interface IAsyncBusinessRule : IBusinessRule
    {
        Task<bool> IsBrokenAsync();
    }
}