using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LearnDash.Dal
{
    public class LearningFlow : RedisEntity
    {
        public string Name { get; set; }
        public ICollection<LearningTask> Tasks { get; set; }

        //has to be defined for the json serializer
        public LearningFlow()
        {
            
        }

        public LearningFlow(string name)
        {
            Tasks = new Collection<LearningTask>();
            Name = name;
        }
    }
}
