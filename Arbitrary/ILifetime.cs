using System;

namespace Arbitrary
{
    public interface ILifetime
    {
        void Register(Func<object> resolutionFunc);
        object Resolve();
    }
}
