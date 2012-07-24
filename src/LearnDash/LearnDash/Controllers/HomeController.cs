using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;
using LearnDash.Dal;
using LearnDash.Dal.Models;
using LearnDash.Dal.NHibernate;
using System.Threading;

namespace LearnDash.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        public ActionResult Index()
        {
            return RedirectToAction("List", "LearningFlow");
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
