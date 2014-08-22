namespace Battleships.Core.Tests.Repositories
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Core.Tests.TestHelpers;
    using Battleships.Core.Tests.TestHelpers.Attributes;
    using Battleships.Core.Tests.TestHelpers.Database;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    [RequiresDatabase]
    public class PlayerRecordsRepositoryTests
    {
        private const string UserName1 = "TestUser1";
        private const string UserName2 = "TestUser2";
        private const string PlayerForUser1 = "TestPlayer";
        private PlayerRecordsRepository repo;
        private string validUserId1;
        private string validUserId2;

        [SetUp]
        public void SetUp()
        {
            var testUserManager = new TestUserManager(TestDb.ConnectionString);
            validUserId1 = testUserManager.CreateUserAndGetId(UserName1);
            validUserId2 = testUserManager.CreateUserAndGetId(UserName2);

            repo = new PlayerRecordsRepository(new TestBattleshipsContext(TestDb.ConnectionString));
            AddPlayerRecordForUser1();
        }

        [Test]
        public void Player_name_exists_if_there_is_a_player_with_that_name()
        {
            // When
            var exists = repo.PlayerNameExists(PlayerForUser1);

            // Then
            exists.Should().Be(true, "because this player should have already been added to the database.");
        }

        [Test]
        public void Player_name_exists_for_user_if_there_is_a_player_with_that_name_and_user_id()
        {
            // When
            var exists = repo.PlayerNameExistsForUser(PlayerForUser1, validUserId1);

            // Then
            exists.Should().Be(true, "because this player should have been added for this user.");
        }

        [Test]
        public void Player_name_for_one_user_does_not_exist_for_a_different_user()
        {
            // When
            var exists = repo.PlayerNameExistsForUser(PlayerForUser1, validUserId2);

            // Then
            exists.Should().Be(false, "because this player should belong to a different user.");
        }

        [Test]
        public void Get_all_for_user_id_only_returns_players_for_given_user()
        {
            // Given
            var names = new[] { "name1", "name2", "name3" };
            foreach (var name in names)
            {
                repo.Add(new PlayerRecord { Name = name, UserId = validUserId2, LastUpdated = DateTime.Now });
            }
            repo.SaveContext();

            // When
            var playersOfUser1 = repo.GetAllForUserId(validUserId2).ToList();

            // Then
            playersOfUser1.Select(x => x.Name).Should().BeEquivalentTo(names);
        }

        [Test]
        public void Marking_player_as_updated_gives_new_lastUpdated_time()
        {
            // Given
            var oldTime = GetPlayer1().LastUpdated;

            // When
            repo.MarkPlayerAsUpdated(PlayerForUser1);
            var newTime = GetPlayer1().LastUpdated;

            // Then
            newTime.Should().NotBe(oldTime);
        }

        private PlayerRecord GetPlayer1()
        {
            return repo.GetPlayerRecordById(1);
        }

        private void AddPlayerRecordForUser1()
        {
            repo.Add(new PlayerRecord { Name = PlayerForUser1, UserId = validUserId1, LastUpdated = DateTime.Now });
            repo.SaveContext();
        }
    }
}