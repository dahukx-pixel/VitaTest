using VitaTest.AppCore.Enums;

namespace VitaTest.AppCore.Services.Interfaces
{
    public interface INotificationService
    {
        void Notify(string message, NotifyType type, TimeSpan? duration = null, bool promote = false);
    }
}
