using Microsoft.AspNetCore.SignalR;

namespace SignalrAuthSample.Tools
{
    public class NameUserIdProvider: IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}
