using VitaTest.Infrastructure.Repositories.Interfaces;

namespace VitaTest.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IDatabaseVersionRepository DatabaseVersionRepository { get; }
        IIncomeRepository IncomeRepository { get; }
        IOrderRepository OrderRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IProcedureRepository ProcedureRepository { get; }

        event EventHandler? DatabaseUpdated;

        Task<bool> ClearChanges();
        void Dispose();
        Task<string> SaveChangesAsync();
    }
}
