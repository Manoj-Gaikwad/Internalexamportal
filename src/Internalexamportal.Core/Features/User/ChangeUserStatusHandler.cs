using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Users
{
	public class ChangeUserStatusModel : IRequest<ChangeUserStatusResult>
	{
		public string UserId { get; set; }
	}

	//Result
	public class ChangeUserStatusResult
	{

	}
	public class ChangeUserStatusHandler : IRequestHandler<ChangeUserStatusModel, ChangeUserStatusResult>
	{
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;
		private readonly InternalExamportalContext _context;
		public ChangeUserStatusHandler(UserManager<User> userManager,
			IMapper mapper,
			InternalExamportalContext context)
		{
			_userManager = userManager;
			_mapper = mapper;
			_context = context;
		}
		public async Task<ChangeUserStatusResult> Handle(ChangeUserStatusModel request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(request.UserId);
			user.IsActive = !user.IsActive;
			await _userManager.UpdateAsync(user);
			await _context.SaveChangesAsync(cancellationToken);
			return new ChangeUserStatusResult();
		}
	}
}
