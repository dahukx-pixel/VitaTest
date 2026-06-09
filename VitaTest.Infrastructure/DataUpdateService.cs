using VitaTest.Infrastructure.Interfaces;

namespace VitaTest.Infrastructure
{
    public class DataUpdateService : IDataUpdateService
    {
        public event EventHandler? DataUpdated;

        public void RaiseDataUpdate()
        {
            DataUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
