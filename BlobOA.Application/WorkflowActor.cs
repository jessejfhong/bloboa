namespace BlobOA.Application;

using Proto;

public class WorkflowActor : IActor
{
    public Task ReceiveAsync(IContext ctx)
    {
        return Task.CompletedTask;
    }
}
