namespace Battleships.Core.Tests.TestHelpers
{
    using Battleships.Core.Models;
    using Battleships.Core.Tests.TestHelpers.Database;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class TestUserManager
    {
        private const string DefaultUserName = "DefaultTestUser";
        private readonly UserManager<User> userManager;

        public TestUserManager(string connectionString)
        {
            var context = new TestBattleshipsContext(connectionString);
            userManager = new UserManager<User>(new UserStore<User>(context));
        }

        public string CreateUserAndGetId(string userName = DefaultUserName)
        {
            userManager.Create(new User { UserName = userName }, "password");
            return userManager.FindByName(userName).Id;
        }
    }
}
