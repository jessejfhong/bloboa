using BlobOA.BlazorClient.Application.Abstractions;

namespace BlobOA.BlazorClient.Application;

internal class AuthService : IAuthService
{
    public async Task SignInAsync(string username, string password)
    {
        await Task.Delay(1);
    }

    public async Task SignOutAsync()
    {
        await Task.Delay(1);
    }
}
