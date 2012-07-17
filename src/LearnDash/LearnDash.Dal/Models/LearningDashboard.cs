using System.Collections.Generic;

namespace LearnDash.Dal.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LearningDashboard : IDBModel
    {
        [Required]
        public virtual int ID { get; set; }

        /// <summary>
        /// Each Dashboard contains many flows. Initiali we are supporting one.
        /// </summary>
        public virtual IList<LearningFlow> Flows { get; set; }
    }
}