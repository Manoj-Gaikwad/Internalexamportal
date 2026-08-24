using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Threading;

namespace Internalexamportal.Core.Features.ConfirmEmail
{

    //Model
    public class ConfirmEmailModel : IRequest<IdentityResult>
    {
        public string UserId { get; set; }
        public string EmailConfirmationCode { get; set; }
    }

    //Handler
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailModel, IdentityResult>
    {
        private readonly UserManager<User> _userManager;
        public ConfirmEmailHandler(
            UserManager<User> userManager
        )
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> Handle(ConfirmEmailModel message, CancellationToken token)
        {
            //Instantiate the return type
            var identityResult = new IdentityResult();

            //Check if the user with id in parameter actually exist
            var user = await _userManager.FindByIdAsync(message.UserId);

            //If user exists, check if confirmation code matches as well.
            if (user != null)
            {
                //Check if the email confirmation code match.
                identityResult = await _userManager.ConfirmEmailAsync(user, message.EmailConfirmationCode);
            }

            return identityResult;
        }
    }
}
