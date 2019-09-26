using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Radix.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(long Id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        bool Add(TEntity entity);
        int AddRange(IEnumerable<TEntity> entities);
        bool Update(long Id, TEntity entity);
        long Count();

        bool Remove(TEntity entity);
        int RemoveRange(IEnumerable<TEntity> entities);
    }
}
