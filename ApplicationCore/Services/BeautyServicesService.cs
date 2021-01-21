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

        public Service GetById(int Id)
        {
            return _iRepository.GetById(Id);
        }

        public void Update(Service entity)
        {
            _iRepository.Update(entity);
        }

        public void Delete(Expression<Func<Service, bool>> where)
        {
            _iRepository.Delete(where);
        }

        public IEnumerable<Service> GetAll()
        {
            return _iRepository.GetAll();
        }

        public IEnumerable<Service> GetAll(params Expression<Func<Service, object>>[] includes)
        {
            return _iRepository.GetAll(includes);
        }

        public IEnumerable<Service> GetList(Expression<Func<Service, bool>> where)
        {
            return _iRepository.GetList(where);
        }

        public IQueryable<Service> _GetList(Expression<Func<Service, bool>> where)
        {
            return _iRepository._GetList(where);
        }

        public IQueryable<Service> _GetList(Expression<Func<Service, bool>> where, params Expression<Func<Service, object>>[] includes)
        {
            return _iRepository._GetList(where, includes);
        }

        public async Task<bool> Save()
        {
            return await _iUnitOfWork.Save();
        }
    }
}
