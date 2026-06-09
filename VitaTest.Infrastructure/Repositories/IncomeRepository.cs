using Microsoft.EntityFrameworkCore;
using VitaTest.Domain.Models;
using VitaTest.Infrastructure.Database;
using VitaTest.Infrastructure.Repositories.Interfaces;

namespace VitaTest.Infrastructure.Repositories
{
    public class IncomeRepository : RepositoryBase<Income>, IIncomeRepository
    {
        public IncomeRepository(VitaDatabase context) : base(context)
        {
        }

        public async Task<decimal> GetCurrentBalanceAsync()
        {
            return await Context.Incomes.Where(i => i.Balance > 0).SumAsync(i => i.Balance);
        }
    }
}
