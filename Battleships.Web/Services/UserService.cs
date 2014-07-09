namespace Battleships.Web.Services
{
    using Battleships.Runner.Models;
    using Microsoft.AspNet.Identity;

    public interface IUserService
    {
        IdentityResult AddUser(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;

        public UserService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public IdentityResult AddUser(string username, string password)
        {
            return userManager.Create(new User { UserName = username }, password);
        }
    }
}