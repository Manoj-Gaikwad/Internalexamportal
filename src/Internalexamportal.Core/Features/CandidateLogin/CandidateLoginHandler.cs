using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Internalexamportal.Core.Configurations;
using Internalexamportal.Core.Features.GenerateJwtToken;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Internalexamportal.Core.Features.CandidateLogin
{
    public class CreateCandidateLoginModel : IRequest<CreateCandidateLoginResult>
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool RememberMe { get; set; }

    }

    public class CreateCandidateLoginResult
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public TokenResult TokenResult { get; set; }
        public bool IsLockedOut { get; set; }
        public Boolean IsSuceeded { get; set; }
        public bool IsValidCredential { get; internal set; }
    }
    public class CandidateLoginHandler : IRequestHandler<CreateCandidateLoginModel, CreateCandidateLoginResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenConfiguration _tokenConfiguration;
        private readonly IJwtSecurityTokenGenerationService _jwtSecurityTokenGenerationService;

        public CandidateLoginHandler(
                 UserManager<User> userManager,
                 IOptions<TokenConfiguration> tokenConfiguration,
                 IJwtSecurityTokenGenerationService jwtSecurityTokenGenerationService
        )
        {
            //_mapper = mapper;
            _userManager = userManager;
            _tokenConfiguration = tokenConfiguration.Value;
            _jwtSecurityTokenGenerationService = jwtSecurityTokenGenerationService;
        }
        public async Task<CreateCandidateLoginResult> Handle(CreateCandidateLoginModel message, CancellationToken token)
        {
            CreateCandidateLoginResult createCandidateLoginResult = new CreateCandidateLoginResult();

            var user = await _userManager.FindByEmailAsync(message.Email);
            var result = new CreateCandidateLoginResult();
            if(user != null)
            {
                //var isValid = true;
                createCandidateLoginResult.Id = user.Id;
                createCandidateLoginResult.Email = user.Email;
                createCandidateLoginResult.PasswordHash = user.PasswordHash;
                createCandidateLoginResult.IsSuceeded = true;
            }
            else if(user == null)
            {
                createCandidateLoginResult.IsSuceeded = false;
            } 
            

            message.RememberMe = true;

           
            var jwtSecurityToken =
               await _jwtSecurityTokenGenerationService.GetJwtSecurityToken(user, message.RememberMe, _tokenConfiguration);

            createCandidateLoginResult.TokenResult = new TokenResult()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Expiration = jwtSecurityToken.ValidTo
            };
            
            //Check if the user is Locked Out of the System
            result.IsLockedOut = (user != null) && await _userManager.IsLockedOutAsync(user);

            //Add to AccessFailedCount upon failed login for user in database. 
            if ((user != null) && (_userManager.SupportsUserLockout && await _userManager.GetLockoutEnabledAsync(user)))
            {
                //Double the lockout time on every login fail after the user gets locked out. 
                if (result.IsLockedOut)
                {
                    await _userManager.SetLockoutEndDateAsync(user, user.LockoutEnd.Value.AddMinutes(user.LockoutEnd.Value.Minute));
                }

                await _userManager.AccessFailedAsync(user);
            }

            return createCandidateLoginResult;

        }
    }
}
