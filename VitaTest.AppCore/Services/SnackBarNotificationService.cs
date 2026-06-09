using MaterialDesignThemes.Wpf;
using VitaTest.AppCore.Enums;
using VitaTest.AppCore.Records;
using VitaTest.AppCore.Services.Interfaces;

namespace VitaTest.AppCore.Services
{
    public sealed class SnackBarNotificationService : INotificationService
    {
        private ISnackbarMessageQueue _messageQueue;

        public SnackBarNotificationService(ISnackbarMessageQueue snackbarMessageQueue)
        {
            _messageQueue = snackbarMessageQueue;
        }

        public void Notify(string message, NotifyType type, TimeSpan? duration = null, bool promote = false)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            var notification = new NotificationMessage(message, type);

            _messageQueue.Enqueue(content: notification,
                                  actionContent: null,
                                  actionHandler: null,
                                  actionArgument: null,
                                  promote: promote,
                                  neverConsiderToBeDuplicate: true,
                                  durationOverride: duration);
        }
    }
}
