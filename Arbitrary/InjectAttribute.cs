using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
