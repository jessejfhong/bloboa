using LooperCorp.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace LooperCorp.Application;

public static class InfraExtensions
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();

        return services;
    }
}