using FluentValidation;
using MyFinanceTracker.Application.Features.Authentication.DTOs;

namespace MyFinanceTracker.Application.Features.Authentication.Validators
{
    internal sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}