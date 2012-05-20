using System;

namespace Arbitrary
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class InjectAttribute : Attribute
    {
        public string Key = null;

        public InjectAttribute(string key = null)
        {
            this.Key = key;
        }
    }
}
