using FluentValidation;

namespace Coaching.Application.Auth;

public sealed class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordValidator() => RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
}
