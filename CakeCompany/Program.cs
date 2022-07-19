// See https://aka.ms/new-console-template for more information

using CakeCompany.Interface;
using CakeCompany.Provider;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
public class Program
{
    public static void Main(string[] args)
    {
       //setup our DI
        var serviceProvider = new ServiceCollection()
            .AddLogging(loggingBuilder => loggingBuilder
                                         .AddConsole()
                                        .SetMinimumLevel(LogLevel.Information)
                                                         )
            .AddSingleton<IOrderProvider, OrderProvider>()
            .AddSingleton<ICakeProvider, CakeProvider>()
            .AddSingleton<IPaymentProvider, PaymentProvider>()
            .AddSingleton<ITransportProvider, TransportProvider>()
            .AddSingleton<IShipmentProvider,ShipmentProvider>()
            .BuildServiceProvider();


     //configure console logging
            var logger = serviceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger<Program>();
             logger.LogInformation($"Starting application:");
 //do the actual work here
           var shipmentProvider = serviceProvider.GetRequiredService<IShipmentProvider>();
           shipmentProvider.GetShipment();
           logger.LogInformation("All done");
           logger.LogDebug("All done");

 }
}