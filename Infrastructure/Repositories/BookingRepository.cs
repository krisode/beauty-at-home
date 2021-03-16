using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Implements;

namespace Infrastructure.Repositories
{
    public interface IBookingRepository : IRepository<Booking, int>
    {

    }
    public class BookingRepository : Repository<Booking, int>, IBookingRepository
    {
        public BookingRepository(BeautyServiceProviderContext dbContext) : base(dbContext)
        {

        }
    }
}
