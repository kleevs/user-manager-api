using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Core.Authentication
{
    public class DefaultAuthenticationHandler : CookieAuthenticationHandler
    {
        public DefaultAuthenticationHandler(IOptionsMonitor<CookieAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) :
            base(options, logger, encoder, clock)
        { }

        protected async override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await Task.Run(() =>
            {
                throw new System.UnauthorizedAccessException();
            });
        }
    }
}
