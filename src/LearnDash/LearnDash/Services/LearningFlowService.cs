namespace LearnDash.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Dal.Models;
    using Dal.NHibernate;

    public class LearningFlowService : ILearningFlowService
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly IRepository<LearningFlow> flowRepo;
        private readonly IRepository<LearningDashboard> dashRepo;
        private readonly IRepository<LearningTask> tasksRepo;

        public LearningFlowService(IRepository<LearningFlow> flowRepo, IRepository<LearningDashboard> dashRepo, IRepository<LearningTask> tasksRepo)
        {
            this.flowRepo = flowRepo;
            this.dashRepo = dashRepo;
            this.tasksRepo = tasksRepo;
        }

        public bool Update(LearningFlow flow)
        {
            var result = this.flowRepo.Update(flow);
            return result;
        }

        public int? Add(LearningFlow flow)
        {
            var currentDashboard = this.dashRepo.GetById(SessionManager.CurrentUserSession.MainDashboardId);
            currentDashboard.Flows.Add(flow);
            this.dashRepo.Update(currentDashboard);
            return flow.ID;
        }

        public LearningFlow Get(int id)
        {
            return this.flowRepo.GetById(id);
        }

        public List<LearningFlow> GetAll()
        {
            return this.flowRepo.GetAll().ToList();
        }

        public bool Remove(int id)
        {
            var learnFlow = this.flowRepo.GetById(id);
            if (learnFlow != null)
            {
                return this.flowRepo.Remove(learnFlow);
            }
            else
            {
                this.logger.Warn("Remove method failed beacuse no learnFlow with id '{0}' exists", id);
                return false;
            }
        }

        public void RemoveTask(LearningFlow flow, int taskId)
        {
            var task = this.tasksRepo.GetById(taskId);
            flow.Tasks.Remove(task);
            this.Update(flow);
        }
    }
}