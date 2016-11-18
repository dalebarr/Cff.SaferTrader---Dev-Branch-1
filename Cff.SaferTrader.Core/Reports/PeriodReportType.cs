using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
     [Serializable]
    public class PeriodReportType
    {
        private readonly int id;
        private readonly string text;
        private static readonly PeriodReportType all = new PeriodReportType(110, "All");
        private static readonly PeriodReportType current = new PeriodReportType(111, "Current");
        private static readonly PeriodReportType oneMonth = new PeriodReportType(112, "1 Month");
        private static readonly PeriodReportType twoMonths = new PeriodReportType(113, "2 Months");
        private static readonly PeriodReportType threeMonths = new PeriodReportType(114, "3 Months");
        private static readonly IList<PeriodReportType> knownTypes = InitializeKnownStatus();

        private static IList<PeriodReportType> InitializeKnownStatus()
        {
            IList<PeriodReportType> options = new List<PeriodReportType>();
            options.Add(all);
            options.Add(current);
            options.Add(oneMonth);
            options.Add(twoMonths);
            options.Add(threeMonths);
            
            return options;
        }

        private PeriodReportType(int id, string text)
        {
            this.id = id;
            this.text = text;
        }

        public static PeriodReportType Instantiate(int id)
        {
            PeriodReportType reportType = null;
            foreach (PeriodReportType knownReportType in knownTypes)
            {
                if (id == knownReportType.id)
                {
                    reportType = knownReportType;
                    break;
                }
            }
            if (reportType == null)
            {
                throw new InvalidOperationException("Unknown calls due report ID");
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

        public static PeriodReportType All
        {
            get { return all;}
        }

        public static PeriodReportType OneMonth
        {
            get { return oneMonth; }
        }

        public static PeriodReportType TwoMonths
        {
            get { return twoMonths; }
        }

        public static PeriodReportType ThreeMonths
        {
            get { return threeMonths; }
        }

        public static PeriodReportType Current
        {
            get { return current; }
        }

        public static IList<PeriodReportType> KnownTypes
        {
            get { return knownTypes; }
        }
    }
}