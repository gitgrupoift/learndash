using System.Collections.Generic;
namespace LearnDash.Dal.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserProfile : IDBModel
    {
        [Required]
        public virtual int ID { get; set; }

        /// <summary>
        /// UserId assiocated withe the OpenId. This value will be in form of a mail 
        /// like stefan.muller@gmail.com
        /// </summary>
        [Required]
        public virtual string UserId { get; set; }

        /// <summary>
        /// Each user can have many dashboards. Initiali we are supporting only one.
        /// </summary>
        public virtual IList<LearningDashboard> Dashboards { get; set; }
    }
}
