using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Arbitrary
{
    public class ArbitraryContainer
    {
        public ArbitraryContainer Register<TInterface, TInstance>(string key = null)
            where TInstance : TInterface
        {
            _types[Tuple.Create<Type, string>(typeof(TInterface), key)] = typeof(TInstance);
            return this;
        }

        public T Resolve<T>(string key = null)
        {
            Type t;
            if (!_types.TryGetValue(Tuple.Create<Type, string>(typeof(T), key), out t))
                return (T)Create(typeof(T));

            return (T)Create(t);
        }

        private readonly Dictionary<Tuple<Type, string>, Type> _types = new Dictionary<Tuple<Type, string>, Type>();

        private object Create(Type type)
        {
            var constructor = type.GetTypeInfo().DeclaredConstructors.FirstOrDefault();
            if (constructor == null)
                throw new NoConstructorException("No constructor for type: " + type.FullName);

            var parameters = new List<object>();

            return constructor.Invoke(parameters.ToArray());
        }
    }
}
