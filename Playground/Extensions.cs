using Proto;
using Proto.Cluster;
using Proto.Cluster.Partition;
using Proto.Cluster.Testing;
using Proto.DependencyInjection;
using Proto.Remote.GrpcNet;

namespace Playground;

internal static class Extensions
{
    public static void AddActorSystem(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var config = new ActorSystemConfig();
            var remoteConfig = GrpcNetRemoteConfig
                .BindToLocalhost();
            var clusterConfig = ClusterConfig
                .Setup(
                    clusterName: "Playground",
                    clusterProvider: new TestProvider(new TestProviderOptions(), new InMemAgent()),
                    identityLookup: new PartitionIdentityLookup()
                )
                .WithClusterKind(
                    kind: SmartBulbGrainActor.Kind,
                    prop: Props.FromProducer(() =>
                        new SmartBulbGrainActor(
                            (context, clusterIdentity) => new SmartBulbGrain(context, clusterIdentity))
                    )
                );

            return new ActorSystem(config)
                .WithServiceProvider(sp)
                .WithRemote(remoteConfig)
                .WithCluster(clusterConfig);
        });
    }
}