using System;

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
