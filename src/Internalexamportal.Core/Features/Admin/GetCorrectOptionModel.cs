using AutoMapper;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin
{
    public class GetCorrectOptionModel :IRequest<ICollection<GetCorrectOptionResult>>
    {
        public int Id { get; set; }
        public string Option { get; set; }
    }

    public class GetCorrectOptionResult
{
        public int Id { get; set; }
        public string Option { get; set; }
    }


    public class GetCorrectOptionHandler : IRequestHandler<GetCorrectOptionModel, ICollection<GetCorrectOptionResult>>
    {
        private readonly IRepository<CorrectOption> _diffManager;
        private readonly IMapper _mapper;
        public GetCorrectOptionHandler(IRepository<CorrectOption> diffManager, IMapper mapper)
        {
            _diffManager = diffManager;
            _mapper = mapper;
        }

        public async Task<ICollection<GetCorrectOptionResult>> Handle(GetCorrectOptionModel request, CancellationToken cancellationToken)
        {
            var options = await _diffManager.FindAll()
                                            .ToListAsync(cancellationToken); // execute query asynchronously

            var mapped = _mapper.Map<List<GetCorrectOptionResult>>(options);
            return mapped;
        }

    }
}
