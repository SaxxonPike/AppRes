using System.Linq;
using System.Reflection;
using AppRes.Application;
using AppRes.Lib.Infrastructure;
using AppRes.Lib.Processes;
using Autofac;

namespace AppRes
{
    internal static class Boot
    {
        internal static void Main(string[] args)
        {
            var container = BuildContainer();
            container.Resolve<IApp>().Start(args);
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(IApp).Assembly, typeof(IProcessFinder).Assembly)
                .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute)).Any())
                .AsImplementedInterfaces()
                .SingleInstance();

            return builder.Build();
        }
    }
}