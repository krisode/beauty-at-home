using Infrastructure.Contexts;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _dbFactory;

        private BeautyServiceProviderContext _context;

        public UnitOfWork(IDatabaseFactory databaseFactory) 
        {
            _dbFactory = databaseFactory;
        }

        private BeautyServiceProviderContext BeautyServiceProviderContext
        {
            get
            {
                return _context ?? (_context = _dbFactory.Init());
            }
        }

        public async Task<bool> Save()
        {
            return await _context.Commit();
        }
    }
}
