using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class AllClientsOrderByType
    {
        private readonly string orderString;
        private readonly string text;

        private static readonly AllClientsOrderByType sortByBalance_Client =
            new AllClientsOrderByType("Balance descending,ClientName", "Balance - Client");

        private static readonly AllClientsOrderByType sortByBalance_Customer =
            new AllClientsOrderByType("Balance descending,CustomerName", "Balance - Customer");

        private static readonly AllClientsOrderByType sortByClient_Balance =
            new AllClientsOrderByType("ClientName,Balance descending", "Client - Balance");

        private static readonly AllClientsOrderByType sortByClient_Customer =
            new AllClientsOrderByType("ClientName,CustomerName", "Client - Customer");

        private static readonly AllClientsOrderByType sortByClient_Date = new AllClientsOrderByType("ClientName,Due",
                                                                                                  "Client - Date");

        private static readonly AllClientsOrderByType sortByCustomer = new AllClientsOrderByType("CustomerName",
                                                                                               "Customer");

        private static readonly AllClientsOrderByType sortByCustomerNumber = new AllClientsOrderByType("CustomerNumber",
                                                                                                     "Customer Number");

        private static readonly AllClientsOrderByType sortByDate_Client =
            new AllClientsOrderByType("Due,ClientName,CustomerName", "Date - Client");

        private static readonly AllClientsOrderByType sortByDate_Customer = new AllClientsOrderByType("Due,CustomerName",
                                                                                                    "Date - Customer");


        private static readonly IList<AllClientsOrderByType> knownTypes = InitializeKnownStatus();


        private static IList<AllClientsOrderByType> InitializeKnownStatus()
        {
            IList<AllClientsOrderByType> options = new List<AllClientsOrderByType>();
            options.Add(sortByBalance_Client);
            options.Add(sortByBalance_Customer);
            options.Add(sortByClient_Balance);
            options.Add(sortByClient_Customer);
            options.Add(sortByClient_Date);
            options.Add(sortByCustomer);
            options.Add(sortByCustomerNumber);
            options.Add(sortByDate_Client);
            options.Add(sortByDate_Customer);
            return options;
        }

        private AllClientsOrderByType(string orderString, string text)
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

        public static AllClientsOrderByType Instantiate(string id)
        {
            AllClientsOrderByType reportType = null;
            foreach (AllClientsOrderByType knownReportType in knownTypes)
            {
                if (id == knownReportType.orderString)
                {
                    reportType = knownReportType;
                    break;
                }
            }
            if (reportType == null)
            {
                throw new InvalidOperationException("Unknown orderBy Query String");
            }
            return reportType;
        }

        public static IList<AllClientsOrderByType> KnownTypes
        {
            get { return knownTypes; }
        }

        public static AllClientsOrderByType SortByBalanceClient
        {
            get { return sortByBalance_Client; }
        }

        public static AllClientsOrderByType SortByBalanceCustomer
        {
            get { return sortByBalance_Customer; }
        }

        public static AllClientsOrderByType SortByClientBalance
        {
            get { return sortByClient_Balance; }
        }

        public static AllClientsOrderByType SortByClientCustomer
        {
            get { return sortByClient_Customer; }
        }

        public static AllClientsOrderByType SortByClientDate
        {
            get { return sortByClient_Date; }
        }

        public static AllClientsOrderByType SortByCustomer
        {
            get { return sortByCustomer; }
        }

        public static AllClientsOrderByType SortByCustomerNumber
        {
            get { return sortByCustomerNumber; }
        }

        public static AllClientsOrderByType SortByDateClient
        {
            get { return sortByDate_Client; }
        }

        public static AllClientsOrderByType SortByDateCustomer
        {
            get { return sortByDate_Customer; }
        }

        public static bool IsValid(string orderByString)
        {
            AllClientsOrderByType reportType = null;
            foreach (AllClientsOrderByType knownReportType in knownTypes)
            {
                if (orderByString == knownReportType.orderString)
                {
                    reportType = knownReportType;
                    break;
                }
            }
            if (reportType == null)
            {
                return false;
            }
            return true;
        }
    }
}