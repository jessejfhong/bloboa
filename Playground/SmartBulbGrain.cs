
using Proto;
using Proto.Cluster;

namespace Playground;

public sealed class SmartBulbGrain : SmartBulbGrainBase
{
    private readonly ClusterIdentity _clusterIdentity;

    private enum SmartBulbState { Unknown, On, Off }
    private SmartBulbState _state = SmartBulbState.Unknown;

    public SmartBulbGrain(IContext context, ClusterIdentity clusterIdentity) : base(context)
    {
        _clusterIdentity = clusterIdentity;

        Console.WriteLine($"{_clusterIdentity.Identity}: created");
    }

    public override Task TurnOn()
    {
        if (_state != SmartBulbState.On)
        {
            Console.WriteLine($"{_clusterIdentity.Identity}: turning smart bulb on");

            _state = SmartBulbState.On;
        }

        return Task.CompletedTask;
    }

    public override Task TurnOff()
    {
        if (_state != SmartBulbState.Off)
        {
            Console.WriteLine($"{_clusterIdentity.Identity}: turning smart bulb off");

            _state = SmartBulbState.Off;
        }

        return Task.CompletedTask;
    }
}