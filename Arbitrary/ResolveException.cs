using System;

namespace Arbitrary
{
    public class ResolveException : Exception
    {
        public ResolveException() { }
        public ResolveException(string message)
            : base(message) { }
        public ResolveException(string message, Exception inner)
            : base(message, inner) { }
    }
}