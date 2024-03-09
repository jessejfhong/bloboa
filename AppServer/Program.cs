using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;

using LooperCorp.AppServer;

const string CorsPolicy = "AllowAny";

var builder = WebApplication.CreateBuilder(args);

var appConfig = builder.Configuration.Get<AppConfig>();
if (appConfig is null || !AppConfig.IsValid(appConfig)) return;

builder.Services
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.IncludeFields = true;
        options.SerializerOptions.WriteIndented = true;
        options.SerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "StockApp";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
        options.Events.OnRedirectToLogin = ctx =>
        {
            // return 401 instead of redirect to login path,
            // redirection will be handle by SPA client
            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        };
    });

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc(appConfig.OpenApi.Version, new OpenApiInfo
        {
            Version = appConfig.OpenApi.Version,
            Title = appConfig.OpenApi.Title,
            Description = appConfig.OpenApi.Description
        });
    });

builder.Services.AddCors(options =>
    options.AddPolicy(CorsPolicy,
        policy =>
            policy.WithOrigins(appConfig.AllowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()));

builder.Services
    .AddTransient<IExceptionHandler, GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseCors(CorsPolicy);

app.UseSwagger()
    .UseSwaggerUI();

app.MapApi();
    //.RequireAuthorization();

app.Run();
