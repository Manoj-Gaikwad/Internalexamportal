using InternalExamportal.DataAccessLayer.Entities;
using System.Linq;

namespace InternalExamportal.DataAccessLayer.Contracts
{
    public interface IDeleteRepository<T>
    {
        IQueryable<T> FindAll();
      //  Subject FindById(int Id);


    }
}