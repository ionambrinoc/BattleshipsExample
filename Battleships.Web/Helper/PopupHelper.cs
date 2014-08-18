namespace Battleships.Web.Helper
{
    using System.Web.Mvc;

    public static class PopupHelper
    {
        public static void AddPopup(this TempDataDictionary tempData, string message)
        {
            tempData.Add("showPopup", message);
        }
    }
}