using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Proto;
using Proto.Remote;

namespace BlobOA.EmailServer;

internal sealed class ActorSystemHostedService : IHostedService
{
    private readonly ILogger<ActorSystemHostedService> _logger;
    private readonly ActorSystem _actorSystem;

    public ActorSystemHostedService(
        ILogger<ActorSystemHostedService> logger,
        ActorSystem actorSystem)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _actorSystem = actorSystem ?? throw new ArgumentNullException(nameof(ActorSystem));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _actorSystem
            .Remote()
            .StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _actorSystem
            .Remote()
            .ShutdownAsync();
    }
}