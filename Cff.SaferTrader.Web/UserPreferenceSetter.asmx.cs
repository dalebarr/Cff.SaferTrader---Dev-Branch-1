using System;
using System.Web.Services;
using Cff.SaferTrader.Core;
namespace Cff.SaferTrader.Web
{
    /// <summary>
    /// Summary description for UserPreferenceSetter
    /// </summary>
    [WebService(Namespace = "http://www.cff.co.nz/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
   
    [System.Web.Script.Services.ScriptService]
    public class UserPreferenceSetter : WebService
    {
        [WebMethod(EnableSession = true)]
        public void SetContactDetailsPreference(bool isContactDetailsShown)
        {
            if (QueryString.ViewIDValue!=null && SessionWrapper.Instance.Get==null)
                SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsContactShown = isContactDetailsShown; //? "display:block;" : "display: none;";
            else if (SessionWrapper.Instance.Get!=null)
                SessionWrapper.Instance.Get.IsContactShown = isContactDetailsShown; //? "display:block;" : "display: none;";
        }

        [WebMethod(EnableSession = true)]
        public  void TogglePermanentNotesGridView(string show)
        {
            if (SessionWrapper.Instance.Get != null)
            {
                SessionWrapper.Instance.Get.IsHidePermanentNotesGridView = Convert.ToBoolean(show);
            }
        }

       
        [WebMethod(EnableSession = true)]
        public void SetEditContactsPreference(bool isEditContactsTriggered)
        {
            SessionWrapper.Instance.Get.IsEditContactDetails = isEditContactsTriggered; //? "display:block;" : "display: none;";
        }

        [WebMethod(EnableSession = true)]
        public void SetEditCContactsPreference(bool isEditCContactsTriggered)
        {
            SessionWrapper.Instance.Get.IsEditCContactDetails = isEditCContactsTriggered; //? "display:block;" : "display: none;";
        }       

    }
}
