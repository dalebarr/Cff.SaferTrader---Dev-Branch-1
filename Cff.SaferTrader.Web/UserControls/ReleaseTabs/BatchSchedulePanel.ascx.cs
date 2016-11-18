using System.Runtime.Serialization.Formatters;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class BatchSchedulePanel : System.Web.UI.UserControl, IScheduleTabView
    {
        public void Clear()
        {
            TotalInvoiceLiteral.Text = string.Empty;
            CheckConfirmLiteral.Text = string.Empty;
            NonFactoredLiteral.Text = string.Empty;
            FactoredLiteral.Text = string.Empty;
            FactoredLiteral01.Text = string.Empty;

            ScheduleDetails.Visible = false;
            BatchInProcessingLiteral.Text = "The batch is yet to be finalised";
            TotalInvoiceLabelLiteral.Text = "Total Invoices Processed";
            NonFactoredLabelLiteral.Text = "Non Funded Invoices";
            FactoredLabelLiteral.Text = "Invoices Funded:";
        }

        public void DisplaySchedule(BatchSchedule batchSchedule)
        {
            BatchScheduleFinanceInfo scheduleFinanceInfo = batchSchedule.ScheduleFinanceInfo;
            //TotalInvoiceLabelLiteral.Text = string.Empty;

            int ClientFacilityType = 0;
            ClientFacilityType = scheduleFinanceInfo.FacilityType; 
            if (ClientFacilityType == 4 || ClientFacilityType == 5)
            {
                TotalInvoiceLabelLiteral.Text = "Total Debit Transactions";
                NonFactoredLabelLiteral.Text = "Fees and Charges";
                FactoredLabelLiteral.Text = "Total Funding Transactions:";
            }
            else if (ClientFacilityType == 2)
            {
                TotalInvoiceLabelLiteral.Text = "Total Invoices Processed";
                NonFactoredLabelLiteral.Text = "Non Funding Invoices";
                FactoredLabelLiteral.Text = "Total Funding Invoices:";
            }
            else
            {
                TotalInvoiceLabelLiteral.Text = "Total Invoices Processed";
                NonFactoredLabelLiteral.Text = "Non Funded Invoices";
                FactoredLabelLiteral.Text = "Invoices Funded:";
            }

            if (ClientFacilityType == 2)
            {
                ScheduleDetails.LiteralAssignCr.Text = string.Format("{0:C}", scheduleFinanceInfo.AssignmentCr());
                ScheduleDetails.PanelAssignmentCr.Visible = true;
                
                if (scheduleFinanceInfo.NonCompliantFee > 0)
                {
                    ScheduleDetails.PanelNonCompliantFee.Visible = true;
                    ScheduleDetails.NonCompliantFeeLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.NonCompliantFee);
                }
                else
                {
                    ScheduleDetails.PanelNonCompliantFee.Visible = false;
                    ScheduleDetails.NonCompliantFeeLiteral.Text = string.Format("{0:C}", 0.00);
                }


                if (scheduleFinanceInfo.Credit > 0)
                {
                    ScheduleDetails.PanelCredit.Visible = true;
                    ScheduleDetails.CreditLiteral100.Visible = true;
                    ScheduleDetails.CreditResidual.Visible = true;
                    ScheduleDetails.CreditLabel.Text = "&nbsp;&nbsp;&nbsp;- Credit Note Residual:";
                    ScheduleDetails.CreditLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.CreditFacility2());
                    ScheduleDetails.CreditLiteral.Visible = true;
                    ScheduleDetails.CreditLabel.Visible = true;
                }
                else
                {
                    ScheduleDetails.PanelCredit.Visible = false;
                    ScheduleDetails.CreditLiteral100.Visible = false;
                    ScheduleDetails.CreditResidual.Visible = false;
                    ScheduleDetails.CreditLabel.Text = "Credits:";
                    ScheduleDetails.CreditLiteral.Text = string.Format("{0:C}", 0.00);

                    ScheduleDetails.CreditLabel.Visible = false;
                    ScheduleDetails.CreditLiteral.Visible = false;
                }

                if (scheduleFinanceInfo.Repurchase > 0)
                {
                    ScheduleDetails.PanelPrepayment.Visible = true;
                    ScheduleDetails.RepurchLiteral100.Visible = true;
                    ScheduleDetails.RepurchResidual.Visible = true;
                    ScheduleDetails.RepurchasesLabel.Text = "&nbsp;&nbsp;&nbsp;- Prepayment Residual:";
                    ScheduleDetails.RepurchasesLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.RepurchaseFacility2());

                }
                else
                {
                    ScheduleDetails.PanelPrepayment.Visible = false;
                    ScheduleDetails.RepurchLiteral100.Visible = false;
                    ScheduleDetails.RepurchResidual.Visible = false;
                    ScheduleDetails.RepurchasesLabel.Text = "Prepayments";
                    ScheduleDetails.RepurchasesLiteral.Text = string.Format("{0:C}", 0.00);
                }
                ScheduleDetails.PanelSumFeesForCA.Visible = true;
                ScheduleDetails.PanelRetention.Visible = false;
                ScheduleDetails.LiteralResidual.Text = string.Format("{0:C}", scheduleFinanceInfo.Retention);
         
                //ScheduleDetails.RepurchasesLabel.Text = "Prepayments:";
                ScheduleDetails.RepurchLiteral100.Visible = true;
                ScheduleDetails.RepurchLiteral100.Text = string.Format("{0:C}", scheduleFinanceInfo.Repurchase);
                ScheduleDetails.CreditLiteral100.Text = string.Format("{0:C}", scheduleFinanceInfo.Credit);
                ScheduleDetails.CreditResidual.Text = string.Format("{0:C}", scheduleFinanceInfo.CreditResidual());
                ScheduleDetails.RepurchResidual.Text = string.Format("{0:C}", scheduleFinanceInfo.RepurchResidual());
                ScheduleDetails.RepurchasesLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.RepurchaseFacility2());
                ScheduleDetails.DeductionsLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.CalculateDeductionsFacility2());
                ScheduleDetails.AvailableForReleaseLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.CalculateAvailableForReleaseFacility2());
                ScheduleDetails.AvailableForReleaseLabel.Text = "Expected Change in Funds:";
                ScheduleDetails.LiteralSumFeesCA.Text = string.Format("{0:C}", scheduleFinanceInfo.CalculateToCAFeesFacility2());
                if (scheduleFinanceInfo.FactorFee > 0)
                {
                    ScheduleDetails.FactorFeeLabel.Text = "Line Fee:";
                }
                else
                {
                    ScheduleDetails.FactorFeeLabel.Visible = false;
                    ScheduleDetails.FactorFeeLiteral.Visible = false;
                }
            }
            else
            {
                ScheduleDetails.PanelAssignmentCr.Visible = false;
                ScheduleDetails.PanelSumFeesForCA.Visible = false;
                ScheduleDetails.PanelCredit.Visible = false;
                ScheduleDetails.PanelPrepayment.Visible = false;
                ScheduleDetails.CreditResidual.Visible = false;
                ScheduleDetails.RepurchResidual.Visible = false;
                ScheduleDetails.PanelRetention.Visible = true;
                ScheduleDetails.PanelNonCompliantFee.Visible = false;
                ScheduleDetails.NonCompliantFeeLiteral.Text = string.Format("{0:C}", 0.00);
                ScheduleDetails.LiteralResidual.Text = string.Format("{0:C}", 0.00);
                ScheduleDetails.LiteralAssignCr.Text = string.Format("{0:C}", 0.00);
           
                //ScheduleDetails.RepurchasesLabel.Text = "Repurchases:";
                ScheduleDetails.RepurchLiteral100.Visible = false;
                ScheduleDetails.CreditLiteral100.Visible = false;
                ScheduleDetails.RepurchLiteral100.Text = string.Format("{0:C}", 0.00);
                ScheduleDetails.CreditLiteral100.Text = string.Format("{0:C}", 0.00);
                ScheduleDetails.CreditResidual.Text = string.Format("{0:C}", 0.00);
                ScheduleDetails.RepurchResidual.Text = string.Format("{0:C}", 0.00);
                if (scheduleFinanceInfo.Credit > 0)
                {
                    ScheduleDetails.CreditLabel.Text = "Credits:";
                    ScheduleDetails.CreditLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.Credit);
                    ScheduleDetails.CreditLiteral.Visible = true;
                    ScheduleDetails.CreditLabel.Visible = true;
                }
                else
                {
                    ScheduleDetails.CreditLabel.Visible = false;
                    ScheduleDetails.CreditLiteral.Visible = false;
                }
                ScheduleDetails.RepurchasesLabel.Text = "Prepayments:";
                ScheduleDetails.RepurchasesLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.Repurchase);
                
                ScheduleDetails.DeductionsLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.CalculateDeductions());
                ScheduleDetails.AvailableForReleaseLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.CalculateAvailableForRelease());
                ScheduleDetails.AvailableForReleaseLabel.Text = "Available for Release:";
                ScheduleDetails.LiteralSumFeesCA.Text = string.Format("{0:C}", 0.00);
                ScheduleDetails.FactorFeeLabel.Text = "Factor Fee:";
            }
        
            bool underlineText = false;

            if (scheduleFinanceInfo.Repurchase > 0)
            {
                ScheduleDetails.PanelViewRepurchases.Visible = true;
            }
            else
            {
                ScheduleDetails.PanelViewRepurchases.Visible = false;
            }

            DivFacilityType1.Visible = true;
            BatchInProcessingLiteral.Text = string.Empty;
            ScheduleDetails.Visible = true;
            ScheduleDetails.AdminFeeLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.AdminFee);
            ScheduleDetails.AdminFeeGstLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.AdminFeeGst);
            ScheduleDetails.AdminFeeTotalLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.CalculateAdminFeeTotal());
            ScheduleDetails.FactorFeeLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.FactorFee);
            ScheduleDetails.RetentionLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.Retention);

            ScheduleDetails.PostageLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.Post);
            ScheduleDetails.PostGstLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.PostGst);
            ScheduleDetails.PostageTotalLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.CalculatePostageTotal());
            ScheduleDetails.ChargesTotalLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.CalculateTotalCharges());

            ScheduleDetails.StatusDescriptionLiteral.Text = batchSchedule.StatusDescription;
            ScheduleDetails.ReleasedLiteral.Text = (string.IsNullOrEmpty(batchSchedule.Released.ToString()))?"": ("Released: " + batchSchedule.Released.ToString());
            ScheduleDetails.DateFinishedLiteral.Text = batchSchedule.DateFinished.ToString();

            ScheduleDetails.ScheduleBatchNoteLabel.Text = "Note:";
            ScheduleDetails.ScheduleBatchNoteLiteral.Text = batchSchedule.Note;

            ScheduleDetails.AdminFeeTotalLabel.Font.Underline = true;
            ScheduleDetails.PostageTotalLabel.Font.Underline = true;
          
            if (scheduleFinanceInfo.AdminFeeGst > 0)
            {
                ScheduleDetails.PanelAdminFeeGst.Visible = true;
                ScheduleDetails.AdminFeeLiteral.Visible = true;
            }
            else
            {
                //ScheduleDetails.PanelAdminFeeGst.Visible = false;    // dbb
                //ScheduleDetails.AdminFeeLiteral.Visible = false;
            }

            ScheduleDetails.PanelAdminFeeGst.Font.Underline = true;    // dbb

            if (scheduleFinanceInfo.PostGst > 0)
            {
                ScheduleDetails.PanelPostGst.Visible = true;
                ScheduleDetails.PostageLiteral.Visible = true;

            }
            else
            {
                //ScheduleDetails.PanelPostGst.Visible = false;    // dbb
                //ScheduleDetails.PostageLiteral.Visible = false;
            }

            ScheduleDetails.PanelPostGst.Font.Underline = true;  // dbb

            if (ClientFacilityType == 2)
            {
                if (scheduleFinanceInfo.CalculateDeductionsFacility2() == 0)
                {
                    ScheduleDetails.DivDeductions.Visible = false;
                    underlineText = true;
                }
                else
                {
                    ScheduleDetails.DivDeductions.Visible = true;
                    underlineText = false;
                }

                if (scheduleFinanceInfo.CalculateTotalCharges() == 0)
                {
                    ScheduleDetails.PanelCharges.Visible = false;
                    underlineText = false; //true;   // dbb
                }
                else
                {
                    ScheduleDetails.PanelCharges.Visible = true;
                    underlineText = false;
                }

            }


            if (scheduleFinanceInfo.CalculateDeductions() == 0)
            {
                ScheduleDetails.DivDeductions.Visible = false;
                //underlineText = true;   //dbb
            }
            else
            {
                ScheduleDetails.DivDeductions.Visible = true;
                underlineText = false;
            }

            if (scheduleFinanceInfo.CalculateTotalCharges() == 0)
            {
                ScheduleDetails.PanelCharges.Visible = false;
                //underlineText = true;   // dbb
            }
            else
            {
                ScheduleDetails.PanelCharges.Visible = true;
                underlineText = false;
            }


            if (ScheduleDetails.PanelViewRepurchases.Visible)
            {
                underlineText = false;
            }


            if (ClientFacilityType == 1)
            {
                underlineText = false;
            }


            if (ScheduleDetails.PanelAssignmentCr.Visible)
            {
                FactoredLiteral01.Visible = true;
                FactoredLiteral.Visible = false;
            }
            else
            {
                FactoredLiteral01.Visible = false;
                FactoredLiteral.Visible = true;
            }


            if (underlineText == true)
            {
                FactoredLiteral.Font.Underline = true;
            }
            else
            {
                FactoredLiteral.Font.Underline = false;
                FactoredLiteral01.Font.Underline = false;
            }

        }

        public void ShowScheduleIsInProcessing(BatchSchedule batchSchedule)
        {
            ScheduleDetails.Visible = false;
            BatchInProcessingLiteral.Text = "The batch is yet to be finalised";
        }

        public void DisplayScheduleSummary(BatchScheduleFinanceInfo scheduleFinanceInfo)
        {
            TotalInvoiceLabelLiteral.Text = scheduleFinanceInfo.strTotalInvoiceLabel;//"Total Invoices Processed";
            NonFactoredLabelLiteral.Text = scheduleFinanceInfo.strNonFactoredLabel;//"Non Funded Invoices";
            FactoredLabelLiteral.Text =  scheduleFinanceInfo.strFactoredLabel; //"Invoices Funded:";
            TotalInvoiceLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.TotalInvoice);
            CheckConfirmLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.CheckConfirm);
            NonFactoredLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.NonFactored);
            FactoredLiteral.Text = string.Format("{0:C}", scheduleFinanceInfo.Factored);
            FactoredLiteral01.Text = string.Format("{0:C}", scheduleFinanceInfo.Factored);
        }

        public void HideNote()
        {
            ScheduleDetails.HideNote();
        }

        public void ShowCheckOrConfirmRow()
        {
            checkOrConfirmRow.Visible = true;
        }

        public void HideCheckOrConfirmRow()
        {
            checkOrConfirmRow.Visible = false;
        }

        public void ShowNote()
        {
            ScheduleDetails.ShowNote();
        }
    }
}