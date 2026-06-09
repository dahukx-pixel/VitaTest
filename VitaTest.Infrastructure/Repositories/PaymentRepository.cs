using VitaTest.Domain.Models;
using VitaTest.Infrastructure.Database;
using VitaTest.Infrastructure.Repositories.Interfaces;

namespace VitaTest.Infrastructure.Repositories
{
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(VitaDatabase context) : base(context)
        {
        }
    }
}
