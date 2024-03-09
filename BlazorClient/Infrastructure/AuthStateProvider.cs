using System.Security.Claims;
using LooperCorp.Domain;
using Microsoft.AspNetCore.Components.Authorization;

namespace LooperCorp.Infrastructure;

internal sealed class AuthStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _principal;

    public AuthStateProvider()
    {
        _principal = GetPrincipal(null);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        Task.FromResult(new AuthenticationState(_principal));

    private ClaimsPrincipal GetPrincipal(UserProfile? profile)
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity()); // anonymous
        if (profile is not null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, profile.Name),
                new Claim(ClaimTypes.Email, profile.Email)
            };
            var identity = new ClaimsIdentity(claims, nameof(AuthStateProvider));
            principal = new ClaimsPrincipal(identity);
        }

        return principal;
    }
}
