using System;

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
