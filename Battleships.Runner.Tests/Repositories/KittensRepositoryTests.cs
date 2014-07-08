namespace Battleships.Runner.Tests.Repositories
{
    using Battleships.Runner.Models;
    using NUnit.Framework;

    [TestFixture]
    public class KittensRepositoryTests
    {
        [Test]
        public void Test()
        {
            var context = new BattleshipsContext("Data Source=Battleships.sdf");
            context.Kittens.Add(new Kitten { Name = "Garfield" });
        }
    }
}
