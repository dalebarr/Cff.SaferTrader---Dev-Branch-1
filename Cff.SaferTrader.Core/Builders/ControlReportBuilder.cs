using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class ControlReportBuilder
    {
        private readonly ICalendar calendar;

        public ControlReportBuilder(ICalendar calendar)
        {
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            this.calendar = calendar;
        }

        public ControlReport Build(DebtorsLedger debtorsLedger, FactorsLedger factorsLedger,
                                    DataRowCollection rows, string clientName, Date endDate, string title, int facilityType)
        {
            ArgumentChecker.ThrowIfNull(rows, "rows");
            
            var reader = new DataRowReader(rows);
            decimal balance = 0;
            decimal nonFundedBal = 0;
            decimal unclaimedRepurchase = 0;
            decimal unclaimedCr = 0;
            decimal unallocatedTransactions = 0;
            decimal allocatedTransactions = 0;
            decimal cbts = 0;

            if (reader.Read())
            {
                 balance = reader.ToDecimal("FundedBal");
                 nonFundedBal = reader.ToDecimal("NonFundedBal");
                 unclaimedRepurchase = reader.ToDecimal("UnclaimedRepurchase");
                 unclaimedCr = reader.ToDecimal("UnclaimedCr");
                 unallocatedTransactions = reader.ToDecimal("UnAllocatedTrns");
                 allocatedTransactions = reader.ToDecimal("AllocatedTrns");
                 cbts = reader.ToDecimal("CBTs");
            }
            return new ControlReport(calendar, title, clientName, debtorsLedger, factorsLedger, 
                                        balance, nonFundedBal, unclaimedRepurchase, unclaimedCr,
                                           unallocatedTransactions, endDate, allocatedTransactions, cbts, facilityType); 
        }
    }
}