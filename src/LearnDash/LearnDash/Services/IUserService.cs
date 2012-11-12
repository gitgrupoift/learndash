namespace LearnDash.Services
{
    using Dal.Models;

    public interface IUserService
    {
        UserProfile GetCurrentUser();
    }
}