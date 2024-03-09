var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

app.UseStaticFiles()
    .UseBlazorFrameworkFiles();

app.MapFallbackToFile("index.html");

app.Run();
