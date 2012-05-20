using System;

namespace Arbitrary
{
    public class ResolveException : Exception
    {
        public ResolveException() { }
        public ResolveException(string msg)
            : base(msg) { }
        public ResolveException(string msg, Exception inner)
            : base(msg, inner) { }
    }
}