using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Web.App_GlobalResources;


namespace Cff.SaferTrader.Web.UserControls
{

     [SupportsEventValidation]
    public class ViewAllButton : ImageButton
    {
        private const string ViewAllRecordsImageUrl = "~/images/btn_view_all_records.png";
        private const string ViewPagesImageUrl = "~/images/btn_view_pages.png";
        private bool isDefaultToAllRecords;
        private bool wasShowingViewPagesImage;

        public bool IsDefaultToAllRecords
        {
            get { return isDefaultToAllRecords; }
            set { isDefaultToAllRecords = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (IsDefaultToAllRecords)
            {
                ShowViewPagesImage();
            }
            else
            {
                ShowViewAllRecordsImage();
            }
        }

        private void SwitchImageOnClick(object sender, ImageClickEventArgs e)
        {
            wasShowingViewPagesImage = (ImageUrl == ViewPagesImageUrl);

            if (wasShowingViewPagesImage)
            {
                ShowViewAllRecordsImage();
            }
            else
            {
                ShowViewPagesImage();
            }
        }

        public ViewAllButton()
        {
            base.CssClass = "viewAll textButton";
            Click += SwitchImageOnClick;
        }

        public bool WasShowingViewPagesImage()
        {
            return wasShowingViewPagesImage;
        }

        public void ShowViewAllRecordsImage()
        {
            ImageUrl = ViewAllRecordsImageUrl;
            AlternateText = Cff_WebResource.viewAllRecordsText;
        }

        public void ShowViewPagesImage()
        {
            ImageUrl = ViewPagesImageUrl;
            AlternateText = Cff_WebResource.viewPagesText;
        }
    }
}