using FluentValidation;
using Microsoft.OpenApi.Models;

namespace LooperCorp.AppServer;

internal sealed class AppConfig
{
    public OpenApiInfo OpenApi { get; set; } = null!;
    public CookieSettings Cookie { get; set; } = null!;

    public static bool IsValid(AppConfig config)
    {
        var validator = new AppConfigValidator();
        var results = validator.Validate(config);
        if (!results.IsValid)
        {
            foreach (var error in results.Errors)
            {
                Console.Error.WriteLine(error.ErrorMessage);
            }
        }

        return results.IsValid;
    }
}

internal sealed class CookieSettings
{
    public string Name { get; set; } = null!;
    public int ExpireHours { get; set; } = 12;
}

internal sealed class AppConfigValidator : AbstractValidator<AppConfig>
{
    public AppConfigValidator()
    {
        RuleFor(c => c.OpenApi)
            .NotNull()
            .SetValidator(new OpenApiConfigValidator());

        RuleFor(c => c.Cookie)
            .NotNull()
            .SetValidator(new CookieSettingsValidator());
    }
}

internal sealed class OpenApiConfigValidator : AbstractValidator<OpenApiInfo>
{
    public OpenApiConfigValidator()
    {
        RuleFor(c => c.Version).NotEmpty();
        RuleFor(c => c.Title).NotEmpty();
        RuleFor(c => c.Description).NotEmpty();
    }
}

internal sealed class CookieSettingsValidator : AbstractValidator<CookieSettings>
{
    public CookieSettingsValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage($"{nameof(CookieSettings.Name)} cannot be empty");
        RuleFor(c => c.ExpireHours).GreaterThan(0).WithMessage($"{nameof(CookieSettings.ExpireHours)} must be greater than 0");
    }
}
