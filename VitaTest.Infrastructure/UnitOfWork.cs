using Microsoft.EntityFrameworkCore;
using VitaTest.Infrastructure.Database;
using VitaTest.Infrastructure.Interfaces;
using VitaTest.Infrastructure.Repositories;
using VitaTest.Infrastructure.Repositories.Interfaces;

namespace VitaTest.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISettingsService _settingsService;
        private readonly IDataUpdateService _dataUpdateService;
        private readonly IDbContextFactory<VitaDatabase> _contextFactory;
        private VitaDatabase _context;

        private VitaDatabase Context => _context ??= _contextFactory.CreateDbContext();

        private IOrderRepository _orderRepository;
        private IIncomeRepository _incomeRepository;
        private IPaymentRepository _paymentRepository;
        private IDatabaseVersionRepository _dataBaseVersionRepository;
        private IProcedureRepository _procedureRepository;

        private object _reinitializeLocker;

        public IOrderRepository OrderRepository => _orderRepository;
        public IIncomeRepository IncomeRepository => _incomeRepository;
        public IPaymentRepository PaymentRepository => _paymentRepository;
        public IDatabaseVersionRepository DatabaseVersionRepository => _dataBaseVersionRepository;
        public IProcedureRepository ProcedureRepository => _procedureRepository;

        public event EventHandler? DatabaseUpdated;



        public UnitOfWork(IDbContextFactory<VitaDatabase> contextFactory,
                          IDataUpdateService dataUpdateService,
                          ISettingsService settingsService)
        {
            _contextFactory = contextFactory;
            _dataUpdateService = dataUpdateService;
            _settingsService = settingsService;
            _reinitializeLocker = new();

            _settingsService.OnChanged += ReinitializeDb;

            InitializeRepositories();
        }

        private void InitializeRepositories()
        {
            _orderRepository = new OrderRepository(Context);
            _incomeRepository = new IncomeRepository(Context);
            _paymentRepository = new PaymentRepository(Context);
            _dataBaseVersionRepository = new DatabaseVersionRepository(Context);
            _procedureRepository = new ProcedureRepository(Context);
        }

        private void ReinitializeDb(object? sender, EventArgs e)
        {
            try
            {
                lock (_reinitializeLocker)
                {
                    _context?.Dispose();
                    _context = null;

                    InitializeRepositories();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка пересоздания БД: {ex.Message}");
            }
        }

        public async Task<string> SaveChangesAsync()
        {
            try
            {
                var result = await Context.SaveChangesAsync();

                if (result == 0)
                {
                    _dataUpdateService.RaiseDataUpdate();
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                //logger
                return ex.InnerException?.Message ?? ex.Message;
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
