using System.Collections.Specialized;
using Autofac;
using Autofac.Extras.Quartz;
using Quartz.Simpl;
using Quartz.Spi;
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
            //builder.RegisterType<SimpleTypeLoadHelper>().As<ITypeLoadHelper>();
            builder.RegisterModule(new QuartzAutofacFactoryModule {ConfigurationProvider = QuartzConfigurationProvider});
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(Program).Assembly));
            return builder.Build();
        }

        private static NameValueCollection QuartzConfigurationProvider(IComponentContext arg)
        {
            return new NameValueCollection
            {
                ["quartz.scheduler.instanceName"] = "XmlConfiguredInstance",
                ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
                ["quartz.threadPool.threadCount"] = "5",
                ["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz.Plugins",
                ["quartz.plugin.xml.fileNames"] = "quartz-jobs.config",
                ["quartz.plugin.xml.FailOnFileNotFound"] = "true",
                ["quartz.plugin.xml.failOnSchedulingError"] = "true"
            };
        }
    }
}
