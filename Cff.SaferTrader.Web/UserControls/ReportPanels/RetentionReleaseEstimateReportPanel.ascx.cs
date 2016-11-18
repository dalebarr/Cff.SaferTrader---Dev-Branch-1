using System;
using System.Collections;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;


namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class RetentionReleaseEstimateReportPanel : ExportableGridPanel
    {

        private CffGenGridView CffGGV_ReportGridView;

        protected override void OnInit(EventArgs e)
        {
            CffGGV_ReportGridView = new CffGenGridView();

            CffGGV_ReportGridView.PageSize = 250;
            CffGGV_ReportGridView.DefaultPageSize = 250;
            CffGGV_ReportGridView.AllowPaging = true;
            CffGGV_ReportGridView.AutoGenerateColumns = true;
            CffGGV_ReportGridView.AllowPaging = true;

            //CffGGV_ReportGridView.CssClass = "cffGGV";
            CffGGV_ReportGridView.HeaderStyle.CssClass = "cffGGVHeader";
            CffGGV_ReportGridView.ShowHeaderWhenEmpty = true;
            CffGGV_ReportGridView.EmptyDataText = "No data to display";
            CffGGV_ReportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            //if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).Browser == "IE")
            //    CffGGV_ReportGridView.BorderWidth = Unit.Pixel(0);
            //else
                CffGGV_ReportGridView.BorderWidth = Unit.Pixel(1);

            GridViewReportPanel.Controls.Add(CffGGV_ReportGridView);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Display(ViewState["RetentionReleaseEstimateReport"] as RetentionReleaseEstimateReport);
        }

        public void Display(RetentionReleaseEstimateReport report)
        {
            ViewState.Add("RetentionReleaseEstimateReport", report);

            if (report != null) 
            {
                RetentionReleaseEstimateReport retentionReleaseEstimateReport = report;

                CffGGV_ReportGridView.DataSource = retentionReleaseEstimateReport.Records;
                CffGGV_ReportGridView.DataBind();

                ReleaseSummary releases = retentionReleaseEstimateReport.ReleaseSummary;
                FundedTransactionReleaseLiteral.Text = releases.FundedTransactionRelease.ToString("C");
                NonFundedTransactionReleaseLiteral.Text = releases.NonFundedTransactionRelease.ToString("C");
                ReleaseTotal.Text = releases.Total.ToString("C");

                Deductables deductables = retentionReleaseEstimateReport.Deductables;
                UnclaimedCreditsLiteral.Text = deductables.UnclaimedCredits.ToString("C");
                UnclaimedRepurchasesLiteral.Text = deductables.UnclaimedRepurchases.ToString("C");
                UnclaimedDiscountsLiteral.Text = deductables.UnclaimedDiscounts.ToString("C");
                LikelyRepurchasesLiteral.Text = deductables.LikelyRepurchases.ToString("C");
                OverdueChargesLiteral.Text = deductables.OverdueCharges.ToString("C");

                Fee chequeFee = deductables.ChequeFee;
                ChequeFeeLiteral.Text = chequeFee.Rate.ToString("C");
                NumberOfChequesLiteral.Text = chequeFee.Count.ToString();
                ChequeFeesLiteral.Text = chequeFee.Total.ToString("C");

                Fee postage = deductables.Postage;
                PostageFeeLiteral.Text = postage.Rate.ToString("C");
                NumberOfPostsLiteral.Text = postage.Count.ToString();
                PostageFeesLiteral.Text = postage.Total.ToString("C");

                Fee letterFee = deductables.LetterFees;
                LetterFeeLiteral.Text = letterFee.Rate.ToString("C");
                NumberOfLettersLiteral.Text = letterFee.Count.ToString();
                LetterFeesLiteral.Text = letterFee.Total.ToString("C");

                DeductablesTotalLiteral.Text = deductables.Total.ToString("C");

                EstimatedReleaseLiteral.Text = retentionReleaseEstimateReport.EstimatedRelease.ToString("C");
                DateViewedLiteral.Text = retentionReleaseEstimateReport.DateViewed.ToDateTimeString();
            }
        }

        public override void Export()
        {
            RetentionReleaseEstimateReport report =
                ViewState["RetentionReleaseEstimateReport"] as RetentionReleaseEstimateReport;

            if (report != null)
            {
                var document = new ExcelDocument();

                document.WriteTitle(report.Title);

                Hashtable hashtable = new Hashtable();
                hashtable.Add("Customer", "CustomerName");

                CffGGV_ReportGridView.WriteToExcelDocumentWithReplaceField(document, hashtable);

                document.InsertEmptyRow();

                document.AddCell("Release from Funded Transactions");
                document.AddCurrencyCell(report.ReleaseSummary.FundedTransactionRelease, 3);
                document.MoveToNextRow();
                document.AddCell("Release from Non Funded Transactions");
                document.AddCurrencyCell(report.ReleaseSummary.NonFundedTransactionRelease, 3);
                document.MoveToNextRow();
                document.AddCurrencyCell(report.ReleaseSummary.Total, 4);

                document.InsertEmptyRow();

                Deductables deductables = report.Deductables;
                document.AddCell("Unclaimed Credits");
                document.AddCurrencyCell(deductables.UnclaimedCredits, 3);
                document.MoveToNextRow();
                document.AddCell("Unclaimed Prepayments");
                document.AddCurrencyCell(deductables.UnclaimedRepurchases, 3);
                document.MoveToNextRow();
                document.AddCell("Unclaimed Discounts");
                document.AddCurrencyCell(deductables.UnclaimedDiscounts, 3);
                document.MoveToNextRow();
                document.AddCell("Likely Repurchases");
                document.AddCurrencyCell(deductables.LikelyRepurchases, 3);
                document.MoveToNextRow();
                document.AddCell("Interest & Charges");
                document.AddCurrencyCell(deductables.OverdueCharges, 3);
                document.MoveToNextRow();
                document.AddCell(string.Format("Cheque Fees ({0} at {1})", deductables.ChequeFee.Count,
                                               deductables.ChequeFee.Rate.ToString("C")));
                document.AddCurrencyCell(deductables.ChequeFee.Total, 3);
                document.MoveToNextRow();
                document.AddCell(string.Format("Postage Fees ({0} at {1})", deductables.Postage.Count,
                                               deductables.Postage.Rate.ToString("C")));
                document.AddCurrencyCell(report.Deductables.Postage.Total, 3);
                document.MoveToNextRow();
                document.AddCell(string.Format("Letter fees ({0} at {1})", deductables.LetterFees.Count,
                                               deductables.LetterFees.Rate.ToString("C")));
                document.AddCurrencyCell(deductables.LetterFees.Total, 3);
                document.MoveToNextRow();
                document.AddCurrencyCell(deductables.Total, 4);

                document.InsertEmptyRow();
                document.AddCell("Estimated Release");
                document.AddCurrencyCell(report.EstimatedRelease, 4);

                document.InsertEmptyRow();
                document.AddCell("Date Viewed");
                document.AddCell(report.DateViewed.ToDateTimeString());

                WriteToResponse(document.WriteToStream(), report.ExportFileName);
            }
        }

        public override void ShowAllRecords()
        {
            //CffGGV_ReportGridView.ShowAllRecords();
            CffGGV_ReportGridView.PageSize = (CffGGV_ReportGridView.DataSource as System.Collections.Generic.List<object>).Count;
        }

        public override void ShowPager()
        {
            //CffGGV_ReportGridView.ShowPager();
        }

        public override void ResetPaginationAndFocus()
        {
            //CffGGV_ReportGridView.ResetPaginationAndFocus();
        }

        public override bool IsViewAllButtonRequired()
        {
            return CffGGV_ReportGridView.IsViewAllButtonRequired;
        }
        public void Clear()
        {
            ViewState.Add("RetentionReleaseEstimateReport", null);

            CffGGV_ReportGridView.DataSource = null;
            CffGGV_ReportGridView.DataBind();


            FundedTransactionReleaseLiteral.Text = string.Empty;
            NonFundedTransactionReleaseLiteral.Text = string.Empty;
            ReleaseTotal.Text = string.Empty;

            UnclaimedCreditsLiteral.Text = string.Empty;
            UnclaimedRepurchasesLiteral.Text = string.Empty;
            UnclaimedDiscountsLiteral.Text = string.Empty;
            LikelyRepurchasesLiteral.Text = string.Empty;
            OverdueChargesLiteral.Text = string.Empty;

            ChequeFeeLiteral.Text = string.Empty;
            NumberOfChequesLiteral.Text = string.Empty;
            ChequeFeesLiteral.Text = string.Empty;

            PostageFeeLiteral.Text = string.Empty;
            NumberOfPostsLiteral.Text = string.Empty;
            PostageFeesLiteral.Text = string.Empty;

            LetterFeeLiteral.Text = string.Empty;
            NumberOfLettersLiteral.Text = string.Empty;
            LetterFeesLiteral.Text = string.Empty;

            DeductablesTotalLiteral.Text = string.Empty;

            EstimatedReleaseLiteral.Text = string.Empty;
            DateViewedLiteral.Text = string.Empty;
        }
    }
}