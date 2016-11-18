using System;
using System.Web.UI;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class RetentionSchedulePopup : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableRetentionSchedule;
            
            if (printable != null)
            {
                RetentionSchedule retentionSchedule = printable.RetentionSchedule;
                if (retentionSchedule != null) {
                    EndOfMonthLiteral.Text = retentionSchedule.EndOfMonth.ToString();
                    clientNameLiteral.Text = retentionSchedule.ClientName;

                    RetentionDetails retnDetails = printable.RetentionDetails;
                    if (retnDetails != null)
                    {
                        if (retnDetails.Hold > 1)
                        {
                            RetnStatementTitleLiteral.Text = "Estimated Retention Release";
                        }
                        else
                        {
                            RetnStatementTitleLiteral.Text = "Retention Statement";
                        }
                        Title = string.Format("{0} - End of month: {1} {2}", RetnStatementTitleLiteral.Text, retentionSchedule.EndOfMonth, retentionSchedule.ClientName);
                    }

                }
                retentionDetailsPanel.DisplayRetentionDetails(printable.RetentionDetails);
            }
        }
    }
}