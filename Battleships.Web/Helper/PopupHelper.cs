namespace Battleships.Web.Helper
{
    using System.Web.Mvc;

    public static class PopupHelper
    {
        public const string PopupKey = "showPopup";
        public const string PopupClass = "popupClass";

        public static bool HasPopup(this TempDataDictionary tempData)
        {
            return tempData[PopupKey] != null;
        }

        public static void AddPopup(this TempDataDictionary tempData, string message, string popupClass)
        {
            tempData.Add(PopupKey, message);
            tempData.Add(PopupClass, popupClass);
        }
    }
}