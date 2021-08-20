using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using MassTransitTest.Components.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace MassTransitTest.Service
{
    public class Program
    {
        private static string _rmqHostname;
        private static string _rmqVirtualHost;
        private static string _rmqUsername;
        private static string _rmqPassword;

        public static async Task Main(string[] args)
        {
            // Start the application
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Set options from appsettings.json
                    _rmqHostname = "localhost";
                    _rmqVirtualHost = "/";
                    _rmqUsername = "guest";
                    _rmqPassword = "guest";


                    // MassTransit config
                    services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

                    services.AddMassTransit(cfg =>
                    {
                        // Consumers
                        cfg.AddConsumersFromNamespaceContaining<ExampleRequestConsumer>();

                        // Request clients
                        cfg.AddRequestClient<IExampleRequest>(RequestTimeout.After(0, 1));

                        // Start bus
                        cfg.AddBus(ConfigureBus);
                    });

                    services.AddHostedService<HostedService>();
                });

        private static IBusControl ConfigureBus(IBusRegistrationContext provider)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(_rmqHostname, _rmqVirtualHost, h =>
                {
                    h.Username(_rmqUsername);
                    h.Password(_rmqPassword);
                });

                cfg.ConfigureEndpoints(provider);
            });
        }
    }
}
