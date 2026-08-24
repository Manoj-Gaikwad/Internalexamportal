using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Threading;

namespace Internalexamportal.Core.Features.CreateUser
{
    public class CheckIfUserNameExistsRequest : IRequest<CheckIfUserNameExistsResponse>
    {
        public string UserName { get; set; }
    }

    public class CheckIfUserNameExistsResponse
    {
        public bool UserNameExists { get; set; }
    }

    public class CheckIfUserNameExistsHandler : IRequestHandler<CheckIfUserNameExistsRequest, CheckIfUserNameExistsResponse>
    {
        private readonly UserManager<User> _userManager;

        public CheckIfUserNameExistsHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CheckIfUserNameExistsResponse> Handle(CheckIfUserNameExistsRequest message, CancellationToken token)
        {
            //Find the username
            var user = await _userManager.FindByNameAsync(message.UserName);

            return new CheckIfUserNameExistsResponse { UserNameExists = (user != null)};
        }
    }
}
