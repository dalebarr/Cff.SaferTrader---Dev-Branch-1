using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class AgedBalancesReportType
    {
        private readonly int id;
        private readonly string text;
        private static readonly AgedBalancesReportType all = new AgedBalancesReportType(1000, "All");
        private static readonly AgedBalancesReportType oneMonth = new AgedBalancesReportType(1010, ">=1 Month");
        private static readonly AgedBalancesReportType twoMonths = new AgedBalancesReportType(1020, ">= 2 Months");
        private static readonly AgedBalancesReportType threeMonths = new AgedBalancesReportType(1030, ">= 3 Months");
        private static readonly AgedBalancesReportType creditBalances = new AgedBalancesReportType(1060, "Credit Balances");

        private static readonly AgedBalancesReportType allNotes = new AgedBalancesReportType(1001, "All with Notes");
        private static readonly AgedBalancesReportType oneMoNotes= new AgedBalancesReportType(1011, ">=1 Month with Notes");
        private static readonly AgedBalancesReportType twoMoNotes= new AgedBalancesReportType(1021, ">=2 Month with Notes");
        private static readonly AgedBalancesReportType threeMoNotes = new AgedBalancesReportType(1031, ">=3 Month with Notes");
        private static readonly AgedBalancesReportType creditBalNotes = new AgedBalancesReportType(1061, "Credit Balances with Notes");

        private static readonly IList<AgedBalancesReportType> knownTypes = InitializeKnownStatus();
        private static readonly IList<AgedBalancesReportType> knownTypesWithNotes = InitializeKnownStatusWithNotes();

        private static IList<AgedBalancesReportType> InitializeKnownStatus()
        {
            IList<AgedBalancesReportType> options = new List<AgedBalancesReportType>();
            options.Add(all);
            options.Add(oneMonth);
            options.Add(twoMonths);
            options.Add(threeMonths);
            options.Add(creditBalances);
            return options;
        }

        private static IList<AgedBalancesReportType> InitializeKnownStatusWithNotes()
        {
            IList<AgedBalancesReportType> options = new List<AgedBalancesReportType>();
            options.Add(allNotes);
            options.Add(oneMoNotes);
            options.Add(twoMoNotes);
            options.Add(threeMoNotes);
            options.Add(creditBalNotes);
            return options;
        }

        private AgedBalancesReportType(int id, string text)
        {
            this.id = id;
            this.text = text;
        }

        public static AgedBalancesReportType Instantiate(int id)
        {
            AgedBalancesReportType reportType = null;
            foreach (AgedBalancesReportType knownReportType in knownTypes)
            {
                if (id == knownReportType.id)
                {
                    reportType = knownReportType;
                    break;
                }
            }

            if (reportType == null)
            {
                foreach (AgedBalancesReportType knownReportType in knownTypesWithNotes)
                {
                    if (id == knownReportType.id)
                    {
                        reportType = knownReportType;
                        break;
                    }
                }
            }

            if (reportType == null)
            {
                throw new InvalidOperationException("Unknown aged balance report ID");
            }
            return reportType;
        }

        public int Id
        {
            get { return id; }
        }

        public string Text
        {
            get { return text; }
        }

        public static AgedBalancesReportType All
        {
            get { return all;}
        }

        public static AgedBalancesReportType OneMonth
        {
            get { return oneMonth; }
        }

        public static AgedBalancesReportType TwoMonths
        {
            get { return twoMonths; }
        }

        public static AgedBalancesReportType ThreeMonths
        {
            get { return threeMonths; }
        }

        public static AgedBalancesReportType CreditBalances
        {
            get { return creditBalances; }
        }


        public static AgedBalancesReportType AllNotes
        {
            get { return allNotes; }
        }

        public static AgedBalancesReportType OneMoNotes
        {
            get { return oneMoNotes; }
        }

        public static AgedBalancesReportType TwoMoNotes
        {
            get { return twoMoNotes; }
        }

        public static AgedBalancesReportType ThreeMoNotes
        {
            get { return threeMoNotes; }
        }

        public static AgedBalancesReportType CreditBalNotes
        {
            get { return creditBalNotes; }
        }
    
        public static IList<AgedBalancesReportType> KnownTypes
        {
            get { return knownTypes; }
        }

        public static IList<AgedBalancesReportType> KnownTypesWithNotes
        {
            get { return knownTypesWithNotes; }
        }
    }
}