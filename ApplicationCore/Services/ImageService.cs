using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IImageService : IService<Image, int>
    {

    }
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IRepository<Image, int> _iRepository;

        public ImageService(IUnitOfWork unitOfWork, IRepository<Image, int> iRepository)
        {
            _iUnitOfWork = unitOfWork;
            _iRepository = iRepository;
        }

        public async Task<Image> AddAsync(Image entity)
        {
            return await _iRepository.AddAsync(entity);
        }

        public void Delete(Image entity)
        {
            _iRepository.Delete(entity);
        }

        public IQueryable<Image> GetAll(params Expression<Func<Image, object>>[] includes)
        {
            return _iRepository.GetAll(includes);
        }

        public async Task<Image> GetByIdAsync(int id)
        {
            return await _iRepository.GetByIdAsync(id);
        }

        public async Task<int> Save()
        {
            return await _iUnitOfWork.Save();
        }

        public void Update(Image entity)
        {
            _iRepository.Update(entity);
        }
    }
}
