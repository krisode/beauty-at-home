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
    public interface IBookingDetailService : IService<BookingDetail, int>
    {

    }
    public class BookingDetailService : IBookingDetailService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IRepository<BookingDetail, int> _iRepository;

        public BookingDetailService(IUnitOfWork unitOfWork, IRepository<BookingDetail, int> iRepository)
        {
            _iUnitOfWork = unitOfWork;
            _iRepository = iRepository;
        }

        public async Task<BookingDetail> AddAsync(BookingDetail entity)
        {
            return await _iRepository.AddAsync(entity);
        }

        public void Delete(BookingDetail entity)
        {
            _iRepository.Delete(entity);
        }
        
        public IQueryable<BookingDetail> GetAll(params Expression<Func<BookingDetail, object>>[] includes)
        {
            return _iRepository.GetAll(includes);
        }

        public async Task<BookingDetail> GetByIdAsync(int id)
        {
            return await _iRepository.GetByIdAsync(id);
        }

        public async Task<int> Save()
        {
            return await _iUnitOfWork.Save();
        }

        public void Update(BookingDetail entity)
        {
            _iRepository.Update(entity);
        }
    }
}
