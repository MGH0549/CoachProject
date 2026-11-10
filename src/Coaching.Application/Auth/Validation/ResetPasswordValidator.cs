using FluentValidation;

namespace Coaching.Application.Auth;

public sealed class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Token).NotEmpty().MaximumLength(4096);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(12).MaximumLength(128);
    }
}
