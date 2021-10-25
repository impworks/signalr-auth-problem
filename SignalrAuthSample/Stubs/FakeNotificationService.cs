using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalrAuthSample.Hubs;

namespace SignalrAuthSample.Stubs
{
    /// <summary>
    /// Fake notification service that sends current date every 2 seconds.
    /// </summary>
    public class FakeNotificationService: IHostedService
    {
        public FakeNotificationService(IHubContext<InfoHub> hubContext, ILogger<FakeNotificationService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
            _cts = new CancellationTokenSource();
        }

        private readonly IHubContext<InfoHub> _hubContext;
        private readonly ILogger _logger;
        private readonly CancellationTokenSource _cts;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // run in the background
            Task.Run(async () =>
            {
                var id = 1;
                while (!_cts.Token.IsCancellationRequested)
                {
                    await Task.Delay(2000);
                    await _hubContext.Clients.Users(new[] {"1"})
                                     .SendAsync("NewNotification", new {Id = id, Date = DateTime.Now});
                    
                    _logger.LogInformation("Sent notification " + id);

                    id++;
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }
    }
}
