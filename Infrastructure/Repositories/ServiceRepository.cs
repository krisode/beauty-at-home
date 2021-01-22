using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Implements;

namespace Infrastructure.Repositories
{
    public interface IServiceRepository : IRepository<Service>
    {

    }
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            // test
            // test 2
        }
    }
}
