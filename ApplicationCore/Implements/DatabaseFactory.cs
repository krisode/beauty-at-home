using ApplicationCore.Interfaces;

namespace ApplicationCore.Implements
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {

        protected override void DisposeCore()
        {
            base.DisposeCore();
        }
    }
}
