using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Interfaces
{
    public interface IRepository<T>
    {
        T GetById(int id);

        T Get(Expression<Func<T, bool>> where);

        void Update(T entity);

        void Add(T entity);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> where);

        IEnumerable<T> GetEnumAll();

        IEnumerable<T> GetEnumAll(params Expression<Func<T, object>>[] includes);

        IEnumerable<T> GetEnumList(Expression<Func<T, bool>> where);

        IQueryable<T> GetQueryList(Expression<Func<T, bool>> where);

        IQueryable<T> GetQueryList(params Expression<Func<T, bool>>[] where);

        IQueryable<T> GetQueryList(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);

    }
}
