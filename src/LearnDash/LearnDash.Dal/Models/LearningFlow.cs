using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LearnDash.Dal.Models
{
    using Microsoft.Build.Framework;

    public class LearningFlow : IModel
    {
        [Required]
        public virtual int ID { get; set; }
        [Required]
        public virtual string Name { get; set; }

        private  ICollection<LearningTask> _tasks;
        public virtual ICollection<LearningTask> Tasks
        {
            get
            {
                if(_tasks == null)
                    _tasks = new Collection<LearningTask>();
                return _tasks;
            }
            set { _tasks = value; }
        }

        [Obsolete("Dont use this.Has to be defined for the json serializer")]
        public  LearningFlow()
        {
            
        }
        public LearningFlow(string name)
        {
            Name = name;
        }
    }
}
