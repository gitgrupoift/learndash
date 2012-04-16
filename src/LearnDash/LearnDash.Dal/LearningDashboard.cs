using System.Collections.Generic;

namespace LearnDash.Dal
{
    public class LearningDashboard : RedisEntity
    {
        public ICollection<LearningFlow> Flows { get; private set; }
    }
}