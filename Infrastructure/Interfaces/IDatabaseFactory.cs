using Infrastructure.Contexts;

namespace Infrastructure.Interfaces
{
    public interface IDatabaseFactory
    {
        BeautyServiceProviderContext Init();
    }
}
