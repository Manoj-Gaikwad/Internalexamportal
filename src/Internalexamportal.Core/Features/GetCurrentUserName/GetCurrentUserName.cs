using Internalexamportal.Core.Commons;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.GetCurrentUserName
{
    //Model
    public class GetUserNameModel : IRequest<string> { }

    //Handler
    public class GetCurrentUserName : IRequestHandler<GetUserNameModel, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCurrentUserName(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<string> Handle(GetUserNameModel request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var userName = user?.Identity?.Name ?? "Anonymous";

            return Task.FromResult(userName);
        }
    }
}
