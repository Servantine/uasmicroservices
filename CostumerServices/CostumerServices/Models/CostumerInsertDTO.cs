using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Models
{
    public class CustomerInsertDTO
    {
        public int CustomerId { get; set; }
        public string Username { get; set; }= null!;
        public string Password { get; set; }= null!;
        public string FullName { get; set; }= null!;
    }
}