using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrary
{
    public class NoConstructorException : ResolveException
    {
        public NoConstructorException() { }
        public NoConstructorException(string msg)
            : base(msg) { }
        public NoConstructorException(string msg, Exception inner)
            : base(msg, inner) { }
    }
}
