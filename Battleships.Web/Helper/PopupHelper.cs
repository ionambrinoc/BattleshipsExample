namespace Battleships.Web.Helper
{
    using System.Web.Mvc;

    public enum PopupType
    {
        Success,
        Info,
        Warning,
        Danger
    };

    public static class PopupHelper
    {
        private const string PopupKey = "popup";

        public static bool HasPopup(this TempDataDictionary tempData)
        {
            return tempData[PopupKey] != null;
        }

        public static void AddPopup(this TempDataDictionary tempData, string message, PopupType popupType)
        {
            tempData.Add(PopupKey, new Popup(message, popupType));
        }

        public static Popup GetPopup(this TempDataDictionary tempData)
        {
            return tempData[PopupKey] as Popup;
        }
    }

    public class Popup
    {
        public Popup(string message, PopupType popupType)
        {
            Message = message;
            switch (popupType)
            {
                case PopupType.Danger:
                    CssClass = "alert-danger";
                    break;
                case PopupType.Info:
                    CssClass = "alert-info";
                    break;
                case PopupType.Success:
                    CssClass = "alert-success";
                    break;
                case PopupType.Warning:
                    CssClass = "alert-warning";
                    break;
            }
        }

        public string Message { get; set; }

        public string CssClass { get; private set; }
    }
}