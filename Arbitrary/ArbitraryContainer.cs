using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Arbitrary
{
    public class ArbitraryContainer
    {
        public ArbitraryContainer Register<TInterface>(string key = null, ILifetime lifetime = null)
        {
            return Register<TInterface, TInterface>(key, lifetime);
        }

        public ArbitraryContainer Register<TInterface, TInstance>(string key = null, ILifetime lifetime = null)
            where TInstance : TInterface
        {
            return Register(typeof (TInterface), typeof (TInstance), key, lifetime);
        }

        public ArbitraryContainer Register<TInterface>(TInterface instance, string key = null)
        {
            return Register(typeof(TInterface), typeof(TInterface), key, new SingletonLifetime(instance));
        }

        public ArbitraryContainer Register(Type interfaceType, Type instanceType, string key = null, ILifetime lifetime = null)
        {
            if(lifetime == null)
                lifetime = new InstanceLifetime();

            lifetime.Register(() => Create(instanceType));
            _types[Tuple.Create(interfaceType, key)] = lifetime;

            return this;
        }

        public T Resolve<T>(string key = null)
        {
            return (T) Resolve(typeof (T), key);
        }

        public object Resolve(Type requestedType, string key = null)
        {
            ILifetime lifetime;
            _types.TryGetValue(Tuple.Create(requestedType, key), out lifetime);
            if (lifetime == null)
            {
                lifetime = new InstanceLifetime();
                lifetime.Register(() => Create(requestedType));
            }

            return lifetime.Resolve();
        }

        private readonly Dictionary<Tuple<Type, string>, ILifetime> _types = new Dictionary<Tuple<Type, string>, ILifetime>();

        private void InjectProperties(object obj)
        {
            try
            {
                foreach (var resolution in
                    from propertyInfo in obj.GetType().GetTypeInfo().DeclaredProperties
                    let injectionAttribute = propertyInfo.GetCustomAttribute<InjectAttribute>()
                    where injectionAttribute != null
                    select
                        new Tuple<PropertyInfo, object>(propertyInfo,
                                                        Resolve(propertyInfo.PropertyType, injectionAttribute.Key)))
                {
                    resolution.Item1.SetValue(obj, resolution.Item2);
                }
            }
            catch(ResolveException exception)
            {
                throw new InjectionException("Could not inject properties for : " + obj.GetType().FullName, exception);
            }
        }

        private object Create(Type type)
        {
            var constructor = type.GetTypeInfo()
                .DeclaredConstructors
                .OrderByDescending(x => x.GetParameters().Length)
                .FirstOrDefault();

            if (constructor == null)
                throw new NoConstructorException("No constructor for type: " + type.FullName);

            object obj;
            try
            {
                obj = constructor.Invoke(
                    constructor.GetParameters()
                    .Select(parameterInfo => Resolve(parameterInfo.ParameterType))
                    .ToArray());

                if(obj == null)
                    throw new Exception();


            }
            catch(Exception exception)
            {
                throw new ResolveException("Could not resolve: " + type.FullName, exception);
            }

            InjectProperties(obj);

            return obj;
        }
    }
}
