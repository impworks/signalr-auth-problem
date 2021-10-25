using System;
using Microsoft.AspNetCore.Authentication;

namespace SignalrAuthSample.Tools
{
    public static class HubTokenAuthenticationDefaults
    {
        public const string AuthenticationScheme = "HubTokenAuthentication";
        public const string Policy = "AuthPolicy";

        public static AuthenticationBuilder AddHubTokenAuthenticationScheme(this AuthenticationBuilder builder)
        {
            return AddHubTokenAuthenticationScheme(builder, (options) => { });
        }

        public static AuthenticationBuilder AddHubTokenAuthenticationScheme(this AuthenticationBuilder builder, Action<HubTokenAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<HubTokenAuthenticationOptions, HubTokenAuthenticationHandler>(AuthenticationScheme, configureOptions);
        }
    }

    public class HubTokenAuthenticationOptions : AuthenticationSchemeOptions { }
}
