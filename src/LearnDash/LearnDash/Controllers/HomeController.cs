using System.Web.Mvc;

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
