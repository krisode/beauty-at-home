using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Implements;

namespace Infrastructure.Repositories
{
    public interface IAddressRepository : IRepository<Address, int>
    {

    }
    public class AddressRepository : Repository<Address, int>, IAddressRepository
    {
        public AddressRepository(BeautyServiceProviderContext dbContext) : base(dbContext)
        {

        }
    }
}
