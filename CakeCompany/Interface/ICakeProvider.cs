using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CakeCompany.Models;

namespace CakeCompany.Interface
{
    public interface ICakeProvider
    {
        DateTime Check(Order order);
        Product Bake(Order order);
    }
}