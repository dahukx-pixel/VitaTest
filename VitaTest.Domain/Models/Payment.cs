using System;
using System.Collections.Generic;
using System.Text;

namespace VitaTest.Domain.Models
{
    public class Payment : BaseDbEntity
    {
        public int OrderId { get; set; }
        public int IncomeId { get; set; }
        public decimal PaymentAmount { get; set; }
        public Order? Order { get; set; }
        public Income? Income { get; set; }
    }
}
