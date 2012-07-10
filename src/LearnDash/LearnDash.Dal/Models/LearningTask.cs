namespace LearnDash.Dal.Models
{
    public enum LearningTaskType
    {
        None=0,
    }

    public class LearningTask : IModel
    {
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
        public virtual string Name { get; set; }

        /// <summary>
        /// Type of the task. Right now we are not supporting this.
        /// </summary>
        public virtual LearningTaskType Type { get; set; }

        /// <summary>
        /// Should task be reused after completion, after completing, it will be displayed again.
        /// </summary>
        public virtual bool Reccuring { get; set; }
        
        /// <summary>
        /// This is counter used to count every user in the system
        /// </summary>
        public virtual int TimesDone { get; set; }

        /// <summary>
        /// Indicates if task isNext to be completed on the LearningFlow
        /// </summary>
        public virtual bool IsNext { get; set; }
    }
}
