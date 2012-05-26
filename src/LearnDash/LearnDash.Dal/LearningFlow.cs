using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LearnDash.Dal
{
    public class LearningFlow : RedisEntity
    {
        private ICollection<LearningTask> _tasks;
        public ICollection<LearningTask> Tasks {
            get
            {
                if(_tasks == null)
                    _tasks = new Collection<LearningTask>();
                return _tasks;
            }
            set { _tasks = value; }
        }


        public string Name { get; set; }

        //has to be defined for the json serializer
        public LearningFlow()
        {
            
        }

        public LearningFlow(string name)
        {
            Name = name;
        }
    }
}
