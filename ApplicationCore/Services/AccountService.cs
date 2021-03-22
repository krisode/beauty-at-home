using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IAccountService : IService<Account, int>
    {
        Account GetByEmail(string email);
    }
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IRepository<Account, int> _iRepository;

        public AccountService(IUnitOfWork iUnitOfWork, IRepository<Account, int> iRepository)
        {
            _iUnitOfWork = iUnitOfWork;
            _iRepository = iRepository;
        }

        public async Task<Account> AddAsync(Account entity)
        {
            return await _iRepository.AddAsync(entity);
        }

        public void Delete(Account entity)
        {
            _iRepository.Delete(entity);
        }

        public async Task<Account> GetByIdAsync(int id)
        {
            return await _iRepository.GetByIdAsync(id);
        }

        public void Update(Account entity)
        {
            _iRepository.Update(entity);
        }
        public async Task<int> Save()
        {
            return await _iUnitOfWork.Save();
        }

        public Account GetByEmail(string email)
        {
            return _iRepository.GetAll(_ => _.Gallery.Images, _ => _.Addresses).Where(a => a.Email == email).FirstOrDefault();
        }

        public IQueryable<Account> GetAll(params Expression<Func<Account, object>>[] includes)
        {
            return _iRepository.GetAll(includes);
        }
    }
}
