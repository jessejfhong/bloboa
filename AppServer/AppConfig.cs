using FluentValidation;

namespace LooperCorp.AppServer;

internal sealed class AppConfig
{
    public OpenApiConfig OpenApi { get; set; } = null!;
    public string[] AllowedOrigins { get; set; } = null!;

    public static bool IsValid(AppConfig config)
    {
        var validator = new AppConfigValidator();
        var results = validator.Validate(config);
        return results.IsValid;
    }
}

internal sealed class OpenApiConfig
{
    public string Version { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}

internal sealed class AppConfigValidator : AbstractValidator<AppConfig>
{
    public AppConfigValidator()
    {
        RuleFor(c => c.OpenApi).SetValidator(new OpenApiConfigValidator());
        RuleForEach(c => c.AllowedOrigins).NotEmpty();
    }
}

internal sealed class OpenApiConfigValidator : AbstractValidator<OpenApiConfig>
{
    public OpenApiConfigValidator()
    {
        RuleFor(c => c.Version).NotEmpty();
        RuleFor(c => c.Title).NotEmpty();
        RuleFor(c => c.Description).NotEmpty();
    }
}
