using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFrontModel
{
    public class InvoiceItems
    {
        public string Description { get; set; }
        public decimal Quantity { get; set; } // Can be negative or decimal
        public decimal Price { get; set; }    // >= 0
        public decimal DiscountPercent { get; set; } // 0 - 100
        public decimal NetAmount { get; set; }
        public decimal VatPercentage { get; set; }
        public decimal VatAmount { get; set; }
        public decimal LineAmount { get; set; }

        public decimal DiscountAmount { get; set; }
    }
}
