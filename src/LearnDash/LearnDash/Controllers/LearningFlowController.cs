using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Core.Logging;
using LearnDash.Dal;
using LearnDash.Services;

namespace LearnDash.Controllers
{
    //[Authorize]
    public class LearningFlowController : Controller
    {
        public ILearningFlowService LearningFlowService { get; set; }

        public ILogger Logger { get; set; } 

        //todo : should open default learing flow
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var flow = LearningFlowService.Get(id);
            return View(flow);
            
        }

        [HttpPost]
        public ActionResult Edit(LearningFlow flow)
        {
            if (ModelState.IsValid)
            {
                LearningFlowService.Save(flow);
                return View(flow);
            }            
            return View();

            
        }


        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Remove(long id)
        {
            var flow = LearningFlowService.Get(id);
            if (flow != null)            
                return View(flow);              
            else
                return View("Error", ErrorType.NotFound);

        }

        [HttpPost]
        public ActionResult Remove(LearningFlow flow)
        {
            LearningFlowService.Remove(flow.Id);            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult List(long id)
        {
            ViewBag.Notification = Notification.ShowNotification(NotificationType.Succesfully_add);
            return View(id);
        }

        [HttpPost]
        public ActionResult Add(LearningFlow newFlow)
        {
            if (ModelState.IsValid)
            {
                newFlow.Tasks = new List<LearningTask>();
                var id = LearningFlowService.Save(newFlow);                
                return RedirectToAction("Edit", new {id});
            }
            return View();
        }


        [HttpPost]
        public ActionResult CompleteTask(long taskId)
        {
            if (taskId >= 0)
            {
                
            }
            return Json(Is.Success);
        }

        public ActionResult View(long id)
        {
            var flow = LearningFlowService.Get(id);
            if (flow != null)
                return View(flow);
            else
                return View("Error", ErrorType.NotFound);
        }

        [HttpPost]
        public ActionResult Save(LearningFlow flow)
        {
            LearningFlowService.Save(flow);
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
