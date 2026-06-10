using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using VitaTest.AppCore.Enums;
using VitaTest.AppCore.Services.Interfaces;
using VitaTest.Domain.Models;
using VitaTest.Infrastructure.Interfaces;

namespace VitaTest.Modules.Incomes.ViewModels
{
    public class IncomesViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly IDataUpdateService _dataUpdateService;

        private List<Income> _incomes;

        private decimal _incomeSum;

        public List<Income> Incomes
        {
            get => _incomes;
            set => SetProperty(ref _incomes, value);
        }

        public decimal IncomeSum
        {
            get => _incomeSum;
            set => SetProperty(ref _incomeSum, value);
        }

        public ICommand AddIncomeCommand => new DelegateCommand(AddIncome);

        public IncomesViewModel(IUnitOfWork unitOfWork,
                                INotificationService notificationService,
                                IDataUpdateService dataUpdateService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _dataUpdateService = dataUpdateService;

            _dataUpdateService.DataUpdated += OnDatabaseUpdated;

            _ = InitializeData();
        }

        private async void OnDatabaseUpdated(object sender, EventArgs e)
        {
            await InitializeData();
        }

        private async void AddIncome()
        {
            if (IncomeSum <= 0)
            {
                _notificationService.Notify("Сумма не может быть меньше или равна 0.", NotifyType.Warning);
                return;
            }

            await _unitOfWork.IncomeRepository.AddAsync(new Income()
            {
                Amount = IncomeSum,
                Balance = IncomeSum,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
            {
                _notificationService.Notify("Пополнение прошло успешно.", NotifyType.Info);
            }
            else
            {
                _notificationService.Notify("Не удалось внести средства.", NotifyType.Error);
            }
        }

        private async Task InitializeData()
        {
            await _unitOfWork.ClearChanges();
            Incomes = (await _unitOfWork.IncomeRepository.GetAllAsync()).OrderByDescending(i => i.UpdatedAt)
                                                                        .ToList();
        }
    }
}
