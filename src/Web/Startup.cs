using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Threading.Tasks;
using System.Xml.XPath;
using Web.Configuration;
using Web.Tools;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Entity.DbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = async (context) => 
                        {
                            await Task.Run(() => 
                            {
                                context.Response.StatusCode = 301;
                            });
                        }
                    };
                    options.LoginPath = "/accounts/login";
                    options.AccessDeniedPath = new PathString("/account/login");
                    options.Cookie.Path = "/";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Cookie.SameSite = SameSiteMode.None;
                });
            services.AddMvc(option => 
            {
                option.Filters.Add(new AuthorizeFilter());
                option.Filters.Add(new BusinessExceptionFilter());
                option.Filters.Add(new DevExceptionFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserManager", Version = "v1" });
                c.IncludeXmlComments(() =>
                {
                    var basePath = System.IO.Directory.GetCurrentDirectory();
                    var fileName = $"Web.xml";
                    return new XPathDocument(Path.Combine(basePath, fileName));
                });
            });

            services.AddCors();
            services.Configure<AppConfig>(Configuration);
            services.Configure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptionsMonitor<AppConfig> optionsAccessor)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:4200", "https://kleevs.github.io")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserManager v1");
            });

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Users}/{action=Index}/{id?}");
            });
        }
    }
}
