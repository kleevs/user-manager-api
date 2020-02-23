using Entity;
using Microsoft.Extensions.DependencyInjection;
using UserManager;
using UserManager.Implementation;
using UserManager.Spi;
using Tool;

namespace Web.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection Configure(this IServiceCollection services)
        {
            // Service
            services.AddScoped<IUserReaderService, UserReaderService>();
            services.AddScoped<IUserWriterService, UserWriterService>();
            services.AddScoped<IIdentityManager, IdentityManager>();

            // Tools
            services.AddScoped<IHasher, Hasher>();

            // Repository 
            services.AddScoped<IUnitOfWork, DbContext>();
            services.AddScoped<IUserReadOnlyRepository, DbContext>();
            services.AddScoped<IUserRepository, DbContext>();
            services.AddScoped<IAccountRepository, DbContext>();


            return services;
        }
    }
}
