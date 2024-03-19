using BlobOA.Shared.Messages;
using Proto;

namespace BlobOA.EmailServer;

public sealed class EmailSender : IActor
{
    public Task ReceiveAsync(IContext ctx)
    {
        if (ctx.Message is Greeting greeting)
        {
            Console.WriteLine($"Hello, {greeting.Message}");
        }

        return Task.CompletedTask;
    }
}
