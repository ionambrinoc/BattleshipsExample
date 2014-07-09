namespace Battleships.Web.Services
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public interface IUserService
    {
        IdentityResult AddUser(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public IdentityResult AddUser(string username, string password)
        {
            return userManager.Create(new IdentityUser { UserName = username }, password);
        }
    }
}