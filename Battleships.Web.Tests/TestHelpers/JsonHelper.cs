namespace Battleships.Web.Tests.TestHelpers
{
    using System.Web.Mvc;

    public static class JsonHelper
    {
        public static object GetProperty(this JsonResult jsonResult, string propertyName)
        {
            var data = jsonResult.Data;
            var property = data.GetType().GetProperty(propertyName);
            return property != null ? property.GetValue(data) : null;
        }
    }
}