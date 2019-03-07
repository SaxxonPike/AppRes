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
                .AsImplementedInterfaces()
                .SingleInstance();
            base.Load(builder);
        }
    }
}