using Microsoft.EntityFrameworkCore;
using VitaTest.Infrastructure.Database;
using VitaTest.Infrastructure.Repositories.Interfaces;

namespace VitaTest.Infrastructure.Repositories
{
    public class ProcedureRepository : IProcedureRepository
    {
        private readonly VitaDatabase _database;

        public ProcedureRepository(VitaDatabase database)
        {
            _database = database;
        }

        public async Task<int> ProcessPayment(decimal paymentAmount, int orderId)
        {
            return await _database.Database.ExecuteSqlInterpolatedAsync($"EXEC [dbo].[SP_ProcessPayment_FIFO] @PaymentAmount = {paymentAmount}, @OrderID = {orderId}");
        }
    }
}
