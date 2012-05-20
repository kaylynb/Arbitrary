using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arbitrary.Test
{
    [TestClass]
    public class ArbitraryTests
    {
        private ArbitraryContainer container;
        [TestInitialize]
        public void Initialize()
        {
            container = new ArbitraryContainer();
        }

        [TestMethod]
        public void RegistrationDoesNotThrow()
        {
            container.Register<ITest, Test1>();
        }

        [TestMethod]
        public void RegisterWithKeyDoesNotThrow()
        {
            container.Register<ITest, Test1>("key1");
        }

        [TestMethod]
        public void ResolveDoesNotThrowAndIsCorrect()
        {
            container.Register<ITest, Test1>();
            var ret = container.Resolve<ITest>();

            Assert.IsInstanceOfType(ret, typeof(Test1));
        }

        [TestMethod]
        public void RegistrationOverwritesType()
        {
            container.Register<ITest, Test1>();
            container.Register<ITest, Test2>();

            var ret = container.Resolve<ITest>();

            Assert.IsInstanceOfType(ret, typeof(Test2));
        }

        [TestMethod]
        public void RegistrationReturnsSameType()
        {
            var ret = container
                .Register<ITest, Test1>()
                .Register<ITest, Test2>();

            Assert.AreSame(container, ret);
        }

        [TestMethod]
        public void ResolvesUnknownTypeIfInstantiable()
        {
            var ret = container.Resolve<Test1>();

            Assert.IsInstanceOfType(ret, typeof(Test1));
        }

        [TestMethod]
        public void ResolvesUnknownTypeIfInstantiableWithKey()
        {
            var ret = container.Resolve<Test1>("key1");

            Assert.IsInstanceOfType(ret, typeof(Test1));
        }

        [TestMethod]
        [ExpectedException(typeof(NoConstructorException))]
        public void DoesNotResolveInterfaceIfUnknown()
        {
            container.Resolve<ITest>();
        }

        [TestMethod]
        [ExpectedException(typeof(NoConstructorException))]
        public void RegistrationThrowsWhenUnknownWithKey()
        {
            container.Resolve<ITest>("key1");
        }

        [TestMethod]
        public void RegistrationWithKeyCorrectlyDifferentiates()
        {
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
            container
                .Register<ITest, Test1>("key1")
                .Register<ITest, Test2>("key1");

            var ret = container.Resolve<ITest>("key1");

            Assert.IsInstanceOfType(ret, typeof(Test2));
        }

        [TestMethod]
        public void ResolveCorrectlyResolvesAndInjectsGreedily()
        {
            container
                .Register<ITest, Test1>()
                .Register<ITestB, TestB1>();

            var ret = container.Resolve<ConstructorsTest>();

            Assert.IsInstanceOfType(ret.Test, typeof(Test1));
            Assert.IsInstanceOfType(ret.TestB, typeof(TestB1));
        }

        [TestMethod]
        [ExpectedException(typeof(ResolveException))]
        public void ResolveThrowsWhenCannotResolve()
        {
            container.Register<ITest, Test1>();

            container.Resolve<ConstructorsTest>();
        }

        [TestMethod]
        public void ResolvesBasicInjections()
        {
            container.Register<ITest, Test1>();

            var ret = container.Resolve<InjectionTest>();

            Assert.IsInstanceOfType(ret.Test, typeof(Test1));
        }

        [TestMethod]
        public void ResolvesInjectionsWithKeys()
        {
            container
                .Register<ITest, Test1>("key1")
                .Register<ITest, Test2>("key2");

            var ret = container.Resolve<InjectionTestWithKey>();

            Assert.IsInstanceOfType(ret.Test, typeof(Test1));
            Assert.IsInstanceOfType(ret.Test2, typeof(Test2));
        }

        [TestMethod]
        [ExpectedException(typeof(InjectionException))]
        public void ThrowsOnMissingInjections()
        {
            container.Resolve<InjectionTestWithKey>();
        }

        [TestMethod]
        [ExpectedException(typeof(InjectionException))]
        public void ThrowsOnMissingInjectionsWithKey()
        {
            container.Register<ITest, Test1>("key1");

            container.Resolve<InjectionTestWithKey>();
        }

        [TestMethod]
        public void CorrectlyInjectsConcreteTypes()
        {
            var ret = container.Resolve<InjectionTestTypes>();

            Assert.IsInstanceOfType(ret.Test, typeof(Test1));
        }

        [TestMethod]
        [ExpectedException(typeof(InjectionException))]
        public void ThrowsExceptionWhenInjectingConcreteWithKey()
        {
            container.Resolve<InjectionTestTypesWithKey>();
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

    class InjectionTest
    {
        [Inject]
        public ITest Test { get; set; }
    }

    class InjectionTestWithKey
    {
        [Inject("key1")]
        public ITest Test { get; set; }

        [Inject(Key = "key2")]
        public ITest Test2 { get; set; }
    }

    class InjectionTestTypes
    {
        [Inject]
        public Test1 Test { get; set; }
    }

    class InjectionTestTypesWithKey
    {
        [Inject("key1")]
        public Test1 Test { get; set; }
    }
}
