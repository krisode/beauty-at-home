using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Implements;

namespace Infrastructure.Repositories
{
    public interface IImageRepository : IRepository<Image, int>
    {

    }
    public class ImageRepository : Repository<Image, int>, IImageRepository
    {
        public ImageRepository(BeautyServiceProviderContext dbContext) : base(dbContext)
        {

        }
    }
}
