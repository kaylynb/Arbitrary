using System;

namespace Arbitrary
{
    public class ConstructorInvocationException : ResolveException
    {
        public ConstructorInvocationException() { }
        public ConstructorInvocationException(string message)
            : base(message) { }
        public ConstructorInvocationException(string message, Exception inner)
            : base(message, inner) { }
    }
}