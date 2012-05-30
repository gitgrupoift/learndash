using System.Collections.Generic;
using System.Linq;
using LearnDash.Controllers;
using LearnDash.Dal;

namespace LearnDash.Services
{
    public class LearningFlowService : ILearningFlowService
    {
        private LearningFlow SortFlow(LearningFlow sourceFlow)
        {
            if (sourceFlow.Tasks != null)
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

        public long Save(LearningFlow flow)
        {
            return LearningFlowRepository.Save(flow);
        }

        public LearningFlow Get(long Id)
        {
            return SortFlow(LearningFlowRepository.Get(Id));
        }

        public List<LearningFlow> GetAll()
        {
            return LearningFlowRepository.GetAll();
        }

        public void Remove(long id)
        {
            LearningFlowRepository.Remove(id);
        }
    }
}