using System.Linq;
using System.Threading.Tasks;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Features.AddUser;
using Internalexamportal.Core.Features.CandidateLogin;
using Internalexamportal.Core.Features.ConfirmEmail;
using Internalexamportal.Core.Features.CreateUser;
using Internalexamportal.Core.Features.ForgotPassword;
using Internalexamportal.Core.Features.GenerateJwtToken;
using Internalexamportal.Core.Features.ImportCandidates;
using Internalexamportal.Core.Features.LogoutUser;
using Internalexamportal.Core.Features.ResetPassword;
using Internalexamportal.Core.Features.Users;
using Internalexamportal.Core.Features.ValidateVerificationCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Internalexamportal.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator) => _mediator = mediator;

        // POST api/account/register
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="createUserModel">Data to create new user</param>
        /// <returns>Action Result</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateUserModel createUserModel)
        {
            // Use Mediatr to request user creation in business logic
            var response = await _mediator.Send(createUserModel);

            // return Ok if success
            if (response.Succeeded) return Ok(response);

            // Add errors to model state with empty key
            // Empty string means generic error in model
            response.Errors
                .ToList()
                .ForEach(x => ModelState.AddModelError(string.Empty, x.Description));

            // return bad request
            return BadRequest(ModelState);
        }

        // POST api/account/register
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="AddCandidate">Data to create new user</param>
        /// <returns>Action Result</returns>
        [HttpPost("AddUser")]
        [Authorize]
        public async Task<IActionResult> AddCandidate(AddUserModel model)
        {
            // Use Mediatr to request user creation in business logic
            var response = await _mediator.Send(model);

            // return Ok if success
            if (response.Succeeded) return Ok(response);

            // Add errors to model state with empty key
            // Empty string means generic error in model
            response.Errors
                .ToList()
                .ForEach(x => ModelState.AddModelError(string.Empty, x.Description));

            // return bad request
            return BadRequest(ModelState);
        }


        //POST api/account/logout
        /// <summary>
        /// Logout user in cookies based authentication.
        /// To logout user in Jwt based authentication, 
        /// Just discard the token.
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // use Mediatr to logout user 
            await _mediator.Send(new LogoutUserModel());

            // return ok
            return Ok();
        }

        //POST api/account/token
        /// <summary>
        /// Provide the user with Jwt Token to be used by user 
        /// to access resources where authentication is necessary
        /// </summary>
        /// <param name="generateJwtTokenModel">Data to generate user token.</param>
        /// <returns>Action Result</returns>
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> Token(GenerateJwtTokenModel generateJwtTokenModel)
        {
            // use Mediatr to request Jwt token from business logic
            var result = await _mediator.Send(generateJwtTokenModel);

            if (!result.IsValidCredential)  
            {          
                ModelState.AddModelError(string.Empty, "Invalid username and password combination.");

                return BadRequest(ModelState);
            }
            if (result.IsLockedOut)
            {
                ModelState
                    .AddModelError(
                        string.Empty, 
                        "Your account has been locked out due to too many login attempts. Please contact your administrator.");

                return BadRequest(ModelState);
            }
            if (!result.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Your Account is not active. Please contact admin.");
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        //Get api/account/confirmEmail
        /// <summary>
        /// Check if the redirect url contains correct userId and code for the user.
        /// </summary>
        /// <param name="confirmEmailModel">Data to verify email of users.</param>
        /// <returns>Action Result</returns>
        [HttpGet("confirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery]ConfirmEmailModel confirmEmailModel)
        {
            var result = await _mediator.Send(confirmEmailModel);

            return Ok(result);
        }

        //Get api/account/verifyCode
        /// <summary>
        ///Verfiy code for two factor authentication.
        /// </summary>
        /// <param name="validateVerificationCodeModel">Data to verify code of users using 2FA.</param>
        /// <returns>Action Result</returns>
        [HttpPost("verifyCode")]
        [AllowAnonymous]
        public async Task<IActionResult> VerfiyCode(
            ValidateVerificationCodeModel validateVerificationCodeModel)
        {
            var result = await _mediator.Send(validateVerificationCodeModel);
            
            if (result.Succeeded) return Ok(result);

            ModelState.AddModelError(string.Empty, "Verification code did not match. ");

            return BadRequest(ModelState);
        }

        //POST api/account/forgotPassword
        /// <summary>
        ///Sends the rest password link to the user. 
        /// </summary>
        /// <param name="forgotPasswordModel">Data to verfiy user is in the database. </param>
        /// <returns>Action Result</returns>
        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            // use Mediatr to request Jwt token from business logic
            var result = await _mediator.Send(forgotPasswordModel);

            // return ok result
            return Ok(result);
        }

        //POST api/account/resetPassword
        /// <summary>
        ///Resets user password if sent code matches.
        /// </summary>
        /// <param name="resetPasswordModel">Data to verfiy user is in the database and reset their password. </param>
        /// <returns>Action Result</returns>
        [HttpPost("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            // use Mediatr to request Jwt token from business logic
            var result = await _mediator.Send(resetPasswordModel);

            // if credential is invalid return bad request
            if (!result.IsValidCredential)
            {
                return BadRequest(result);
            }

            // return ok result
            return Ok(result);
        }


        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody]
            ChangePasswordModel model)
        {
            var result = await _mediator.Send(model);
            if (!result.IsSucceeded)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        [HttpPost("CreateCandidateLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> CandidateLogin([FromBody]CreateCandidateLoginModel createCandidateLoginModel)
        {
            // use Mediatr to request Jwt token from business logic
            var result = await _mediator.Send(createCandidateLoginModel);

            if(result.IsSuceeded) return Ok(result);

            ModelState.AddModelError(string.Empty, "Register the candidate");

            return BadRequest(ModelState);

        }

        [HttpPost("checkifUserNameExists")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckifUserNameExists(CheckIfUserNameExistsRequest checkIfUserNameExists)
        {
            var response = await _mediator.Send(checkIfUserNameExists);
            return Ok(response);
        }


        [HttpPost("ImportCandidates")]
        public async Task<IActionResult> ImportCandidates([FromBody]ImportCandidatesModel model)
        {
            // Use Mediatr to request user creation in business logic
            var response = await _mediator.Send(model);

            // return response
            return Ok(response);
        }

        [HttpGet("GetUserDetails/{Id}")]
        [Authorize]
        public async Task<IActionResult> GetUserDetails([FromRoute]string Id)
        {
            var response = await _mediator.Send(new GetUserDetailsModel { Id = Id  });
            return Ok(response);

        }

        [HttpPost("UpdateProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDetailsModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpGet("ChangeUserStatus/{UserId}")]
        [Authorize]
        public async Task<IActionResult> ChangeUserStatus([FromRoute] ChangeUserStatusModel Id)
        {
            var response = await _mediator.Send(Id);
            return Ok(response);

        }
    }
}
