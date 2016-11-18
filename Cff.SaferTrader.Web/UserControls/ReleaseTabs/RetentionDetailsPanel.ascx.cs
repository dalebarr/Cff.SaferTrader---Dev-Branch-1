using System.Web.UI;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class RetentionDetailsPanel : UserControl
    {
        private bool showSummary = true;
        private int facilityType = 0;
        public void DisplayRetentionDetails(RetentionDetails retentionDetails)
        {
            if (retentionDetails != null)
            {
                //int facilityType = 0;
                facilityType = retentionDetails.ClientFacilityType;
                RetentionDeductable retentionDeductable = retentionDetails.RetentionDeductable;

                if (facilityType == 1)
                {
                    RetentionPanel.Visible = true;
                    SpaceerPanel.Visible = false;
                    Disc_Cr_PrepayPanel.Visible = true;
                    NFRec_PostEOM_etcPanel.Visible = true;
                    ReleasePanel.Visible = true;

                    //ReleasePanel2.Visible = false;
                    
                    AdjustmentsLiteral.Visible = true;
                    CA_DrMgt_AdjustmentsLiteralLiteral.Visible = false;
                    summarySection.Visible = true;
                    //LessLabel.Text = "Less:";
                    DebtorsLedgerLabel.Text = "Debtors Ledger:";
                    NonFundedLabel.Text =  "Non Funded:";
                    FundedInvoicesLabel.Text = "Funded Invoices:";
                    RepaymentLabel.Text = "Repayment:";
                    OverdueChargesLabel.Text = "Interest & Charges**:";
                    EstimatedRetentionReleaseLiteral.Text = retentionDetails.CalculateEstimatedRetentionRelease().ToString("C");
                    RepaymentLiteral.Font.Underline = true;
                    if (retentionDetails.Hold > 1)
                    {//Ref: CFF-21
                        LikelyRepurchasesAmountLiteral.Text = retentionDeductable.LikelyRepurchases.ToString("C");
                        LikelyRepurchasesLiteral.Visible = true;

                        LikelyRepurchasesAmountLiteral.Visible = true;
                        ReleaseLiteral.Text = "Estimated Release:";
                        if (retentionDetails.Hold == 3)
                        {
                            heldLiteral.Text = "Hold";
                            heldLiteral.Visible = true;
                        }
                    }
                    else
                    {
                        LikelyRepurchasesLiteral.Visible = false;
                        LikelyRepurchasesAmountLiteral.Visible = false;
                        ReleaseLiteral.Text = "Retention Release:";
                        heldLiteral.Text = "Held";
                    }

                }
                else
                {
                    RetentionPanel.Visible = false;
                    SpaceerPanel.Visible = true;
                    Disc_Cr_PrepayPanel.Visible = false;
                    NFRec_PostEOM_etcPanel.Visible = false;
                    ReleasePanel.Visible = false;
                    
                    //ReleasePanel2.Visible = true;
                    
                    AdjustmentsLiteral.Visible = false;
                    CA_DrMgt_AdjustmentsLiteralLiteral.Visible = true;
                    summarySection.Visible = false;
                    showSummary = false;

                    //LessLabel.Text = "Sum Charged Via Current Account";
                    //SumChargedHeaderLabel.Text = "Sum Charged Via Current Account";
                    OverdueChargesLabel.Text = "Charges Where Funding Period Exceeded**:";
                    EstimatedRetentionReleaseLiteral.Text = retentionDetails.CalculateSumChargedViaCA().ToString("C");
                    RepaymentLiteral.Font.Underline = false;
                    ReleaseLiteral.Text = "";
                    if (facilityType == 2)//Dr Mgt
                    {
                        DebtorsLedgerLabel.Text = "Debtors Ledger Summary:";
                        FundedInvoicesLabel.Text = "Funding Invoices:";
                        NonFundedLabel.Text = "Non Funding Invoices:";
                        RepaymentLabel.Text = "Monthly Administration Fee:";
                    }
                    else // Current Account 
                    {
                        DebtorsLedgerLabel.Text = "Current Account Summary:";
                        FundedInvoicesLabel.Text = "Drawings:";
                        NonFundedLabel.Text = "Non Funding Transactions:";
                        RepaymentLabel.Text = "Scheduled Repayment:";
                    }
                }
               
                heldLiteral.Visible = retentionDetails.IsHeld;
                endOfMonthLiteralTwo.Text = retentionDetails.EndOfMonth.ToString();

                NonFactoredLiteral.Text = retentionDetails.NonFactored.ToString("C");
                FactoredLiteral.Text = retentionDetails.Factored.ToString("C");
                totalLedgerLiteral.Text = retentionDetails.CalculateTotalLedger().ToString("C");

                RetentionInfo retentionInfo = retentionDetails.RetentionInfo;
                RetentionHeldLiteral.Text = retentionInfo.RetentionHeld.ToString("C");
                PercentageHeldLiteral.Text = retentionInfo.FactoredInvoicesPercentage.ToString();
                FactoredInvoicesPercentageLiteral.Text = retentionInfo.FactoredRetention.ToString("C");
                NetRetentionLiteral.Text = retentionInfo.Surplus.ToString("C");

                //RetentionDeductable retentionDeductable = retentionDetails.RetentionDeductable;
                OverdueChargesLiteral.Text = retentionDeductable.OverdueCharges.ToString("C");
                PostageRateLiteral.Text = retentionDeductable.PostRate.ToString("C");
                PostageAmountLiteral.Text = retentionDeductable.PostAmount.ToString("C");
                BankFeesLiteral.Text = retentionDeductable.BankFees.ToString("C");
                DiscountsLiteral.Text = retentionDeductable.Discounts.ToString("C");
                RepurchasesLiteral.Text = retentionDeductable.Repurchases.ToString("C");
                CreditNotesLiteral.Text = retentionDeductable.CreditNotes.ToString("C");
                TollsLiteral.Text = retentionDeductable.Tolls.ToString("C");
                LettersSentLiteral.Text = retentionDeductable.LettersSent.ToString("C");
                RepaymentLiteral.Text = retentionDeductable.Repayment.ToString("C");
                TotalDeductablesLiteral.Text = retentionDeductable.CalculateTotal().ToString("C");

                RetnNotes.Text = retentionDeductable.RetentionNotes;
                GstLiteral.Text = retentionDetails.CalculateGst().ToString("C");
                SurplusLessDeductablesLiteral.Text = retentionDetails.CalculateSurplusLessDeductables().ToString("C");
                NonFactoredReceiptsLiteral.Text = retentionDetails.NonFactoredReceipts.ToString("C");
                RetentionPriorToEndOfMonthLiteral.Text = retentionDetails.CalculateRetentionReleasePriorToEndOfMonth().ToString("C");

                //if (retentionDetails.Hold>1)
                //{//Ref: CFF-21
                //    LikelyRepurchasesAmountLiteral.Text = retentionDeductable.LikelyRepurchases.ToString("C");
                //    LikelyRepurchasesLiteral.Visible = true;
                    
                //    LikelyRepurchasesAmountLiteral.Visible = true;
                //    ReleaseLiteral.Text = "Estimated";
                //    if (retentionDetails.Hold==3)
                //    {
                //        heldLiteral.Text = "Hold";
                //        heldLiteral.Visible = true;
                //    }
                //}
                //else
                //{
                //    LikelyRepurchasesLiteral.Visible = false;
                //    LikelyRepurchasesAmountLiteral.Visible = false;
                //    ReleaseLiteral.Text = "Retention";
                //    heldLiteral.Text = "Held";
                //}

                TransactionsAfterEndOfMonth transactionsAfterEndOfMonth = retentionDetails.TransactionsAfterEndOfMonth;
                RepurchasesAfterEOMLiteral.Text = transactionsAfterEndOfMonth.Repurchases.ToString("C");
                CreditNotesAfterEOMLiteral.Text = transactionsAfterEndOfMonth.CreditNotes.ToString("C");
                NetRetentionAfterEOMLiteral.Text = transactionsAfterEndOfMonth.Balance.ToString("C");

                AdjustmentsLiteral.Text = retentionDetails.Adjustments.ToString("C");
                CA_DrMgt_AdjustmentsLiteralLiteral.Text = retentionDetails.Adjustments.ToString("C");
                //EstimatedRetentionReleaseLiteral.Text = retentionDetails.CalculateEstimatedRetentionRelease().ToString("C");

                
                summarySection.Visible = showSummary;
                if (showSummary)
                {
                    RetentionSummary retentionSummary = retentionDetails.RetentionSummary;
                    FactorDaysLiteral.Text = retentionSummary.FactorDays.ToString() + " Days";
                    OpeningBalanceLiteral.Text = retentionSummary.OpeningBalance.ToString("C");
                    InvoicesPurchasedLiteral.Text = retentionSummary.InvoicesPurchased.ToString("C");
                    CreditTransactionsLiteral.Text = retentionSummary.CalculateCreditTransactions().ToString("C");
                    ClosingBalanceLiteral.Text = retentionSummary.CalculateClosingBalance().ToString("C");
                }
            }
        }

        public void Clear()
        {
            NonFactoredLiteral.Text = string.Empty;
            FactoredLiteral.Text = string.Empty;

            RetentionHeldLiteral.Text = string.Empty;
            PercentageHeldLiteral.Text = string.Empty;
            FactoredInvoicesPercentageLiteral.Text = string.Empty;
            NetRetentionLiteral.Text = string.Empty;

            OverdueChargesLiteral.Text = string.Empty;
            PostageRateLiteral.Text = string.Empty;
            PostageAmountLiteral.Text = string.Empty;
            BankFeesLiteral.Text = string.Empty;
            DiscountsLiteral.Text = string.Empty;
            RepurchasesLiteral.Text = string.Empty;
            CreditNotesLiteral.Text = string.Empty;
            TollsLiteral.Text = string.Empty;
            LettersSentLiteral.Text = string.Empty;
            RepaymentLiteral.Text = string.Empty;
            TotalDeductablesLiteral.Text = string.Empty;

            RetnNotes.Text = string.Empty;
            GstLiteral.Text = string.Empty;

            // TODO: A-B.. rename
            SurplusLessDeductablesLiteral.Text = string.Empty;
            NonFactoredReceiptsLiteral.Text = string.Empty;
            RetentionPriorToEndOfMonthLiteral.Text = string.Empty;

            RepurchasesAfterEOMLiteral.Text = string.Empty;
            CreditNotesAfterEOMLiteral.Text = string.Empty;
            NetRetentionAfterEOMLiteral.Text = string.Empty;

            AdjustmentsLiteral.Text = string.Empty;
            EstimatedRetentionReleaseLiteral.Text = string.Empty;

            FactorDaysLiteral.Text = string.Empty;
            OpeningBalanceLiteral.Text = string.Empty;
            InvoicesPurchasedLiteral.Text = string.Empty;
            CreditTransactionsLiteral.Text = string.Empty;
            ClosingBalanceLiteral.Text = string.Empty;
        }

        public bool ShowSummary
        {
            get { return showSummary; }
            set { showSummary = value;}
        }
    }
}