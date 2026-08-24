using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.LogoutUser
{
    //Model
    public class LogoutUserModel : IRequest<Unit>
    {
    }

    //Handler
    public class LogoutUserHandler : IRequestHandler<LogoutUserModel, Unit>
    {
        private readonly SignInManager<User> _signInManager;

        public LogoutUserHandler(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<Unit> Handle(LogoutUserModel request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            return Unit.Value;
        }
    }
}
