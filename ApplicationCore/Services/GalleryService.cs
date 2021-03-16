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
    public interface IGalleryService : IService<Gallery, int>
    {

    }
    public class GalleryService : IGalleryService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IRepository<Gallery, int> _iRepository;

        public GalleryService(IUnitOfWork unitOfWork, IRepository<Gallery, int> iRepository)
        {
            _iUnitOfWork = unitOfWork;
            _iRepository = iRepository;
        }

        public async Task<Gallery> AddAsync(Gallery entity)
        {
            return await _iRepository.AddAsync(entity);
        }

        public void Delete(Gallery entity)
        {
            _iRepository.Delete(entity);
        }

        public IQueryable<Gallery> GetAll(params Expression<Func<Gallery, object>>[] includes)
        {
            return _iRepository.GetAll(includes);
        }

        public async Task<Gallery> GetByIdAsync(int id)
        {
            return await _iRepository.GetByIdAsync(id);
        }

        public async Task<int> Save()
        {
            return await _iUnitOfWork.Save();
        }

        public void Update(Gallery entity)
        {
            _iRepository.Update(entity);
        }
    }
}
