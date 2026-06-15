using VitaTest.Domain.Models;

namespace VitaTest.Infrastructure.Repositories.Interfaces
{
    public interface IIncomeRepository : IRepositoryBase<Income>
    {
        Task<List<Income>> GetAvailableIncomesAsync();
        Task<decimal> GetCurrentBalanceAsync();
    }
}
