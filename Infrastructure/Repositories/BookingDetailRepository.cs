using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Implements;

namespace Infrastructure.Repositories
{
    public interface IBookingDetailRepository : IRepository<BookingDetail, int>
    {

    }
    public class BookingDetailRepository : Repository<BookingDetail, int>, IBookingDetailRepository
    {
        public BookingDetailRepository(BeautyServiceProviderContext dbContext) : base(dbContext)
        {

        }
    }
}
