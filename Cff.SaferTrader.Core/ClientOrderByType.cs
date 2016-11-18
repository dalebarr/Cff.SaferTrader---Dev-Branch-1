using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    public class ClientOrderByType
    {

        private readonly string orderString;
        private readonly string text;

        private static readonly ClientOrderByType sortByBalance =
            new ClientOrderByType("Balance descending", "Balance");

        private static readonly ClientOrderByType sortByCustomer =
            new ClientOrderByType("CustomerName", "Customer");

        private static readonly ClientOrderByType sortByCustomerNumber =
            new ClientOrderByType("CustomerNumber", "Customer Number");

        private static readonly ClientOrderByType sortByDate =
            new ClientOrderByType("Due", "Date");

        public static ClientOrderByType SortByBalance
        {
            get { return sortByBalance; }
        }

        public static ClientOrderByType SortByCustomer
        {
            get { return sortByCustomer; }
        }

        public static ClientOrderByType SortByCustomerNumber
        {
            get { return sortByCustomerNumber; }
        }

        public static ClientOrderByType SortByDate
        {
            get { return sortByDate; }
        }

        private static readonly IList<ClientOrderByType> knownTypes = InitializeKnownStatus();


        private static IList<ClientOrderByType> InitializeKnownStatus()
        {
            IList<ClientOrderByType> options = new List<ClientOrderByType>();
            options.Add(sortByBalance);
            options.Add(sortByCustomer);
            options.Add(sortByCustomerNumber);
            options.Add(sortByDate);
            return options;
        }

        private ClientOrderByType(string orderString, string text)
        {
            this.orderString = orderString;
            this.text = text;
        }

        public string Text
        {
            get { return text; }
        }

        public string OrderString
        {
            get { return orderString; }
        }

        public static ClientOrderByType Instantiate(string id)
        {
            ClientOrderByType orderByType = null;
            foreach (ClientOrderByType knownReportType in knownTypes)
            {
                if (id == knownReportType.orderString)
                {
                    orderByType = knownReportType;
                    break;
                }
            }
            if (orderByType == null)
            {
                throw new InvalidOperationException("Unknown orderBy Query String");
            }
            return orderByType;
        }

        public static IList<ClientOrderByType> KnownTypes
        {
            get { return knownTypes; }
        }

        public static bool IsValid(string orderByString)
        {

            ClientOrderByType orderByType = null;
            foreach (ClientOrderByType knownReportType in knownTypes)
            {
                if (orderByString == knownReportType.orderString)
                {
                    orderByType = knownReportType;
                    break;
                }
            }
            if (orderByType == null)
            {
                return false;
            }
            return true;
        }
    }
}
