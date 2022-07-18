using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CakeCompany.Interface;
using CakeCompany.Models;
using CakeCompany.Provider;
using Moq;
using NUnit;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace CakeCompany.UnitTest.Test
{

[TestFixture]
    public class ShipmentProviderTest
    {
        private readonly Mock<IOrderProvider> _mockOrderProvider;
        private readonly Mock<ICakeProvider>  _mockCakeProvider ;
        private readonly Mock<IPaymentProvider> _mockPaymentProvider ;
        private readonly Mock<ITransportProvider> _mockTransportProvider ;

        private readonly Mock<ILogger> _mockLogger;

        public ShipmentProviderTest()
        {
            
            _mockOrderProvider= new Mock<IOrderProvider>();
            _mockCakeProvider=new Mock<ICakeProvider>();
            _mockPaymentProvider=new Mock<IPaymentProvider>();
            _mockTransportProvider=new Mock<ITransportProvider>();
            _mockLogger=new Mock<ILogger>();

        }

        /// <summary>
        /// Testing method for Product List
       /// </summary>
        [Test]
        public void GetShipment_Get_Product_List()
        {
         //Arrange
        var order= new Order[]
        {
            new(
                "CakeBox Edgware",DateTime.Now,1,Cake.RedVelvet,120.25)
        };
         var expectedProduct=new Product
         {
            
            Id =Guid.NewGuid(),
            Cake =Cake.Chocolate,
            Quantity =10,
            OrderId =1
         };
         var expectedProductList= new List<Product>();
         expectedProductList.Add(expectedProduct);
         var estimatedBakeTime =DateTime.Now.AddDays(-1);
          var paymentIn=new PaymentIn
            {
                HasCreditLimit = false,
                IsSuccessful = true
            };
         _mockOrderProvider.Setup(x=>x.GetLatestOrders()).Returns(order);
         _mockCakeProvider.Setup(x=>x.Check(It.IsAny<Order>())).Returns(estimatedBakeTime);
         _mockCakeProvider.Setup(x=>x.Bake(It.IsAny<Order>())).Returns(expectedProduct);
         _mockPaymentProvider.Setup(x=>x.Process(It.IsAny<Order>())).Returns(paymentIn);
         _mockTransportProvider.Setup(x=>x.CheckForAvailability(It.IsAny<List<Product>>())).Returns("Car");
          var sut= new ShipmentProvider(_mockOrderProvider.Object,_mockCakeProvider.Object,_mockPaymentProvider.Object,_mockTransportProvider.Object,_mockLogger.Object);

        // Act
        sut.GetShipment();
        var actualProductList =sut.ProductList;

         //Assert
         Assert.NotNull(expectedProduct);
         expectedProductList.Should().BeEquivalentTo(actualProductList);
        } 

   /// <summary>
   /// Test Method to generate exception
   /// </summary>
    [Test]
 public void GetShipment_Order_Exception()
 {
    //Arrange
        var order= new Order[0];
        var expectedProductList= new List<Product>();
        var exception= new Exception();
        var expectedMessage="Shipment failed";
        _mockOrderProvider.Setup(x=>x.GetLatestOrders()).Throws(exception);
        var sut= new ShipmentProvider(_mockOrderProvider.Object,_mockCakeProvider.Object,_mockPaymentProvider.Object,_mockTransportProvider.Object,_mockLogger.Object);

    // Act
        sut.GetShipment();
        var actualMessage =sut.Message;

    //Assert
         Assert.AreEqual(expectedMessage,actualMessage);
 }

   /// <summary>
   /// Test Method to check empty order.
   /// </summary>
 [Test]
 public void GetShipment_Order_Empty()
 {
    //Arrange
        var order= new Order[0];
        var expectedProductList= new List<Product>();
        _mockOrderProvider.Setup(x=>x.GetLatestOrders()).Returns(order);
        _mockOrderProvider.Setup(x=>x.GetLatestOrders()).Returns(order);
        var sut= new ShipmentProvider(_mockOrderProvider.Object,_mockCakeProvider.Object,_mockPaymentProvider.Object,_mockTransportProvider.Object,_mockLogger.Object);

    // Act
        sut.GetShipment();
        var actualProductList =sut.ProductList;

    //Assert
      expectedProductList.Should().BeEquivalentTo(actualProductList);
 }

    }
}