//using BlobOA.Shared.Messages;
using Microsoft.OpenApi.Models;
// using Proto;
// using Proto.DependencyInjection;
// using Proto.Remote;
// using Proto.Remote.GrpcNet;

namespace BlobOA.AppServer;

internal static class Extensions
{
    const string CorsPolicy = "AllowedOrigins";

    internal static void AddDevelopmentServices(this WebApplicationBuilder builder, OpenApiInfo openApi)
    {
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc(openApi.Version, new OpenApiInfo
                {
                    Version = openApi.Version,
                    Title = openApi.Title,
                    Description = openApi.Description
                });
            });

        var allowedOrigin = builder.Configuration.GetValue<string>(CorsPolicy) ?? string.Empty;
        builder.Services
            .AddCors(options =>
                options.AddPolicy(CorsPolicy,
                    policy =>
                        policy.WithOrigins(allowedOrigin)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()));
    }

    internal static void UseDevelopmentMiddleware(this IApplicationBuilder app)
    {
        app.UseSwagger()
            .UseSwaggerUI()
            .UseCors(CorsPolicy)
            .UseWebAssemblyDebugging();
    }

    internal static IApplicationBuilder UseBlazorEnvHeader(this IApplicationBuilder app, IWebHostEnvironment env) =>
        app.Use(async (ctx, next) =>
        {
            ctx.Response.Headers.TryAdd("Blazor-Environment", env.EnvironmentName);
            await next();
        });

    // internal static IServiceCollection AddActorSystem(this IHostApplicationBuilder builder) =>
    //     builder.Services
    //         .AddSingleton(sp =>
    //         {
    //             var actorSystemConfig = new ActorSystemConfig();
    //             var remoteConfog = GrpcNetRemoteConfig
    //                 .BindToLocalhost()
    //                 .WithProtoMessages(MessagesReflection.Descriptor);

    //             return new ActorSystem(actorSystemConfig)
    //                 .WithServiceProvider(sp)
    //                 .WithRemote(remoteConfog);
    //         })
    //         .AddSingleton(sp => sp.GetRequiredService<ActorSystem>().Root);
}
