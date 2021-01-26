using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Implements;

namespace Infrastructure.Repositories
{
    public interface IServiceRepository : IRepository<Service, int>
    {

    }
    public class ServiceRepository : Repository<Service, int>, IServiceRepository
    {
        public ServiceRepository(BeautyServiceProviderContext dbContext) : base(dbContext)
        {

        }
    }
}
