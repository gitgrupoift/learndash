namespace LearnDash.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using LearnDash.Dal;
    using LearnDash.Dal.Models;
    using LearnDash.Dal.NHibernate;

    public interface IUserService
    {
        //UserProfile GetCurrentUser();
    }

    public class UserService : IUserService
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public IRepository<UserProfile> UserRepo { get; set; }

        //public UserProfile GetCurrentUser()
        //{
        //    UserRepo.GetByParameterEqualsFilter("UserId", User.Identity.Name).SingleOrDefault();
        //    throw new System.NotImplementedException();
        //}
    }
}