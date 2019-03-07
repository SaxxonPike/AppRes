using AppRes.Application;
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
            builder.RegisterModule<BootAutofacModule>();
            return builder.Build();
        }
    }
}