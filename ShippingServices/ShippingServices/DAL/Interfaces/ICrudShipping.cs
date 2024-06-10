using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShippingServices.Models;

namespace ShippingServices.DAL.Interfaces
{
    public interface ICrudShipping<T>
    {
        IEnumerable<T> GetAll();
        T GetByShippingId(int id);
        Shipping Insert(T obj);
        void Update(T obj);
        void Delete(int id);
    }
}