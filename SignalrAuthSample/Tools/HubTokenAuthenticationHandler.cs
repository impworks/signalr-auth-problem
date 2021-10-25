using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalrAuthSample.Stubs.AuthApi;

namespace SignalrAuthSample.Tools
{
    /// <summary>
    /// Authorization handler that validates the token against a remote API.
    /// </summary>
    public class HubTokenAuthenticationHandler : AuthenticationHandler<HubTokenAuthenticationOptions>
    {
        public HubTokenAuthenticationHandler(
            IOptionsMonitor<HubTokenAuthenticationOptions> options,
            ILoggerFactory logFactory,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthApiClient api
        )
            : base(options, logFactory, encoder, clock)
        {
            _api = api;
        }

        private readonly IAuthApiClient _api;

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                // uncommenting this line makes everything suddenly work
                // return SuccessResult(1);
                
                var token = GetToken();
                if (string.IsNullOrEmpty(token))
                    return AuthenticateResult.NoResult();
            
                var userId = await _api.GetUserIdAsync(token);
                return SuccessResult(userId);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }
        
        /// <summary>
        /// Returns an identity with the specified user id.
        /// </summary>
        private AuthenticateResult SuccessResult(int userId)
        {
            var identity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString())
                }
            );
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        /// <summary>
        /// Checks if there is a token specified.
        /// </summary>
        private string GetToken()
        {
            const string Scheme = "Bearer ";

            var auth = Context.Request.Headers["Authorization"].ToString() ?? "";
            return auth.StartsWith(Scheme)
                ? auth.Substring(Scheme.Length)
                : "";
        }
    }
}
