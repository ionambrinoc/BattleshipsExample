namespace Battleships.Runner.Tests.Repositories
{
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using FakeItEasy;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class PastGameRepositoryTests
    {
        private MatchResult fakePastGame;
        private MatchResultsRepository fakeMatchResultsRepository;
        private List<MatchResult> fakePastGames;

        [SetUp]
        public void SetUp()
        {
            fakeMatchResultsRepository = A.Fake<MatchResultsRepository>();
        }

        /*[Test]
        public void GetAll_should_Return_PastGames()
        {
            // Given
            fakePastGame = new MatchResult
                           {
                               FirstPlayer = "Jasper",
                               SecondPlayer = "Bingqian",
                               FirstPlayerWon = true,
                               TimePlayed = new DateTime(2014, 7, 7, 12, 12, 12),
                           };
            fakePastGames = new List<MatchResult> { fakePastGame };
            fakeMatchResultsRepository.PastGames = fakePastGames;

            //Then
            fakeMatchResultsRepository.GetAll().ShouldBeEquivalentTo(fakePastGames);
        }*/
    }
}