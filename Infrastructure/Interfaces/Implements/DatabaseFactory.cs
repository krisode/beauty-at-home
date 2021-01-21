using Infrastructure.Contexts;

namespace Infrastructure.Interfaces.Implements
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private BeautyServiceProviderContext _context;

        public BeautyServiceProviderContext Init()
        {
            return _context ?? (_context = new BeautyServiceProviderContext());
        }

        protected override void DisposeCore()
        {
            if(_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
