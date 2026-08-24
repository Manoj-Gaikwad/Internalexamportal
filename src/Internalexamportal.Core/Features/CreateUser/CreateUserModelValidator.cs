using FluentValidation;
using Internalexamportal.Core.CustomValidators;

namespace Internalexamportal.Core.Features.CreateUser
{
    /// <summary>
    /// Validator class to validate CreateUserViewModel.
    /// </summary>
    public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
    {
        public CreateUserModelValidator()
        {
            // username cannot be empty.
            RuleFor(user => user.UserName).NotEmpty();

            // validate email address.
            RuleFor(user => user.Email).EmailAddress();

            // this password validation is for view model only
            RuleFor(user => user.Password).MustBeValidPassword();

            // confirm password should match with password.
            RuleFor(user => user.ConfirmPassword)
                .Must((user, confirmPassword) => confirmPassword == user.Password)
                .WithMessage("Password doesnot match.");

            // user should agree to terms
            RuleFor(user => user.AgreedToTerms)
                .Equal(true)
                .WithMessage("Terms of condition must be checked.");

        }
    }
}
