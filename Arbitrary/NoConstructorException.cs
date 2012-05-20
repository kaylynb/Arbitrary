using System;

namespace Arbitrary
{
    public class NoConstructorException : ResolveException
    {
        public NoConstructorException() { }
        public NoConstructorException(string message)
            : base(message) { }
        public NoConstructorException(string message, Exception inner)
            : base(message, inner) { }
    }
}
