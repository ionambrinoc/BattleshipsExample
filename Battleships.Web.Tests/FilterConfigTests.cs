namespace Battleships.Web.Tests
{
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    [TestFixture]
    public class FilterConfigTests
    {
        [SetUp]
        public void SetUp()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        [TearDown]
        public void TearDown()
        {
            GlobalFilters.Filters.Clear();
        }

        [TestCase(typeof(HandleErrorAttribute))]
        public void Type_is_registered_as_filter(Type filterType)
        {
            var filters = GlobalFilters.Filters.Select(f => f.Instance);
            Assert.That(filters, Has.Exactly(1).TypeOf(filterType));
        }
    }
}
