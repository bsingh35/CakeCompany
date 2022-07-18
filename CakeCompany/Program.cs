// See https://aka.ms/new-console-template for more information

using CakeCompany.Interface;
using CakeCompany.Provider;
using Microsoft.Extensions.Logging;

//Generate instance of the class using intialization of interface.
IOrderProvider orderProvider= new OrderProvider();
ICakeProvider cakeProvider=new CakeProvider();
IPaymentProvider paymentProvider=new PaymentProvider();
ITransportProvider transportProvider= new TransportProvider();

//Creating a logger factory for logging Information
ILoggerFactory loggerFactory=   LoggerFactory.Create(builder => builder
                .AddFilter("Microsoft", LogLevel.Debug)
                .AddFilter("System", LogLevel.Debug)
                .AddFilter("Namespace.Class", LogLevel.Debug)
                .AddConsole()
                );
 // Creating a logger to log message               
ILogger logger = loggerFactory.CreateLogger("Log Message");

//passing object to shipment provider class
var shipmentProvider = new ShipmentProvider(orderProvider,cakeProvider,paymentProvider,transportProvider,logger);
logger.LogInformation("Application  Started");
shipmentProvider.GetShipment();
Console.WriteLine("Shipment Details...");
logger.LogInformation("Application End ");
