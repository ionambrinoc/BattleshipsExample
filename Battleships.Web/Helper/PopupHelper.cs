namespace Battleships.Web.Helper
{
    using System.Web.Mvc;

    public static class PopupHelper
    {
        public const string PopupKey = "showPopup";

        public static bool HasPopup(this TempDataDictionary tempData)
        {
            return tempData[PopupKey] != null;
        }

        public static void AddPopup(this TempDataDictionary tempData, string message)
        {
            tempData.Add(PopupKey, message);
        }
    }
}