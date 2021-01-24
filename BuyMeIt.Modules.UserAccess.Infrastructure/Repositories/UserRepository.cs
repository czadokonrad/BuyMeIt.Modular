using BuyMeIt.Modules.UserAccess.Domain.Users;
using System.Threading.Tasks;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private UserAccessContext _context;

        public UserRepository(UserAccessContext context) =>
            _context = context;

        public async Task AddAsync(User user) =>
            await _context.Users.AddAsync(user);
    }
}
