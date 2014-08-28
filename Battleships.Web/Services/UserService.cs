namespace Battleships.Web.Services
{
    using Battleships.Core.Models;
    using Microsoft.AspNet.Identity;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IUserService
    {
        IdentityResult AddUser(string userName, string password);

        Task<IdentityResult> ChangePassword(string userName, string currentPassword, string newPassword);

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

        public Task<IdentityResult> ChangePassword(string userId, string currentPassword, string newPassword)
        {
            return userManager.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        /*    public Task<IdentityResult> ResetPassword(string userName)
        {
            return userManager.ResetPasswordAsync(userName, token, newPassword);
        }*/

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