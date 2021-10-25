using System.Threading.Tasks;

namespace SignalrAuthSample.Stubs.AuthApi
{
    public interface IAuthApiClient
    {
        Task<int> GetUserIdAsync(string token);
    }
}
