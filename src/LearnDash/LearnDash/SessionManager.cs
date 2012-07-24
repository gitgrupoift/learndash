namespace LearnDash
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Security;

    using LearnDash.Controllers;
    using LearnDash.Dal.Models;
    using LearnDash.Dal.NHibernate;

    public static class SessionManager
    {
        private const string CurrentUserKey = "CurrentUserSession";
        private const string CurrentListOfNotificationKey = "ListOfNotification";

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();



        public static UserProfileSession CurrentUserSession
        {
            get
            {
                var data = Session<UserProfileSession>(CurrentUserKey);

                if (data == null)
                {
                    Logger.Error("No Current User in Session. \r\nThis value should be set in Account Controller on Logon.\n deleting authentication token.");

                    FormsAuthentication.SignOut();
                }

                return data;
            }

            set
            {
                Logger.Trace("UserProfile session created");
                HttpContext.Current.Session[CurrentUserKey] = value;
            }
        }

        public static List<Notification> ListOfNotifications
        {            
            get
            {
                var data = Session<List<Notification>>(CurrentListOfNotificationKey);

                if (data == null)
                {
                    Logger.Error("No Current Notification in Session \r\n This Value should be set in Notification Method - Add while we add new notification");
                }

                return data;
            }

            set
            {
                Logger.Trace("Notification session created");
                HttpContext.Current.Session[CurrentListOfNotificationKey] = value;
            }
        }

        private static T Session<T>(string key)
            where T : class
        {
            if (HttpContext.Current.Session[key] != null)
            {
                var obj = HttpContext.Current.Session[key] as T;

                if (obj != null)
                {
                    return obj;
                }
            }

            return null;
        }
    }
}