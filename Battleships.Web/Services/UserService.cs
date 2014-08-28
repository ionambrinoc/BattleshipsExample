namespace Battleships.Web.Services
{
    using Battleships.Core.Models;
    using Microsoft.AspNet.Identity;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;

    public interface IUserService
    {
        IdentityResult AddUser(string userName, string password);

        IdentityResult ChangePassword(string userName, string currentPassword, string newPassword);

        User Find(string userName, string password);

        ClaimsIdentity CreateIdentity(User user, string authenticationType);

        bool DoesUserExist(string userName);
    }

    [ExcludeFromCodeCoverage]
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

        public IdentityResult ChangePassword(string userId, string currentPassword, string newPassword)
        {
            var result = userManager.ChangePasswordAsync(userId, currentPassword, newPassword).Result;
            return result;
        }

        public IdentityResult ResetPassword(string userName, string token, string newPassword)
        {
            var userId = userManager.FindByName(userName).Id;
            var result = userManager.ResetPasswordAsync(userId, token, newPassword).Result;
            return result;
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