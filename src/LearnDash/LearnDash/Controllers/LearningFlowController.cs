﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Core.Logging;
using LearnDash.Dal;
using LearnDash.Dal.Models;
using LearnDash.Dal.NHibernate;
using LearnDash.Services;

namespace LearnDash.Controllers
{
    [Authorize]
    public class LearningFlowController : Controller
    {
        public ILearningFlowService LearningFlowService { get; set; }

        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        //todo : should open default learing flow
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var flow = LearningFlowService.Get(id);
            return View(flow);
        }

        [HttpPost]
        public ActionResult Edit(LearningFlow flow)
        {
            if (ModelState.IsValid)
            {
                LearningFlowService.Update(flow);
                return View(flow);
            }

            return View();
        }


        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(LearningFlow newFlow)
        {
            if (ModelState.IsValid)
            {
                newFlow.Tasks = new List<LearningTask>();
                var id = LearningFlowService.Add(newFlow);
                return RedirectToAction("Edit", new { id });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Remove(int id)
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
            LearningFlowService.Remove(flow.ID);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult List(long id)
        {
            return View(id);
        }

        //todo : Complete action should be a separate action that doesnt send whole data as it happens with save action
        [HttpPost]
        public ActionResult CompleteTask(long taskId)
        {
            if (taskId >= 0)
            {
                
            }
            return Json(Is.Success);
        }

        public ActionResult View(int id)
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
            if (ModelState.IsValid)
            {
                LearningFlowService.Update(flow);
                return Json(Is.Success);
            }
            return Json(Is.Fail.Message("Save Failed"));
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
