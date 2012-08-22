namespace LearnDash.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

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