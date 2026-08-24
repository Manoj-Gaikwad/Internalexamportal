using AutoMapper;
using Internalexamportal.Core.Configurations;
using Internalexamportal.Core.Features.CreateUser;
using Internalexamportal.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;

namespace Internalexamportal.Core.Commons
{
    public class DefaultUserCreatorService: IDefaultUserCreatorService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly DefaultUserConfiguration _defaultUserConfiguration;

        /// <summary>
        /// Creates default user on pre application setup
        /// </summary>
        /// <param name="mapper">Automapper mapper class.</param>
        /// <param name="userManager">Identity user manager</param>
        /// <param name="defaultUserConfiguration">Default user configuration from appsettings.</param>
        public DefaultUserCreatorService(
            IMapper mapper,
            UserManager<User> userManager,
            IOptions<DefaultUserConfiguration> defaultUserConfiguration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _defaultUserConfiguration = defaultUserConfiguration.Value;
        }

        /// <summary>
        /// Creates default user using useremail and password provided in appsetting configuraiton.
        /// </summary>
        public void CreateUser()
        {
            // do not proceed if
            // 1. DefaultMasterUserEmail is not provided.
            // 2. DefaultMasterPassword is not provided.
            Console.WriteLine("inside create user");
            if (string.IsNullOrWhiteSpace(_defaultUserConfiguration.DefaultMasterUserName)
                || string.IsNullOrWhiteSpace(_defaultUserConfiguration.DefaultMasterPassword)) return;

            // find user in database 
            var user = _userManager.FindByEmailAsync(_defaultUserConfiguration.DefaultMasterUserName).Result;

            // if user is not in database
            if (user == null)
            {

                //TODO: refactor and make a service 
                // use the same service in create user handler and here to keep code DRY  

                // setup create user model
                var createUserModel = new CreateUserModel
                {
                    UserName = _defaultUserConfiguration.DefaultMasterUserName,
                    Password = _defaultUserConfiguration.DefaultMasterPassword,
                    Email = _defaultUserConfiguration.DefaultMasterEmail
                };

                // map model to entity.
                user = _mapper.Map<User>(createUserModel);

                //Set email confirmed property to true. By Default. 
                user.EmailConfirmed = true;

                // create the user in database
                var identitResult = _userManager
                    .CreateAsync(user, _defaultUserConfiguration.DefaultMasterPassword).Result;

                // throw error if not successful to create new user.
                if (!identitResult.Succeeded)
                {
                    throw new Exception("Could not create default user.");
                }
            }

            // check if user has admin role
            if (!_userManager.IsInRoleAsync(user, GlobalConstants.SuperAdminRoleName).Result)
            {
                //if not assign admin role
                var identityResult = _userManager
                    .AddToRoleAsync(user, GlobalConstants.SuperAdminRoleName).Result;

                if (!identityResult.Succeeded)
                {
                    throw new Exception("Could not assign admin role to default user.");
                }
            }
        }

    }
}
