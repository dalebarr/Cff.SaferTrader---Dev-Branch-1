using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class StatementReportPanel : UserControl
    {
        public CffGenGridView cffGGV_StatementGridView;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            cffGGV_StatementGridView = new CffGenGridView();
            cffGGV_StatementGridView.ID = "cffGGV_StatementGridView";
            cffGGV_StatementGridView.PageSize = 1000; //displayall
            cffGGV_StatementGridView.DefaultPageSize = 1000;
            cffGGV_StatementGridView.AllowPaging = false;
            cffGGV_StatementGridView.AutoGenerateColumns = false;
            cffGGV_StatementGridView.SetSortExpression = "Date";

            //if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).Browser == "IE")
            //    cffGGV_StatementGridView.BorderWidth = Unit.Pixel(0);
            //else
                cffGGV_StatementGridView.BorderWidth = Unit.Pixel(1);
            
            //cffGGV_StatementGridView.CssClass = "cffGGV";
            cffGGV_StatementGridView.HeaderStyle.CssClass = "cffGGVHeader";
            cffGGV_StatementGridView.FooterStyle.CssClass = "dxgvFooter";
            cffGGV_StatementGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            cffGGV_StatementGridView.ShowHeaderWhenEmpty = true;
            cffGGV_StatementGridView.EmptyDataText = "No data to display";
            cffGGV_StatementGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            cffGGV_StatementGridView.Width = Unit.Percentage(100);

            cffGGV_StatementGridView.EnableViewState = true;
            cffGGV_StatementGridView.ShowFooter = true;
            cffGGV_StatementGridView.TotalsSummarySettings.SetColumnTotals("Debits,Credits");
            cffGGV_StatementGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Debits", "cffGGV_rightAlignedCell");
            cffGGV_StatementGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Credits", "cffGGV_rightAlignedCell");

            cffGGV_StatementGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;

            CffGenGridViewPlaceHolder.Controls.Clear();
            CffGenGridViewPlaceHolder.Controls.Add(cffGGV_StatementGridView);
        }

        private void configureGridColumns()
        {
            cffGGV_StatementGridView.Columns.Clear();
            cffGGV_StatementGridView.InsertDataColumn("Date", "Dated", CffGridViewColumnType.Date, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            cffGGV_StatementGridView.InsertDataColumn("Description", "Description", CffGridViewColumnType.Text, "10%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            cffGGV_StatementGridView.InsertDataColumn("Invoice", "Number", CffGridViewColumnType.Text, "10%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            cffGGV_StatementGridView.InsertDataColumn("Reference", "Reference", CffGridViewColumnType.Text, "10%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            cffGGV_StatementGridView.InsertCurrencyColumn("Debits", "Debits", "10%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            cffGGV_StatementGridView.InsertCurrencyColumn("Credits", "Credits", "10%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
        }

        public void ShowReport(StatementReport report)
        {
            //ac = false;
            if (report != null)
            {
                ManagementDetails managementDetails = report.ManagementDetails;
                accountNameLiteral.Text = managementDetails.Name;
                bankAndBranchLiteral.Text = managementDetails.BankDetails.NameAndBranch;
                accountNumberLiteral.Text = accountNumberLiteralTwo.Text = managementDetails.BankDetails.AccountNumber;

                PurchaserDetails purchaserDetails = report.PurchaserDetails;
                customerNameLiteral.Text = purchaserDetails.Name;
                customerAddressOneLiteral.Text = purchaserDetails.Address.AddressOne;
                customerAddressTwoLiteral.Text = purchaserDetails.Address.AddressTwo;
                customerAddressThreeLiteral.Text = purchaserDetails.Address.AddressThree;
                customerAddressFourLiteral.Text = purchaserDetails.Address.AddressFour;
                customerIdLiteral.Text = purchaserDetails.Id.ToString();
                referenceLiteral.Text = referenceLiteralTwo.Text = referenceLiteralThree.Text = purchaserDetails.Reference;

                configureGridColumns();
                cffGGV_StatementGridView.DataSource = report.Records;
                cffGGV_StatementGridView.DataBind();
                
                AgeingBalances ageingBalances = report.AgeingBalances;
                threeMonthsOrOverLiteral.Text = ageingBalances.ThreeMonthsAndOver.ToString("C");
                twoMonthsLiteral.Text = ageingBalances.TwoMonthAgeing.ToString("C");
                oneMonthLiteral.Text = ageingBalances.OneMonthAgeing.ToString("C");
                currentLiteral.Text = ageingBalances.Current.ToString("C");
                balanceLiteral.Text = ageingBalances.Balance.ToString("C");

                //Summary
                managementNameLiteral.Text = managementDetails.Name;
                managementAddressOneLiteral.Text = managementDetails.Address.AddressOne;
                managementAddressTwoLiteral.Text = managementDetails.Address.AddressTwo;
                managementAddressThreeLiteral.Text = managementDetails.Address.AddressThree;
                managementAddressFourLiteral.Text = managementDetails.Address.AddressFour;

                customerNameLiteral2.Text = purchaserDetails.Name;
                customerNumberLiteral.Text = purchaserDetails.Number.ToString();
                monthEndingLiteral.Text = report.MonthEnding.ToString();
                clientNameLiteral.Text = purchaserDetails.ClientName;
            }
        }


        public void ShowPager()
        {
            cffGGV_StatementGridView.PagerSettings.Visible = true;
        }

        public void ShowAllRecords()
        {
            cffGGV_StatementGridView.PageSize = cffGGV_StatementGridView.Rows.Count;
        }

        public bool IsViewAllButtonRequired()
        {
            return  (cffGGV_StatementGridView.CustomPagerSettingsMode == CffCustomPagerMode.None?false:true);
        }

        public bool ReadOnly
        {
            get; set;
        }
    }
}