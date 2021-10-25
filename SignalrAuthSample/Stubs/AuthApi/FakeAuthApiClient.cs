using System;
using System.Threading.Tasks;

namespace SignalrAuthSample.Stubs.AuthApi
{
    public class FakeAuthApiClient: IAuthApiClient
    {
        public Task<int> GetUserIdAsync(string token)
        {
            if(token == "123")
                return Task.FromResult(1);

            throw new Exception("Authorization failed!");
        }
    }
}
