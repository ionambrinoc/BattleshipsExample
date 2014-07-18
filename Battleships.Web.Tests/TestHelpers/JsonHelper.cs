﻿namespace Battleships.Web.Tests.TestHelpers
{
    using System.Web.Mvc;

    public static class JsonHelper
    {
        public static object GetProperty(this ActionResult jsonResult, string propertyName)
        {
            var data = ((JsonResult)jsonResult).Data;
            var property = data.GetType().GetProperty(propertyName);
            return property != null ? property.GetValue(data) : null;
        }
    }
}