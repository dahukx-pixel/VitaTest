namespace VitaTest.Infrastructure.Repositories.Interfaces
{
    public interface IProcedureRepository
    {
        Task<int> ProcessPayment(decimal paymentAmount, int orderId);
    }
}
