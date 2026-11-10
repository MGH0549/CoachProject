using FluentValidation;

namespace Coaching.Application.Auth;

public sealed class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(12).MaximumLength(128);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.WhatsAppNumber).Matches(@"^\+?[0-9]{8,15}$");
        RuleFor(x => x.Consent).Equal(true).WithMessage("يجب الموافقة على معالجة البيانات وسياسة الخصوصية");
    }
}
