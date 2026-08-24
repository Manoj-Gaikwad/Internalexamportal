using System;
using System.Threading.Tasks;
using Internalexamportal.Core.Features.GetCurrentUserName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Internalexamportal.Web.Controllers
{
    [Route("api/[controller]")]
	public class HomeController : Controller
	{
		private readonly IHostingEnvironment _env;
		private readonly IMediator _mediator;
		public HomeController(IHostingEnvironment env, IMediator mediator)
		{
			_env = env;
			_mediator = mediator;
        }

		/// <summary>
		/// User this method to check if a user is authenticated.
		/// </summary>
		/// <returns>String "You are authenticated." if user is authenticated.</returns>
		[HttpGet("checkauthentication")]
		public async Task<String> CheckAuthentication()
		{
			// this is just an example to demonstrate 
			// how to get username/userid outside contoller.
			// if you need to get username/id inside controller
			// simply use: var username = User.Identity.GetUserName();
			// using Fusionstak.Core.Commons.IdentityExtensions
			
			return $"Hi! {await _mediator.Send(new GetUserNameModel())}. You are authenticated.";
		}

		/// <summary>
		/// Use this method to test if API is live.
		/// </summary>
		/// <returns>String "I am alive." if alive.</returns>
		[HttpGet("live")]
		[AllowAnonymous]
		public string Live() => "I am alive.";

	    /// <summary>
		/// Use this method to test if XSFR is working.
		/// </summary>
		/// <returns>String "Valid Xsfr Token." if success.</returns>
		[HttpGet("checkxsfr")]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public string CheckXsfr() => "Valid Xsfr Token.";

	    /// <summary>
		/// Use this method to check current environment.
		/// </summary>
		/// <returns>string with current environment name.</returns>
		[HttpGet("environment")]
		[AllowAnonymous]
		public string Environment() => $"Environment Name : {_env.EnvironmentName}";
	}
}
