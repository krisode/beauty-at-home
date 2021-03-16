using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IBookingService : IService<Booking, int>
    {

    }
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IRepository<Booking, int> _iRepository;

        public BookingService(IUnitOfWork unitOfWork, IRepository<Booking, int> iRepository)
        {
            _iUnitOfWork = unitOfWork;
            _iRepository = iRepository;
        }

        public async Task<Booking> AddAsync(Booking entity)
        {
            return await _iRepository.AddAsync(entity);
        }

        public void Delete(Booking entity)
        {
            _iRepository.Delete(entity);
        }
        
        public IQueryable<Booking> GetAll(params Expression<Func<Booking, object>>[] includes)
        {
            return _iRepository.GetAll(includes);
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _iRepository.GetByIdAsync(id);
        }

        public async Task<int> Save()
        {
            return await _iUnitOfWork.Save();
        }

        public void Update(Booking entity)
        {
            _iRepository.Update(entity);
        }
    }
}
