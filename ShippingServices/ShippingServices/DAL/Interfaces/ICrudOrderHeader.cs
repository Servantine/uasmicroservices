using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShippingServices.Models;

namespace ShippingServices.DAL.Interfaces
{
    public interface ICrudOrderHeader<T>
    {
        IEnumerable<T> GetAll();
        T GetByOrderHeaderId(int id);
        OrderHeader Insert(T obj);
        void Update(T obj);
        void Delete(int id);
    }
}