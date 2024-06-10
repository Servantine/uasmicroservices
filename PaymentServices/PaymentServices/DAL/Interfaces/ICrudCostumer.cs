using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentServices.Models;

namespace PaymentServices.DAL.Interfaces
{
    public interface ICrudCustomer<T>
    {
        IEnumerable<T> GetAll();
        T GetByCustomerId(int id);
        Customer Insert(T obj);
        void Update(T obj);
        void Delete(int id);
    }
}