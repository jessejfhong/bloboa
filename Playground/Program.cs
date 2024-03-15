using Playground;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHostedService<ActorSystemCluster>()
    .AddHostedService<SmartBulbSimulator>()
    .AddActorSystem();

var app = builder.Build();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
Proto.Log.SetLoggerFactory(loggerFactory);

app.MapGet("/", () => "Hello World!");

app.Run();
