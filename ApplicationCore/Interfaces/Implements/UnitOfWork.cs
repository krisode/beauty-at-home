using ApplicationCore.Interfaces;
using System;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {

        }

        public Task<bool> Save()
        {
            throw new NotImplementedException();
        }
    }
}
