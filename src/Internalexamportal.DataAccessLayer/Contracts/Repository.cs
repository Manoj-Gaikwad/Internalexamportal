using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Internalexamportal.DataAccessLayer.Contracts
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        internal DbSet<T> _dbSet;
        internal DbContext _dbContext;

        /// <summary>
        /// Initialize a new repository.
        /// </summary>
        /// <param name="context">Database Context to access dbset for given entity type T.</param>
        public Repository(DbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }

        ///// <summary>
        ///// Initialize a new repository with default dbContext (CloudGavelContext). 
        ///// </summary>
        //public Repository(DbContext context)
        //{
        //    _dbContext = new FusionstakContext();
        //    _dbSet = _dbContext.Set<T>();
        //}

        /// <summary>
        /// Gets all entities T where predicate returns true.
        /// </summary>
        /// <param name="predicate">Search criteria as predicate.</param>
        /// <returns>Queryable list of Entities</returns>
        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        /// <summary>
        /// Finds an entity with the given primary key values. If an entity with the given primary 
        /// key values exists in the context, then it is returned immediately without making a 
        /// request to the store. Otherwise, a request is made to the store for an entity with the 
        /// given primary key values and this entity, if found, is attached to the context and 
        /// returned. If no entity is found in the context or the store, then null is returned.
        /// </summary>
        /// <param name="keyValues">Key values.</param>
        /// <returns>Entity with the given primary key.</returns>
        public virtual T Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        /// <summary>
        /// Gets all entities T.
        /// </summary>
        /// <returns>Queryable list of entities T.</returns>
        public virtual IQueryable<T> FindAll()
        {

            //return _dbSet.Where(p => EF.Property<bool>(p, "IsDeleted") == false); 
            return _dbSet;
            
        }

        /// <summary>
        /// Adds new entity to the database.
        /// </summary>
        /// <param name="newEntity">entity to add.</param>
        public virtual void Add(T newEntity)
        {
            _dbSet.Add(newEntity);
        }

        /// <summary>
        /// Add IEnumerable colletion of entities T.
        /// </summary>
        /// <param name="newEntities">Enumerable Collection of new entites to add.</param>
        public virtual void AddRange(IEnumerable<T> newEntities)
        {
            _dbSet.AddRange(newEntities);
        }

        /// <summary>
        /// Marks the given entity as Deleted such that it will be deleted from the database when 
        /// SaveChanges is called. Note that the entity must exist in the context in some other 
        /// state before this method is called.
        /// </summary>
        /// <param name="entity">Entity to be added to database.</param>
        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        //public virtual void Deleted(T entity)
        //{
        //    _dbSet.Add(entity);
        //}

        /// <summary>
        /// Puts the entity in  modified state so that it will be updated in the database when 
        /// SaveChanges is called. Note that the entity must exist in the context in some other 
        /// state before this method is called.
        /// </summary>
        /// <param name="entity">Entity to be updated in database.</param>
        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Deletes/Removes enumerable colletion of entities T for database.
        /// </summary>
        /// <param name="entities">Enumerable list of entities to be deleted</param>
        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
        
        //Saves changes in the database
        public virtual void SaveChanges() {
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// Saves all changes made to context to dispose the respository.
        /// </summary>
        public virtual void Dispose()
        {
            _dbContext.SaveChanges();
        }

       
    }
}
