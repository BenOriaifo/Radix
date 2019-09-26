using Microsoft.EntityFrameworkCore;
using Radix.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Radix.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
        }

        public bool Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            return true;
        }

        public int AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
            return entities.Count();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public virtual TEntity Get(long Id)
        {
            return Context.Set<TEntity>().Find(Id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public bool Update(long Id, TEntity entity)
        {
            TEntity ExistingEntity = Context.Set<TEntity>().Find(Id);
            Context.Entry(ExistingEntity).CurrentValues.SetValues(entity);
            return true;
        }

        public bool Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            return true;
        }

        public int RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
            return entities.Count();
        }

        public long Count()
        {
            return Context.Set<TEntity>().Count();
        }
    }
}
