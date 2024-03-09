namespace LooperCorp.Domain;

internal sealed class UserProfile
{
    public string Name { get; }
    public string Email { get; }
    public string Department { get; }

    public UserProfile(string name, string email, string department)
    {
        Name = name;
        Email = email;
        Department = department;
    }
}