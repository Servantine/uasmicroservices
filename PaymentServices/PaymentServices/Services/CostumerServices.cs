using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentServices.Models;

namespace PaymentServices.Services
{
    public interface ICustomerServices
    {
        Task<IEnumerable<Icostumers>> GetAllCostumers();
        Task<Icostumers> GetByCostumerId(int CustomerId);
    }

}