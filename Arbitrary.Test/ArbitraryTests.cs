using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [TestMethod]
        public void ResolveCorrectlyResolvesAndInjectsGreedily()
        {
            var container = new ArbitraryContainer();

            container
                .Register<ITest, Test1>()
                .Register<ITestB, TestB1>();

            var ret = container.Resolve<ConstructorsTest>();

            Assert.IsInstanceOfType(ret.Test, typeof(Test1));
            Assert.IsInstanceOfType(ret.TestB, typeof(TestB1));
        }
    }

    // Fixtures
    interface ITest { }
    class Test1 : ITest { }
    class Test2 : ITest { }

    interface ITestB { }
    class TestB1 : ITestB { }

    class ConstructorsTest
    {
        public ConstructorsTest()
            : this(null, null) { }

        public ConstructorsTest(ITest test)
            : this(test, null) { }

        public ConstructorsTest(ITest test, ITestB testb)
        {
            Test = test;
            TestB = testb;
        }

        public ITest Test { get; private set; }
        public ITestB TestB { get; private set; }
    }
}
