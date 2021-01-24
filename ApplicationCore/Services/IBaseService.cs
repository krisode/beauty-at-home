using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IBaseService<T>
    {
        void Add(T entity);
        T GetById(int Id);
        void Update(T entity);
        void Delete(Expression<Func<T, bool>> where);
        Task<bool> Save();
    }
}
