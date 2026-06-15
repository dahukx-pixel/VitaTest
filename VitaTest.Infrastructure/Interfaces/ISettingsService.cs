using VitaTest.AppCore;

namespace VitaTest.Infrastructure.Interfaces
{
    public interface ISettingsService
    {
        AppSettings Current { get; }
        event EventHandler? OnChanged;
        Task UpdateAsync(Action<AppSettings> updateAction);
        Task SaveToFileAsync();
    }
}
