using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentServices.Models;

namespace PaymentServices.DAL.Interfaces
{
    public interface IPayment : ICrudPayment<Payment>
    {
        IEnumerable<Payment> GetByPaymentName(string name);

    }

}