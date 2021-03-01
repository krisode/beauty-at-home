using Infrastructure.Interfaces;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace ApplicationCore.Services
{
    public interface IBeautyServicesService : IService<Service, int>
    {

    }
    public class BeautyServicesService : IBeautyServicesService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IRepository<Service, int> _iRepository;

        public BeautyServicesService(IUnitOfWork unitOfWork, IRepository<Service, int> iRepository)
        {
            _iUnitOfWork = unitOfWork;
            _iRepository = iRepository;
        }

        public async Task<Service> AddAsync(Service entity)
        {
            return await _iRepository.AddAsync(entity);
        }

        public void Delete(Service entity)
        {
            _iRepository.Delete(entity);
        }

        public IQueryable<Service> GetAll(params Expression<Func<Service, object>>[] includes)
        {
            return _iRepository.GetAll(includes);
        }

        public async Task<Service> GetByIdAsync(int id)
        {
            return await _iRepository.GetByIdAsync(id);
        }

        public async Task<int> Save()
        {
            return await _iUnitOfWork.Save();
        }

        public void Update(Service entity)
        {
            _iRepository.Update(entity);
        }
    }
}
