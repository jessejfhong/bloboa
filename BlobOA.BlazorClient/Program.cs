using BlobOA.BlazorClient;
using BlobOA.BlazorClient.Application;
using BlobOA.BlazorClient.Application.Abstractions;
using BlobOA.BlazorClient.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<App>());

builder.Services
    .AddAuthorizationCore()
    .AddSingleton(sp => new AuthStateProvider())
    .AddTransient<AuthenticationStateProvider>(sp => sp.GetRequiredService<AuthStateProvider>())
    .AddCascadingAuthenticationState();

var baseAddress = builder.HostEnvironment.BaseAddress;
#if DEBUG
baseAddress = "http://localhost:5200/";
#endif
builder.Services
    .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(Consts.ApiKey))
    .AddHttpClient(Consts.ApiKey, client => client.BaseAddress = new Uri(baseAddress));

builder.Services
    .AddTransient<IAuthService, AuthService>();

var app = builder.Build();

await app.RunAsync();
