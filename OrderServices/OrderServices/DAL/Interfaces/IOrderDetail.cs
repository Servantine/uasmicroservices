using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServices.Models;

namespace OrderServices.DAL.Interfaces
{
    public interface IOrderDetail : ICrudOrderDetail<OrderDetail>
    {
        
    }

    public interface ICrudOrderDetail<T>
    {
        IEnumerable<T> GetAll();
        T GetByOrderDetailId(int id);
        OrderDetail Insert(T obj);
        void Update(T obj);
        void Delete(int id);
    }
}