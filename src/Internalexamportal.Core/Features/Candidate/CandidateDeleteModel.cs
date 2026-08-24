using AutoMapper;
using Internalexamportal.DataAccessLayer.Contracts;
using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Candidate
{
    public class CandidateDeleteModel : IRequest<CandidateDeleteResult>
    {
        public string Id { get; set; }
    }

    public class CandidateDeleteResult
    {
        //public bool IsDelete { get; set; }
        //public UserPersonalDetail userPersonalDetail { get; set; }
    }

    public class CandidateDeleteHandler : IRequestHandler<CandidateDeleteModel, CandidateDeleteResult>
    {
        private readonly IRepository<UserPersonalDetail> _userpersonaldetail;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
      

        public CandidateDeleteHandler(
            IMapper mapper,
            IRepository<UserPersonalDetail> userpersonaldetail,
               UserManager<User> userManager)
        {
            _mapper = mapper;
            _userpersonaldetail = userpersonaldetail;
            _userManager = userManager;
        }
        public async Task<CandidateDeleteResult> Handle(CandidateDeleteModel deleteModel, CancellationToken cancellationToken)
        {
            CandidateDeleteResult result = new CandidateDeleteResult();
            
            var user = await _userManager.FindByIdAsync(deleteModel.Id);
            if (user != null)
            {
                var appendString = DateTime.UtcNow.ToString("dd_MM_yyyy_HH_mm_ss");
                //var appendString = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
                user.IsDelete = true;
                user.UserName = user.UserName + appendString;
                user.Email = user.Email + appendString;
                await _userManager.UpdateAsync(user);
            }
            return result;
        }
    }
}
