namespace VitaTest.AppCore.Services.Interfaces
{
    public interface ISettingsService
    {
        AppSettings Current { get; }
        Task UpdateAsync(Action<AppSettings> updateAction);
        Task SaveToFileAsync();
    }
}
