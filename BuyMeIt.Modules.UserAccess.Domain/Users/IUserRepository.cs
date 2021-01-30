using System.Threading.Tasks;

namespace BuyMeIt.Modules.UserAccess.Domain.Users
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
    }
}
