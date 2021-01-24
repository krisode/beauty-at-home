using Infrastructure.Interfaces;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IBeautyServicesService : IService<Service>
    {

    }
    public class BeautyServicesService : IBeautyServicesService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IServiceRepository _iRepository;

        public BeautyServicesService(IUnitOfWork unitOfWork, IServiceRepository serviceRepository)
        {
            _iUnitOfWork = unitOfWork;
            _iRepository = serviceRepository;
        }
        public void Add(Service service)
        {
            _iRepository.Add(service);
        }

        public Service GetById(int id)
        {
            return _iRepository.GetById(id);
        }

        public void Update(Service entity)
        {
            _iRepository.Update(entity);
        }

        public void Delete(Expression<Func<Service, bool>> where)
        {
            _iRepository.Delete(where);
        }

        public IEnumerable<Service> GetEnumAll()
        {
            return _iRepository.GetEnumAll();
        }

        public IEnumerable<Service> GetEnumAll(params Expression<Func<Service, object>>[] includes)
        {
            return _iRepository.GetEnumAll(includes);
        }

        public IEnumerable<Service> GetEnumList(Expression<Func<Service, bool>> where)
        {
            return _iRepository.GetEnumList(where);
        }

        public IQueryable<Service> GetQueryList(Expression<Func<Service, bool>> where)
        {
            return _iRepository.GetQueryList(where);
        }

        public IQueryable<Service> GetQueryList(params Expression<Func<Service, bool>>[] where)
        {
            return _iRepository.GetQueryList(where);
        }

        public IQueryable<Service> GetQueryList(Expression<Func<Service, bool>> where, params Expression<Func<Service, object>>[] includes)
        {
            return _iRepository.GetQueryList(where, includes);
        }

        public async Task<bool> Save()
        {
            return await _iUnitOfWork.Save();
        }
    }
}
