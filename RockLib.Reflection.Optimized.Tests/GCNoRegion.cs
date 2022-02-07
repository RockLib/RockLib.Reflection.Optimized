using System;
using Xunit.Sdk;

namespace RockLib.Reflection.Optimized.Tests
{
    public class GCNoRegion : IDisposable
    {
        private bool isDisposed;

        public GCNoRegion(long totalSize)
        {
            if (!GC.TryStartNoGCRegion(totalSize, true))
            {
                throw new XunitException("GC was not able to commit the required amount of memory and the garbage collector was not able to enter no GC region latency mode.");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {

                GC.EndNoGCRegion();
            }

            isDisposed = true;
        }
    }
}
