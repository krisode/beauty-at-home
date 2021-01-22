﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IService<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(long Id);
        IEnumerable<T> GetEnumAll();
        IEnumerable<T> GetEnumAll(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetEnumList(Expression<Func<T, bool>> where);
        IQueryable<T> GetQueryList(Expression<Func<T, bool>> where);
        IQueryable<T> GetQueryList(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        Task<bool> Save();
    }
}
