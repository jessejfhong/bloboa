using Microsoft.Extensions.DependencyInjection;
using Proto;
using Proto.DependencyInjection;
using Proto.Remote.GrpcNet;

namespace BlobOA.EmailServer;

internal static class Extensions
{
    public static IServiceCollection AddActorSystem(this IServiceCollection services) =>
        services.AddSingleton(sp =>
        {
            var actorSystemConfig = new ActorSystemConfig();
            var remoteConfig = GrpcNetRemoteConfig
                .BindToLocalhost(8000);

            return new ActorSystem(actorSystemConfig)
                .WithServiceProvider(sp)
                .WithRemote(remoteConfig);
        });
}
