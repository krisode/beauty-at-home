using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IAccountService
    {
        void Add(Account entity);
        Account GetByEmail(string email);
        Task<bool> Save();
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

        public Account GetByEmail(string email)
        {
            var accountQueryList = _iRepository.GetQueryList(acc => acc.Email == email);
            return accountQueryList.Any() ? accountQueryList.First() : null;
        }

        public async Task<bool> Save()
        {
            return await _iUnitOfWork.Save();
        }
    }
}
