using Autofac;
using Service.IoC;

namespace Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = BuildContainer();
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<ConventionModule>();
            return builder.Build();
        }
    }
}
