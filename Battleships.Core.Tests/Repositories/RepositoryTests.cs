namespace Battleships.Core.Tests.Repositories
{
    using Battleships.Core.Repositories;
    using Battleships.Core.Tests.TestHelpers.Attributes;
    using Battleships.Core.Tests.TestHelpers.Database;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    [TestFixture]
    [RequiresDatabase]
    public class RepositoryTests
    {
        private TestRepository repo;
        private TestEntity entity1;
        private TestEntity entity2;

        [SetUp]
        public void SetUp()
        {
            repo = new TestRepository(new TestBattleshipsContext(TestDb.ConnectionString));
            entity1 = new TestEntity { Name = "Entity with English name" };
            entity2 = new TestEntity { Name = "Объект с именем Российской Федерации" };
        }

        [Test]
        public void Can_add_entities()
        {
            // When
            repo.Add(entity1);
            repo.Add(entity2);
            repo.SaveContext();

            // Then
            var entities = TestDb.GetAll<TestEntity>();
            ListShouldContainTwoEntities(entities);
        }

        [Test]
        public void Can_get_all_entities()
        {
            // Given
            repo.Add(entity1);
            repo.Add(entity2);
            repo.SaveContext();

            // When
            var result = repo.GetAll().ToList();

            // Then
            ListShouldContainTwoEntities(result);
        }

        [Test]
        public void Can_remove_range()
        {
            // Given
            repo.Add(entity1);
            repo.Add(entity2);
            repo.SaveContext();

            // When
            repo.RemoveRange(new List<TestEntity> { entity1, entity2 });
            repo.SaveContext();

            // Then
            TestDb.GetAll<TestEntity>().Should().HaveCount(0);
        }

        private static Expression<Func<TestEntity, bool>> EntityNamed(string name)
        {
            return entity => entity.Name == name;
        }

        private void ListShouldContainTwoEntities(List<TestEntity> entities)
        {
            entities.Should().HaveCount(2);
            entities.Should().Contain(EntityNamed(entity1.Name));
            entities.Should().Contain(EntityNamed(entity2.Name));
        }

        private class TestRepository : Repository<TestEntity>
        {
            public TestRepository(DbContext context) : base(context) {}
        }
    }
}
