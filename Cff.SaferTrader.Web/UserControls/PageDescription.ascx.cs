using System;
namespace Cff.SaferTrader.Web.UserControls
{
    public partial class PageDescription : System.Web.UI.UserControl
    {
        private string descriptionTitle;
        private string descriptionContent;

        public string DescriptionTitle
        {
            get { return descriptionTitle; }
            set { descriptionTitle = value; }
        }
        public string DescriptionContent
        {
            get { return descriptionContent; }
            set { descriptionContent = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            pageDescriptionLiteral.Text = DescriptionContent;
        }
    }
}