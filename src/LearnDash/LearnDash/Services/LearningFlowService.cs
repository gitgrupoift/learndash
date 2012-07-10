using System.Collections.Generic;
using System.Linq;
using LearnDash.Controllers;
using LearnDash.Dal;
using LearnDash.Dal.Models;
using LearnDash.Dal.NHibernate;

namespace LearnDash.Services
{
    public class LearningFlowService : ILearningFlowService
    {
        public NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger(); 
        public IRepository<LearningFlow> _flowRepo { get; set; }
        public IRepository<LearningDashboard> _dashRepo { get; set; }
        public IRepository<LearningTask> _tasksRepo { get; set; } 

        //utest
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
            var result = _flowRepo.Update(flow);
            return result;
        }

        public int? Add(LearningFlow flow)
        {
            var currentDashboard = _dashRepo.GetById(1);
            currentDashboard.Flows.Add(flow);
            _dashRepo.Update(currentDashboard);
            return flow.ID;
        }

        public LearningFlow Get(int id)
        {
            return SortFlow(_flowRepo.GetById(id));
        }

        public List<LearningFlow> GetAll()
        {
            return _flowRepo.GetAll().ToList();
        }

        public void Remove(int id)
        {
            var learnFlow = _flowRepo.GetById(id);
            if (learnFlow != null)
            {
                _flowRepo.Remove(learnFlow);
            }
            else
                Logger.Warn("Remove method failed beacuse no learnFlow with id '{0}' exists",id );
        }
    }
}