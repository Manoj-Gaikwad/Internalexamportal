using FluentValidation;

namespace Internalexamportal.Core.Features.ConfirmEmail
{
    public class ConfirmEmailModelValidator: AbstractValidator<ConfirmEmailModel>
    {
        public ConfirmEmailModelValidator()
        {
            // userId cannot be empty.
            RuleFor(model => model.UserId).NotEmpty();

            // EmailConfirmationCode cannot be empty.
            RuleFor(model => model.EmailConfirmationCode).NotEmpty();
        }
    }
}
