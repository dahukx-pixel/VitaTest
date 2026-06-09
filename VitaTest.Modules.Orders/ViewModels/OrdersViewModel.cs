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
            Orders = (await _unitOfWork.OrderRepository.GetAllAsync()).ToList();
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

            var updatesCount = await _unitOfWork.SaveChangesAsync();

            if (updatesCount == 0)
            {
                await InitializeData();
                _notificationService.Notify("Платёж успешно добавлен.", NotifyType.Info);

                NewOrderTotalAmount = 0;
            }
            else
            {
                _notificationService.Notify("Не удалось добавить платёж.", NotifyType.Error);
            }
        }

        private async void Pay(Order order)
        {
            if (_paySum == 0)
            {
                _notificationService.Notify("Сумма оплаты должна быть больше 0.", NotifyType.Warning);
                return;
            }

            if (_paySum > order.BalanceToPay)
            {
                _notificationService.Notify("Сумма оплаты не может быть больше необходимой в заказе.", NotifyType.Warning);
                return;
            }

            if (_balance < _paySum)
            {
                _notificationService.Notify("Недостаточно средств для совершения платежа.", NotifyType.Warning);
                return;
            }

            await _unitOfWork.ProcedureRepository.ProcessPayment(PaySum, order.ID);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
            {
                _notificationService.Notify("Оплата прошла успешно.", NotifyType.Info);
            }
            else
            {
                _notificationService.Notify("Не удалось провести оплату.", NotifyType.Error);
            }
        }
    }
}
