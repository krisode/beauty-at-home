using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ApplicationCore.Interfaces
{
    public interface IRepository<T>
    {
        T GetById(int id);

        T Get(Expression<Func<T, bool>> where);

        void Update(T entity);

        void Add(T entity);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> where);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);

        IEnumerable<T> GetList(Expression<Func<T, bool>> where);

        IQueryable<T> _GetList(Expression<Func<T, bool>> where);

        IQueryable<T> _GetList(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);

    }
}
