using Entity;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using UserManager;
using UserManager.Implementation;
using UserManager.Spi;
using Web.Tools;

namespace Web.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection Configure(this IServiceCollection services)
        {
            services.AddScoped<IUserManager, UserManager.Implementation.UserManager>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDbContext, DbContext>();
            services.AddScoped<IHasher, Hasher>();
            services.AddScoped<IIdentityManager, IdentityManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            return services;
        }
    }
}
