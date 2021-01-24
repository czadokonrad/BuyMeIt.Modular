using BuyMeIt.Modules.UserAccess.Domain.UserRegistrations;
using BuyMeIt.Modules.UserAccess.Domain.Users;
using BuyMeIt.Modules.UserAccess.Infrastructure.EFConfigurations;
using Microsoft.EntityFrameworkCore;

namespace BuyMeIt.Modules.UserAccess.Infrastructure
{
    public class UserAccessContext : DbContext
    {
        public UserAccessContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserRegistration> UserRegistrations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserRegistrationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        }
    }
}
