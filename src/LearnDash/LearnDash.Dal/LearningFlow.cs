using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnDash.Dal
{
    public class LearningFlow : RedisEntity
    {
        public ICollection<LearningTask> Tasks { get; set; }
    }
}
