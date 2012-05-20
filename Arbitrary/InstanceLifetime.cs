using System;

namespace Arbitrary
{
    public class InstanceLifetime : ILifetime
    {
        private Func<object> _resolutionFunc;
 
        public void Register(Func<object> resolutionFunc)
        {
            if(resolutionFunc == null)
                throw new ArgumentNullException("resolutionFunc");

            _resolutionFunc = resolutionFunc;
        }

        public object Resolve()
        {
            return _resolutionFunc == null ? null : _resolutionFunc();
        }
    }
}
