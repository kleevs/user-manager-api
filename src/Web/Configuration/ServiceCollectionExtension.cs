using Entity;
using Microsoft.Extensions.DependencyInjection;
using Entity.Repository;
using UserManager;
using UserManager.Implementation;
using UserManager.Model;
using UserManager.Spi;
using Web.Tools;
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

            // Filter
            services.AddScoped<IFilterManager<IFilter, IUserFilterable>, UserFilterManager>();
            services.AddScoped<IFilterManager<ILoginFilter, IUserLoginFilterable>, LoginFilterManager>();

            // Repository 
            services.AddScoped<IGenericReaderRepository<IUserFilterable>, UserRepository>();
            services.AddScoped<IGenericWriterRepository<INewUser>, UserRepository>();
            services.AddScoped<IGenericWriterRepository<IUpdateUser, int>, UserRepository>();
            services.AddScoped<IGenericReaderRepository<IUserEmailable>, UserRepository>();
            services.AddScoped<IGenericReaderRepository<IUserLoginFilterable>, UserRepository>();

            // DB Context
            services.AddScoped<IDbContext, DbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
