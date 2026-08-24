using System.Linq;
using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternalExamportal.DataAccessLayer.Contracts
{
    public class DeleteRepository<T> : IDeleteRepository<T> where T : class
    {
        internal InternalExamportalContext _dbContext;
        internal DbSet<T> _dbSet;
        public DeleteRepository(InternalExamportalContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();

        }

        public IQueryable<T> FindAll()
        {
            return _dbSet.SkipDeleted();
        }

        //public Subject FindById(int Id)
        //{
        //    return _dbContext.Subject.FirstOrDefault(prop => prop.Id == Id);
        //}

    }
    public static class DbExtension
    {
        public static IQueryable<T> SkipDeleted<T>(this IQueryable<T> input)
        {
            return input.Where(p => EF.Property<bool>(p, "IsDeleted") == false);
        }
    }

}

