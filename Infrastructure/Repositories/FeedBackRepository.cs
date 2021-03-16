using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Implements;


namespace Infrastructure.Repositories
{
    public interface IFeedBackRepository : IRepository<FeedBack, int>
    {

    }
    public class FeedBackRepository : Repository<FeedBack, int>, IFeedBackRepository
    {
        public FeedBackRepository(BeautyServiceProviderContext dbContext) : base(dbContext)
        {

        }
    }
}
