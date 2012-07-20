using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Castle.Core.Logging;
using LearnDash.Dal.Models;
using LearnDash.Dal.NHibernate;
using LearnDash.Controllers;

namespace LearnDash
{
    public static class SessionManager
    {
        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private const string CurrentUserKey = "CurrentUser";
        private const string CurrentListOfNotification = "ListOfNotification";
        public static UserProfile CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session[CurrentUserKey] != null)
                    return HttpContext.Current.Session[CurrentUserKey] as UserProfile;
                else
                {
                    Logger.Error("No Current User in Session. \r\nThis value should be set in Account Controller on Logon.\n deleting authentication token.");

                    FormsAuthentication.SignOut();
                    return null;
                }
            }
            set { HttpContext.Current.Session[CurrentUserKey] = value; }
        }

        public static List<Notification> ListOfNotifications
        {            
            get
            {
                if (HttpContext.Current.Session[CurrentListOfNotification] != null)
                    return HttpContext.Current.Session[CurrentListOfNotification] as List<Notification>;
                else
                {
                    Logger.Error("No Current Notification in Session \r\n This Value should be set in Notification Method - Add while we add new notification");
                    return null;
                }
            }
            set
            {                
                HttpContext.Current.Session[CurrentListOfNotification] = value;
            }
        }
    }
}