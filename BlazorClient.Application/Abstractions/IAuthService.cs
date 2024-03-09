namespace LooperCorp.Application.Abstractions;

public interface IAuthService
{
    Task SignInAsync(string username, string password);
    Task SignOutAsync();
}
