using Entity;
using Microsoft.Extensions.DependencyInjection;
using Tool;

namespace Web.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection Configure(this IServiceCollection services)
        {
            // Tools
            services.AddScoped<IHasher, Hasher>();

            // Repository 
            services.AddScoped<DbContext>();

            return services;
        }
    }
}
