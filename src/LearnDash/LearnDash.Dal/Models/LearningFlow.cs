namespace LearnDash.Dal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public enum FlowType
    {
        All = 0,
        Static = 1,
        Dynamic = 2
    }

    public class LearningFlow : IDBModel
    {
        [Required]
        public virtual int ID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Learning flow name can have only 50 characters.")]
        public virtual string Name { get; set; }

        public virtual ICollection<LearningTask> Tasks { get; set; }

        public virtual FlowType FlowType { get; set; }

        [Obsolete("Dont use this.Has to be defined for the json serializer")]
        public LearningFlow()
        {
            
        }

        public LearningFlow(string name)
        {
            Name = name;
        }
    }
}
