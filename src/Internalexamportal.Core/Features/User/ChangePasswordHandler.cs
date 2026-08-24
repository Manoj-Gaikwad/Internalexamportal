using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Users
{
    public class ChangePasswordModel : IRequest<ChangePasswordResult>
    {
        public string UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangePasswordResult
    {
        public bool IsSucceeded { get; set; }
        public string ErrorMessage { get; set; }

        public ChangePasswordResult(bool isSucceeded, string message)
        {
            IsSucceeded = isSucceeded;
            ErrorMessage = message;
        }

    }
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordModel, ChangePasswordResult>
    {
        private readonly UserManager<User> _userManager;

        public ChangePasswordHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ChangePasswordResult> Handle(
            ChangePasswordModel request, 
            CancellationToken cancellationToken)
        {
            var passwordResult = new ChangePasswordResult(false, null);

            if (string.IsNullOrEmpty(request.UserId))
            {
                passwordResult.ErrorMessage = "Invalid User Details.";
                return passwordResult;
            }
            
            User currentUser = await _userManager
                .FindByIdAsync(request.UserId);

            if (currentUser == null)
            {
                passwordResult.ErrorMessage = "User does not exist.";
                return passwordResult;
            }

            bool isValidOldPassword = await _userManager
                .CheckPasswordAsync(currentUser, request.CurrentPassword);

            if (!isValidOldPassword)
            {
                passwordResult.ErrorMessage = "Current password is not valid.";
                return passwordResult;
            }

            var resetToken = await _userManager
                .GeneratePasswordResetTokenAsync(currentUser);

            var result = await _userManager
                .ResetPasswordAsync(currentUser, resetToken, request.NewPassword);

            if (result.Succeeded)
            {
                passwordResult.IsSucceeded = true;
                passwordResult.ErrorMessage = null;
                return passwordResult;
            }

            return passwordResult;
        }
    }
}
