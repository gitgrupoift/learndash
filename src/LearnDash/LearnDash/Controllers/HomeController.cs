using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;
using LearnDash.Dal;

namespace LearnDash.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        public ILogger Logger { get; set; }

        public ActionResult Index()
        {
            Logger.Info("Home Controller Index");
            var flows = LearningFlowRepository.GetAll();
            ViewBag.Notification = Notification.ShowNotification(NotificationType.Succesfully_add);
            
            return View(flows);
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
