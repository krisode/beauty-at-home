using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Implements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
    }

    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
