using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IAccountService : IService<Account>
    {

    }
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IAccountRepository _iRepository;

        public AccountService(IUnitOfWork iUnitOfWork, IAccountRepository iRepository)
        {
            _iUnitOfWork = iUnitOfWork;
            _iRepository = iRepository;
        }

        public void Add(Account entity)
        {
            _iRepository.Add(entity);
        }

        public void Delete(Expression<Func<Account, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Account GetById(long Id)
        {
            return _iRepository.GetById(Id);
        }

        public IEnumerable<Account> GetEnumAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Account> GetEnumAll(params Expression<Func<Account, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Account> GetEnumList(Expression<Func<Account, bool>> where)
        {
            return _iRepository.GetEnumList(where);
        }

        public IQueryable<Account> GetQueryList(Expression<Func<Account, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Account> GetQueryList(Expression<Func<Account, bool>> where, params Expression<Func<Account, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Account entity)
        {
            throw new NotImplementedException();
        }
    }
}
