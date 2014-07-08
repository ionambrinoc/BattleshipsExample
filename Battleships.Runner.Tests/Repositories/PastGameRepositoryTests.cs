namespace Battleships.Runner.Tests.Repositories
{
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class PastGameRepositoryTests
    {
        private PastGame fakePastGame;
        private PastGameRepository fakePastGameRepository;
        private List<PastGame> fakePastGames;

        [SetUp]
        public void SetUp()
        {
            fakePastGameRepository = A.Fake<PastGameRepository>();
        }

        [Test]
        public void GetAll_should_Return_PastGames()
        {
            // Given
            fakePastGame = new PastGame
                           {
                               FirstPlayer = "Jasper",
                               SecondPlayer = "Bingqian",
                               FirstPlayerWon = true,
                               TimePlayed = new DateTime(2014, 7, 7, 12, 12, 12),
                           };
            fakePastGames = new List<PastGame> { fakePastGame };
            fakePastGameRepository.PastGames = fakePastGames;

            //Then
            fakePastGameRepository.GetAll().ShouldBeEquivalentTo(fakePastGames);
        }
    }
}