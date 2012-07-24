using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;

namespace LearnDash.Controllers
{
    public class NotificationController : Controller
    {
        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();         

        public ActionResult Index()
        {            
            
            return View("Index", "Home");
        }

        public ActionResult Add()
        {
            if (Session["ListOfNotification"] != null)
            {
               
            }
            else
            {
                Logger.Warn("Notification Session doesn't exist ");                
            }

            return View("Index", "Home");
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Notification/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
