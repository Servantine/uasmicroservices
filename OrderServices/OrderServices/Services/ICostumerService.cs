using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServices.Models;

namespace OrderServices.Services
{
    public interface ICustomerService 
    {
        Task<IEnumerable<Costumers>> GetAllCostumers();
        Task<Costumers> GetByCostumerId(int CustomerId);
    }

}