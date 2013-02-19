namespace LearnDash
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Castle.Windsor;

    using Controllers;

    using Core;

    using Services;

    public static class SessionManager
    {
        private const string CurrentUserKey = "CurrentUserSession";
        private const string CurrentListOfNotificationKey = "ListOfNotification";

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();


        public static IUserService UserService 
        { 
            get
            {
                return Container.Resolve<IUserService>();
            } 
        } 


        public static UserProfileSession CurrentUserSession
        {
            get
            {
                var data = Session<UserProfileSession>(CurrentUserKey);

                if (data == null)
                {
                    Logger.Warn("No Current User in Session. \r\n Retrieving UserProfile linked with current authentication token.");
                    var user = UserService.GetCurrentUser();
                    data = new UserProfileSession
                        {
                            ID = user.ID,
                            MainDashboardId = user.Dashboards.First().ID,
                            UserId = user.UserId
                        };
                    HttpContext.Current.Session[CurrentUserKey] = data;
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
                    Logger.Warn("No Current Notification in Session \r\n Creating empty notification list.");
                    data = new List<Notification>();
                    HttpContext.Current.Session[CurrentListOfNotificationKey] = data;
                }

                return data;
            }

            set
            {
                Logger.Trace("Notification session created");
                HttpContext.Current.Session[CurrentListOfNotificationKey] = value;
            }
        }

        internal static IWindsorContainer Container { private get; set; }

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