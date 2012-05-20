using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Arbitrary
{
    public class ArbitraryContainer
    {
        public ArbitraryContainer Register<TInterface, TInstance>(string key = null)
            where TInstance : TInterface
        {
            _types[Tuple.Create(typeof(TInterface), key)] = typeof(TInstance);
            return this;
        }

        public T Resolve<T>(string key = null)
        {
            return (T) Resolve(typeof (T), key);
        }

        public object Resolve(Type requestedType, string key = null)
        {
            Type t;
            if (!_types.TryGetValue(Tuple.Create(requestedType, key), out t))
                return Create(requestedType);

            return Create(t);
        }

        private readonly Dictionary<Tuple<Type, string>, Type> _types = new Dictionary<Tuple<Type, string>, Type>();

        private object Create(Type type)
        {
            var constructor = type.GetTypeInfo()
                .DeclaredConstructors
                .OrderByDescending(x => x.GetParameters().Length)
                .FirstOrDefault();

            if (constructor == null)
                throw new NoConstructorException("No constructor for type: " + type.FullName);

            return constructor.Invoke(
                constructor.GetParameters()
                .Select(parameterInfo => Resolve(parameterInfo.ParameterType))
                .ToArray());
        }
    }
}
