using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class DeductablesBuilder
    {
        public Deductables Build(DataRowCollection rows)
        {
            decimal unclaimedCredits = 0;
            decimal unclaimedRepurchases = 0;
            decimal unclaimedDiscount = 0;
            decimal likelyRepurchases = 0;
            decimal overdueCharges = 0;
            Fee chequeFee = Fee.None;
            Fee postage = Fee.None;
            Fee letterFee = Fee.None;

            DataRowReader reader = new DataRowReader(rows);
            if (reader.Read())
            {
                unclaimedCredits = reader.ToDecimal("UnclaimedCredits");
                unclaimedRepurchases = reader.ToDecimal("UnclaimedRepurchase");
                unclaimedDiscount = reader.ToDecimal("UnclaimedDiscount");
                likelyRepurchases = reader.ToDecimal("LikelyRepurchases");
                overdueCharges = reader.ToDecimal("OverdueCharges");

                chequeFee = new Fee(reader.ToDecimal("ChequeFee"), reader.ToInteger("CountReceipts"));
                postage = new Fee(reader.ToDecimal("CustomerPostage"), reader.ToInteger("CountCustWithInvoice"));
                letterFee = new Fee(reader.ToDecimal("Letterrate"), reader.ToInteger("LetterCount"));
            }
            return new Deductables(unclaimedCredits, 
                                        unclaimedRepurchases, 
                                        unclaimedDiscount, 
                                        likelyRepurchases, 
                                        overdueCharges,
                                        chequeFee,
                                        postage,
                                        letterFee);
        }
    }
}