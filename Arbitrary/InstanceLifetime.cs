using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
