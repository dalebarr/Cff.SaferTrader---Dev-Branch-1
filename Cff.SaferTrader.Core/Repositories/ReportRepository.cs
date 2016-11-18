using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Cff.SaferTrader.Core.Builders;
using Cff.SaferTrader.Core.Reports;
using DataEntry;
using System;

namespace Cff.SaferTrader.Core.Repositories
{
    //Note: do not forget that we are calling functions from VB DLL so we need to check if tables are nullified
    public class ReportRepository : SqlManager, IReportRepository
    {
        private readonly ICalendar calendar;
        private readonly DataEntry.Reports reports;
        private readonly Customer customers;
        private readonly DataEntry.GetOldData deOldData;

        public ReportRepository(ICalendar calendar, string connectionString): base(connectionString)
        {
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            this.calendar = calendar;
            reports = new DataEntry.Reports();
            customers = new Customer();
            deOldData = new DataEntry.GetOldData();
        }
        
        public TransactionReport LoadCreditNotesReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            try
            {
              DataRowCollection rows = null;
                DataTableCollection tables = LoadReportTables((int)ReportType.CreditNotesReport, endDate, clientId, ClientFacilityType);
                if (tables != null) {
                    if (tables.Count > 1)
                         rows = tables[1].Rows;
                  
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    string readReportTitle = ReadReportTitle(tables[0].Rows);
                    IList<TransactionReportRecord> records = new TransactionReportRecordBuilder().Build(rows, clientNumber, clientName, SessionWrapper.Instance.Get.SelectedTransactionFilter);

                    if (SessionWrapper.Instance.Get != null)
                        readReportTitle = SessionWrapper.Instance.Get.SelectedTransactionFilter != null && SessionWrapper.Instance.Get.SelectedTransactionFilter != "All" ? string.Format("{0} {1}", SessionWrapper.Instance.Get.SelectedTransactionFilter, readReportTitle) : readReportTitle;

                    return new TransactionReport(calendar, readReportTitle, clientName, records, "Credit Notes");
                }
            } 
            catch
            {
            
            }
            return null;
        }

        public TransactionReport LoadJournalsReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadReportTables((int)ReportType.JournalsReport, endDate, clientId, ClientFacilityType);
                if (tables != null) {
                    if (tables.Count > 1)
                        rows = tables[1].Rows;

                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<TransactionReportRecord> records = new TransactionReportRecordBuilder().Build(rows, clientNumber, clientName, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    string readReportTitle = ReadReportTitle(tables[0].Rows);
                    if (SessionWrapper.Instance.Get != null)
                        readReportTitle = SessionWrapper.Instance.Get.SelectedTransactionFilter != null && SessionWrapper.Instance.Get.SelectedTransactionFilter != "All" ? string.Format("{0} {1}", SessionWrapper.Instance.Get.SelectedTransactionFilter, readReportTitle) : readReportTitle;
                    
                    return new TransactionReport(calendar, readReportTitle, clientName, records, "Journals");
                }
            }
            catch { }
            return null;
        }

        public TransactionReport LoadReceiptsReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadReportTables((int)ReportType.ReceiptsReport, endDate, clientId, ClientFacilityType);
                if (tables != null) {
                    if (tables.Count > 1)
                            rows = tables[1].Rows;

                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<TransactionReportRecord> records = new TransactionReportRecordBuilder().Build(rows, clientNumber, clientName, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    string readReportTitle = ReadReportTitle(tables[0].Rows);
                    if (SessionWrapper.Instance.Get != null)
                        readReportTitle = SessionWrapper.Instance.Get.SelectedTransactionFilter != null && SessionWrapper.Instance.Get.SelectedTransactionFilter != "All" ? string.Format("{0} {1}", SessionWrapper.Instance.Get.SelectedTransactionFilter, readReportTitle) : readReportTitle;

                    return new TransactionReport(calendar, readReportTitle, clientName, records, "Receipts");
                }
            }
            catch { }
            return null;
        }

        public CustomerTransactionReport LoadReceiptReportForCustomer(DateRange dateRange, int customerId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            try
            {
                DataTableCollection tables = LoadCustomerReportTables(ReportType.ReceiptsReport, dateRange, customerId);
                if (tables != null) {
                    IList<CustomerTransactionReportRecord> records = new CustomerTransactionReportRecordBuilder(calendar).Build(tables[1].Rows);
                    return new CustomerTransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadCustomerName(tables[0].Rows), records, "Receipts");
                }
            }
            catch { }
            return null;
        }

        public InvoicesTransactionReport LoadInvoicesReport(Date endDate, int clientId, string TransactionFilter, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadReportTables((int)ReportType.InvoicesReport, endDate, clientId, ClientFacilityType);
                if (tables != null) {
                    string clientName = ReadClientName(tables[0].Rows);
                    if (tables.Count > 1)
                        rows = tables[1].Rows;
                    IList<InvoicesTransactionReportRecord> records = new InvoicesTransactionReportRecordBuilder(calendar).Build(rows, ReadClientNumber(tables[0].Rows), clientName, TransactionFilter);
                    string readReportTitle = ReadReportTitle(tables[0].Rows);
                    readReportTitle = TransactionFilter != null && TransactionFilter != "All" ? string.Format("{0} {1}", TransactionFilter, readReportTitle) : readReportTitle;
                    return new InvoicesTransactionReport(calendar, readReportTitle, clientName, records);
                }
            }
            catch { }
            return null;
        }

        public InvoicesTransactionReport LoadInvoicesReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadReportTables((int)ReportType.InvoicesReport, endDate, clientId, ClientFacilityType);
                if (tables != null) {
                    string clientName = ReadClientName(tables[0].Rows);
                    if (tables.Count > 1)
                            rows = tables[1].Rows;

                    IList<InvoicesTransactionReportRecord> records = new InvoicesTransactionReportRecordBuilder(calendar).Build(rows, ReadClientNumber(tables[0].Rows), clientName);
                    return new InvoicesTransactionReport(calendar, ReadReportTitle(tables[0].Rows), clientName, records);
                }
            }
            catch { }
            return null;
        }


        public InvoicesTransactionReport LoadInvoicesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType,string TransactionFilter)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");

            try
            {
                IList<InvoicesTransactionReportRecord> records = new List<InvoicesTransactionReportRecord>();
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.InvoicesReport, endDate, facilityType, isSalvageIncluded);
                if (tables != null) {
                    if(tables.Count > 1)
                        records = new InvoicesTransactionReportRecordBuilder(calendar).BuildForAllClients(tables[1].Rows, TransactionFilter);
                    return new InvoicesTransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records);
                }
            }
            catch { }
            return null;
        }

        public CustomerInvoicesTransactionReport LoadInvoicesReportForCustomer(DateRange dateRange, int customerId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            try
            {
                IList<CustomerInvoicesTransactionReportRecord> records = new List<CustomerInvoicesTransactionReportRecord>();
                DataTableCollection tables = LoadCustomerReportTables(ReportType.InvoicesReport, dateRange, customerId);
                if (tables != null) {
                    if (tables.Count > 1)
                        records = new CustomerInvoicesTransactionReportRecordBuilder(calendar).Build(tables[1].Rows);
                    return new CustomerInvoicesTransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadCustomerName(tables[0].Rows), records);
                }
            }
            catch { }
            return null;
        }

        private DataTableCollection LoadCustomerReportTables(ReportType reportType, DateRange dateRange, int customerId)
        {
            try {
                Date startDate = dateRange.StartDate;
                Date endDate = dateRange.EndDate;

                int whichType;
                int archiveLength;

                // if date range is a single month
                if (startDate.Year == endDate.Year && startDate.Month == endDate.Month)
                {
                    whichType = 7;
                    archiveLength = 0;
                }
                else
                {
                    whichType = 8;
                    archiveLength = startDate.ToYearMonthValue();
                }

                DataSet theDS = null;
                if (customers == null)
                { //handle when null
                    Customer xCust = new Customer();
                    theDS = xCust.getCustomerHistory(customerId, (short)whichType, (int)reportType, endDate.ToYearMonthDayValue(), archiveLength);
                }
                else
                    theDS = customers.getCustomerHistory(customerId, (short)whichType, (int)reportType, endDate.ToYearMonthDayValue(), archiveLength);
                if (theDS != null)
                    if (theDS.Tables != null)
                        return theDS.Tables;
            }
            catch {}
            return null;
        }

        public CurrentPaymentsReport LoadCurrentOverpaidReport(int clientId, int ClientFacilityType)
        {
            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadReportTables((int)ReportType.CurrentOverpaidReport, calendar.Now, clientId, ClientFacilityType);
                if (tables != null) {
                    if (tables.Count > 1)
                        rows = tables[1].Rows;

                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<CurrentPaymentsReportRecord> records = new CurrentsPaymentReportRecordBuilder().Build(rows, clientNumber, clientName);
                    return new CurrentPaymentsReport(calendar, ReadReportTitle(tables[0].Rows), clientName, records, "Current Overpaid Report");
                }
            }
            catch { }
            return null;
        }

        public CurrentPaymentsReport LoadCurrentOverpaidReportForAllClients(FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.CurrentOverpaidReport, calendar.Now, facilityType, isSalvageIncluded);
                if (tables != null){
                    if (tables.Count>1)
                        rows = tables[1].Rows;

                    IList<CurrentPaymentsReportRecord> records = new CurrentsPaymentReportRecordBuilder().BuildForAllClients(rows);
                    return new CurrentPaymentsReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Current Overpaid Report");
                }
            }
            catch { }
            return null;
        }

        public TransactionReport LoadDiscountsReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadReportTables((int)ReportType.DicountsReport, endDate, clientId, ClientFacilityType);
                if (tables != null) {
                    if (tables.Count >1 )
                            rows = tables[1].Rows;

                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<TransactionReportRecord> records = new TransactionReportRecordBuilder().Build(rows, clientNumber, clientName, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    string readReportTitle = ReadReportTitle(tables[0].Rows);

                    if (SessionWrapper.Instance.Get != null)
                        readReportTitle = SessionWrapper.Instance.Get.SelectedTransactionFilter != null && SessionWrapper.Instance.Get.SelectedTransactionFilter != "All" ? string.Format("{0} {1}", SessionWrapper.Instance.Get.SelectedTransactionFilter, readReportTitle) : readReportTitle;

                    return new TransactionReport(calendar, readReportTitle, clientName, records, "Discounts");
                }
            }
            catch { }
            return null;
        }

        public TransactionReport LoadRepurchasesReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadReportTables((int)ReportType.RepurchaseReport, endDate, clientId, ClientFacilityType);
                if (tables != null) {
                    if (tables.Count > 1)
                        rows = tables[1].Rows;

                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<TransactionReportRecord> records = new TransactionReportRecordBuilder().Build(rows, clientNumber, clientName, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    string readReportTitle = ReadReportTitle(tables[0].Rows);

                    if (SessionWrapper.Instance.Get != null)
                        readReportTitle = SessionWrapper.Instance.Get.SelectedTransactionFilter != null && SessionWrapper.Instance.Get.SelectedTransactionFilter != "All" ? string.Format("{0} {1}", SessionWrapper.Instance.Get.SelectedTransactionFilter, readReportTitle) : readReportTitle;

                    return new TransactionReport(calendar, readReportTitle, clientName, records, "Repurchases");
                }
            }
            catch { }
            return null;
        }

        public TransactionReport LoadCreditBalanceTransfersReport(Date endDate, int clientId, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");

            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadReportTables((int)ReportType.CreditBalanceTransfersReport, endDate, clientId, ClientFacilityType);
                if (tables != null) {
                    if (tables.Count>1)
                            rows = tables[1].Rows;

                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<TransactionReportRecord> records = new TransactionReportRecordBuilder().Build(rows, clientNumber, clientName, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    string readReportTitle = ReadReportTitle(tables[0].Rows);
                    if (SessionWrapper.Instance.Get != null)
                        readReportTitle = SessionWrapper.Instance.Get.SelectedTransactionFilter != null && SessionWrapper.Instance.Get.SelectedTransactionFilter != "All" ? string.Format("{0} {1}", SessionWrapper.Instance.Get.SelectedTransactionFilter, readReportTitle) : readReportTitle;

                    return new TransactionReport(calendar, readReportTitle, clientName, records, "Credit Balance Transfers");
                }
            }
            catch { }
            return null;
        }

        public AgedBalancesReport LoadAgedBalancesReport(Date endDate, int clientId, AgedBalancesReportType reportType, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(reportType, "reportType");

            try
            {
                DataTableCollection tables = LoadReportTables(reportType.Id, endDate, clientId, ClientFacilityType);
                if (tables != null)
                {
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);

                    IList<AgedBalancesReportRecord> records;
                    if (tables.Count == 1)
                    {
                        records = new AgedBalancesReportRecordBuilder().Build(tables[0].Rows, clientNumber, clientName);
                    }
                    else
                    {
                        records = new AgedBalancesReportRecordBuilder().Build(tables[1].Rows, clientNumber, clientName);
                    }

                    return new AgedBalancesReport(calendar, ReadReportTitle(tables[0].Rows), records, reportType, clientName, endDate);
                }
              
            }
            catch { }
            return null;
        }

        public AgedBalancesReport LoadAgedBalancesReportForAllClients(AgedBalancesReportType reportType, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(reportType, "reportType");
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");

            try {
                IList<AgedBalancesReportRecord> records;
                DataTableCollection tables = LoadAllClientsReportTables(reportType.Id, calendar.Now, facilityType, isSalvageIncluded);
                if (tables != null) {
                    if (tables.Count == 1)
                    {
                        records = new AgedBalancesReportRecordBuilder().Build(tables[0].Rows, -1, "All Clients");
                    }
                    else
                    {
                        records = new AgedBalancesReportRecordBuilder().BuildForAllClients(tables[1].Rows);
                    }

                    return new AgedBalancesReport(calendar, ReadReportTitle(tables[0].Rows), records, reportType, ReadClientName(tables[0].Rows), calendar.Now);
                }
            }
            catch { }
            return null;
        }

        public CurrentPaymentsReport LoadCurrentShortPaidReportForAllClients(FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            try
            {
                IList<CurrentPaymentsReportRecord> records = null;
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.CurrentShortPaidReport, calendar.Now, facilityType, isSalvageIncluded);
                if (tables!=null)
                    if (tables.Count > 1)
                            records = new CurrentsPaymentReportRecordBuilder().BuildForAllClients(tables[1].Rows);

                return new CurrentPaymentsReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Current Short Paid");
            }
            catch { }
            return null;
        }

        private int ClientFacilityType
        {
            get
            {
                if (SessionWrapper.Instance.Get != null)
                {
                    if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
                        return SessionWrapper.Instance.Get.ClientFromQueryString.ClientFacilityType;
                }
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                {
                    if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString != null)
                        return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.ClientFacilityType;
                }

                return 0;
            }
        }

        public ControlReport LoadControlReport(Date endDate, int clientId)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");

            int facilityType = this.ClientFacilityType;

            try
            {
                Subledger broughtForwardLedger  = null;
                DebtorsLedger debtorsLedger  = null;
                Subledger currentLedger = null;

                DataTableCollection tables = reports.Get_RptsTrns((int)ReportType.ControlReport, clientId, -1, endDate.ToShortDateString(), 7, 0, 0, -1, 0, 0, 1, "Factoring").Tables;
                if (tables != null) {
                    if (tables.Count > 1)
                        debtorsLedger = new DebtorsLedgerBuilder().Build(tables[1].Rows);

                    if (tables.Count > 2)
                        broughtForwardLedger = new SubledgerBuilder().Build(tables[2].Rows);

                    if (tables.Count > 3)
                        currentLedger = new SubledgerBuilder().Build(tables[3].Rows);

                    FactorsLedger factorsLedger = new FactorsLedgerBuilder().Build(broughtForwardLedger, currentLedger);

                    if (tables.Count>1)
                        return new ControlReportBuilder(calendar).Build(debtorsLedger, factorsLedger, tables[1].Rows, ReadClientName(tables[0].Rows),
                                                            endDate, ReadReportTitle(tables[0].Rows), facilityType);
                }
            }
            catch { }
            return null;
        }

        public ControlReport LoadControlReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");

            try
            {
                DebtorsLedger debtorsLedger  = null;
                Subledger broughtForwardLedger = null;
                Subledger currentLedger = null;

                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.ControlReport, endDate, facilityType, isSalvageIncluded);
                if (tables != null) {
                    if (tables.Count>1)
                        debtorsLedger = new DebtorsLedgerBuilder().Build(tables[1].Rows);

                    if (tables.Count>2)
                        broughtForwardLedger = new SubledgerBuilder().Build(tables[2].Rows);
                    
                    if (tables.Count > 3)
                     currentLedger = new SubledgerBuilder().Build(tables[3].Rows);

                    FactorsLedger factorsLedger = new FactorsLedgerBuilder().Build(broughtForwardLedger, currentLedger);
                    if (tables.Count > 1)
                        return new ControlReportBuilder(calendar).Build(debtorsLedger, factorsLedger, tables[1].Rows, ReadClientName(tables[0].Rows), endDate, ReadReportTitle(tables[0].Rows), facilityType.Id);
                }
            }
            catch { }
            return null;
        }

        public CurrentPaymentsReport LoadCurrentShortPaidReport(int clientId, int ClientFacilityType)
        {
            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadReportTables((int)ReportType.CurrentShortPaidReport, calendar.Now, clientId, ClientFacilityType);
                if (tables != null) {
                    if (tables.Count > 1)
                            rows = tables[1].Rows;

                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<CurrentPaymentsReportRecord> records = new CurrentsPaymentReportRecordBuilder().Build(rows, clientNumber, clientName);
                    return new CurrentPaymentsReport(calendar, ReadReportTitle(tables[0].Rows), clientName, records, "Current Short Paid");
                }
            }
            catch { }
            return null;
        }

        public RetentionReleaseEstimateReport LoadRetentionReleaseEstimateReport(int clientId)
        {
            try
            {
                ReleaseSummary releaseSummary = null;
                Deductables deductables = null;
                IList<RetentionReleaseEstimateReportRecord> records = new List<RetentionReleaseEstimateReportRecord>();

                DataTableCollection tables = reports.Get_RptsTrns((int)ReportType.RetentionReleaseEstimateReport, clientId, -1,
                                                    calendar.Today.ToShortDateString(), 7, 0, 0, -1, -1, 0, 1, "Factoring").Tables;
                if (tables != null) {
                    if (tables.Count > 1) 
                        records = new RetentionReleaseEstimateReportRecordBuilder().Build(tables[1]);

                    if (tables.Count > 2) 
                        releaseSummary = new ReleaseSummaryBuilder().Build(tables[2].Rows);
                    
                    if (tables.Count > 3) 
                        deductables = new DeductablesBuilder().Build(tables[2].Rows);

                    return new RetentionReleaseEstimateReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, releaseSummary, deductables);
                }
            }
            catch { }
            return null;
        }

        public StatusReport LoadStatusReport(Date endDate, int clientId, TransactionStatus status, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(status, "status");

            try
            {
                IList<StatusReportRecord> records;
                DataTableCollection tables = LoadReportTables((int)ReportType.StatusReport, endDate, clientId, ClientFacilityType);
                if (tables != null) {
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    if (status == TransactionStatus.Funded)
                    {
                        records = new StatusReportRecordBuilder().BuildFactoredInvoices(tables, clientNumber, clientName);
                    }
                    else if (status == TransactionStatus.NonFunded)
                    {
                        records = new StatusReportRecordBuilder().BuildNonFactoredInvoices(tables, clientNumber, clientName);
                    }
                    else
                    {
                        records = new StatusReportRecordBuilder().Build(tables, clientNumber, clientName);
                    }
                    return new StatusReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, status);
                }
            }
            catch { }
            return null;
        }

        public StatusReport LoadStatusReportForCustomer(Date endDate, int clientId, CffCustomer customer, TransactionStatus status, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(status, "status");

            try
            {
                IList<StatusReportRecord> records;
                DataTableCollection tables = LoadCustomerReportTables(ReportType.StatusReport, clientId, customer.Id, endDate, ClientFacilityType);
                if (tables != null) {
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    if (status == TransactionStatus.Funded)
                    {
                        records = new StatusReportRecordBuilder().BuildFactoredInvoices(tables, clientNumber, clientName);
                    }
                    else if (status == TransactionStatus.NonFunded)
                    {
                        records = new StatusReportRecordBuilder().BuildNonFactoredInvoices(tables, clientNumber, clientName);
                    }
                    else
                    {
                        records = new StatusReportRecordBuilder().Build(tables, clientNumber, clientName);
                    }
                    return new StatusReport(calendar, ReadReportTitle(tables[0].Rows), customer.Name, records, status);
                }
            }
            catch { }
            return null;

        }

        public StatusReport LoadStatusReportForAllClients(Date endDate, TransactionStatus status, FacilityType facilityType, bool isSalvageIncluded)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(status, "status");
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");

            try
            {
                IList<StatusReportRecord> records;
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.StatusReport, endDate, facilityType, isSalvageIncluded);
                if (tables != null)
                {
                    if (status == TransactionStatus.Funded)
                    {
                        records = new StatusReportRecordBuilder().BuildFactoredInvoicesForAllClients(tables);
                    }
                    else if (status == TransactionStatus.NonFunded)
                    {
                        records = new StatusReportRecordBuilder().BuildNonFactoredInvoicesForAllClients(tables);
                    }
                    else
                    {
                        records = new StatusReportRecordBuilder().BuildForAllClients(tables);
                    }
                    return new StatusReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, status);
                }
            }
            catch { }
            return null;
        }

        public OverdueChargesReport LoadOverdueChargesReport(Date endDate, int clientId, TransactionStatus status, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(status, "status");

            try
            {
                IList<OverdueChargesReportRecord> reportRecords;
                DataTableCollection tables = LoadReportTables((int)ReportType.OverdueCharges, endDate, clientId, ClientFacilityType);
                if (tables != null) {
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    if (status == TransactionStatus.Funded)
                    {
                        reportRecords = new OverdueChargesReportRecordBuilder().BuildFactoredInvoices(tables, clientNumber, clientName);
                    }
                    else if (status == TransactionStatus.NonFunded)
                    {
                        reportRecords = new OverdueChargesReportRecordBuilder().BuildNonFactoredInvoices(tables, clientNumber, clientName);
                    }
                    else
                    {
                        reportRecords = new OverdueChargesReportRecordBuilder().Build(tables, clientNumber, clientName);
                    }

                    return new OverdueChargesReport(calendar, ReadReportTitle(tables[0].Rows), clientName, reportRecords, status, ClientFacilityType);
                }
            }
            catch { }
            return null;
        }

        public OverdueChargesReport LoadOverdueChargesReportForCustomer(Date endDate, int clientId, CffCustomer customer, TransactionStatus status, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(status, "status");

            try
            {
                IList<OverdueChargesReportRecord> reportRecords;
                DataTableCollection tables = LoadCustomerReportTables(ReportType.OverdueCharges, clientId, customer.Id, endDate, ClientFacilityType);
                if (tables != null) {
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    if (status == TransactionStatus.Funded)
                    {
                        reportRecords = new OverdueChargesReportRecordBuilder().BuildFactoredInvoices(tables, clientNumber, clientName);
                    }
                    else if (status == TransactionStatus.NonFunded)
                    {
                        reportRecords = new OverdueChargesReportRecordBuilder().BuildNonFactoredInvoices(tables, clientNumber, clientName);
                    }
                    else
                    {
                        reportRecords = new OverdueChargesReportRecordBuilder().Build(tables, clientNumber, clientName);
                    }

                    return new OverdueChargesReport(calendar, ReadReportTitle(tables[0].Rows), customer.Name, reportRecords, status, ClientFacilityType);
                }
            }
            catch { }
            return null;

        }

        public StatementReport LoadStatementReport(int clientId, int customerId, Date endDate)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");

            try {
                DataSet theDS = reports.Get_RptsTrns((int)ReportType.StatementReport, clientId, customerId, endDate.ToShortDateString(),
                                                              7, 0, 0, endDate.IsThisMonth(calendar), 0, 0, 1, "Factoring");

                if (theDS != null) {
                    DataTableCollection tables = theDS.Tables;
                    if (tables!=null){
                        IList<StatementReportRecord> records = new StatementReportRecordBuilder(tables[2]).Build();
                        if (tables.Count > 3) {
                            DataRowReader reader = new DataRowReader(tables[3].Rows);
                            ManagementDetails managementDetails = new ManagementDetailsBuilder(reader).Build();
                            PurchaserDetails purchaserDetails = new PurchaserDetailsBuilder(tables).Build();
                            return new StatementReportBuilder().Build(managementDetails, purchaserDetails, records, calendar, endDate, ReadReportTitle(tables[0].Rows));
                        }
                    }
                }
            }
            catch { } 
            return null;
        }

        public OverdueChargesReport LoadOverdueChargesReportForAllClients(Date endDate, TransactionStatus status, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(status, "status");
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");

            try
            {
                IList<OverdueChargesReportRecord> reportRecords;
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.OverdueCharges, endDate, facilityType, isSalvageIncluded);
                if (tables != null) {
                    if (status == TransactionStatus.Funded)
                    {
                        reportRecords = new OverdueChargesReportRecordBuilder().BuildFactoredInvoicesForAllClients(tables);
                    }
                    else if (status == TransactionStatus.NonFunded)
                    {
                        reportRecords = new OverdueChargesReportRecordBuilder().BuildNonFactoredInvoicesForAllClients(tables);
                    }
                    else
                    {
                        reportRecords = new OverdueChargesReportRecordBuilder().BuildForAllClients(tables);
                    }

                    return new OverdueChargesReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), reportRecords, status, ClientFacilityType);
                }
            }
            catch { }
            return null;
        }

        public TransactionReport LoadUnallocatedReportForAllClients(FacilityType facilityType, bool isSalvageIncluded)
        {
            try
            {
                IList<TransactionReportRecord> records  = null;
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.UnallocatedReport, calendar.Now, facilityType, isSalvageIncluded);
                if (tables != null) {
                    if (tables.Count > 1)
                            records = new TransactionReportRecordBuilder().BuildForAllClients(tables[1].Rows, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    return new TransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Unallocated");
                }
            }
            catch { }
            return null;
        }

        public TransactionReport LoadUnallocatedReport(int clientId, int ClientFacilityType)
        {
            try {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadReportTables((int)ReportType.UnallocatedReport, calendar.Now, clientId, ClientFacilityType);
                if (tables != null) {
                    if (tables.Count > 1)
                            rows = tables[1].Rows;

                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<TransactionReportRecord> records = new TransactionReportRecordBuilder().Build(rows, clientNumber, clientName);
                    return new TransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Unallocated");
                }
            } catch {}
            return null;
        }

        public TransactionReport LoadOverpaymentsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded)
        {
            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.Overpayments, endDate, facilityType, isSalvageIncluded);
                if (tables!=null)
                    if (tables.Count>1)
                        rows = tables[1].Rows;

                IList<TransactionReportRecord> records = new TransactionReportRecordBuilder().BuildForAllClients(rows, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                return new TransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Overpayments");
            }
            catch { }
            return null;
        }
    
        public TransactionReport LoadOverpaymentsReport(Date endDate, int clientId, int ClientFacilityType)
        {
            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadReportTables((int)ReportType.Overpayments, endDate, clientId, ClientFacilityType);
                if (tables != null) {
                    if(tables.Count > 1)
                        rows = tables[1].Rows;
                    
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);

                    IList<TransactionReportRecord> records = new TransactionReportRecordBuilder().Build(rows, clientNumber, clientName, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    string readReportTitle = ReadReportTitle(tables[0].Rows);
                    readReportTitle = SessionWrapper.Instance.Get.SelectedTransactionFilter != null && SessionWrapper.Instance.Get.SelectedTransactionFilter != "All" ? string.Format("{0} {1}", SessionWrapper.Instance.Get.SelectedTransactionFilter, readReportTitle) : readReportTitle;
                    return new TransactionReport(calendar, readReportTitle, ReadClientName(tables[0].Rows), records, "Overpayments");
                }
            }
            catch { }
            return null;
        }

        public CustomerOverpaymentsTransactionReport LoadOverpaymentsReportForCustomer(DateRange date, int customerId)
        {
            ArgumentChecker.ThrowIfNull(date, "date");
            try
            {
                IList<CustomerOverpaymentsTransactionReportRecord> records = new List<CustomerOverpaymentsTransactionReportRecord>();
                DataTableCollection tables = LoadCustomerReportTables(ReportType.Overpayments, date, customerId);
                if (tables != null) {
                    if(tables.Count > 1)
                            records = new CustomerOverpaymentsTransactionReportRecordBuilder(calendar).Build(tables[1].Rows);
                    return new CustomerOverpaymentsTransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadCustomerName(tables[0].Rows), records);
                }
            }
            catch { }
            return null;

        }

        public TransactionReport LoadCreditNotesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded)
        {
            try
            {
                DataRowCollection rows  = null;
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.CreditNotesReport, endDate, facilityType, isSalvageIncluded);
                if (tables != null) {
                    if (tables.Count>1)
                        rows = tables[1].Rows;
                    IList<TransactionReportRecord> records = new TransactionReportRecordBuilder().BuildForAllClients(rows, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    return new TransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Credit Notes");
                }
            }
            catch { }
            return null;
        }

        public CustomerTransactionReport LoadCreditNotesReportForCustomer(DateRange dateRange, int customerId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            try
            {
                IList<CustomerTransactionReportRecord> records = new List<CustomerTransactionReportRecord>();
                DataTableCollection tables = LoadCustomerReportTables(ReportType.CreditNotesReport, dateRange, customerId);
                if (tables != null) {
                    if (tables.Count >1 )
                        records = new CustomerTransactionReportRecordBuilder(calendar).Build(tables[1].Rows);
                    return new CustomerTransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadCustomerName(tables[0].Rows), records, "Credit Notes");
                }
            }
            catch { }
            return null;
        }

        public CustomerTransactionReport LoadJournalsReportForCustomer(DateRange dateRange, int customerId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            try
            {
                IList<CustomerTransactionReportRecord> records = null;
                DataTableCollection tables = LoadCustomerReportTables(ReportType.CustoemrJournalsReport, dateRange, customerId);
                if (tables != null) {
                    if (tables.Count > 1)
                        records = new CustomerTransactionReportRecordBuilder(calendar).Build(tables[1].Rows);
                    return new CustomerTransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadCustomerName(tables[0].Rows), records, "Journals");
                }
            }
            catch { }
            return null;
        }

        public CustomerTransactionReport LoadCreditBalanceTransfersReportForCustomer(DateRange dateRange, int customerId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            try
            {
                IList<CustomerTransactionReportRecord> records = new List<CustomerTransactionReportRecord>();
                DataTableCollection tables = LoadCustomerReportTables(ReportType.CustomerCreditBalanceTransfersReport, dateRange, customerId);
                if (tables != null) {
                    if (tables.Count > 1)
                        records = new CustomerTransactionReportRecordBuilder(calendar).Build(tables[1].Rows);
                    return new CustomerTransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadCustomerName(tables[0].Rows), records, "Credit Balance Transfers");
                }                
            }
            catch { }
            return null;
        }

        public CustomerTransactionReport LoadDiscountsReportForCustomer(DateRange dateRange, int customerId)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            try
            {
                IList<CustomerTransactionReportRecord> records = null;
                DataTableCollection tables = LoadCustomerReportTables(ReportType.DicountsReport, dateRange, customerId);
                if (tables != null)
                    if (tables.Count > 1)
                        records = new CustomerTransactionReportRecordBuilder(calendar).Build(tables[1].Rows);
                return new CustomerTransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadCustomerName(tables[0].Rows), records, "Discounts");
            }
            catch { }
            return null;
        }

        public TransactionReport LoadReceiptsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            try
            {
                DataRowCollection rows = null;
                IList<TransactionReportRecord> records = new List<TransactionReportRecord>();
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.ReceiptsReport, endDate, facilityType, isSalvageIncluded);
                if (tables != null) {
                    if (tables.Count > 1)
                    {
                        rows = tables[1].Rows;
                        records = new TransactionReportRecordBuilder().BuildForAllClients(rows, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    }
                    return new TransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Receipts");
                }
            }
            catch { }
            return null;
        }

        public TransactionReport LoadCreditBalanceTransfersReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded)
        {
            try
            {
                DataRowCollection rows = null;
                IList<TransactionReportRecord> records = new List<TransactionReportRecord>();
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.CreditBalanceTransfersReport, endDate, facilityType, isSalvageIncluded);
                if (tables != null) {
                    if (tables.Count > 1) {
                      rows = tables[1].Rows;
                      records = new TransactionReportRecordBuilder().BuildForAllClients(rows, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    }
                    return new TransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Credit Balance Transfers");
                }
            }
            catch { }
            return null;
        }

        public TransactionReport LoadJournalsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            try {
                DataRowCollection rows = null;
                IList<TransactionReportRecord> records = new List<TransactionReportRecord>();
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.JournalsReport, endDate, facilityType, isSalvageIncluded);
                if (tables != null) {
                    if (tables.Count > 1)
                    {
                        rows = tables[1].Rows;
                        records = new TransactionReportRecordBuilder().BuildForAllClients(rows, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    }
                    return new TransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Journals");
                }
            } catch { }
            return null;
        }

        public TransactionReport LoadDiscountsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            try
            {
                DataRowCollection rows = null;
                IList<TransactionReportRecord> records = new List<TransactionReportRecord>();
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.DicountsReport, endDate, facilityType, isSalvageIncluded);
                if (tables != null) {
                    if (tables.Count > 1)
                    {
                        rows = tables[1].Rows;
                        records = new TransactionReportRecordBuilder().BuildForAllClients(rows, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    }
                    return new TransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Dicounts");
                }
            }
            catch { }
            return null;
        }

        public TransactionReport LoadRepurchasesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            try
            {
                DataRowCollection rows = null;
                IList<TransactionReportRecord> records = new List<TransactionReportRecord>();
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.RepurchaseReport, endDate, facilityType, isSalvageIncluded);
                if (tables != null) {
                    if (tables.Count > 1) {
                        rows = tables[1].Rows;
                        records = new TransactionReportRecordBuilder().BuildForAllClients(rows, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    }
                    return new TransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Repurchase");
                }
            }
            catch { }
            return null;
        }

        public TransactionReport LoadUnclaimedRepurchasesReport(Date endDate, int clientId, int ClientFacilityType)
        {//do not forget the try-catch on these babies. Remember we are calling the VB DLL here and returned DataSet may be null.
            try
            {
                // DataTableCollection tables = LoadReportTables((int)ReportType.RepurchaseReport, endDate, clientId, ClientFacilityType);  // dbb
                DataTableCollection tables = LoadReportTables((int)ReportType.UnclaimedRepurchasesReport, endDate, clientId, ClientFacilityType);
                if (tables != null)
                {
                    DataRowCollection rows = null;
                    if (tables.Count > 1)
                        rows = tables[1].Rows;
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    string yearMonth = tables[0].Rows[0].ItemArray[10].ToString();
                    if (rows != null)
                    {
                        IList<TransactionReportRecord> records = new TransactionReportRecordBuilder().Build(rows, clientNumber, clientName, "", yearMonth);
                        return new TransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Unclaimed Prepayments");  // formerly "Unclaimed Repurchase"
                    }
                }
            }
            catch (Exception u) { Console.WriteLine("Error {0}", u.Message); }
            return null;
        }

        public TransactionReport LoadUnclaimedRepurchasesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        { //do not forget the try-catch on these babies. Remember we are calling the VB DLL here and returned DataSet may be null.
            try
            {
                DataRowCollection rows = null;
                IList<TransactionReportRecord> records = new List<TransactionReportRecord>();
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.UnclaimedRepurchasesReport, endDate, facilityType, isSalvageIncluded);
                if (tables != null) {
                    if (tables.Count > 1){
                        rows = tables[1].Rows;
                        records = new TransactionReportRecordBuilder().BuildForAllClients(rows, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    }
                    return new TransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Unclaimed Repurchase");
                }
            }
            catch { }
            return null;
        }

        public AccountTransactionReport LoadAccountTransactionReport(int codeRef, Date endDate, int clientId)
        {
            try
            {
                System.Collections.ArrayList thePar = new System.Collections.ArrayList();
                thePar.Add("ACCTTRNS");
                thePar.Add(clientId);
                thePar.Add(codeRef); //codeRef.ToString
                thePar.Add(endDate.DateTime); //strBDate
                thePar.Add(0);
                thePar.Add(endDate.Year.ToString() + endDate.Month.ToString().PadLeft(2, '0')); //Yrmth.ToString
                thePar.Add(0);
                thePar.Add(0);
                DataTableCollection tables = LoadGeneralLedTransData(thePar);
                if (tables != null)
                {
                    if (tables[0].Rows.Count > 0)
                    {
                        DataRowCollection rows = tables[0].Rows;
                        DataRowCollection sumRow = tables[1].Rows;
                        int clientNumber = clientId;
                        //string clientName = ReadClientName(tables[0].Rows);

                        AccountTransactionReportBuilder acctTrxBuilder = new AccountTransactionReportBuilder(rows, sumRow);
                        return new AccountTransactionReport(calendar, ReadReportTitle(tables[0].Rows), "",
                                acctTrxBuilder.Records(),
                                "Account Transaction", acctTrxBuilder.CreditTotal(), acctTrxBuilder.DebitTotal(),
                                acctTrxBuilder.MovementAmt(), acctTrxBuilder.ClosingBalance(), acctTrxBuilder.OpeningBalance());
                    }
                }
            }
            catch { }
            return null;
        }

        public AccountTransactionReport LoadAccountTransactionReportForAllClients(int coderef, Date endDate, FacilityType facilityType, bool isSalvageIncluded)
        {
            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.UnclaimedRepurchasesReport, endDate, facilityType, isSalvageIncluded);
                if (tables != null) {
                    if (tables.Count > 1)
                        rows = tables[1].Rows;

                    IList<AccountTransactionReport> records = new List<AccountTransactionReport>();
                    var reader = new DataRowReader(rows);
                    while (reader.Read())
                    {
                        //retrieve client id, retrieve current date
                        System.Collections.ArrayList thePar = new System.Collections.ArrayList();
                        thePar.Add("ACCTTRNS");
                        thePar.Add(reader.ToInteger("clientid"));
                        thePar.Add(5000 + reader.ToInteger("clientid")); //codeRef.ToString
                        thePar.Add(endDate.DateTime); //strBDate
                        thePar.Add(endDate.Year.ToString() + endDate.Month.ToString().PadLeft(2, '0')); //Yrmth.ToString
                        thePar.Add(0);
                        thePar.Add(0);
                        thePar.Add(0);
                        DataTableCollection dtc = LoadGeneralLedTransData(thePar);
                        if (dtc != null)
                        {
                            DataRowCollection dtcRows = tables[0].Rows;
                            DataRowCollection dtcSumRow = tables[1].Rows;
                            AccountTransactionReportBuilder acctTrxBuilder = new AccountTransactionReportBuilder(dtcRows, dtcSumRow);
                            records.Add(new AccountTransactionReport(calendar,
                                            ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows),
                                            acctTrxBuilder.Records(), "Account Transaction",
                                            acctTrxBuilder.CreditTotal(), acctTrxBuilder.DebitTotal(),
                                            acctTrxBuilder.MovementAmt(), acctTrxBuilder.ClosingBalance(),
                                            acctTrxBuilder.OpeningBalance()));
                        }
                    }

                    return new AccountTransactionReport(calendar, ReadReportTitle(tables[0].Rows), "All Clients", records, "Account Transaction");
                }
          
            }
            catch { }
            return null;
        }

        public TransactionReport LoadUnclaimedCreditNotesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {//do not forget the try-catch on these babies. Remember we are calling the VB DLL here and returned DataSet may be null.
            try
            {
                DataRowCollection rows = null;
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.UnclaimedCreditNotesReport, endDate, facilityType, isSalvageIncluded);
                if (tables!=null)
                    if (tables.Count > 1)
                        rows = tables[1].Rows;
                if (rows != null) {
                    IList<TransactionReportRecord> records = new TransactionReportRecordBuilder().BuildForAllClients(rows, SessionWrapper.Instance.Get.SelectedTransactionFilter);
                    return new TransactionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Unclaimed Credit Notes");
                }
            }
            catch { }
            return null;
        }

        public TransactionReport LoadUnclaimedCreditNotesReport(Date endDate, int clientId, int ClientFacilityType)
        {
            try {
                DataTableCollection tables = LoadReportTables((int)ReportType.UnclaimedCreditNotesReport, endDate, clientId, ClientFacilityType);
                if (tables != null) {
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<TransactionReportRecord> records = new TransactionReportRecordBuilder().Build(tables[1].Rows, clientNumber, clientName);
                    return new TransactionReport(calendar, ReadReportTitle(tables[0].Rows), clientName, records, "Unclaimed Credit Notes");
                }
            }
            catch { }
            return null;
        }

        public PromptReport LoadPromptReportForFactoredInvoicesForAllClients(int promptDays, FacilityType facilityType, bool isSalvageIncluded)
        {
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");
            try
            {
                IList<PromptReportRecord> reportRecords = new List<PromptReportRecord>();
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.PromptReportForFactoredInvoices, calendar.Now, facilityType, promptDays, isSalvageIncluded);
                if (tables != null) {
                    if (tables.Count > 1)
                        reportRecords = new PromptReportRecordBuilder().BuildForAllClients(tables[1].Rows);
                    return new FactoredPromptReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), reportRecords);
                }
            }
            catch { }
            return null;
        }

        public PromptReport LoadPromptReportForFactoredInvoices(int promptDays, int clientId, int ClientFacilityType)
        {
            int reportType = (int)ReportType.PromptReportForFactoredInvoices + promptDays;
            try
            {
                DataTableCollection tables = LoadReportTables(reportType, calendar.Today, clientId, ClientFacilityType);
                if (tables != null) {
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);

                    IList<PromptReportRecord> reportRecords = new PromptReportRecordBuilder().Build(tables[1].Rows, clientNumber, clientName);
                    return new FactoredPromptReport(calendar, ReadReportTitle(tables[0].Rows), clientName, reportRecords);
                }
            }
            catch { }
            return null;
        }

        public PromptReport LoadPromptReportForAllInvoices(int promptDays, int clientId, int ClientFacilityType)
        {
            int reportType = (int)ReportType.PromptReportForAllInvoices + promptDays;
            try
            {
                DataTableCollection tables = LoadReportTables(reportType, calendar.Today, clientId, ClientFacilityType);
                if (tables != null) {
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<PromptReportRecord> reportRecords = new PromptReportRecordBuilder().Build(tables[1].Rows, clientNumber, clientName);
                    return new PromptReport(calendar, ReadReportTitle(tables[0].Rows), clientName, reportRecords);
                }
            }
            catch { }
            return null;
        }

        public PromptReport LoadPromptReportForAllInvoicesForAllClients(int promptDays, FacilityType facilityType, bool isSalvageIncluded)
        {
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");

            try
            {
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.PromptReportForAllInvoices, calendar.Now, facilityType, promptDays, isSalvageIncluded);
                if (tables!=null)
                    if (tables.Count > 1) {
                        IList<PromptReportRecord> reportRecords = new PromptReportRecordBuilder().BuildForAllClients(tables[1].Rows);
                        return new FactoredPromptReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), reportRecords);
                    }
            }
            catch { }
            return null;
        }

        public CreditLimitExceededReport LoadCreditLimitExceededReport(int clientId, int ClientFacilityType)
        {
            try
            {
                DataTableCollection tables = LoadReportTables((int)ReportType.CreditLimitExceededReport, calendar.Now, clientId, ClientFacilityType);
                if (tables != null) {
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<CreditLimitExceededReportRecord> records = new CreditLimitExceededReportRecordBuilder().Build(tables[1].Rows, clientNumber, clientName);
                    return new CreditLimitExceededReport(calendar, ReadReportTitle(tables[0].Rows), clientName, records);
                }
            } catch { }
            return null;
        }

        public CreditLimitExceededReport LoadCreditLimitExceededReportForAllClients(FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");
            try
            {
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.CreditLimitExceededReport, calendar.Now, facilityType, isSalvageIncluded);
                if (tables != null) {
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<CreditLimitExceededReportRecord> records = new CreditLimitExceededReportRecordBuilder().BuildForAllClients(tables[1].Rows);
                    return new CreditLimitExceededReport(calendar, ReadReportTitle(tables[0].Rows), clientName, records);
                }
            }
            catch { }
            return null;
        }

        public CreditStopSuggestionsReport LoadCreditStopSuggestionsReport(int clientId, int ClientFacilityType)
        {
            try
            {
                DataTableCollection tables = LoadReportTables((int)ReportType.CreditStopSuggestionsReport, calendar.Now, clientId, ClientFacilityType);
                if (tables != null) {
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<CreditStopSuggestionsReportRecord> records = new CreditStopSuggestionsReportRecordBuilder().Build(tables[1].Rows, clientNumber, clientName);
                    return new CreditStopSuggestionsReport(calendar, ReadReportTitle(tables[0].Rows), clientName, records);
                }
            }
            catch { }
            return null;
        }

        public CreditStopSuggestionsReport LoadCreditStopSuggestionsReportForAllClients(FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");
            try
            {
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.CreditStopSuggestionsReport, calendar.Now, facilityType, isSalvageIncluded);
                if (tables != null) {
                    string clientName = ReadClientName(tables[0].Rows);
                    IList<CreditStopSuggestionsReportRecord> records = new CreditStopSuggestionsReportRecordBuilder().BuildForAllClients(tables[1].Rows);
                    return new CreditStopSuggestionsReport(calendar, ReadReportTitle(tables[0].Rows), clientName, records);
                }
            }
            catch { }
            return null;
        }


        public int CalculatePromptDays(int clientId)
        {
            int promptDays = 0;
            using (SqlConnection connection = CreateConnection())
            {
                SqlParameter clientIdParameter = new SqlParameter("@ClientID", SqlDbType.Int);
                clientIdParameter.Value = clientId;

                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "PromptDays_Calculate",
                                                                          new[] {clientIdParameter}))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull && cleverReader.Read())
                    {
                        promptDays = cleverReader.ToInteger("PromptDays");
                    }
                }
            }
            return promptDays;
        }

        public CallsDueReport LoadCallsDueReport(int clientId, PeriodReportType reportType, BalanceRange balanceRange, string orderByString)
        {
            try
            {
                DataTableCollection tables = LoadReportTables(reportType.Id, calendar.Now, clientId, balanceRange);
                if (tables != null) {
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);

                    IList<CallsDueReportRecord> records = new CallsDueReportRecordBuilder().Build(tables[1].Rows, clientNumber, clientName);
                    return new CallsDueReport(calendar, ReadReportTitle(tables[0].Rows), clientName, records, "Calls Due", orderByString);
                }
            }
            catch { }
            return null;
        }

        public CallsDueReport LoadCallsDueReportForAllClients(PeriodReportType reportType, BalanceRange balanceRange, FacilityType facilityType, bool isSalvageIncluded, string orderByString)
        {
            try
            {
                DataTableCollection tables = LoadAllClientsReportTables(reportType.Id, calendar.Now, balanceRange, facilityType, isSalvageIncluded, -5);
                if (tables != null) {
                    if (tables.Count > 1) {
                        IList<CallsDueReportRecord> records = new CallsDueReportRecordBuilder().Build(tables[1].Rows);
                        return new CallsDueReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Calls Due", orderByString);
                    }
                }
            } catch { }
            return null;
        }

        public ClientActionReport LoadClientActionReport(int clientId)
        {
            try
            {
                IList<ClientActionReportRecord> records = new List<ClientActionReportRecord>();
                DataTableCollection tables = LoadReportTables((int)ReportType.ClientAction, calendar.Now, clientId, new BalanceRange(0, 0));
                if (tables != null) {
                    
                    int clientNumber = ReadClientNumber(tables[0].Rows);
                    string clientName = ReadClientName(tables[0].Rows);

                    if (tables.Count>1)
                            records = new ClientActionReportRecordBuilder().Build(tables[1].Rows, clientNumber, clientName);
                
                    return new ClientActionReport(calendar, ReadReportTitle(tables[0].Rows), clientName, records, "Client Action", ClientOrderByType.SortByCustomer.OrderString);
                }
            }
            catch { }
            return null;
        }

        public ClientActionReport LoadClientActionReportForAllClients(BalanceRange balanceRange, FacilityType facilityType, bool isSalvageIncluded, string orderByString)
        {
            try
            {
                IList<ClientActionReportRecord> records = new List<ClientActionReportRecord>();
                DataTableCollection tables = LoadAllClientsReportTables((int)ReportType.ClientAction, calendar.Now, balanceRange, facilityType, isSalvageIncluded, -5);
                if (tables != null)
                {
                    if (tables.Count > 1)
                        records = new ClientActionReportRecordBuilder().Build(tables[1].Rows);

                    return new ClientActionReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Client Action", orderByString);
                }
            }
            catch { }
            return null;
        }

        public CallsDueReport LoadCustomerWatchReportForAllClients(PeriodReportType reportType, BalanceRange balanceRange, FacilityType facilityType, bool isSalvageIncluded, string orderByString)
        {
            try
            {
                IList<CallsDueReportRecord> records = new List<CallsDueReportRecord>();
                DataTableCollection tables = LoadAllClientsReportTables(reportType.Id, calendar.Now, balanceRange, facilityType, isSalvageIncluded, -6);
                if (tables != null) {
                    if (tables.Count > 1)
                        records = new CallsDueReportRecordBuilder().Build(tables[1].Rows);
                    return new CallsDueReport(calendar, ReadReportTitle(tables[0].Rows), ReadClientName(tables[0].Rows), records, "Customer Watch", orderByString);
                }
            }
            catch { }
            return null;
        }

        #region Private Helper Methods
        private DataTableCollection LoadGeneralLedTransData(System.Collections.ArrayList theArrayParameter)
        {
            try
            {
                // MSarza[20160415] value set for param SwitchToXeroYrmth is temporary and is to be replaced by either a web config  setting or
                //  some other finalized strategy prior to Xero integration release
                DataSet theDS = deOldData.get_GeneralLedTrnsData(theArrayParameter,201812); 
                if (theDS!=null)
                    return theDS.Tables;
            } catch  {}
            return null;
        }

        private DataTableCollection LoadReportTables(int reportType, Date endDate, int clientId, int ClientFacilityType)
        {
            try
            {
                //string dtStampMMDDYY = endDate.Month.ToString().PadLeft(2, '0') + "/" + endDate.Day.ToString().PadLeft(2, '0') + "/" + endDate.Year.ToString();
                DataSet theDS = reports.Get_RptsTrns(reportType, clientId, -1, endDate.ToString(),
                                            7, 0, 0, endDate.IsThisMonth(calendar), 0, 0, ClientFacilityType, "strFacilityType");
                if (theDS != null)
                    return theDS.Tables;
            }
            catch (System.Exception exc)
            {
                string exm = exc.Message;
            }
            return null;
        }

        private static SqlParameter[] CreateReportParameters(int reportType, int clientId, int custId, int endDateStr, string endDate, int user, int rangeFrom, int rangeTo)
        {
            SqlParameter reportTypeParameter = new SqlParameter("@RptType", SqlDbType.Int);
            reportTypeParameter.Value = reportType;
            SqlParameter clientIdParameter = new SqlParameter("@ClientID", SqlDbType.BigInt);
            clientIdParameter.Value = clientId;
            SqlParameter custIdParameter = new SqlParameter("@CustID", SqlDbType.BigInt);
            custIdParameter.Value = custId;
            SqlParameter endDateParameter = new SqlParameter("@Yrmth", SqlDbType.Int);
            endDateParameter.Value = endDateStr;
            SqlParameter dateAsAtParameter = new SqlParameter("@dtDateAsAt", SqlDbType.Text);
            dateAsAtParameter.Value = endDate;
            SqlParameter userParameter = new SqlParameter("@UserID", SqlDbType.Int);
            userParameter.Value = user;
            SqlParameter rangeFromParameter = new SqlParameter("@RangeFrom", SqlDbType.Int);
            rangeFromParameter.Value = rangeFrom;
            SqlParameter rangeToParameter = new SqlParameter("@RangeTo", SqlDbType.Int);
            rangeToParameter.Value = rangeTo;
            return new[] { reportTypeParameter, clientIdParameter, custIdParameter, endDateParameter, dateAsAtParameter, userParameter, rangeFromParameter, rangeToParameter };
        }

        private DataTableCollection LoadAllClientsReportTables(int reportType, Date endDate, FacilityType facilityType, bool isSalvageIncluded)
        {
            return LoadAllClientsReportTables(reportType, endDate, facilityType, 0, isSalvageIncluded);
        }

        private DataTableCollection LoadAllClientsReportTables(int reportType, Date endDate, BalanceRange balanceRange, FacilityType facilityType, bool isSalvageIncluded, int isNow)
        {
            try
            {
                int clientId = 0;
                if (isSalvageIncluded)
                {
                    clientId = -50;
                }

                DataSet theDS = reports.Get_RptsTrns(reportType, clientId, -1, endDate.ToShortDateString(),
                                            7, balanceRange.MinBalance, balanceRange.MaxBalance, isNow, 0, 0, facilityType.Id, facilityType.Name);
                if (theDS!=null)
                    return theDS.Tables;
            }
            catch { }
            return null;
        }

        private DataTableCollection LoadAllClientsReportTables(int reportType, Date endDate, FacilityType facilityType, int promptDays, bool isSalvageIncluded)
        {

            try
            {
                int clientId = 0;
                if (isSalvageIncluded)
                {
                    clientId = -50;
                }

                DataSet theDS = reports.Get_RptsTrns(reportType, clientId, -1, endDate.ToShortDateString(),
                                            7, 0, 0, endDate.IsThisMonth(calendar), promptDays, 0, facilityType.Id, facilityType.Name);
                if (theDS!=null)
                    return theDS.Tables;
            }
            catch { }
            return null;
        }

        private DataTableCollection LoadCustomerReportTables(ReportType reportType, int clientId, int customerId, Date endDate, int ClientFacilityType)
        {
            try
            {
                DataSet theDS = reports.Get_RptsTrns((int)reportType, clientId, customerId, endDate.ToShortDateString(),
                                7, 0, 0, endDate.IsThisMonth(calendar), 0, 0, ClientFacilityType, "FaciltyType");
                if (theDS!=null)
                        return theDS.Tables;
            }
            catch { 
            }
            return null;
        }

        private DataTableCollection LoadReportTables(int reportType, Date endDate, int clientId, BalanceRange balanceRange)
        {
            try {
                DataSet theDS = reports.Get_RptsTrns(reportType, clientId, -1, endDate.ToShortDateString(),
                                           7, balanceRange.MinBalance, balanceRange.MaxBalance, endDate.IsThisMonth(calendar), 0, 0, 1, "Factoring");
                if (theDS != null)
                    return theDS.Tables;
            } catch{}
            return null;
        }

        private static string ReadClientName(DataRowCollection rows)
        {
            string clientName = string.Empty;
            DataRowReader reader = new DataRowReader(rows);
            if (reader.Read())
            {
                clientName = reader.ToString("ClientName");
            }
            return clientName;
        }

        private static int ReadClientNumber(DataRowCollection rows)
        {
            int clientNumber = -1;
            DataRowReader reader = new DataRowReader(rows);
            if (reader.Read())
            {
                clientNumber = reader.ToInteger("ClientNum");
            }
            return clientNumber;
        }

        private static string ReadCustomerName(DataRowCollection rows)
        {
            string customerName = string.Empty;
            DataRowReader reader = new DataRowReader(rows);
            if (reader.Read())
            {
                customerName = reader.ToString("Customer");
            }
            return customerName;
        }

        private static string ReadReportTitle(DataRowCollection rows)
        {
            string reportTitle = string.Empty;
            DataRowReader reader = new DataRowReader(rows);
            try
            {
                if (reader.Read())
                {
                    reportTitle = reader.ToString("TransType");
                }
                return reportTitle;
            }
            catch 
            {
                return "";            
            }
        }
#endregion 
    }
}