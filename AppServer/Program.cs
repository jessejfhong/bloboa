using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using LooperCorp.AppServer;


var builder = WebApplication.CreateBuilder(args);
var appConfig = builder.Configuration.Get<AppConfig>() ?? new AppConfig();
if (!AppConfig.IsValid(appConfig)) return;

#if DEBUG
builder.AddDevelopmentServices(appConfig.OpenApi);
#endif

builder.Services
    .AddTransient<IExceptionHandler, GlobalExceptionHandler>()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.IncludeFields = true;
        options.SerializerOptions.WriteIndented = true;
        options.SerializerOptions.PropertyNameCaseInsensitive = true;
    })
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = appConfig.Cookie.Name;
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(appConfig.Cookie.ExpireHours);
        options.Events.OnRedirectToLogin = ctx =>
        {
            // return 401 instead of redirect to login path,
            // redirection will be handle by SPA client
            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        };
    });


var app = builder.Build();
app.UseExceptionHandler()
    .UseStaticFiles()
    .UseBlazorFrameworkFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();
app.MapApi()
    .MapFallbackToFile("index.html");

#if DEBUG
app.UseDevelopmentMiddleware();
#endif

app.Run();
