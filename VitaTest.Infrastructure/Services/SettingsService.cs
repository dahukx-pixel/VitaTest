using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VitaTest.Infrastructure.Interfaces;

namespace VitaTest.AppCore.Services
{
    public sealed class SettingsService : ISettingsService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IDataUpdateService _dataUpdateService;
        private readonly string _settingsPath;

        public event EventHandler? OnChanged;

        public AppSettings Current => _settings.Value;

        public SettingsService(IOptions<AppSettings> settings, IDataUpdateService dataUpdateService)
        {
            _settings = settings;
            _dataUpdateService = dataUpdateService;

            _settingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        }

        public async Task UpdateAsync(Action<AppSettings> updateAction)
        {
            var settings = _settings.Value;

            updateAction(settings);
            await SaveToFileAsync();
        }

        public async Task SaveToFileAsync()
        {
            var json = JsonConvert.SerializeObject(new { AppSettings = _settings.Value });

            await File.WriteAllTextAsync(_settingsPath, json);

            OnChanged?.Invoke(this, EventArgs.Empty);
            _dataUpdateService.RaiseDataUpdate();
        }
    }
}
