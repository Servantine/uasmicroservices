using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentServices.Models
{
    public class PaymentGetDTO
    {
        public int PaymentId { get; set; }
        public int CustomerId { get; set; }
        public string PaymentWallet { get; set; } = null!;
        public int Saldo { get; set; }
    }
}