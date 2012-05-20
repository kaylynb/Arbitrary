using System;

namespace Arbitrary
{
    public class SingletonLifetime : ILifetime
    {
        public SingletonLifetime(object instance = null)
        {
            _object = instance;
        }

        private object _object;
        public void Register(Func<object> resolutionFunc)
        {
            if(resolutionFunc == null)
                throw new ArgumentNullException("resolutionFunc");

            if(_object == null)
                _object = resolutionFunc();
        }

        public object Resolve()
        {
            return _object;
        }
    }
}
