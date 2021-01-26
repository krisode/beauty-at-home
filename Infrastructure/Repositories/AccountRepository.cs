using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Implements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface IAccountRepository : IRepository<Account, int>
    {
    }

    public class AccountRepository : Repository<Account, int>, IAccountRepository
    {
        public AccountRepository(BeautyServiceProviderContext dbContext) : base(dbContext)
        {
        }
    }
}
