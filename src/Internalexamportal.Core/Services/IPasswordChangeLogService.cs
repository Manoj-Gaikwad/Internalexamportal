using System;
using System.Collections.Generic;
using System.Text;

namespace Internalexamportal.Core.Services
{
    public interface IPasswordChangeLogService
    {
        void SaveUpdatedPasswordChangeHistory(string userId);
    }
}
