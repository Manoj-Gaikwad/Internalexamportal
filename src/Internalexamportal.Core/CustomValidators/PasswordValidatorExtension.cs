using System.Linq;
using FluentValidation;
using Internalexamportal.Core.Commons;

namespace Internalexamportal.Core.CustomValidators
{
    public static class PasswordValidatorExtension{

        /// <summary>
        /// Custom validator to verify a password.
        /// This is used for view model validation only. To change identity configuration 
        /// see startup configuration in <see cref="IdentityConfigurationExtension"/>.
        /// </summary>
        /// <typeparam name="T">Object to validate</typeparam>
        /// <param name="x">A RuleBuilderInitial for string property of T</param>
        public static void MustBeValidPassword<T>(this IRuleBuilderInitial<T, string> x)
        {
            // password should have atleast 8 characters.
            x.MinimumLength(8) 
                // password must have atleat one uppercase.
                .Must(p => p.Any(char.IsUpper))
                .WithMessage("{PropertyName} must have atleast one uppercase.")
                // password must have atleast one lowercase.
                .Must(p => p.Any(char.IsLower))
                .WithMessage("{PropertyName} must have atleast one lowercase.")
                // password must have atleast one digit.
                .Must(p => p.Any(char.IsDigit))
                .WithMessage("{PropertyName} must have atleast one digit.")
                // password must have atleast one non alphanumeric character.
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("{PropertyName} must have atleast one non alpha numeric character.");
        }
    }
}