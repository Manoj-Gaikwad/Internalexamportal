using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Services
{
    public interface IRollNumberingService
    {
        Task<string> GetNextRollNumber();
        Task<bool> RevertRollNumberCount();
    }
}
