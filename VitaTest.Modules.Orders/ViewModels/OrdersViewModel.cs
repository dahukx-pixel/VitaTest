using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using VitaTest.AppCore.Enums;
using VitaTest.AppCore.Services.Interfaces;
using VitaTest.Domain.Models;
using VitaTest.Infrastructure.Interfaces;
using VitaTest.Modules.Orders.Validators;

namespace VitaTest.Modules.Orders.ViewModels
{
    public class OrdersViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly IDataUpdateService _dataUpdateService;

        private decimal _newOrderTotalAmount;
        private decimal _balance;
        private decimal _paySum;

        private List<Order> _orders;
        private List<Income> _availableIncomes;

        private Income _selectedIncome;

        public decimal NewOrderTotalAmount
        {
            get => _newOrderTotalAmount;
            set => SetProperty(ref _newOrderTotalAmount, value);
        }

        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }

        public decimal PaySum
        {
            get => _paySum;
            set => SetProperty(ref _paySum, value);
        }

        public List<Order> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        public List<Income> AvailableIncomes
        {
            get => _availableIncomes;
            set => SetProperty(ref _availableIncomes, value);
        }

        public Income SelectedIncome
        {
            get => _selectedIncome;
            set => SetProperty(ref _selectedIncome, value);
        }

        public ICommand CreateOrderCommand => new DelegateCommand(CreateOrder);
        public ICommand PayCommand => new DelegateCommand<Order>(Pay);

        public OrdersViewModel(IUnitOfWork unitOfWork,
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

        private async Task InitializeData()
        {
            await _unitOfWork.ClearChanges();
            Orders = await _unitOfWork.OrderRepository.GetAllAsync();
            AvailableIncomes = await _unitOfWork.IncomeRepository.GetAvailableIncomesAsync();
            Balance = await _unitOfWork.IncomeRepository.GetCurrentBalanceAsync();
        }

        private async void CreateOrder()
        {
            OrderValidator validator = new OrderValidator();
            var validationResult = validator.Validate(_newOrderTotalAmount);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _notificationService.Notify($"Сумма платежа: {error.ErrorMessage}.",
                                                NotifyType.Error,
                                                TimeSpan.FromSeconds(2));
                }

                return;
            }

            var nowTime = DateTime.UtcNow;

            await _unitOfWork.OrderRepository.AddAsync(new Order()
            {
                TotalAmount = _newOrderTotalAmount,
                PaidAmount = 0,
                CreatedAt = nowTime,
                UpdatedAt = nowTime
            });

            var result = await _unitOfWork.SaveChangesAsync();

            if (string.IsNullOrEmpty(result))
            {
                await InitializeData();
                _notificationService.Notify("Платёж успешно добавлен.", NotifyType.Info);

                NewOrderTotalAmount = 0;
            }
            else
            {
                _notificationService.Notify($"Ошибка: {result}", NotifyType.Error);
            }
        }

        private async void Pay(Order order)
        {
            if (SelectedIncome is null)
            {
                _notificationService.Notify("Необходимо выбрать пополнение для оплаты.", NotifyType.Warning);
                return;
            }

            await _unitOfWork.ClearChanges();

            var orderFromDb = await _unitOfWork.OrderRepository.GetAsync(order.ID);

            if (orderFromDb.UpdatedAt > order.UpdatedAt)
            {
                _notificationService.Notify("Не удалось выполнить операцию. Состояние заказа изменилось.", NotifyType.Warning);
                _dataUpdateService.RaiseDataUpdate();
                return;
            }

            var incomeFromDb = await _unitOfWork.IncomeRepository.GetAsync(_selectedIncome.ID);

            if (incomeFromDb.UpdatedAt > _selectedIncome.UpdatedAt)
            {
                _notificationService.Notify("Не удалось выполнить операцию. Состояние пополнения изменилось.", NotifyType.Warning);
                _dataUpdateService.RaiseDataUpdate();
                return;
            }

            await _unitOfWork.PaymentRepository.AddAsync(new Payment
            {
                OrderId = order.ID,
                IncomeId = _selectedIncome.ID,
                CreatedAt = DateTime.Now,
                PaymentAmount = _paySum
            });

            var result = await _unitOfWork.SaveChangesAsync();

            if (string.IsNullOrEmpty(result))
            {
                _notificationService.Notify("Оплата прошла успешно.", NotifyType.Info);
            }
            else
            {
                _notificationService.Notify($"Ошибка: {result}", NotifyType.Error);
            }

            PaySum = 0;
        }
    }
}
