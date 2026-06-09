using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitaTest.AppCore.Services.Interfaces;
using VitaTest.Domain.Models;
using VitaTest.Infrastructure.Interfaces;

namespace VitaTest.Modules.Payments.ViewModels
{
    public class PaymentsViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly IDataUpdateService _dataUpdateService;

        private List<Payment> _payments;
        public List<Payment> Payments
        {
            get => _payments;
            set => SetProperty(ref _payments, value);
        }

        public PaymentsViewModel(IUnitOfWork unitOfWork,
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
            Payments = (await _unitOfWork.PaymentRepository.GetAllAsync()).OrderByDescending(i => i.CreatedAt)
                                                                          .ToList();
        }
    }
}
