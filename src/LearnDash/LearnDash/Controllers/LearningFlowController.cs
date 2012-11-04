namespace LearnDash.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Dal.Models;
    using Dal.NHibernate;
    using Services;

    [Authorize]
    public class LearningFlowController : Controller
    {
        public ILearningFlowService LearningFlowService { get; set; }

        public IRepository<LearningTask> LearningTaskRepository { get; set; }
        public IRepository<UserProfile> UserRepository { get; set; } 


        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return this.RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            this.ViewBag.FlowTypes = new LearningFlow().FlowType.ToSelectList();
            var flow = LearningFlowService.Get(id);
            if (flow != null)
            {
                return this.View(flow);
            }
            else
            {
                return this.View("Error", ErrorType.NotFound);
            }
        }

        [HttpPost]
        public ActionResult Edit(LearningFlow flow)
        {
            this.ViewBag.FlowTypes = new LearningFlow().FlowType.ToSelectList();
            if (ModelState.IsValid)
            {
                var lastFlow = LearningFlowService.Get(flow.ID);
                lastFlow.Name = flow.Name;

                var state = LearningFlowService.Update(lastFlow);
                if (state)
                {
                    Notification.Notify(NotificationType.Success, "Edit Successfull");
                }
                else
                {
                    Notification.Notify(NotificationType.Fail, "Edit Failed");
                }

                return this.View(flow);
            }

            Logger.Warn("Flow model not valid!. Propably client validation didn't worked out.");
            Notification.Notify(NotificationType.Fail, "Validation Failed!");
            return this.View(flow);
        }

        [HttpGet]
        public ActionResult EditItems(int id)
        {
            var flow = LearningFlowService.Get(id);
            if (flow != null)
            {
                return this.View(flow);
            }
            else
            {
                return this.View("Error", ErrorType.NotFound);
            }
        }

        [HttpPost]
        public ActionResult EditItems(LearningFlow flow)
        {
            if (ModelState.IsValid)
            {
                var state = LearningFlowService.Update(flow);
                if (state)
                {
                    Notification.Notify(NotificationType.Success, "Edit Successfull");
                }
                else
                {
                    Notification.Notify(NotificationType.Fail, "Edit Failed");
                }

                return this.View(flow);
            }

            Logger.Warn("Flow model not valid!. Propably client validation didn't worked out.");
            Notification.Notify(NotificationType.Fail, "Validation Failed!");
            return this.View(flow);
        }

        [HttpGet]
        public ActionResult Add()
        {
            this.ViewBag.FlowTypes = new LearningFlow().FlowType.ToSelectList();

            return this.View();
        }

        [HttpPost]
        public ActionResult Add(LearningFlow newFlow)
        {
            if (ModelState.IsValid)
            {
                newFlow.Tasks = new List<LearningTask>();
                var id = LearningFlowService.Add(newFlow);

                if (id > 0)
                {
                    Notification.Notify(NotificationType.Success, "Adding new flow successfull");
                }
                else
                {
                    Notification.Notify(NotificationType.Fail, "Adding new flow failed");
                }

                return this.RedirectToAction("EditItems", new { id });
            }

            Logger.Warn("Flow model not valid!. Propably client validation didn't worked out.");
            Notification.Notify(NotificationType.Fail, "Validation Failed!");
            return this.View();
        }

        [HttpGet]
        public ActionResult Remove(int id)
        {
            var flow = LearningFlowService.Get(id);
            if (flow != null)
            {
                return this.View(flow);
            }
            else
            {
                Logger.Warn("Unable to remove flow beacuse it wasn't found in DB. Possible error. Beacuse flow is visible in view but not visible in DB.");
                return this.View("Error", ErrorType.NotFound);
            }
        }

        [HttpPost]
        public ActionResult Remove(LearningFlow flow)
        {
            if (this.LearningFlowService.Remove(flow.ID))
            {
                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                return this.View("Error", ErrorType.Internal);
            }
        }

        [HttpGet]
        public ActionResult List()
        {
            // refactor : here we are using GetByParameteres euqls which return IList unnecesary implement nhibernate Linq :X
            var user = this.UserRepository.GetByParameterEqualsFilter("UserId", User.Identity.Name).SingleOrDefault();

            if (user != null)
            {
                var flows = user.Dashboards.First().Flows.ToList();
                return this.View(flows);
            }
            else
            {
                Logger.Warn("User '{0}' doesn't exist \r\nPropably session should be recycled and register procedure performed again.", User.Identity.Name);
                return this.RedirectToAction("Logout", "Account");
            }
        }

        [HttpPost]
        public ActionResult AddTask(LearningTask task, int flowId)
        {
            if (ModelState.IsValid)
            {
                var flow = LearningFlowService.Get(flowId);
                if (flow != null)
                {
                    task.Name = Server.HtmlEncode(task.Name);

                    flow.Tasks.Add(task);
                    LearningFlowService.Update(flow);

                    return this.Json(Is.Success.Message(task.ID.ToString()));
                }

                Logger.Warn("User tried to add task to non existing flow");
                return this.Json(Is.Fail.Message("Flow doesn't exist"));
            }

            Logger.Warn("Add Task - Validation Fail");
            return this.Json(Is.Fail.Message("Valdiation Failed"));
        }

        [HttpPost]
        public ActionResult RemoveTask(int taskId, int flowId)
        {
            var flow = LearningFlowService.Get(flowId);
            if (flow != null)
            {
                if (flow.Tasks.Any(t => t.ID == taskId))
                {
                    LearningFlowService.RemoveTask(flow, taskId);
                    return this.Json(Is.Success.Empty);
                }

                Logger.Warn("User tried to remove non exisitng task");
                return this.Json(Is.Fail.Message("Task doesn't exist"));
            }

            Logger.Warn("User tried to remove task from non existing flow");
            return this.Json(Is.Fail.Message("Flow doesn't exist"));
        }

        [HttpPost]
        public ActionResult CompleteTaskAndMakeNewNext(int flowID, int newCompleteTaskId, int currentCompleteTaskId)
        {
            if (flowID >= 0 && newCompleteTaskId >= 0)
            {
                var flow = this.LearningFlowService.Get(flowID);

                var task = flow.Tasks.First(t => t.ID == newCompleteTaskId);
                var currentTask = flow.Tasks.First(t => t.ID == currentCompleteTaskId);

                currentTask.TimesDone = currentTask.TimesDone + 1;
                task.IsNext = true;

                foreach (var learningTask in flow.Tasks.Where(t => t.ID != newCompleteTaskId).ToList())
                {
                    learningTask.IsNext = false;
                }

                this.LearningFlowService.Update(flow);
                return this.Json(Is.Success.Empty);
            }

            Logger.Warn("Wrong data sent to the action \r\nparams: flowId - {0}\r\n nTaskId - {1} ", flowID, newCompleteTaskId);
            return this.Json(Is.Fail.Message("Wrong data sent"));
        }

        [HttpPost]
        public ActionResult CompleteTask(int flowID, int currentCompleteTaskId)
        {
            if (flowID >= 0)
            {
                var flow = this.LearningFlowService.Get(flowID);

                var currentTask = flow.Tasks.First(t => t.ID == currentCompleteTaskId);

                currentTask.TimesDone = currentTask.TimesDone + 1;

                this.LearningFlowService.Update(flow);
                return this.Json(Is.Success.Empty);
            }

            Logger.Warn("Wrong data sent to the action \r\nparams: flowId - {0}\r\n nTaskId - {1} ", flowID, currentCompleteTaskId);
            return this.Json(Is.Fail.Message("Wrong data sent"));
        }

        [HttpPost]
        public ActionResult MakeNext(int flowID, int newCompleteTaskId)
        {
            if (flowID >= 0 && newCompleteTaskId >= 0)
            {
                var flow = this.LearningFlowService.Get(flowID);

                var task = flow.Tasks.First(t => t.ID == newCompleteTaskId);
                task.IsNext = true;

                foreach (var learningTask in flow.Tasks.Where(t => t.ID != newCompleteTaskId).ToList())
                {
                    learningTask.IsNext = false;
                }

                this.LearningFlowService.Update(flow);
                return this.Json(Is.Success.Empty);
            }

            Logger.Warn("Wrong data sent to the action \r\nparams: flowId - {0}\r\n nTaskId - {1} ", flowID, newCompleteTaskId);
            return this.Json(Is.Fail.Message("Wrong data sent"));
        }

        public ActionResult View(int id)
        {
            var flow = this.LearningFlowService.Get(id);
            if (flow != null)
            {
                return this.View(flow);
            }
            else
            {
                Logger.Warn("Requested flow with id - {0} that doesn't exist", id);
                return this.View("Error", ErrorType.NotFound);
            }
        }

        public ActionResult ViewTest(int id)
        {
            var flow = this.LearningFlowService.Get(id);

            if (flow != null)
            {
                return this.View(flow);
            }
            else
            {
                Logger.Warn("Requested flow with id - {0} that doesn't exist", id);
                return this.View("Error", ErrorType.NotFound);
            }
        }

        [HttpPost]
        public ActionResult Clear(LearningFlow flow)
        {
            var state = LearningFlowService.Update(flow);
            if (state)
            {
                return this.Json(Is.Success.Empty);
            }
            else
            {
                return this.Json(Is.Fail.Message("Clear Failed"));
            }
        }
    }

    public static class Is
    {
        public class Success
        {
            public static object Empty
            {
                get
                {
                    return new { isSuccess = true, message = string.Empty };
                }
            }

            public static object Message(string message)
            {
                return new
                {
                    isSuccess = true,
                    message
                };
            }
        }

        public class Fail
        {
            public static object Message(string message)
            {
                return new
                           {
                               isSuccess = false,
                               message
                           };
            }
        }
    }
}
