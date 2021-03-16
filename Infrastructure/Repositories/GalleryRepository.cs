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
    public interface IGalleryRepository : IRepository<Gallery, int>
    {

    }
    public class GalleryRepository : Repository<Gallery, int>, IGalleryRepository
    {
        public GalleryRepository(BeautyServiceProviderContext dbContext) : base(dbContext)
        {

        }
    }
}
