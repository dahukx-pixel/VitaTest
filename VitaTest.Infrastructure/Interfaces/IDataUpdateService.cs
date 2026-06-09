namespace VitaTest.Infrastructure.Interfaces
{
    public interface IDataUpdateService
    {
        event EventHandler? DataUpdated;
        void RaiseDataUpdate();
    }
}
