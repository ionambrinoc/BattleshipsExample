namespace Battleships.Core.Tests.TestHelpers.Database
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class TestDb
    {
        public static string ConnectionString
        {
            get { return String.Format("Data Source={0}.sdf", TestContext.CurrentContext.Test.FullName); }
        }

        public static List<T> GetAll<T>() where T : class
        {
            using (var context = new TestBattleshipsContext(ConnectionString))
            {
                return context.Set<T>().ToList();
            }
        }

        public static void Delete()
        {
            using (var context = new TestBattleshipsContext(ConnectionString))
            {
                context.Database.Delete();
            }
        }
    }
}
