namespace LearnDash.Services
{
    using System.Linq;

    using LearnDash.Dal.Models;
    using LearnDash.Dal.NHibernate;

    public class UserService : IUserService
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public IRepository<UserProfile> UserRepo { get; set; }

        public UserProfile GetCurrentUser()
        {
            var currentUser = this.UserRepo.GetByParameterEqualsFilter("UserId", SessionManager.CurrentUserSession.UserId).SingleOrDefault();
            return currentUser;
        }
    }
}