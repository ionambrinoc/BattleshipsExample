namespace Battleships.Core.Tests.Repositories
{
    using Battleships.Core.Repositories;
    using Battleships.Core.Tests.TestHelpers.Attributes;
    using Battleships.Core.Tests.TestHelpers.Database;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    [RequiresDatabase]
    public class LeagueRecordsRepositoryTests
    {
        private LeagueRecordsRepository repo;

        [SetUp]
        public void SetUp()
        {
            repo = new LeagueRecordsRepository(new TestBattleshipsContext(TestDb.ConnectionString));
        }

        [Test]
        public void Can_record_a_league()
        {
            // Given
            var testLeagueTime = new DateTime(2000, 1, 1);
            repo.AddLeague(testLeagueTime);
            repo.SaveContext();

            // When
            var firstTime = repo.GetAll().First().StartTime;

            // Then
            repo.GetAll().Count().Should().Be(1);
            firstTime.Should().Be(testLeagueTime);
        }

        [Test]
        public void Can_get_time_of_most_recent_league()
        {
            // Given
            var earlierTime = new DateTime(2000, 1, 1);
            var laterTime = new DateTime(2001, 1, 1);
            repo.AddLeague(earlierTime);
            repo.AddLeague(laterTime);
            repo.SaveContext();

            // When
            var latestTime = repo.GetLatestLeagueTime();

            // Then
            repo.GetAll().Count().Should().Be(2);
            latestTime.Should().Be(laterTime);
        }

        [Test]
        public void Returns_minimum_datetime_when_league_repo_is_empty()
        {
            // Given
            repo.SaveContext();

            // When
            var latestTime = repo.GetLatestLeagueTime();

            // Then
            repo.GetAll().Count().Should().Be(0);
            latestTime.Should().Be(DateTime.MinValue);
        }
    }
}