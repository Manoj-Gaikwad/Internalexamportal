using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Services
{
	public interface IClientManagerService
	{
		Task<Tuple<int, bool>> GetClientId();
	}
}
