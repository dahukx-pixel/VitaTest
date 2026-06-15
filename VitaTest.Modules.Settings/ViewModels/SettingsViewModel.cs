using FluentValidation.Results;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using VitaTest.AppCore;
using VitaTest.AppCore.Enums;
using VitaTest.AppCore.Services.Interfaces;
using VitaTest.Infrastructure.Interfaces;
using VitaTest.Modules.Settings.Validators;

namespace VitaTest.Modules.Settings.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private readonly INotificationService _notificationService;
        private readonly ISettingsService _settingsService;

        private string _dbAddress;
        private string _dbName;
        private string _dbLogin;
        private bool _localDb;

        public string DbAddress
        {
            get => _dbAddress;
            set => SetProperty(ref _dbAddress, value);
        }
        public string DbName
        {
            get => _dbName;
            set => SetProperty(ref _dbName, value);
        }
        public string DbLogin
        {
            get => _dbLogin;
            set => SetProperty(ref _dbLogin, value);
        }
        public bool LocalDb
        {
            get => _localDb;
            set => SetProperty(ref _localDb, value);
        }

        public SettingsViewModel(INotificationService notificationService,
                                 ISettingsService settingsService)
        {
            _notificationService = notificationService;
            _settingsService = settingsService;

            InitializeData();
        }

        private void InitializeData()
        {
            DbAddress = _settingsService.Current.DbAddress;
            DbLogin = _settingsService.Current.DbLogin;
            DbName = _settingsService.Current.DbName;
            LocalDb = _settingsService.Current.LocalDb;
        }

        public async Task ProcessSave(string password)
        {
            var settings = new AppSettings()
            {
                LocalDb = _localDb,
                DbAddress = _dbAddress,
                DbLogin = _dbLogin,
                DbName = _dbName,
                DbPassword = password
            };

            SettingsValidator validator = new SettingsValidator();
            ValidationResult validationResult = validator.Validate(settings);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _notificationService.Notify($"{error.PropertyName} - {error.ErrorMessage}",
                                                NotifyType.Error,
                                                TimeSpan.FromSeconds(2));
                }

                return;
            }

            await _settingsService.UpdateAsync(settings =>
            {
                settings.LocalDb = LocalDb;
                settings.DbAddress = DbAddress.Replace("\\\\", "\\");
                settings.DbName = DbName;
                settings.DbLogin = DbLogin;
                settings.DbPassword = password;
            });

            _notificationService.Notify("Настройки успешно сохранены.",
                                        NotifyType.Info,
                                        TimeSpan.FromSeconds(2));
        }
    }
}
