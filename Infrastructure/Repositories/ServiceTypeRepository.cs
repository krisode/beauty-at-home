using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Implements;

namespace Infrastructure.Repositories
{
    public interface IServiceTypeRepository : IRepository<ServiceType, int>
    {

    }
    class ServiceTypeRepository : Repository<ServiceType, int>, IServiceTypeRepository
    {
        public ServiceTypeRepository(BeautyServiceProviderContext dbContext) : base(dbContext)
        {

        }
    }
}
