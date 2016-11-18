using System;
using System.Web;

namespace Cff.SaferTrader.Core
{
    public class QueryString
    {
        private readonly string name;

        private QueryString(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }

        public static QueryString Customer
        {
            get { return new QueryString("Customer"); }
        }

        public static QueryString Client
        {
            get { return new QueryString("Client"); }
        }

        public static QueryString Criteria
        {
            get { return new QueryString("Criteria"); }
        }

        public static QueryString StartsWith
        {
            get { return new QueryString("StartsWith"); }
        }

        public static QueryString User
        {
            get { return new QueryString("User"); }
        }

        public static QueryString LogonParam
        {
            get { return new QueryString("ComID"); }
        }
        public static QueryString ActivateParam1
        {
            get { return new QueryString("uKey"); }
        }
        public static QueryString ActivateParam2
        {
            get { return new QueryString("pKey"); }
        }
        public static QueryString ApprovalParam1
        {
            get { return new QueryString("mKey"); }
        }
        public static QueryString ApprovalParam2
        {
            get { return new QueryString("action"); }
        }

        public static QueryString LogonParamId
        {
            get { return new QueryString("Id"); }
        }

        public static QueryString ViewID
        {
            get { return new QueryString("ViewID"); }
        }

        public static QueryString Batch
        {
            get { return new QueryString("Batch"); }
        }

        public static bool IsInteger(string value)
        {
            bool isInteger = false;
            if (!string.IsNullOrEmpty(value))
            {
                int parsed;
                try
                {
                    parsed = int.Parse(value);
                }
                catch (FormatException)
                {
                    parsed = int.MinValue;
                }
                isInteger = parsed != int.MinValue;
            }
            return isInteger;
        }

        public static int? ClientId
        {
            get
            {
                return GetFromQueryString("Client");
            }
        }

        //public static int? CustomerId
        //{
        //    get { return (string.IsNullOrEmpty(GetFromQueryString2("Customer"))?null:Convert.ToInt32(GetFromQueryString2("Customer"))); } 
        //    set { HttpContext.Current.Request.QueryString["Customer"] = value; }
        //}

        public static int? CustomerId
        {
            get { return GetFromQueryString("Customer"); }
            set { HttpContext.Current.Request.QueryString["CustomerId"] = value.ToString(); }
        }


        public static int? UserId
        {
            get { return GetFromQueryString("User"); }
        }

        public static int? CriteriaValue
        {
            get { return GetFromQueryString("Criteria"); }
        }

        public static int? StartsWithValue
        {

            get { return GetFromQueryString("StartsWith"); }
        }

        public static string ViewIDValue
        {
            get { return GetFromQueryString2("ViewID"); }
            set { HttpContext.Current.Request.QueryString["ViewID"] = value; }
        }


        private static int? GetFromQueryString(string key)
        {
            string value = HttpContext.Current.Request.QueryString[key];
            if (IsInteger(value))
            {
                return int.Parse(value);
            }
            return null;
        }

        private static string GetFromQueryString2(string key)
        {
            if (HttpContext.Current.Request.QueryString[key] == null)
                return null;

            return (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[key].ToString()) ? null : HttpContext.Current.Request.QueryString[key].ToString());
        }


    }
}