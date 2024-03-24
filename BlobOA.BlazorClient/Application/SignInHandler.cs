using BlobOA.BlazorClient.Application.Abstractions;
using MediatR;

namespace BlobOA.BlazorClient.Application;

public sealed class SignInAction : MenuAction
{
    public string Username { get; }
    public string Password { get; }

    public SignInAction(string username, string password)
    {
        Username = username;
        Password = password;
    }
}

internal sealed class SignInActionHandler : IRequestHandler<SignInAction>
{
    private readonly IAuthService _authService;

    public SignInActionHandler(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(IAuthService));
    }

    public async Task Handle(SignInAction request, CancellationToken cancellationToken)
    {
        await _authService.SignInAsync(request.Username, request.Password);
        Console.WriteLine($"SignIn {request.Username}");
    }
}
