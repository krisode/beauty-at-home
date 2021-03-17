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
    public interface IAddressService : IService<Address, int>
    {

    }
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IRepository<Address, int> _iRepository;

        public AddressService(IUnitOfWork unitOfWork, IRepository<Address, int> iRepository)
        {
            _iUnitOfWork = unitOfWork;
            _iRepository = iRepository;
        }

        public async Task<Address> AddAsync(Address entity)
        {
            return await _iRepository.AddAsync(entity);
        }

        public void Delete(Address entity)
        {
            _iRepository.Delete(entity);
        }

        public IQueryable<Address> GetAll(params Expression<Func<Address, object>>[] includes)
        {
            return _iRepository.GetAll(includes);
        }

        public async Task<Address> GetByIdAsync(int id)
        {
            return await _iRepository.GetByIdAsync(id);
        }

        public async Task<int> Save()
        {
            return await _iUnitOfWork.Save();
        }

        public void Update(Address entity)
        {
            _iRepository.Update(entity);
        }
    }
}
