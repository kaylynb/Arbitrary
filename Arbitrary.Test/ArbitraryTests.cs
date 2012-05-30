using System;
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
        public void DoesNotResolveUnmarkedProperties()
        {
            container.Register<ITest, Test1>();

            var ret = container.Resolve<NonInjectionTest>();

            Assert.IsNull(ret.Test);
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
        public void CorrectlyInjectsConcreteTypesEvenIfKeyDoesNotExist()
        {
            var ret = container.Resolve<InjectionTestTypesWithKey>();

            Assert.IsInstanceOfType(ret.Test, typeof(Test1));
        }

        [TestMethod]
        public void RegisterInstanceOfObject()
        {
            var test1 = new Test1();
            container.Register<ITest>(test1);

            var ret = container.Resolve<ITest>();

            Assert.AreSame(test1, ret);
        }

        [TestMethod]
        public void CheckDifferentInstantiations()
        {
            container.Register<ITest, Test1>();

            var ret1 = container.Resolve<ITest>();
            var ret2 = container.Resolve<ITest>();

            Assert.AreNotSame(ret1, ret2);
        }

        [TestMethod]
        public void CheckSameInstantiationsWithSingleton()
        {
            container.Register<ITest, Test1>(lifetime: new SingletonLifetime());

            var ret1 = container.Resolve<ITest>();
            var ret2 = container.Resolve<ITest>();

            Assert.AreSame(ret1, ret2);
        }

        [TestMethod]
        public void TestInjectionInstantationsWithSingleton()
        {
            container.Register<ITest, Test1>(lifetime: new SingletonLifetime());

            var ret1 = container.Resolve<InjectionTest>();
            var ret2 = container.Resolve<InjectionTest>();

            Assert.AreNotSame(ret1, ret2);
            Assert.AreSame(ret1.Test, ret2.Test);
        }

        [TestMethod]
        public void TestRegistrationWithDifferentLifetimesAndKeys()
        {
            container.Register<ITest, Test1>();
            container.Register<ITest, Test2>("Singleton", new SingletonLifetime());

            var ret1 = container.Resolve<ITest>();
            var ret2 = container.Resolve<ITest>();

            Assert.AreNotSame(ret1, ret2);

            ret1 = container.Resolve<ITest>("Singleton");
            ret2 = container.Resolve<ITest>("Singleton");

            Assert.AreSame(ret1, ret2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void InvalidCastWhenSettingSingleton()
        {
            container.Register<ITest, Test1>(lifetime: new SingletonLifetime(new TestB1()));

            var ret = container.Resolve<ITest>();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void InvalidCastWhenNotUsingImpliedType()
        {
            container.Register<ITest, Test1>(lifetime: new SingletonLifetime(new TestB1()));

            ITest ret = container.Resolve<ITest>();
        }

        [TestMethod]
        public void AWayToShootYourselfInTheFootSoUseSpecificConstructorsWhenPossible()
        {
            container.Register<ITest, Test1>(lifetime: new SingletonLifetime(new TestB1()));

            var ret = container.Resolve(typeof(ITest));

            Assert.IsNotInstanceOfType(ret, typeof(Test1));
            Assert.IsInstanceOfType(ret, typeof(TestB1));
        }

        [TestMethod]
        public void ComplexResolution()
        {
            container
                .Register<ITest, Test3>()
                .Register<ITestB, TestB1>()
                .Register<ConstructorsTest, ConstructorsTest>(lifetime: new SingletonLifetime())
                .Register<ITest, Test2>()
                .Register<ITwoKeyedInjectionTests, FirstKeyedInjectionTest>()
                .Register<ITest, Test3>("KeyedProperty")
                .Register<ITest, Test1>("key1")
                .Register<ITest, Test2>("key2");

            var ret = container.Resolve<ComplicatedInjection>();

            Assert.IsInstanceOfType(ret.constructedTest1, typeof(Test1));

            Assert.IsInstanceOfType(ret.constructedTestInterface, typeof(Test2));

            Assert.IsInstanceOfType(ret.constructedConstructorsTest, typeof(ConstructorsTest));
            Assert.IsInstanceOfType(ret.constructedConstructorsTest.Test, typeof(Test3));
            Assert.IsInstanceOfType(ret.constructedConstructorsTest.TestB, typeof(TestB1));
            Assert.AreSame(ret.constructedConstructorsTest, ret.ConstructorsTestInjection);

            Assert.IsInstanceOfType(ret.constructedTwoKeyedInjection, typeof(FirstKeyedInjectionTest));
            Assert.IsInstanceOfType(ret.constructedTwoKeyedInjection.Test, typeof(Test1));

            Assert.IsInstanceOfType(ret.FirstTestProperty, typeof(Test2));
            Assert.IsInstanceOfType(ret.SecondTestProperty, typeof(Test2));
            Assert.AreNotSame(ret.FirstTestProperty, ret.SecondTestProperty);

            Assert.IsInstanceOfType(ret.KeyedProperty, typeof(Test3));

            Assert.IsInstanceOfType(ret.TwoKeyedInjection, typeof(FirstKeyedInjectionTest));
            Assert.AreNotSame(ret.constructedTwoKeyedInjection.Test, ret.TwoKeyedInjection);

            Assert.IsInstanceOfType(ret.SecondKeyedInjection, typeof(SecondKeyedInjectionTest));
            Assert.IsInstanceOfType(ret.SecondKeyedInjection.Test, typeof(Test2));
        }
    }

    // Fixtures
    internal interface ITest
    {
    }

    internal class Test1 : ITest
    {
    }

    internal class Test2 : ITest
    {
    }

    internal class Test3 : ITest
    {
    }

    internal interface ITestB
    {
    }

    internal class TestB1 : ITestB
    {
    }

    internal class ConstructorsTest
    {
        public ConstructorsTest()
            : this(null, null)
        {
        }

        public ConstructorsTest(ITest test)
            : this(test, null)
        {
        }

        public ConstructorsTest(ITest test, ITestB testb)
        {
            Test = test;
            TestB = testb;
        }

        public ITest Test { get; private set; }
        public ITestB TestB { get; private set; }
    }

    internal class InjectionTest
    {
        [Inject]
        public ITest Test { get; set; }
    }

    internal class NonInjectionTest
    {
        public ITest Test { get; set; }
    }

    internal class InjectionTestWithKey
    {
        [Inject("key1")]
        public ITest Test { get; set; }

        [Inject(Key = "key2")]
        public ITest Test2 { get; set; }
    }

    internal class InjectionTestTypes
    {
        [Inject]
        public Test1 Test { get; set; }
    }

    internal class InjectionTestTypesWithKey
    {
        [Inject("key1")]
        public Test1 Test { get; set; }
    }

    internal interface ITwoKeyedInjectionTests
    {
        ITest Test { get; set; }
    }

    internal class FirstKeyedInjectionTest : ITwoKeyedInjectionTests
    {
        [Inject("key1")]
        public ITest Test { get; set; }
    }

    internal class SecondKeyedInjectionTest : ITwoKeyedInjectionTests
    {
        [Inject("key2")]
        public ITest Test { get; set; }
    }

    // Complicated fixtures
    internal class ComplicatedInjection
    {
        public Test1 constructedTest1;
        public ITest constructedTestInterface;
        public ConstructorsTest constructedConstructorsTest;
        public ITwoKeyedInjectionTests constructedTwoKeyedInjection;

        public ComplicatedInjection(Test1 test1, ITest testInterface, ConstructorsTest constructorsTest,
                                    ITwoKeyedInjectionTests twoKeyedInjection)
        {
            constructedTest1 = test1;
            constructedTestInterface = testInterface;
            constructedConstructorsTest = constructorsTest;
            constructedTwoKeyedInjection = twoKeyedInjection;
        }

        [Inject]
        public ITest FirstTestProperty { get; set; }

        [Inject]
        public ITest SecondTestProperty { get; set; }

        [Inject("KeyedProperty")]
        public ITest KeyedProperty { get; set; }

        [Inject]
        public ITwoKeyedInjectionTests TwoKeyedInjection { get; set; }

        [Inject]
        public SecondKeyedInjectionTest SecondKeyedInjection { get; set; }

        [Inject]
        public ConstructorsTest ConstructorsTestInjection { get; set; }
    }
}