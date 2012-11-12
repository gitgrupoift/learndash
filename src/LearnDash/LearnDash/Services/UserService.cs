namespace LearnDash.Services
{
    using System.Linq;
    using System.Web;

    using Dal.Models;
    using Dal.NHibernate;

    public class UserService : IUserService
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public IRepository<UserProfile> UserRepo { get; set; }

        public UserProfile GetCurrentUser()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var currentUser =
                        this.UserRepo.GetByParameterEqualsFilter("UserId", HttpContext.Current.User.Identity.Name).SingleOrDefault();
                    return currentUser;
                }
                else
                {
                    logger.Warn("User not authenticated! Cant get Current User. Possible error in code logic");
                }
            }
            else
            {
                logger.Error("HttpContext missing! If this happens our logic is wrong we need to fix this asap!");
            }

            return null;
        }
    }
}