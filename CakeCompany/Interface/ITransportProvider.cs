using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CakeCompany.Models;

namespace CakeCompany.Interface
{
    public interface ITransportProvider
    {
        string CheckForAvailability(List<Product> products);
    }
}