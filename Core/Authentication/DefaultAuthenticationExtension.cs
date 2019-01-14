using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Core.Authentication
{
    public static class DefaultAuthenticationExtension
    {
        public static AuthenticationBuilder AddDefault(this AuthenticationBuilder builder)
        {
            return builder.AddDefault((option) => { });
        }

        public static AuthenticationBuilder AddDefault(this AuthenticationBuilder builder, Action<CookieAuthenticationOptions> configureOptions)
        {
            return builder.AddDefault(CookieAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddDefault(this AuthenticationBuilder builder, string authenticationScheme, Action<CookieAuthenticationOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>());
            return builder.AddScheme<CookieAuthenticationOptions, DefaultAuthenticationHandler>(authenticationScheme, null, configureOptions);
        }

        public static IApplicationBuilder UseUnauthorizeExceptionMiddleware(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<UnauthorizeExceptionMiddleware>();
        }
    }
}
