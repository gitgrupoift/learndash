using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnDash.Controllers
{
    public class Notification
    {       
        public static string ShowNotification(NotificationType type)
        {
            return type.ToString().Replace("_", " ");
        }        
    }
}