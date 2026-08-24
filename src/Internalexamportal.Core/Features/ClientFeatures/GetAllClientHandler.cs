using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.ClientFeatures
{
    public class GetAllClientModel : IRequest<GetAllClientResult>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SearchTerm { get; set; }
        public string SortingColumn { get; set; }
        public string SortingDirection { get; set; }
    }
    public class GetAllClientResult
    {
        public List<ClientGridModel> Clients { get; set; }
        public int TotalCount { get; set; }
    }

    public class ClientGridModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
    public class GetAllClientHandler : IRequestHandler<GetAllClientModel, GetAllClientResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllClientHandler(IMapper mapper, InternalExamportalContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetAllClientResult> Handle(GetAllClientModel request, CancellationToken cancellationToken)
            {
            var result = new GetAllClientResult();

           var clients = _dbContext.Client;

            result.Clients = _mapper.Map<List<ClientGridModel>>(await Paginate(clients, request));

            result.TotalCount = clients.Count();

            return result;
        }


        private async Task<List<Client>> Paginate(
          IQueryable<Client> clients,
          GetAllClientModel model)
        {
            //search
            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
                clients = SearchClients(clients, model.SearchTerm);

            //sort
            if (!string.IsNullOrWhiteSpace(model.SortingColumn) &&
               !string.IsNullOrWhiteSpace(model.SortingDirection))
            {
                clients = SortClients(clients, model.SortingColumn, model.SortingDirection);
            }

            if (model.PageNumber == 0) model.PageNumber = 1;

            //paginate
            return await clients
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToListAsync();
        }

        private IQueryable<Client> SearchClients(
            IQueryable<Client> clients,
            string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return clients
                .Where(c => c.Name.StartsWith(searchTerm)
                    || c.Email.StartsWith(searchTerm)
                    || c.Phone.StartsWith(searchTerm));
        }

        private IQueryable<Client> SortClients(
           IQueryable<Client> clients,
           string sortColumn,
           string sortDirection)
        {
            switch (sortColumn)
            {

                case "name":
                    clients = (sortDirection == "desc") ?
                    clients.OrderByDescending(s => s.Name) :
                    clients.OrderBy(s => s.Name);
                    break;
                case "email":
                    clients = (sortDirection == "desc") ?
                    clients.OrderByDescending(s => s.Email) :
                    clients.OrderBy(s => s.Email);
                    break;
                case "phone":
                    clients = (sortDirection == "desc") ?
                    clients.OrderByDescending(s => s.Phone) :
                    clients.OrderBy(s => s.Phone);
                    break;

            }

            return clients;
        }
    }

}
