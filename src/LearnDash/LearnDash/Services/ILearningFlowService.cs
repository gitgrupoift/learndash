using System.Collections.Generic;
using LearnDash.Dal;
using LearnDash.Dal.Models;

namespace LearnDash.Services
{
    public interface ILearningFlowService 
    {
        bool Update(LearningFlow flow);
        int? Add(LearningFlow flow);
        LearningFlow Get(int id);
        List<LearningFlow> GetAll();
        void Remove(int id);
    }
}