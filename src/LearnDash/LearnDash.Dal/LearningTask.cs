using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnDash.Dal
{
    public enum LearningTaskType
    {
        None=0,
    }

    public class LearningTask : RedisEntity
    {
        /// <summary>
        /// This allows to set specific order on tasks
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// This affects algorithms for creating learning flows randomly
        /// </summary>
        public int Importance { get; set; }

        public string Name { get; set; }
        public LearningTaskType Type { get; set; }

        /// <summary>
        /// Should task be reused after completion
        /// </summary>
        public bool Reccuring { get; set; }
        

        /// <summary>
        /// This is counter used to count every user in the system
        /// </summary>
        public int TimesDone { get; set; }
    }
}
