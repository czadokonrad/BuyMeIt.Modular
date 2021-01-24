using BuyMeIt.Modules.UserAccess.Domain.UserRegistrations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Repositories
{
    public sealed class UserRegistrationRepository : IUserRegistrationRepository
    {
        private readonly UserAccessContext _context;

        public UserRegistrationRepository(UserAccessContext context) =>
            _context = context;
        public async Task AddAsync(UserRegistration userRegistration) => 
            await _context.UserRegistrations.AddAsync(userRegistration);
       
        public async Task<UserRegistration> GetByIdAsync(UserRegistrationId userRegistrationId) =>
            await _context.UserRegistrations.SingleOrDefaultAsync(ur => ur.Id == userRegistrationId);
    }
}
