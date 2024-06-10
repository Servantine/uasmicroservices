using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentServices.Models;

namespace PaymentServices.DAL.Interfaces
{
    public interface ICrudPayment<T>
    {
        IEnumerable<T> GetAll();
        T GetByPaymentId(int id);
        Payment Insert(T obj);
        void Update(T obj);
        void Delete(int id);
    }
}