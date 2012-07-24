namespace LearnDash.Services
{
    using System.Collections.Generic;

    using LearnDash.Dal;
    using LearnDash.Dal.Models;

    public interface IUserService
    {
        UserProfile GetCurrentUser();
    }
}