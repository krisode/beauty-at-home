using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IAccountService : IBaseService<Account>
    {
        Account GetByEmail(string email);
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
            _iRepository.Delete(where);
        }

        public Account GetByEmail(string email)
        {
            var accountQueryList = _iRepository.GetQueryList(acc => acc.Email == email);
            return accountQueryList.Any() ? accountQueryList.First() : null;
        }

        public Account GetById(int id)
        {
            return _iRepository.GetById(id);
        }

        public void Update(Account entity)
        {
            _iRepository.Update(entity);
        }
        public async Task<bool> Save()
        {
            return await _iUnitOfWork.Save();
        }
    }
}
