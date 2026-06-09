namespace VitaTest.Domain.Models
{
    public class Income : BaseDbEntity
    {
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<Payment>? Payments { get; set; }
    }
}
