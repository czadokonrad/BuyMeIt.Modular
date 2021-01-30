using System.Threading.Tasks;

namespace BuyMeIt.Modules.UserAccess.Domain.UserRegistrations
{
    public interface IUserRegistrationRepository
    {
        Task AddAsync(UserRegistration userRegistration);

        Task<UserRegistration> GetByIdAsync(UserRegistrationId userRegistrationId);
    }
}
