using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using SignalrAuthSample.Hubs;
using SignalrAuthSample.Stubs;
using SignalrAuthSample.Stubs.AuthApi;
using SignalrAuthSample.Tools;

namespace SignalrAuthSample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<FakeNotificationService>();
            services.AddSingleton<IAuthApiClient, FakeAuthApiClient>();
            
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

            services.AddAuthentication(opts =>
                    {
                        opts.DefaultAuthenticateScheme = HubTokenAuthenticationDefaults.AuthenticationScheme;
                        opts.DefaultChallengeScheme = HubTokenAuthenticationDefaults.AuthenticationScheme;
                    })
                    .AddHubTokenAuthenticationScheme();

            services.AddRouting(opts =>
            {
                opts.AppendTrailingSlash = false;
                opts.LowercaseUrls = false;
            });

            services.AddSignalR(opts => opts.EnableDetailedErrors = true);
            services.AddControllers();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(x =>
            {
                x.MapHub<InfoHub>("/signalr/info");
                x.MapControllers();
            });
        }
    }
}
