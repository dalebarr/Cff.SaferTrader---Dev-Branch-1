namespace Cff.SaferTrader.Core
{
    public static class LinkHelper
    {
        static string _strRowIndex;

        public static void SetRowIndex(string value) {
            _strRowIndex = value;
        }

        public static string NavigateUrlFormatToDashboardForCustomer
        {
            get
            {
                string url;
                url = string.Format("~/{0}?{1}={{0}}&{2}={3}", Config.DashboardPage, QueryString.Customer, QueryString.Client, QueryString.ClientId);

                if (!string.IsNullOrEmpty(QueryString.UserId.ToString()))
                {
                    url += string.Format("&{0}={1}", QueryString.User, QueryString.UserId);
                }
                if (!string.IsNullOrEmpty(QueryString.CriteriaValue.ToString()))
                {
                    url += string.Format("&{0}={1}", QueryString.Criteria, QueryString.CriteriaValue);
                }
                else {
                    url += string.Format("&{0}={1}", QueryString.Criteria, 0);
                }
                if (!string.IsNullOrEmpty(QueryString.StartsWithValue.ToString()))
                {
                    url += string.Format("&{0}={1}", QueryString.StartsWith, QueryString.StartsWithValue);
                }
                else
                {
                    url += string.Format("&{0}={1}", QueryString.StartsWith, 1);
                }

                if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                {
                    url += string.Format("&{0}={1}", QueryString.ViewID, (QueryString.ViewIDValue + ( (string.IsNullOrEmpty(_strRowIndex)) ? "" : _strRowIndex.ToString())));
                }
                else
                {
                    //TODO:: generate new viewID here or redirect to logon
                    string viewIDValue = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);
                    url += string.Format("&{0}={1}", QueryString.ViewID, (viewIDValue + ((string.IsNullOrEmpty(_strRowIndex)) ? "" : _strRowIndex.ToString())));
                }

                return url;
            }
        }

        public static string NavigateUrlFormatToDashboardForClient
        {
            get
            {
                string url;
                url = string.Format("~/{0}?", Config.DashboardPage);

                if (!string.IsNullOrEmpty(QueryString.ClientId.ToString()))
                {
                    url += string.Format("{0}={1}", QueryString.Client, QueryString.ClientId);
                }
                if (!string.IsNullOrEmpty(QueryString.UserId.ToString()))
                {
                    url += string.Format("&{0}={1}", QueryString.User, QueryString.UserId);
                }
                if (!string.IsNullOrEmpty(QueryString.CriteriaValue.ToString()))
                {
                    url += string.Format("&{0}={1}", QueryString.Criteria, QueryString.CriteriaValue);
                }
                else
                {
                    url += string.Format("&{0}={1}", QueryString.Criteria, 0);
                }
                if (!string.IsNullOrEmpty(QueryString.StartsWithValue.ToString()))
                {
                    url += string.Format("&{0}={1}", QueryString.StartsWith, QueryString.StartsWithValue);
                }
                else
                {
                    url += string.Format("&{0}={1}", QueryString.StartsWith, 1);
                }

                if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                {
                    url += string.Format("&{0}={1}", QueryString.ViewID.ToString(), (QueryString.ViewIDValue + ((string.IsNullOrEmpty(_strRowIndex)) ? "" : _strRowIndex.ToString())));
                }
                else
                { //TODO:: generate new viewID here or redirect to logon
                    string viewIDValue = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);
                    url += string.Format("&{0}={1}", QueryString.ViewID, (viewIDValue + ((string.IsNullOrEmpty(_strRowIndex)) ? "" : _strRowIndex.ToString())));
                }

                return url;
            }
        }

        public static string NavigateUrlFormatToDashboardForAGivenClientId(string clientID)
        {
                string url;
                url = string.Format("~/{0}?", Config.DashboardPage);

                if (!string.IsNullOrEmpty(clientID))
                {
                    url += string.Format("{0}={1}", QueryString.Client, clientID);
                }

                if (!string.IsNullOrEmpty(QueryString.UserId.ToString()))
                {
                    url += string.Format("&{0}={1}", QueryString.User, QueryString.UserId);
                }
                
                if (!string.IsNullOrEmpty(QueryString.CriteriaValue.ToString()))
                {
                    url += string.Format("&{0}={1}", QueryString.Criteria, QueryString.CriteriaValue);
                }
                else
                {
                    url += string.Format("&{0}={1}", QueryString.Criteria, 0);
                }
                
                if (!string.IsNullOrEmpty(QueryString.StartsWithValue.ToString()))
                {
                    url += string.Format("&{0}={1}", QueryString.StartsWith, QueryString.StartsWithValue);
                }
                else
                {
                    url += string.Format("&{0}={1}", QueryString.StartsWith, 1);
                }

                if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                {
                    url += string.Format("&{0}={1}", QueryString.ViewID.ToString(), (QueryString.ViewIDValue + ((string.IsNullOrEmpty(_strRowIndex)) ? "" : _strRowIndex.ToString())));
                }
                else
                { //TODO:: generate new viewID here or redirect to logon
                    string viewIDValue = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);
                    url += string.Format("&{0}={1}", QueryString.ViewID, (viewIDValue + ((string.IsNullOrEmpty(_strRowIndex)) ? "" : _strRowIndex.ToString())));
                }

                return url;
        }

        public static string NavigateUrlFormatToDashboardForGivenCustomerId(string strCustId, int? pClientId=null)
        {
                string url;
                url = string.Format("~/{0}?{1}={2}&{3}={4}", Config.DashboardPage, QueryString.Customer,
                            strCustId, QueryString.Client, ((pClientId == null) ? QueryString.ClientId : pClientId));

                if (!string.IsNullOrEmpty(QueryString.UserId.ToString()))
                {
                    url += string.Format("&{0}={1}", QueryString.User, QueryString.UserId);
                }
                if (!string.IsNullOrEmpty(QueryString.CriteriaValue.ToString()))
                {
                    url += string.Format("&{0}={1}", QueryString.Criteria, QueryString.CriteriaValue);
                }
                else
                {
                    url += string.Format("&{0}={1}", QueryString.Criteria, 0);
                }
                if (!string.IsNullOrEmpty(QueryString.StartsWithValue.ToString()))
                {
                    url += string.Format("&{0}={1}", QueryString.StartsWith, QueryString.StartsWithValue);
                }
                else
                {
                    url += string.Format("&{0}={1}", QueryString.StartsWith, 1);
                }
            
                if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                {
                    url += string.Format("&{0}={1}", QueryString.ViewID.ToString(), (QueryString.ViewIDValue + ((string.IsNullOrEmpty(_strRowIndex)) ? "" : _strRowIndex.ToString())));
                }
                else
                { //TODO:: generate new viewID here or redirect to logon
                    string viewIDValue = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);
                    url += string.Format("&{0}={1}", QueryString.ViewID, (viewIDValue + ((string.IsNullOrEmpty(_strRowIndex)) ? "" : _strRowIndex.ToString())));
                }

                return url;
        }



        public static string NavigateUrlFormatToInvoiceBatchesForClient
        {
            get
            {
                string url;
                //string path = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority;
                url = string.Format("~/{0}?", Config.BatchDetailsPage);

                if (!string.IsNullOrEmpty(QueryString.ClientId.ToString()))
                {
                    url += string.Format("{0}={1}", QueryString.Client, QueryString.ClientId);
                }
                if (!string.IsNullOrEmpty(QueryString.UserId.ToString()))
                {
                    url += string.Format("&{0}={1}", QueryString.User, QueryString.UserId);
                }
                
                if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                {
                    url += string.Format("&{0}={1}", QueryString.ViewID.ToString(), (QueryString.ViewIDValue + ((string.IsNullOrEmpty(_strRowIndex)) ? "" : _strRowIndex.ToString())));
                }
                else
                { //TODO:: generate new viewID here or redirect to logon
                    string viewIDValue = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);
                    url += string.Format("&{0}={1}", QueryString.ViewID, (viewIDValue + ((string.IsNullOrEmpty(_strRowIndex)) ? "" : _strRowIndex.ToString())));
                }

                url += string.Format("&Batch={{0}}");

                return url;
            }
        }

        public static string NavigateUrlFormatToInvoiceBatchesForClientWBatchID(string batchID, string strCustID, string strClientID="")
        {
            string url;
            bool bHasClientQueryString = false;
            string sClientId = QueryString.ClientId.ToString();

            Scope sScope = (QueryString.ClientId < 0)?Scope.AllClientsScope:(QueryString.CustomerId!=null)?Scope.CustomerScope:Scope.ClientScope;
            if (!string.IsNullOrEmpty(QueryString.ViewIDValue)) {
                sScope = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).Scope;
                if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString != null) {
                    bHasClientQueryString = true;
                    sClientId = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id.ToString();
                }
            }

            if (SessionWrapper.Instance.Get != null) {
                sScope = SessionWrapper.Instance.Get.Scope;
                if  (SessionWrapper.Instance.Get.ClientFromQueryString!=null) {
                    bHasClientQueryString = true;
                    sClientId = SessionWrapper.Instance.Get.ClientFromQueryString.Id.ToString();
                }
            }
            
            url = string.Format("~/{0}?", Config.BatchDetailsPage);  //string path = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority;
            if (!string.IsNullOrEmpty(strClientID))
            {
                url += string.Format("{0}={1}", QueryString.Client, strClientID);
            }
            else if (!string.IsNullOrEmpty(QueryString.ClientId.ToString()))
            {
                url += string.Format("{0}={1}", QueryString.Client, QueryString.ClientId);
            }
            else {
                if ((sScope == Scope.CustomerScope) && bHasClientQueryString)
                    url += string.Format("&{0}={1}", QueryString.Client, sClientId);
            }

            if (!string.IsNullOrEmpty(strCustID))
            {
                url += string.Format("&{0}={1}", QueryString.Customer, strCustID);
            }
            else {
                if ((sScope == Scope.CustomerScope) && (QueryString.CustomerId!=null)) 
                        url += string.Format("&{0}={1}", QueryString.Customer, QueryString.CustomerId);
            }

            if (!string.IsNullOrEmpty(QueryString.UserId.ToString()))
            {
                url += string.Format("&{0}={1}", QueryString.User, QueryString.UserId);
            }

            if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
            {
                url += string.Format("&{0}={1}", QueryString.ViewID.ToString(), (QueryString.ViewIDValue + ( (string.IsNullOrEmpty(_strRowIndex)) ? "" : _strRowIndex.ToString())));
            }
            else
            { //TODO:: generate new viewID here or redirect to logon
                string viewIDValue = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);
                url += string.Format("&{0}={1}", QueryString.ViewID, (viewIDValue + ((string.IsNullOrEmpty(_strRowIndex)) ? "" : _strRowIndex.ToString())));
            }

            url += string.Format("&Batch={0}", batchID);
            return url;
        }

    }
}