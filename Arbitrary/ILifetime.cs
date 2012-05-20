using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrary
{
    public interface ILifetime
    {
        void Register(Func<object> resolutionFunc);
        object Resolve();
    }
}
