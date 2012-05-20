using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrary
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Inject : Attribute
    {
        public string Key = null;

        public Inject(string key = null)
        {
            this.Key = key;
        }
    }
}
