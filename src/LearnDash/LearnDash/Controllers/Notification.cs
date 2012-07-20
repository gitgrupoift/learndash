using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnDash.Controllers
{
    public class Notification
    {

        public NotificationType Type { get; set; }
        public DateTime NotificationTime { get; set; }

        public Notification(NotificationType type, DateTime notificationType)
        {
            Type = type;
            NotificationTime = notificationType;
        }

        public static void Add(Notification Item)
        {
            List<Notification> list = null;

            if (SessionManager.ListOfNotifications != null)
            {
                list = SessionManager.ListOfNotifications;
                list.Add(Item);
                SessionManager.ListOfNotifications = list;
            }
            else
            {
                list = new List<Notification>();
                list.Add(Item);
                SessionManager.ListOfNotifications = list;
            }
        }

    }



}