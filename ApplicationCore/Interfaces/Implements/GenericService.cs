using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Implements
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private IUnitOfWork _iUnitOfWork;

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public T GetById(Guid Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetList(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Save()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> _GetList(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> _GetList(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }
    }
}
