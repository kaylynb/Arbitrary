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
        public void DoesNotResolveInterfaceIfUnknown()
        {
            var container = new ArbitraryContainer();

            TestHelpers.AssertThrows<NoConstructorException>(() =>
                {
                    container.Resolve<ITest>();
                });
        }
    }

    // Fixtures
    interface ITest { }
    class Test1 : ITest { }
    class Test2 : ITest { }
}
