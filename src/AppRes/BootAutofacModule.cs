using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AppRes.Application;
using AppRes.Lib.Infrastructure;
using AppRes.Lib.Processes;
using Autofac;

namespace AppRes
{
    public class BootAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IApp).Assembly, typeof(IProcessFinder).Assembly)
                .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute)).Any())
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Register(c => Console.Out)
                .As<TextWriter>()
                .SingleInstance();
            
            base.Load(builder);
        }
    }
}