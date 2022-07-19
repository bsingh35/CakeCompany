using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using CakeCompany.Interface;
using CakeCompany.Models;
using CakeCompany.Models.Transport;
using Microsoft.Extensions.Logging;
namespace CakeCompany.Provider;

public class ShipmentProvider:IShipmentProvider
{
   private readonly ILogger<ShipmentProvider> logger;
   private readonly IOrderProvider orderProvider;
   private readonly ICakeProvider cakeProvider ;
   private readonly IPaymentProvider paymentProvider ;
   private readonly ITransportProvider transportProvider ;
   private List<Product> _productList=new List<Product>();
   private string _message=string.Empty;
   public List<Product> ProductList {get=>_productList;set{_productList=value;}}   
   public string Message {get=>_message;set{_message=value;}}

//Using microsoft dependency injection to initialize object
   public ShipmentProvider(IOrderProvider _orderProvider,ICakeProvider  _cakeProvider,IPaymentProvider _paymentProvider,ITransportProvider _transportProvider,ILogger<ShipmentProvider> _logger )
   {
    orderProvider= _orderProvider;
    cakeProvider=  _cakeProvider;
    paymentProvider=_paymentProvider;
    transportProvider= _transportProvider;
    logger=_logger;
   }
   
    /// <summary>
    ///  Method to get Shipment details
    /// </summary>
    public void GetShipment()
    {
        try
        {
        //Call order to get new orders
        logger.LogInformation("Get Shipment Started");

        ProductList=GetOrderDetails();
        GetTransportDetails(ProductList);
        logger.LogInformation("Get Shipment End");
        }
        catch(Exception ex)
        {
            Message= "Shipment failed";
            logger.LogInformation($"{Message}{ex.Message}");
        }
    }

    /// <summary>
    ///  Method to get Order Details 
    /// </summary>
    private List<Product> GetOrderDetails()
    {
      logger.LogInformation("Get Order Details Start");
      var orders = orderProvider.GetLatestOrders();
      var products = new List<Product>();
        if (!orders.Any())
        {
            return products;
        }

        var cancelledOrders = new List<Order>();
      

        foreach (var order in orders)
        {
            var estimatedBakeTime = cakeProvider.Check(order);
            if (estimatedBakeTime > order.EstimatedDeliveryTime)
            {
                cancelledOrders.Add(order);
                continue;
            }
            if (!paymentProvider.Process(order).IsSuccessful)
            {
                cancelledOrders.Add(order);
                continue;
            }
            var product = cakeProvider.Bake(order);
            products.Add(product);        
        }
        logger.LogInformation("Get Order Details End");
        return products;
    }

    /// <summary>
    ///  Method to get Transport Details 
    /// </summary>
    private void GetTransportDetails(List<Product> products)
    {
         logger.LogInformation("Get Transport Details  Started");
         var transport = transportProvider.CheckForAvailability(products);

        if (transport == "Van")
        {
            var van = new Van();
            van.Deliver(products);
        }

        if (transport == "Truck")
        {
            var truck = new Truck();
            truck.Deliver(products);
        }

        if (transport == "Ship")
        {
            var ship = new Ship();
            ship.Deliver(products);
        }
         logger.LogInformation("Get Transport Details  Started");
    }
}
