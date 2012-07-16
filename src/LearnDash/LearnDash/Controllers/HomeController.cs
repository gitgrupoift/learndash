using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;
using LearnDash.Dal;
using LearnDash.Dal.Models;
using LearnDash.Dal.NHibernate;

namespace LearnDash.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public IRepository<UserProfile> userRepository { get; set; } 

        public ActionResult Index()
        {
            // refactor : here we are using GetByParameteres euqls which return IList unnecesary implement nhibernate Linq :X
            var user = this.userRepository.GetByParameterEqualsFilter("UserId", User.Identity.Name).SingleOrDefault();

            if (user != null)
            {
                var flows = user.Dashboards.First().Flows.ToList();
                ViewBag.Notification = Notification.ShowNotification(NotificationType.Succesfully_add);
                return View(flows);
            }
            else
            {
                Logger.Warn("User '{0}' doesn't exist \r\nPropably session should be recycled and register procedure performed again.", User.Identity.Name);
                return RedirectToAction("Logout", "Account");
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
