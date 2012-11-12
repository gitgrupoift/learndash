namespace LearnDash.Controllers
{
    public class Notification
    {
        public NotificationType Type { get; set; }

        public string Message { get; set; }

        public static void Notify(NotificationType type, string message)
        {
            var newNotification = new Notification { Type = type, Message = message };
            SessionManager.ListOfNotifications.Add(newNotification);
        }

    }
}