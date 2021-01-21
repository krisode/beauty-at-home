using System;

namespace Infrastructure.Interfaces.Implements
{
    public class Disposable : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        ///  ~ symbol is used to declare destructors
        /// </summary>
        ~Disposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                DisposeCore();
            }

            isDisposed = true;
        }

         /// <summary>
         /// Overide this method to dispose custom objects
         /// </summary>
        protected virtual void DisposeCore()
        {

        }
    }
}
