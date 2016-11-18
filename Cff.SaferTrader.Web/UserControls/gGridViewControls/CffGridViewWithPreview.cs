using System;
using DevExpress.Web.ASPxGridView;

namespace Cff.SaferTrader.Web.UserControls
{
    public class CffGridViewWithPreview : CffGridView
    {
        private readonly int previewPageSize;

        public CffGridViewWithPreview(int defaultPageSize, int previewPageSize) : base(defaultPageSize)
        {
            this.previewPageSize = previewPageSize;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            if (IsShowAllRecords)
            {
                // Show all records if configured to do so
                ShowAllRecords();
            }
            else
            {
                // Else show preview
                ShowPreview();
            }
        }

        /// <summary>
        /// Show three records
        /// </summary>
        public void ShowPreview()
        {
            SettingsPager.Visible = false;
            Settings.ShowFooter = false;
            SettingsPager.PageSize = previewPageSize;
            SettingsPager.Mode = GridViewPagerMode.ShowPager;
        }

        /// <summary>
        /// Shows default number of records
        /// </summary>
        public void ShowDefaultView()
        {
            ShowPager();
            Settings.ShowFooter = true;
            SettingsPager.PageSize = defaultPageSize;
        }
    }
}