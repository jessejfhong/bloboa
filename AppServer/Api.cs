using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using LooperCorp.Shared.Dtos;

namespace LooperCorp.AppServer;

internal static class MapApis
{
    public static IEndpointRouteBuilder MapApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("ping", () => "May the force be with you!");

        var app = builder.MapGroup("api/v1/")
            .RequireAuthorization();

        var auth = app.MapGroup("auth/");
        auth.MapPost("signin", SignInAsync)
            .AllowAnonymous();

        // add and extra parameter to workaround with https://github.com/dotnet/aspnetcore/issues/44970
        // since no session data is stored in server, calling this api
        // just simply clear the cookie in the browser.
        auth.MapGet("signout", async (int? _, HttpContext ctx) => await ctx.SignOutAsync());

        return builder;
    }

    public static async Task<Results<Ok<UserProfileDTO>, UnauthorizedHttpResult>> SignInAsync(
        [FromBody] LoginDTO dto,
        HttpContext ctx)
    {
        var profile = new UserProfileDTO { Name = "Jesse", Email = "jesse@example.com" };

        if (ctx.User.Identity is not null && ctx.User.Identity.IsAuthenticated)
        {
            return TypedResults.Ok(profile);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "Jesse"),
            new Claim(ClaimTypes.Role, "Developer")
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);
        await ctx.SignInAsync(claimsPrinciple);

        return TypedResults.Ok(profile);
    }
}
