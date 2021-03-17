using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceTypeService : IService<ServiceType, int>
    {

    }
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IRepository<ServiceType, int> _iRepository;

        public ServiceTypeService(IUnitOfWork unitOfWork, IRepository<ServiceType, int> iRepository)
        {
            _iUnitOfWork = unitOfWork;
            _iRepository = iRepository;
        }

        public async Task<ServiceType> AddAsync(ServiceType entity)
        {
            return await _iRepository.AddAsync(entity);
        }

        public void Delete(ServiceType entity)
        {
            _iRepository.Delete(entity);
        }

        public IQueryable<ServiceType> GetAll(params Expression<Func<ServiceType, object>>[] includes)
        {
            return _iRepository.GetAll(includes);
        }

        public async Task<ServiceType> GetByIdAsync(int id)
        {
            return await _iRepository.GetByIdAsync(id);
        }

        public async Task<int> Save()
        {
            return await _iUnitOfWork.Save();
        }

        public void Update(ServiceType entity)
        {
            _iRepository.Update(entity);
        }
    }
}
