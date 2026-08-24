using FluentValidation;
using Internalexamportal.Core.Features.LoginUser;

namespace Internalexamportal.Core.Features.GenerateJwtToken
{
    /// <summary>
    /// Validator class to validate login user view model.
    /// </summary>
    public class GenerteJwtTokenModelValidator : AbstractValidator<LoginUserModel>
    {
        public GenerteJwtTokenModelValidator()
        {
            // username cannot be empty.
            RuleFor(user => user.UserName).NotEmpty();

            // password cannot be empty.
            RuleFor(user => user.Password).NotEmpty();
        }
    }
}