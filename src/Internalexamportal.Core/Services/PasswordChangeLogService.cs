using Internalexamportal.DataAccessLayer.Contracts;
using Internalexamportal.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internalexamportal.Core.Services
{
    public class PasswordChangeLogService : IPasswordChangeLogService
    {
        private readonly IRepository<User> _userManager;

        public  PasswordChangeLogService(
            IRepository<User> userPassword
            )
        {
            _userManager = userPassword;
        }

        public void SaveUpdatedPasswordChangeHistory(string userId)
        {
            _userManager.Update(new User
            {
                Id = userId
                //UserId = userId;
            });
            _userManager.SaveChanges();
        }
    }
    
}
