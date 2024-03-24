using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlobOA.BlazorClient.Components;

public sealed class Redirect2Login : ComponentBase
{
    [Inject]
    public NavigationManager Nav { get; set; } = null!;

    protected override void OnInitialized()
    {
        Nav.NavigateToLogin("/login");
    }
}