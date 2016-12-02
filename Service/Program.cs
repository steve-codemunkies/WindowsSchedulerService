using Autofac;
using Service.IoC;
using Topshelf;
using Topshelf.Autofac;

namespace Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = BuildContainer();

            HostFactory.Run(configurator =>
            {
                configurator.UseSerilog();
                configurator.UseAutofacContainer(container);

                configurator.Service<ITestService>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsingAutofacContainer();
                    serviceConfigurator.WhenStarted(service => service.Start());
                    serviceConfigurator.WhenStopped(service => service.Stop());
                });

                configurator.RunAsLocalSystem();
                configurator.StartAutomaticallyDelayed();

                configurator.SetDescription("Sample Topshelf/Quartz scheduler");
                configurator.SetDisplayName("Topshelf Quartz Scheduler");
                configurator.SetServiceName("TQScheduler");
            });
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<ConventionModule>();
            builder.RegisterModule<LoggingModule>();
            return builder.Build();
        }
    }
}
