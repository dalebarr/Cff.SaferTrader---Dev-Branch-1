using System.Web;
using System.Web.Services;

using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;

namespace Cff.SaferTrader.Web
{
    [WebService(Namespace = "http://www.cff.co.nz/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ClientSearch : IHttpHandler, IClientSearchView
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request.HttpMethod == "POST")
                {
                    string clientSearchString = context.Request.Params["q"];
                    ClientSearchPresenter presenter = ClientSearchPresenter.Create(this);
                    System.Diagnostics.Debug.WriteLine("Process Rrequest clientSearchString: " + clientSearchString);
                    if (string.IsNullOrEmpty(clientSearchString))
                    {
                        if ((((CffPrincipal)context.User).IsInClientRole == true || ((CffPrincipal)context.User).IsInCustomerRole == true))
                        {
                            //do nothing
                        }
                        else {
                            if ((((CffPrincipal)context.User).IsInAdministratorRole) || (((CffPrincipal)context.User).IsInManagementRole))
                            {
                                clientSearchString = ((CffPrincipal)context.User).CffUser.ClientId.ToString();
                                System.Diagnostics.Debug.WriteLine("Process Rrequest clientSearchString: " + clientSearchString);
                            }
                        }
                    }

                    presenter.ShowMatchedNames(clientSearchString);
                }
            } catch (System.Exception) {}
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public void DisplayMatchedClientNameAndNum(string matchedClientsJSON)
        {
            matchedClientsJSON = "[" + matchedClientsJSON + "]";
            HttpContext.Current.Response.Write(matchedClientsJSON);
        }

    }
}