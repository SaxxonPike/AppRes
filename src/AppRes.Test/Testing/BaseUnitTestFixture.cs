using System;
using System.Diagnostics;
using Autofac.Core;
using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;

namespace AppRes.Test.Testing
{
    public abstract class BaseUnitTestFixture<T> : BaseUnitTestFixture
    {
        private Lazy<T> _subject;

        [SetUp]
        public void __SetUpSubject()
        {
            _subject = new Lazy<T>(__CreateSubject);
        }

        protected virtual T __CreateSubject()
        {
            return CreateService<T>();
        }

        protected T Subject => _subject.Value;
    }

    [TestFixture]
    public abstract class BaseUnitTestFixture : BaseTestFixture
    {
        [SetUp]
        [DebuggerStepThrough]
        public void __SetUpContext()
        {
            AutoMock = new Lazy<AutoMock>(Autofac.Extras.Moq.AutoMock.GetStrict);
        }
        
        private Lazy<AutoMock> AutoMock { get; set; }

        [DebuggerStepThrough]
        protected Mock<T> MockService<T>() where T : class
        {
            return AutoMock.Value.Mock<T>();
        }

        [DebuggerStepThrough]
        protected Mock<T> MockService<T>(Action<Mock<T>> setup) where T : class
        {
            var mock = MockService<T>();
            setup(mock);
            return mock;
        }

        [DebuggerStepThrough]
        protected void Inject<T>(T obj) where T : class
        {
            AutoMock.Value.Provide(obj);
        }

        [DebuggerStepThrough]
        protected T CreateService<T>(params Parameter[] parameters)
        {
            return AutoMock.Value.Create<T>(parameters);
        }
    }
}