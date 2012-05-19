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
            _types[typeof(TInterface)] = typeof(TInstance);
            return this;
        }

        public T Resolve<T>()
        {
            Type t;
            if (!_types.TryGetValue(typeof(T), out t))
                return (T)Create(typeof(T));

            return (T)Create(t);
        }

        private readonly Dictionary<Type, Type> _types = new Dictionary<Type,Type>();

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
