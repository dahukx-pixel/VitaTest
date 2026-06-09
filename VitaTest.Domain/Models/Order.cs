namespace VitaTest.Domain.Models
{
    public class Order : BaseDbEntity
    {
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime UpdatedAt { get; set; }

        public decimal BalanceToPay => TotalAmount - PaidAmount;

        public IEnumerable<Payment>? Payments { get; set; }
    }
}
