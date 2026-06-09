using Microsoft.EntityFrameworkCore;
using VitaTest.Infrastructure.Database;
using VitaTest.Infrastructure.Interfaces;
using VitaTest.Infrastructure.Repositories;
using VitaTest.Infrastructure.Repositories.Interfaces;

namespace VitaTest.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDataUpdateService _dataUpdateService;
        private readonly IDbContextFactory<VitaDatabase> _contextFactory;
        private VitaDatabase _context;

        private VitaDatabase Context => _context ??= _contextFactory.CreateDbContext();

        private readonly IOrderRepository _orderRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IDatabaseVersionRepository _dataBaseVersionRepository;
        private readonly IProcedureRepository _procedureRepository;
        public IOrderRepository OrderRepository => _orderRepository;
        public IIncomeRepository IncomeRepository => _incomeRepository;
        public IPaymentRepository PaymentRepository => _paymentRepository;
        public IDatabaseVersionRepository DatabaseVersionRepository => _dataBaseVersionRepository;
        public IProcedureRepository ProcedureRepository => _procedureRepository;

        public event EventHandler? DatabaseUpdated;

        public UnitOfWork(IDbContextFactory<VitaDatabase> contextFactory,
                          IDataUpdateService dataUpdateService)
        {
            _contextFactory = contextFactory;
            _dataUpdateService = dataUpdateService;

            _orderRepository = new OrderRepository(Context);
            _incomeRepository = new IncomeRepository(Context);
            _paymentRepository = new PaymentRepository(Context);
            _dataBaseVersionRepository = new DatabaseVersionRepository(Context);
            _procedureRepository = new ProcedureRepository(Context);
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                var result = await Context.SaveChangesAsync();

                if (result == 0)
                {
                    _dataUpdateService.RaiseDataUpdate();
                }

                return result;
            }
            catch
            {
                return 1;
            }
        }

        public async Task<bool> ClearChanges()
        {
            try
            {
                Context.ChangeTracker.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
