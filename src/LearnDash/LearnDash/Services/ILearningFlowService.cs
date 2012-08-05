namespace LearnDash.Services
{
    using System.Collections.Generic;
    using LearnDash.Dal;
    using LearnDash.Dal.Models;

    public interface ILearningFlowService
    {
        bool Update(LearningFlow flow);

        int? Add(LearningFlow flow);

        LearningFlow Get(int id);

        List<LearningFlow> GetAll();

        bool Remove(int id);

        void RemoveTask(LearningFlow flow, int taskId);
    }
}