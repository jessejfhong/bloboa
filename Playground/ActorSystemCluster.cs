
using Proto;
using Proto.Cluster;

namespace Playground;

public sealed class ActorSystemCluster : IHostedService
{
    private readonly ActorSystem _actorSystem;

    public ActorSystemCluster(ActorSystem actorSystem)
    {
        _actorSystem = actorSystem ?? throw new ArgumentNullException(nameof(ActorSystem));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _actorSystem
            .Cluster()
            .StartMemberAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _actorSystem
            .Cluster()
            .ShutdownAsync();
    }
}
