using System;
using System.Diagnostics;
using Autofac;
using NUnit.Framework;

namespace AppRes.Test.Testing
{
    public abstract class BaseIntegrationTestFixture<T> : BaseIntegrationTestFixture
    {
        private Lazy<T> _subject;

        [SetUp]
        public void __SetUpSubject()
        {
            _subject = new Lazy<T>(Resolve<T>);
        }

        protected T Subject => _subject.Value;
    }

    [TestFixture]
    public abstract class BaseIntegrationTestFixture : BaseTestFixture
    {
        [SetUp]
        public void __SetUpContext()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<BootAutofacModule>();
            __SetUpRegistrations(builder);
            Context = builder.Build();
        }

        protected virtual void __SetUpRegistrations(ContainerBuilder builder)
        {
        }

        private IContainer Context { get; set; }
        
        [DebuggerStepThrough]
        protected T Resolve<T>()
        {
            return Context.Resolve<T>();
        }
    }
}