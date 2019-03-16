using Entity;
using Entity.Filter;
using Microsoft.Extensions.DependencyInjection;
using Entity.Repository;
using UserManager;
using UserManager.Implementation;
using UserManager.Model;
using UserManager.Spi;
using Web.Tools;

namespace Web.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection Configure(this IServiceCollection services)
        {
            services.AddScoped<IUserManager, UserManager.Implementation.UserManager>();
            services.AddScoped<IDbContext, DbContext>();
            services.AddScoped<IHasher, Hasher>();
            services.AddScoped<IIdentityManager, IdentityManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            services.AddScoped<IGenericReaderRepository<IFilter, IUserData>, UserRepository>();
            services.AddScoped<IGenericWriterRepository<INewUser>, UserRepository>();
            services.AddScoped<IGenericWriterRepository<IUpdateUser>, UserRepository>();
            services.AddScoped<IGenericWriterRepository<IUpdateUser, int>, UserRepository>();
            services.AddScoped<IGenericReaderRepository<IFilter, IUserEmailable>, UserRepository>();

            services.AddScoped<IFilterManager<IFilter, User>, UserFilterManager>();

            return services;
        }
    }
}
