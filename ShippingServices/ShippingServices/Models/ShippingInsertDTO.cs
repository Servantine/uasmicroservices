using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingServices.Models
{
    public class ShippingInsertDTO
    {
        public int ShippingId { get; set; }
        public string ShippingVendor { get; set; } = null!;
        public DateTime ShippingDate { get; set; }
        public string ShippingStatus { get; set; } = null!;
        public int OrderHeaderId { get; set; }
        public int BeratBarang { get; set; }
        public int BiayaShipping { get; set; }
    }
}