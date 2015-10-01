using System;
using System.Configuration;
using System.Threading;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Logging;

namespace ConsoleApplication1
{
    public class ConfigSource : IConfigurationSource
    {public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof (T) == typeof (MessageForwardingInCaseOfFaultConfig))
            {
                return new MessageForwardingInCaseOfFaultConfig()
                {
                    ErrorQueue = "TestEndpoint.Error"
                } as T;
            }

            return ConfigurationManager.GetSection(typeof (T).Name) as T;
        }
    }

    public class Program
    {
        private static string BuildRabbitConnectionString()
        {
            var baseConnection = "host=" + "localhost:5672" + ";Username=" + "WimtUser" + ";Password=" + "sRzBFDexuESZCHzmaB7b";
            return baseConnection;
        }

        private static string BuidAzureString()
        {
            var baseConnection = "UseDevelopmentStorage=true";
            return baseConnection;
        }

        private static BusConfiguration GetConfig()
        {
            var busConfiguration = new BusConfiguration();
            DefaultFactory defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Level(LogLevel.Debug);

            busConfiguration.UseTransport<MsmqTransport>();
            //busConfiguration.UseTransport<RabbitMQTransport>()
            //    .ConnectionString(BuildRabbitConnectionString());
            //busConfiguration.UseTransport<AzureStorageQueueTransport>()
            //    .ConnectionString(BuidAzureString());

            busConfiguration.EndpointName("Testing.Yous");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.CustomConfigurationSource(new ConfigSource());

        return busConfiguration;
        }
        public static void Main(string[] args)
        {
            using (var bus = Bus.Create(GetConfig()).Start())
            {
                while (true)
                {
                    bus.Publish(new TestEvent() { AggregateId = Guid.NewGuid().ToString() });

                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    Console.WriteLine("Looped");
                }
            }
        }
    }
}
