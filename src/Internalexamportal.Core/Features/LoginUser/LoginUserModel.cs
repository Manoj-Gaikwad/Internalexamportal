using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Internalexamportal.Core.Features.LoginUser
{
    public class LoginUserModel : IRequest<SignInResult>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}