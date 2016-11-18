using System;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class ControlReportPanel : ExportablePanel
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Display(ViewState["ControlReport"] as ControlReport);
            }
        }

        public void Display(ControlReport report)
        {
            ViewState.Add("ControlReport", report);

            if (report != null)
            {
                DebtorsLedger debtorsLedger = report.DebtorsLedger;
                CurrentLiteral.Text = debtorsLedger.Current.ToString("C");
                OneMonthLiteral.Text = debtorsLedger.OneMonth.ToString("C");
                TwoMonthsLiteral.Text = debtorsLedger.TwoMonths.ToString("C");
                ThreeMonthsLiteral.Text = debtorsLedger.ThreeMonthsAndOver.ToString("C");
                TotalDebtorsLedgerLiteral.Text = debtorsLedger.Total.ToString("C");

                FactorsLedger factorsLedger = report.FactorsLedger;
                Subledger broughtForwardLedger = factorsLedger.BroughtForwardLedger;
                FundedInvoicesBroughtForwardLiteral.Text = broughtForwardLedger.FundedInvoices.ToString("C");
                NonFundedInvoicesBroughtForwardLiteral.Text = broughtForwardLedger.NonFundedInvoices.ToString("C");
                CreditNotesBroughtForwardLiteral.Text = broughtForwardLedger.CreditNotes.ToString("C");
                NetJournalsBroughtForwardLiteral.Text = broughtForwardLedger.NetJournals.ToString("C");
                CashReceiveBroughtForwardLiteral.Text = broughtForwardLedger.CashReceived.ToString("C");
                OverpaymentsBroughtForwardLiteral.Text = broughtForwardLedger.Overpayments.ToString("C");
                NetAdjustmentBroughtForwardLiteral.Text = broughtForwardLedger.NetAdjustment.ToString("C");
                DiscountsBroughtForwardLiteral.Text = broughtForwardLedger.Discounts.ToString("C");
                TotalBroughtForwardLiteral.Text = broughtForwardLedger.Total.ToString("C");

                Subledger currentLedger = factorsLedger.CurrentLedger;
                FundedInvoicesCurrentLiteral.Text = currentLedger.FundedInvoices.ToString("C");
                NonFundedInvoicesCurrentLiteral.Text = currentLedger.NonFundedInvoices.ToString("C");
                CreditNotesCurrentLiteral.Text = currentLedger.CreditNotes.ToString("C");
                NetJournalsCurrentLiteral.Text = currentLedger.NetJournals.ToString("C");
                CashReceivedCurrentLiteral.Text = currentLedger.CashReceived.ToString("C");
                OverpaymentsCurrentLiteral.Text = currentLedger.Overpayments.ToString("C");
                NetAdjustmentCurrentLiteral.Text = currentLedger.NetAdjustment.ToString("C");
                DiscountsCurrentLiteral.Text = currentLedger.Discounts.ToString("C");
                TotalCurrentLiteral.Text = currentLedger.Total.ToString("C");
                TotalFactorsLedgerLiteral.Text = factorsLedger.Total.ToString("C");

                FundedInvoicesBalanceLiteral.Text = report.FundedInvoicesBalance.ToString("C");
                NonFundedInvoicesBalanceLiteral.Text = report.NonFundedInvoicesBalance.ToString("C");
                FundedToBeRepurchasedLiteral.Text = report.FundedToBeRepurchased.ToString("C");
                CreditsToBeClaimedLiteral.Text = report.CreditsToBeClaimed.ToString("C");
                UnallocatedTransactionsLiteral.Text = report.UnallocatedTransactions.ToString("C");
                RepurchasesThisMonthLiteral.Text = report.RepurchasesThisMonth.ToString("C");
                AllocatedInPeriodLiteral.Text = report.AllocatedThisPeriod.ToString("C");
                CBTSInPeriodLiteral.Text = report.CbtsInPeriod.ToString("C");

               
            }
        }

        public override void Export()
        {
            ControlReport report = ViewState["ControlReport"] as ControlReport;

            if (report != null)
            {
                ExcelDocument document = new ExcelDocument(true);
                document.WriteTitle(report.Title);

                DebtorsLedger debtorsLedger = report.DebtorsLedger;
                document.FormatAsHeaderRow(3);
                document.AddHeaderCell("Debtors Ledger");
                document.MoveToNextRow();
                document.AddCell("Current:");
                document.AddCurrencyCell(debtorsLedger.Current);
                document.MoveToNextRow();
                document.AddCell("1 month:");
                document.AddCurrencyCell(debtorsLedger.OneMonth);
                document.MoveToNextRow();
                document.AddCell("2 months:");
                document.AddCurrencyCell(debtorsLedger.TwoMonths);
                document.MoveToNextRow();
                document.AddCell("3 months & over:");
                document.AddCurrencyCell(debtorsLedger.ThreeMonthsAndOver);
                document.MoveToNextRow();
                document.AddCell("Total Ledger:");
                document.AddCurrencyCell(debtorsLedger.Total, 2);
                document.MoveToNextRow();

                FactorsLedger factorsLedger = report.FactorsLedger;
                document.FormatAsHeaderRow(3);
                document.AddHeaderCell("Factors Ledger");
                document.MoveToNextRow();

                Subledger broughtForwardLedger = factorsLedger.BroughtForwardLedger;
                document.FormatAsSubheaderRow(3);
                document.AddHeaderCell("Brought Forward");
                document.MoveToNextRow();

                document.AddCell("Funded Invoices:");
                document.AddCurrencyCell(broughtForwardLedger.FundedInvoices);
                document.MoveToNextRow();
                document.AddCell("Non Funded Invoices:");
                document.AddCurrencyCell(broughtForwardLedger.NonFundedInvoices);
                document.MoveToNextRow();
                document.AddCell("Credit Notes:");
                document.AddCurrencyCell(broughtForwardLedger.CreditNotes);
                document.MoveToNextRow();
                document.AddCell("Net Journals:");
                document.AddCurrencyCell(broughtForwardLedger.NetJournals);
                document.MoveToNextRow();
                document.AddCell("Cash Received:");
                document.AddCurrencyCell(broughtForwardLedger.CashReceived);
                document.MoveToNextRow();
                document.AddCell("Overpayments:");
                document.AddCurrencyCell(broughtForwardLedger.Overpayments);
                document.MoveToNextRow();
                document.AddCell("Net Adjustment:");
                document.AddCurrencyCell(broughtForwardLedger.NetAdjustment);
                document.MoveToNextRow();
                document.AddCell("Discounts:");
                document.AddCurrencyCell(broughtForwardLedger.Discounts);
                document.MoveToNextRow();
                document.AddCell("Total Brought Foward:");
                document.AddCurrencyCell(broughtForwardLedger.Total, 2);
                document.MoveToNextRow();

                Subledger currentLedger = factorsLedger.CurrentLedger;
                document.FormatAsSubheaderRow(3);
                document.AddHeaderCell("Current");
                document.MoveToNextRow();

                document.AddCell("Funded Invoices:");
                document.AddCurrencyCell(currentLedger.FundedInvoices);
                document.MoveToNextRow();
                document.AddCell("Non Funded Invoices:");
                document.AddCurrencyCell(currentLedger.NonFundedInvoices);
                document.MoveToNextRow();
                document.AddCell("Credit Notes:");
                document.AddCurrencyCell(currentLedger.CreditNotes);
                document.MoveToNextRow();
                document.AddCell("Net Journals:");
                document.AddCurrencyCell(currentLedger.NetJournals);
                document.MoveToNextRow();
                document.AddCell("Cash Received:");
                document.AddCurrencyCell(currentLedger.CashReceived);
                document.MoveToNextRow();
                document.AddCell("Overpayments:");
                document.AddCurrencyCell(currentLedger.Overpayments);
                document.MoveToNextRow();
                document.AddCell("Net Adjustment:");
                document.AddCurrencyCell(currentLedger.NetAdjustment);
                document.MoveToNextRow();
                document.AddCell("Discounts:");
                document.AddCurrencyCell(currentLedger.Discounts);
                document.MoveToNextRow();

                document.FormatAsSubheaderRow(3);
                document.AddHeaderCell("Total Current:");
                document.AddCurrencyHeaderCellLedger(currentLedger.Total, 2, false);
                document.MoveToNextRow();

                document.FormatAsSubheaderRow(3);
                document.AddHeaderCell("Total Ledger:");
                document.AddCurrencyHeaderCellLedger(factorsLedger.Total, 2, true);

                document.InsertEmptyRow();

                document.AddCell("Balance of Funded Invoices:");
                document.AddCurrencyCell(report.FundedInvoicesBalance);
                document.MoveToNextRow();
                document.AddCell("Balance of Non Funded Invoices:");
                document.AddCurrencyCell(report.NonFundedInvoicesBalance);
                document.MoveToNextRow();
                document.AddCell("Unallocated Transactions:");
                document.AddCurrencyCell(report.UnallocatedTransactions);
                document.AddCurrencyCellLedger(report.FundedInvoicesBalance + report.NonFundedInvoicesBalance + report.UnallocatedTransactions, 2, true);
                document.MoveToNextRow();
                document.AddCell("Prepayments this Month:");
                document.AddCurrencyCell(report.RepurchasesThisMonth);
                document.MoveToNextRow();
                document.AddCell("Prepayments to be Claimed:");
                document.AddCurrencyCell(report.FundedToBeRepurchased);
                document.MoveToNextRow();
                document.AddCell("Credits to be Claimed:");
                document.AddCurrencyCell(report.CreditsToBeClaimed);
                document.MoveToNextRow();
                document.AddCell("Allocated in Period:");
                document.AddCurrencyCell(report.AllocatedThisPeriod);
                document.MoveToNextRow();
                document.AddCell("CBT's in Period:");
                document.AddCurrencyCell(report.CbtsInPeriod);
                document.MoveToNextRow();

                document.InsertEmptyRow();
                document.AddCell("Date Viewed");
                document.AddCell(report.DateViewed.ToDateTimeString());

                WriteToResponse(document.WriteToStream(), report.ExportFileName);
            }
        }

        public void Clear()
        {
            ViewState.Add("ControlReport", null);
            CurrentLiteral.Text = string.Empty;
            OneMonthLiteral.Text = string.Empty;
            TwoMonthsLiteral.Text = string.Empty;
            ThreeMonthsLiteral.Text = string.Empty;
            TotalDebtorsLedgerLiteral.Text = string.Empty;

            FundedInvoicesBroughtForwardLiteral.Text = string.Empty;
            NonFundedInvoicesBroughtForwardLiteral.Text = string.Empty;
            CreditNotesBroughtForwardLiteral.Text = string.Empty;
            NetJournalsBroughtForwardLiteral.Text = string.Empty;
            CashReceiveBroughtForwardLiteral.Text = string.Empty;
            OverpaymentsBroughtForwardLiteral.Text = string.Empty;
            NetAdjustmentBroughtForwardLiteral.Text = string.Empty;
            DiscountsBroughtForwardLiteral.Text = string.Empty;
            TotalBroughtForwardLiteral.Text = string.Empty;

            FundedInvoicesCurrentLiteral.Text = string.Empty;
            NonFundedInvoicesCurrentLiteral.Text = string.Empty;
            CreditNotesCurrentLiteral.Text = string.Empty;
            NetJournalsCurrentLiteral.Text = string.Empty;
            CashReceivedCurrentLiteral.Text = string.Empty;
            OverpaymentsCurrentLiteral.Text = string.Empty;
            NetAdjustmentCurrentLiteral.Text = string.Empty;
            DiscountsCurrentLiteral.Text = string.Empty;
            TotalCurrentLiteral.Text = string.Empty;
            TotalFactorsLedgerLiteral.Text = string.Empty;

            FundedInvoicesBalanceLiteral.Text = string.Empty;
            NonFundedInvoicesBalanceLiteral.Text = string.Empty;
            FundedToBeRepurchasedLiteral.Text = string.Empty;
            CreditsToBeClaimedLiteral.Text = string.Empty;
            UnallocatedTransactionsLiteral.Text = string.Empty;
            RepurchasesThisMonthLiteral.Text = string.Empty;

        }
    }
}