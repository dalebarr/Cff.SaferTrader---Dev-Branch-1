using System.Web;

namespace Cff.SaferTrader.Core
{
    public class PopupHelper
    {
        public static string ShowPopup(IPrintable printable, HttpServerUtility serverUtility)
        {
            if (SessionWrapper.Instance.Get != null)
                SessionWrapper.Instance.Get.PrintBag = printable;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).PrintBag = printable;

            return string.Format("window.open('{0}Popups/{1}', 'Popup', 'status=yes, scrollbars=yes, menubar=no, toolbar=no, location=no, width=954, height=640, resizable=no');",
                                 RelativePathComputer.ComputeRelativePathToRoot(serverUtility), printable.PopupPageName);
        }

        public static string ShowPopup(IDocument document, HttpServerUtility serverUtility)
        {
            if (SessionWrapper.Instance.Get != null)
                SessionWrapper.Instance.Get.DocBag = document;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).DocBag = document;
            
            return string.Format("window.open('{0}Popups/{1}', 'Popup', 'status=yes, scrollbars=yes, menubar=no, toolbar=no, location=no, width=860, height=640, resizable=yes');",
                                 RelativePathComputer.ComputeRelativePathToRoot(serverUtility), document.PopupPageName);            
        }

        public static string ShowPopupReportType(IPrintable printable, HttpServerUtility serverUtility, bool reportWide)
        {
            if (SessionWrapper.Instance.Get != null)
                SessionWrapper.Instance.Get.PrintBag = printable;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).PrintBag = printable;

            if (reportWide)
            {
                return
                    string.Format(
                        "window.open('{0}Popups/{1}', 'Popup', 'status=yes, scrollbars=yes, menubar=no, toolbar=no, location=no, width=1050, height=640, resizable=no');",
                        RelativePathComputer.ComputeRelativePathToRoot(serverUtility), printable.PopupPageName);
            }
            else
            {
                return
                    string.Format(
                        "window.open('{0}Popups/{1}', 'Popup', 'status=yes, scrollbars=yes, menubar=no, toolbar=no, location=no, width=958, height=640, resizable=no');",
                        RelativePathComputer.ComputeRelativePathToRoot(serverUtility), printable.PopupPageName);
            }

        }

    }
}