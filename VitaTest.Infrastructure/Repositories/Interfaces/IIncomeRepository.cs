using VitaTest.Domain.Models;

namespace VitaTest.Infrastructure.Repositories.Interfaces
{
    public interface IIncomeRepository : IRepositoryBase<Income>
    {
        Task<decimal> GetCurrentBalanceAsync();
    }
}
