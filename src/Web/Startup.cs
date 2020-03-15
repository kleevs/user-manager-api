using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;
using Web.Configuration;
using Web.Tools;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            ////services.AddDbContext<Entity.DbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<Entity.DbContext>(options => options.UseInMemoryDatabase("UserManager"));
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
                option.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

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
            services.AddHealthChecks().AddDbContextCheck<Entity.DbContext>();
            services.AddCors();
            services.Configure<AppConfig>(Configuration);
            services.Configure();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="optionsAccessor"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptionsMonitor<AppConfig> optionsAccessor)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();

            app.UseCors(builder => builder.WithOrigins("http://localhost:4200", "https://kleevs.github.io")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserManager v1");
                c.InjectStylesheet("https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css");
                c.InjectJavascript("https://code.jquery.com/jquery-3.3.1.min.js");
                c.InjectJavascript("https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js");
                c.InjectJavascript("https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js");
                c.InjectJavascript("/swagger-ui.js");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = WriteResponse
                });
            });

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Users}/{action=Index}/{id?}");
            });
        }

        private static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));

            return context.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }
    }
}
