using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LearnDash.Dal;

namespace LearnDash.Controllers
{
    public class LearningFlowController : Controller
    {
        // GET: /LearningFlow/

        public ActionResult Index()
        {
            var testFlow =  RedisDal.GetTestFlow();
            return View("View",testFlow);
        }


        public ActionResult View(long id)
        {
            var flow = RedisDal.Get(id);
            return View(flow);
        }

        [HttpPost]
        public ActionResult Save(LearningFlow flow)
        {
            RedisDal.Save(flow);
            return Json(Is.Success);
        }
    }

    public static class Is
    {
        public static object Success
        {
            get
            {
                return new
                           {
                               isSuccess = true
                           };
            }
        }

        public class Fail
        {
            public static object Message(string message)
            {
                return new
                           {
                               isSuccsess = false,
                               message
                           };
            }
        }
    }
}
