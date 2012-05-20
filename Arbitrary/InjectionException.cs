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
        public InjectionException(string message)
            : base(message) { }
        public InjectionException(string message, Exception inner)
            : base(message, inner) { }
    }
}
