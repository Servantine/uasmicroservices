using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServices.Models;

namespace OrderServices.DAL.Interfaces
{
    public interface IOrderHeader : ICrudOrderHeader<OrderHeader>
    {
        IEnumerable<OrderHeader> GetByOrderDate(DateTime date);
    }
}