using System.Threading.Tasks;

namespace BuyMeIt.Modules.UserAccess.Domain.UserRegistrations
{
    public interface IUserUniqueness
    {
        bool IsUserUnique(string login, string email);
        
        Task<bool> IsUserUniqueAsync(string login, string email);
    }
}
