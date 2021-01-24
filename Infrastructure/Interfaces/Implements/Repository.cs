using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Interfaces.Implements
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private BeautyServiceProviderContext _context;
        private DbSet<T> _dbSet;
        protected IDatabaseFactory _dbFactory
        {
            get;
            private set;
        }

        private BeautyServiceProviderContext BeautyServiceProviderContext
        {
            get
            {
                return _context ?? (_context = _dbFactory.Init());
            }
        }

        protected Repository(IDatabaseFactory databaseFactory)
        {
            _dbFactory = databaseFactory;
            _dbSet = BeautyServiceProviderContext.Set<T>();
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return _dbSet.Find(where);
        }

        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        } 

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var lstRemove = _dbSet.Where(where);
            _dbSet.RemoveRange(lstRemove);
        }

        public virtual IEnumerable<T> GetEnumAll()
        {
            return _dbSet.ToList();
        }

        public virtual IEnumerable<T> GetEnumAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> lstGet = _dbSet;
            foreach (var expression in includes)
            {
                lstGet = lstGet.Include(expression);
            }

            return lstGet;
        } 

        public virtual IEnumerable<T> GetEnumList(Expression<Func<T, bool>> where)
        {
            var lstGet = _dbSet.Where(where);
            return lstGet.ToList();
        }

        public virtual IQueryable<T> GetQueryList(Expression<Func<T, bool>> where)
        {
            var lstGet = _dbSet.Where(where);
            return lstGet;
        }

        public virtual IQueryable<T> GetQueryList(params Expression<Func<T, bool>>[] where)
        {
            IQueryable<T> lstGet = null;
            foreach (var expression in where)
            {
                lstGet = _dbSet.Where(expression);
            }
            return lstGet;
        }

        public virtual IQueryable<T> GetQueryList(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> lstGet = _dbSet.Where(where);
            foreach (var expression in includes)
            {
                lstGet = lstGet.Include(expression);
            }

            return lstGet;
        }
    }
}
