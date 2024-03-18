using Microsoft.Extensions.Hosting;
using BlobOA.EmailServer;
using Microsoft.Extensions.DependencyInjection;
using Proto;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddActorSystem()
    .AddHostedService<ActorSystemHostedService>();

var app = builder.Build();

var context = app.Services.GetRequiredService<IRootContext>();
var props = Props.FromProducer(() => new EmailSender());
context.SpawnNamed(props, "email-sender");

app.Run();
