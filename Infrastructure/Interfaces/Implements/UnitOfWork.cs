using Infrastructure.Contexts;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BeautyServiceProviderContext _context;

        public UnitOfWork(BeautyServiceProviderContext context)
        {
            _context = context;
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
