using System.Collections.Generic;
using LearnDash.Dal;

namespace LearnDash.Services
{
    public interface ILearningFlowService
    {
        long Save(LearningFlow flow);
        LearningFlow Get(long Id);
        List<LearningFlow> GetAll();
        void Remove(long id);
    }
}