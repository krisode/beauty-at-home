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
    public interface IFeedBackService : IService<FeedBack, int>
    {

    }
    public class FeedBackService : IFeedBackService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IRepository<FeedBack, int> _iRepository;

        public FeedBackService(IUnitOfWork unitOfWork, IRepository<FeedBack, int> iRepository)
        {
            _iUnitOfWork = unitOfWork;
            _iRepository = iRepository;
        }

        public async Task<FeedBack> AddAsync(FeedBack entity)
        {
            return await _iRepository.AddAsync(entity);
        }

        public void Delete(FeedBack entity)
        {
            _iRepository.Delete(entity);
        }

        public IQueryable<FeedBack> GetAll(params Expression<Func<FeedBack, object>>[] includes)
        {
            return _iRepository.GetAll(includes);
        }

        public async Task<FeedBack> GetByIdAsync(int id)
        {
            return await _iRepository.GetByIdAsync(id);
        }

        public async Task<int> Save()
        {
            return await _iUnitOfWork.Save();
        }

        public void Update(FeedBack entity)
        {
            _iRepository.Update(entity);
        }
    }
}
