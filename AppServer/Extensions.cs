using Microsoft.OpenApi.Models;

namespace LooperCorp.AppServer;

internal static class Extensions
{
    const string CorsPolicy = "AllowedOrigins";

    public static void AddDevelopmentServices(this WebApplicationBuilder builder, OpenApiInfo openApi)
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

    public static void UseDevelopmentMiddleware(this IApplicationBuilder app)
    {
        app.UseSwagger()
            .UseSwaggerUI()
            .UseCors(CorsPolicy)
            .UseWebAssemblyDebugging();
    }
}
