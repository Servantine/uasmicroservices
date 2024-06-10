using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShippingServices.Models;

namespace ShippingServices.DAL.Interfaces
{
    public interface IShipping : ICrudShipping<Shipping>
    {
        IEnumerable<Shipping> GetByShippingVendor(string name);

    }

}