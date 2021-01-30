using Autofac;
using BuyMeIt.BuildingBlocks.Application.Data;
using BuyMeIt.BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration.DataAccess
{
    internal sealed class DataAccessModule : Module
    {
        private readonly string _databaseConnectionString;

        internal DataAccessModule(string databaseConnectionString)
        {
            _databaseConnectionString = databaseConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionFactory>()
                .As<ISqlConnectionFactory>()
                .WithParameter("connectionString", _databaseConnectionString)
                .InstancePerLifetimeScope();

            builder.Register(component =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<UserAccessContext>();
                dbContextOptionsBuilder.UseSqlServer(_databaseConnectionString);

                dbContextOptionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                return new UserAccessContext(dbContextOptionsBuilder.Options);
            })
            .AsSelf()
            .As<DbContext>()
            .InstancePerLifetimeScope();


            var infrastructureAssembly = typeof(UserAccessContext).Assembly;

            builder.RegisterAssemblyTypes(infrastructureAssembly)
                .Where(type => type.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .FindConstructorsWith(new AllConstructorFinder());
        }
    }
}
