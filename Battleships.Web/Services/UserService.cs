namespace Battleships.Web.Services
{
    using Battleships.Core.Models;
    using Microsoft.AspNet.Identity;
    using System.Security.Claims;

    public interface IUserService
    {
        IdentityResult AddUser(string userName, string password);
        User Find(string userName, string password);
        ClaimsIdentity CreateIdentity(User user, string authenticationType);
        bool DoesUserExist(string userName);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;

        public UserService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public IdentityResult AddUser(string userName, string password)
        {
            return userManager.Create(new User { UserName = userName }, password);
        }

        public User Find(string userName, string password)
        {
            return userManager.Find(userName, password);
        }

        public ClaimsIdentity CreateIdentity(User user, string authenticationType)
        {
            return userManager.CreateIdentity(user, authenticationType);
        }

        public bool DoesUserExist(string name)
        {
            return userManager.FindByName(name) != null;
        }
    }
}
