namespace LearnDash.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Dal.Models;
    using Dal.NHibernate;

    public class LearningFlowService : ILearningFlowService
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public IRepository<LearningFlow> FlowRepo { get; set; }

        public IRepository<LearningDashboard> DashRepo { get; set; }

        public IRepository<LearningTask> TasksRepo { get; set; }

        // utest

        /// <summary>
        /// This function sorts flow so that first element is the one with IsNext = true
        /// </summary>
        public LearningFlow SortFlow(LearningFlow sourceFlow)
        {
            if (sourceFlow != null && sourceFlow.Tasks != null)
            {
                var tasksPreSort = sourceFlow.Tasks.ToList();
                var tasksPreNext = new List<LearningTask>();

                while (tasksPreSort.FirstOrDefault() != null && !tasksPreSort.First().IsNext)
                {
                    tasksPreNext.Add(tasksPreSort.First());
                    tasksPreSort.RemoveAt(0);
                }

                tasksPreNext.InsertRange(0, tasksPreSort);

                sourceFlow.Tasks = tasksPreNext;
            }
            return sourceFlow;
        }

        public bool Update(LearningFlow flow)
        {
            var result = this.FlowRepo.Update(flow);
            return result;
        }

        public int? Add(LearningFlow flow)
        {
            var currentDashboard = this.DashRepo.GetById(SessionManager.CurrentUserSession.MainDashboardId);
            currentDashboard.Flows.Add(flow);
            this.DashRepo.Update(currentDashboard);
            return flow.ID;
        }

        public LearningFlow Get(int id)
        {
            return this.FlowRepo.GetById(id);
        }

        public List<LearningFlow> GetAll()
        {
            return this.FlowRepo.GetAll().ToList();
        }

        public bool Remove(int id)
        {
            var learnFlow = this.FlowRepo.GetById(id);
            if (learnFlow != null)
            {
                return this.FlowRepo.Remove(learnFlow);
            }
            else
            {
                this.logger.Warn("Remove method failed beacuse no learnFlow with id '{0}' exists", id);
                return false;
            }
        }

        public void RemoveTask(LearningFlow flow, int taskId)
        {
            var task = TasksRepo.GetById(taskId);
            flow.Tasks.Remove(task);
            this.Update(flow);
        }
    }
}