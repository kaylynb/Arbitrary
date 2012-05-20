using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrary
{
    public class InjectionException : ResolveException
    {
        public InjectionException() { }
        public InjectionException(string msg)
            : base(msg) { }
        public InjectionException(string msg, Exception inner)
            : base(msg, inner) { }
    }
}
