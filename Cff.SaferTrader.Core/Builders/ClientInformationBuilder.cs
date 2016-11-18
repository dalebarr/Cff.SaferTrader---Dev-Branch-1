using System;
using System.Reflection;
namespace Cff.SaferTrader.Core.Builders
{
    public class ClientInformationBuilder
    {
        private readonly CleverReader reader;

        public ClientInformationBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public ClientInformation Build()
        {
            //MSarza [20150901] : Data type changed from bool to small int for dbo.ClientFinancials.CffDebtorAdmin.
            //string cffDebtorAdmin = "Yes";
            //if (reader.ToBoolean("DebtorAdmin") == false)
            //{
            //    cffDebtorAdmin = "No";
            //}
            
            bool cffDebtorAdmin = ( CffDebtorAdminHelper.CffIsDebtorAdminForClient(reader.ToSmallInteger("DebtorAdmin"))) ? true : false;
            //string debtorAdministration = Enum.Parse(typeof(CffDebtorAdmin), reader.ToSmallInteger("DebtorAdmin").ToString()).ToString();

            return new ClientInformation(reader.FromBigInteger("ClientNum"), 
                                         reader.FromBigInteger("Companynum"), 
                                         reader.ToInteger("ClientActions"), 
                                         reader.ToDate("Created"), 
                                         reader.ToString("StandardTerms"), 
                                         reader.ToString("SetCustTerms"), 
                                         reader.ToString("SetInvTerms"), 
                                         reader.ToString("Facilitytype"), 
                                         reader.ToString("GSTNumber"),
                                         reader.ToDecimal("CurrentAccountCustLimitSum"),
                                         reader.ToBoolean("HasOwnLetterTemplates"),
                                         //debtorAdministration
                                         reader.ToSmallInteger("DebtorAdmin")
                                         );
        }
    }
}