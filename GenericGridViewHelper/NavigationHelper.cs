using System;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DomainModel;

namespace GenericGridViewHelper
{

    public class NavigationHelper
    {
        //ListControl _ddlRowsPPg;

        public static void SetUpNavigationBar(
            GridView gvGeneric,
            int totalRowCount,
            ImageButton imgFirst,
            ImageButton imgPrevious,
            ImageButton imgNext,
            ImageButton imgLast,
            Label lblStartIndex,
            Label lblEndIndex,
            Label lblTotalCount)
        {

            int startIndex;
            int endIndex;
            SetStartEndIndex(gvGeneric, totalRowCount, out startIndex, out endIndex);

            lblStartIndex.Text = startIndex.ToString();
            lblEndIndex.Text = endIndex.ToString();
            lblTotalCount.Text = totalRowCount.ToString();
       

            if (startIndex == 1)
            {
                if (endIndex < totalRowCount) // at the first page, can move to next one
                {
                    imgFirst.ImageUrl = "~/images/inactive-control-double-180.gif";
                    imgFirst.Enabled = false;
                    imgPrevious.ImageUrl = "~/images/inactive-control-180.gif";
                    imgPrevious.Enabled = false;

                    imgNext.ImageUrl = "~/images/control.gif";
                    imgLast.ImageUrl = "~/images/control-double.gif";
                }
                else // no paging at all
                {
                    imgFirst.ImageUrl = "~/images/inactive-control-double-180.gif";
                    imgFirst.Enabled = false;
                    imgPrevious.ImageUrl = "~/images/inactive-control-180.gif";
                    imgPrevious.Enabled = false;
                    imgNext.ImageUrl = "~/images/inactive-control.gif";
                    imgNext.Enabled = false;
                    imgLast.ImageUrl = "~/images/inactive-control-double.gif";
                    imgLast.Enabled = false;
                }
            }
            else if (endIndex == totalRowCount)
            {
                imgFirst.ImageUrl = "~/images/control-double-180.gif";
                imgPrevious.ImageUrl = "~/images/control-180.gif";

                imgNext.ImageUrl = "~/images/inactive-control.gif";
                imgNext.Enabled = false;
                imgLast.ImageUrl = "~/images/inactive-control-double.gif";
                imgLast.Enabled = false;
            }
            else
            {
                imgFirst.ImageUrl = "~/images/control-double-180.gif";
                imgPrevious.ImageUrl = "~/images/control-180.gif";
                imgNext.ImageUrl = "~/images/control.gif";
                imgLast.ImageUrl = "~/images/control-double.gif";
            }
        }

        public static void PreparePageIndex(GridView gvGeneric, string imgButtonID)
        {
            switch (imgButtonID)
            {
                case "imgNext":
                    gvGeneric.PageIndex++;
                    break;
                case "imgLast":
                    gvGeneric.PageIndex = gvGeneric.PageCount;
                    break;
                case "imgPrevious":
                    var newInt = gvGeneric.PageIndex - 1;
                    gvGeneric.PageIndex = Math.Max(0, newInt);
                    break;
                case "imgFirst":
                    gvGeneric.PageIndex = 0;
                    break;
            }
        }

        public static void PreparePaging(GridView gvGeneric, int totalRowCount, ListControl ddlRowsPerPage, TemplateControl page, bool allowPaging, Control navigationBar)
        {
            if (allowPaging)
            {
                var pageSize = ddlRowsPerPage.SelectedValue == "all" ? totalRowCount.ToString() : ddlRowsPerPage.SelectedValue;
//                if (ddlRowsPerPage != null)
//                {                    
//                    gvGeneric.PageSize = int.Parse(pageSize);
//                }
//                else {
                   gvGeneric.PageSize = int.Parse(pageSize);
//                }
            }
            else
            {
                page.Controls.Remove(navigationBar);
            }
        }

        private static void SetStartEndIndex(GridView gridView, int totalRowCount, out int startIndex, out int endIndex)
        {
            startIndex = gridView.PageIndex * gridView.PageSize + 1;
            endIndex = Math.Min(totalRowCount, (gridView.PageIndex + 1) * gridView.PageSize);
        }
    }
}
