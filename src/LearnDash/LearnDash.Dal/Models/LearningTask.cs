namespace LearnDash.Dal.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LearningTask : IDBModel
    {
        [Required]
        public virtual int ID { get; set; }

        /// <summary>
        /// This allows to set specific ordering of tasks
        /// </summary>
        public virtual int TaskOrder { get; set; }

        /// <summary>
        /// This affects algorithms for creating learning flows randomly
        /// </summary>
        public virtual int Importance { get; set; }

        /// <summary>
        /// Name is displayed on the learning flow interface.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Task name can have only 50 characters.")]
        public virtual string Name { get; set; }

        /// <summary>
        /// Should task be reused after completion, after completing, it will be displayed again.
        /// </summary>
        public virtual bool Reccuring { get; set; }
        
        /// <summary>
        /// This is counter used to count every use in the system
        /// </summary>
        public virtual int TimesDone { get; set; }

        /// <summary>
        /// Indicates if task isNext to be completed on the LearningFlow
        /// </summary>
        public virtual bool IsNext { get; set; }
    }
}
