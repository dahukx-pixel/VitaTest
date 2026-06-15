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

        public async Task<List<Income>> GetAvailableIncomesAsync()
        {
            try
            {
                return await Context.Incomes.Where(i => i.Balance > 0).ToListAsync();
            }
            catch (Exception ex)
            {
                //Logger
                return new List<Income>();
            }
        }

        public async Task<decimal> GetCurrentBalanceAsync()
        {
            try
            {
                return await Context.Incomes.Where(i => i.Balance > 0).SumAsync(i => i.Balance);
            }
            catch (Exception ex)
            {
                //Logger

                return 0;
            }
        }
    }
}
