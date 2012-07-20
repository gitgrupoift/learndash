using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnDash.Controllers
{
    public class Notification
    {
        #region Field Region
        
        private NotificationType type;
        private DateTime notificationTime;

        #endregion

        #region Property Region
        
        public NotificationType Type
        {
            get { return type; }
            set { type = value; }
        }        

        public DateTime NotificationTime
        {
            get { return notificationTime; }
            set { notificationTime = value; }
        }

        #endregion

        #region Constructor Region

        public Notification()
        {

        }

        public Notification(NotificationType type, DateTime notificationType)
        {
            Type = type;
            NotificationTime = notificationType;
        }

        #endregion

        #region Notification Methods

        public static string ShowNotification(NotificationType type)
        {
            return type.ToString().Replace("_", " ");
        }

        #endregion
    }
}