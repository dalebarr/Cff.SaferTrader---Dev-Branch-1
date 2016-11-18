using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Web.UserControls
{
    public class ViewPreviewButton : ImageButton
    {
        private bool wasShowingPreviewImage;
        private const string CollapseImageUrl = "~/images/preview_toggle_collapse.png";
        private const string CollapseText = "Collapse";
        private const string ExpandImageUrl = "~/images/preview_toggle_expand.png";
        private const string ExpandText = "Expand";
        private const string NormalClass = "normal";
        private const string PreviewClass = "preview";
        private const string PreviewTooltip = "Preview";
        private const string ExpandToolTip = "Expand";

        public ViewPreviewButton()
        {
            base.ImageUrl = ExpandImageUrl;
            base.AlternateText = ExpandText;
            base.ToolTip = ExpandToolTip;

            Click += SwitchImageOnClick;
        }

        private void SwitchImageOnClick(object sender, ImageClickEventArgs e)
        {
            wasShowingPreviewImage = IsShowingViewPreviewImage();
            if (wasShowingPreviewImage)
            {
                CssClass = NormalClass;
                ImageUrl = ExpandImageUrl;
                AlternateText = ExpandText;
                ToolTip = ExpandToolTip;
            }
            else
            {
                CssClass = PreviewClass;
                ImageUrl = CollapseImageUrl;
                AlternateText = CollapseText;
                ToolTip = PreviewTooltip;
            }
        }

        public bool WasShowingPreviewImage()
        {
            return wasShowingPreviewImage;
        }

        public bool IsShowingViewPreviewImage()
        {
            return ImageUrl.Equals(CollapseImageUrl);
        }
    }
}