using VitaTest.Domain.Models;
using VitaTest.Infrastructure.Database;
using VitaTest.Infrastructure.Repositories.Interfaces;

namespace VitaTest.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(VitaDatabase context) : base(context)
        {

        }
    }
}
