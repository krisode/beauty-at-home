using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
