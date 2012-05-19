using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Arbitrary;

namespace Arbitrary.Test
{
    [TestClass]
    public class ArbitraryTests
    {
        [TestMethod]
        public void RegistrationDoesNotThrow()
        {
            var container = new ArbitraryContainer();
            container.Register<ITest, Test1>();
        }

        [TestMethod]
        public void RegisterWithKeyDoesNotThrow()
        {
            var container = new ArbitraryContainer();

            container.Register<ITest, Test1>("key1");
        }

        [TestMethod]
        public void ResolveDoesNotThrowAndIsCorrect()
        {
            var container = new ArbitraryContainer();
            container.Register<ITest, Test1>();
            var ret = container.Resolve<ITest>();

            Assert.IsInstanceOfType(ret, typeof(Test1));
        }

        [TestMethod]
        public void RegistrationOverwritesType()
        {
            var container = new ArbitraryContainer();
            container.Register<ITest, Test1>();
            container.Register<ITest, Test2>();

            var ret = container.Resolve<ITest>();

            Assert.IsInstanceOfType(ret, typeof(Test2));
        }

        [TestMethod]
        public void RegistrationReturnsSameType()
        {
            var container = new ArbitraryContainer();
            var ret = container
                .Register<ITest, Test1>()
                .Register<ITest, Test2>();

            Assert.AreSame(container, ret);
        }

        [TestMethod]
        public void ResolvesUnknownTypeIfInstantiable()
        {
            var container = new ArbitraryContainer();
            var ret = container.Resolve<Test1>();

            Assert.IsInstanceOfType(ret, typeof(Test1));
        }

        [TestMethod]
        public void ResolvesUnknownTypeIfInstantiableWithKey()
        {
            var container = new ArbitraryContainer();
            var ret = container.Resolve<Test1>("key1");

            Assert.IsInstanceOfType(ret, typeof(Test1));
        }

        [TestMethod]
        [ExpectedException(typeof(NoConstructorException))]
        public void DoesNotResolveInterfaceIfUnknown()
        {
            var container = new ArbitraryContainer();
            container.Resolve<ITest>();
        }

        [TestMethod]
        [ExpectedException(typeof(NoConstructorException))]
        public void RegistrationThrowsWhenUnknownWithKey()
        {
            var container = new ArbitraryContainer();

            container.Resolve<ITest>("key1");
        }

        [TestMethod]
        public void RegistrationWithKeyCorrectlyDifferentiates()
        {
            var container = new ArbitraryContainer();

            container
                .Register<ITest, Test1>()
                .Register<ITest, Test2>("key1");

            var retTest1 = container.Resolve<ITest>();
            var retTest2 = container.Resolve<ITest>("key1");

            Assert.IsInstanceOfType(retTest1, typeof(Test1));
            Assert.IsInstanceOfType(retTest2, typeof(Test2));
        }

        [TestMethod]
        public void RegistrationCorrectlyOverwritesKeys()
        {
            var container = new ArbitraryContainer();

            container
                .Register<ITest, Test1>("key1")
                .Register<ITest, Test2>("key1");

            var ret = container.Resolve<ITest>("key1");

            Assert.IsInstanceOfType(ret, typeof(Test2));
        }
    }

    // Fixtures
    interface ITest { }
    class Test1 : ITest { }
    class Test2 : ITest { }
}
