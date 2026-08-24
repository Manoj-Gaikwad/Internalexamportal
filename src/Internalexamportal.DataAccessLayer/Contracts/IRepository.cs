using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Internalexamportal.DataAccessLayer.Contracts
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        T Find(params object[] keyValues);
        IQueryable<T> FindAll();
        void Add(T newEntity);
        void AddRange(IEnumerable<T> newEntities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);

        void SaveChanges();

    }
}
