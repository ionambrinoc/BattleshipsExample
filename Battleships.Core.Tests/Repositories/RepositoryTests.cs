namespace Battleships.Core.Tests.Repositories
{
    using Battleships.Core.Repositories;
    using Battleships.Core.Tests.TestHelpers.Attributes;
    using Battleships.Core.Tests.TestHelpers.Database;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Data.Entity;
    using System.Linq.Expressions;

    [TestFixture]
    [RequiresDatabase]
    public class RepositoryTests
    {
        private TestRepository repo;

        [SetUp]
        public void SetUp()
        {
            repo = new TestRepository(new TestBattleshipsContext(TestDb.ConnectionString));
        }

        [Test]
        public void Can_add_entities()
        {
            // Given
            var entity1 = new TestEntity { Name = "Entity with English name" };
            var entity2 = new TestEntity { Name = "Объект с именем Российской Федерации" };

            // When
            repo.Add(entity1);
            repo.Add(entity2);
            repo.SaveContext();

            // Then
            var entities = TestDb.GetAll<TestEntity>();
            entities.Should().HaveCount(2);
            entities.Should().Contain(EntityNamed(entity1.Name));
            entities.Should().Contain(EntityNamed(entity2.Name));
        }

        private static Expression<Func<TestEntity, bool>> EntityNamed(string name)
        {
            return entity => entity.Name == name;
        }

        private class TestRepository : Repository<TestEntity>
        {
            public TestRepository(DbContext context) : base(context) {}
        }
    }
}
