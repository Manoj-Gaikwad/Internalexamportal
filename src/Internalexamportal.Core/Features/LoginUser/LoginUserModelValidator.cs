using FluentValidation;

namespace Internalexamportal.Core.Features.LoginUser
{
    /// <summary>
    /// Validator class to validate login user view model.
    /// </summary>
    public class LoginUserModelValidator : AbstractValidator<LoginUserModel>
    {
        public LoginUserModelValidator()
        {
            // username cannot be empty.
            RuleFor(user => user.UserName).NotEmpty();

            // password cannot be empty.
            RuleFor(user => user.Password).NotEmpty();
        }
    }
}