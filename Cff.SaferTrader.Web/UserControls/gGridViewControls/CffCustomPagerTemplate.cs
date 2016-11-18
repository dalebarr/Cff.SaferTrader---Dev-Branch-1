using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;


namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{
    /// <summary>
    /// Paging template for the <see cref="CffGenGridView"/>
    /// </summary>
    public class CffCustomPagerTemplate : ITemplate
    {
        private int _pageSize;
        private int _pageCount;
        private int _rowsCount;

        private bool _isFirstPage;
        private bool _isLastPage;

        private bool _isGroupedBy;
        private int _groupedRowCount;
        private int _groupedPageCount;

        public TextBox txtBoxPage;

        public Button btnFirst;
        public Button btnPrev;
        public Button btnNext;
        public Button btnLast;

        /// <summary>
        /// Initializes a new instance of the <see cref="CffCustomPagerTemplate"/> class.
        /// </summary>
        /// <param name="CustomPagerSettingsMode">The <see cref="CustomPagerMode"/>.</param>
        /// <param name="pagedDataSource">The <see cref="PagedDataSource"/>.</param>
        /// <param name="dGGView">A reference to the <see cref="CffGenGridView"/>.</param>
        //public CffCustomPagerTemplate(CffCustomPagerMode CustomPagerSettingsMode, PagedDataSource pagedDataSource, CffGenGridView dGGView)
        public CffCustomPagerTemplate(CffGenGridView dGGView)
        {
            txtBoxPage = new TextBox();

            btnFirst = new Button();
            btnPrev  = new Button();
            btnNext  = new Button();
            btnLast  = new Button();

            btnFirst.SkinID = "none";
            btnPrev.SkinID = "none";
            btnNext.SkinID = "none";
            btnLast.SkinID = "none";

            _isGroupedBy = false;
            _groupedRowCount = 0;
            _groupedPageCount = 0;
        }

        /// <summary>
        /// When implemented by a class, defines the <see cref="T:System.Web.UI.Control">
        /// </see> object that child controls and templates belong to. These child controls are in turn defined within an inline template.
        /// </summary>
        /// <param name="container">The <see cref="T:System.Web.UI.Control"></see> object to contain the instances of controls from the inline template.</param>
        void ITemplate.InstantiateIn(Control container)
        {
            TableCell tCell = (TableCell)container;
            tCell.Width = Unit.Percentage(100);
            tCell.Height = Unit.Percentage(100);
            tCell.BorderStyle = BorderStyle.Solid;
            tCell.BorderColor = System.Drawing.Color.LightGray;
            tCell.BorderWidth = Unit.Pixel(1);


            CffGenGridView theGrid = ((CffGenGridView)(((System.Web.UI.Control)(container)).DataKeysContainer));

            this._isFirstPage = true;
            this._isLastPage = false;
            this._rowsCount = theGrid.RowCount;
            this._pageSize = theGrid.PageSize;
            this._pageCount = (int)(this._rowsCount / this._pageSize) + (((this._rowsCount % this._pageSize) > 0) ? 1 : 0);


            tCell.DataBinding += tCell_DataBinding;
        }

        void tCell_DataBinding(object sender, EventArgs e)
        {
            TableCell tCell = (TableCell)sender;
            CffGenGridView theGrid = ((CffGenGridView)(((System.Web.UI.Control)(sender)).DataKeysContainer));

            if (this._pageCount <= 0)
            {
                this._pageCount = 1;
                this._isLastPage = true;
            }

            if (theGrid.DataSource != null)
            {
                this._rowsCount = ((IEnumerable)theGrid.DataSource).Cast<object>().ToList().Count();

                if (this._rowsCount <= this._pageSize)
                {
                    this._isFirstPage = true;
                    this._isLastPage = true;
                }
                else
                    this._isLastPage = false;

                if (theGrid.AllowGroupBy)
                {
                    CffCustomPagerGroupSettings xSettings = theGrid.CustomPagerGroupedBySettings;
                    this._groupedRowCount = xSettings.GroupedRowCount;
                    this._groupedPageCount = (xSettings.PageCount > 0) ? xSettings.PageCount : 1;
                    this._isGroupedBy = true;
                }

            }

            Literal space = new Literal();
            space.Text = "&nbsp;";

            //start pager settings rendering
            TableRow PagerTableRow = new TableRow();

            if (theGrid.CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.Rows))
            { //select number of rows per page
                TableCell tCellRowsPerPage = new TableCell();
                tCellRowsPerPage.Width = Unit.Percentage(25);

                Label lb = new Label();
                lb.Text = "Rows Per Page: ";
                lb.ID = "lb_" + tCell.ID;
                tCellRowsPerPage.Controls.Add(lb);

                int i;
                int max = (this._rowsCount < 50) ? this._rowsCount : 50;
                const int increment = 5;
                bool alreadySelected = false;
                DropDownList _ddlPageSize = new DropDownList();
                _ddlPageSize.ToolTip = "Select number of rows per page";

                ListItem item;
                for (i = increment; i <= max; i += increment)
                {
                    item = new ListItem(i.ToString());
                    if (i == this._pageSize)
                    {
                        item.Selected = true;
                        alreadySelected = true;
                    }
                    _ddlPageSize.Items.Add(item);
                }

                item = new ListItem("All", this._rowsCount.ToString());
                if (this._rowsCount == this._pageSize && alreadySelected == false)
                {
                    item.Selected = true;
                    alreadySelected = true;
                }

                if (this._rowsCount > (i - increment) && alreadySelected == false)
                {
                    item.Selected = true;
                }

                //_ddlPageSize.AutoPostBack = true;
                _ddlPageSize.Items.Add(item);
                _ddlPageSize.Attributes.Add("runat", "server");
                _ddlPageSize.SelectedIndexChanged += new EventHandler(_ddlPageSize_SelectedIndexChanged);

                tCellRowsPerPage.Controls.Add(_ddlPageSize);
                PagerTableRow.Cells.Add(tCellRowsPerPage);
            }

            TableCell tCellNoRecords = new TableCell();
            tCellNoRecords.Width = Unit.Percentage(25);
            Label recordsLabel = new Label();
            if (this._isGroupedBy)
            {
                recordsLabel.Text = String.Format("Found {0} grouped record{1} out of {2}.", this._groupedRowCount, (this._groupedRowCount <= 1) ? String.Empty : "s", this._rowsCount);
            }
            else
                recordsLabel.Text = String.Format("Found {0} record{1}.", this._rowsCount, (this._rowsCount <= 1) ? String.Empty : "s");
            recordsLabel.Style.Add(HtmlTextWriterStyle.PaddingLeft, "30px");
            tCellNoRecords.Controls.Add(recordsLabel);
            PagerTableRow.Cells.Add(tCellNoRecords);


            if (theGrid.CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.Page))
            {
                TableCell tCellPageSelect = new TableCell();
                tCellPageSelect.Width = Unit.Percentage(25);

                Label lbl = new Label();
                lbl.Text = "Page";
                lbl.Style.Add(HtmlTextWriterStyle.PaddingRight, "3px");
                tCellPageSelect.Controls.Add(lbl);
                tCellPageSelect.Controls.Add(space);

                txtBoxPage.ToolTip = "Enter page number";
                txtBoxPage.MaxLength = 4;
                txtBoxPage.Width = Unit.Pixel(30);
                txtBoxPage.Style.Add(HtmlTextWriterStyle.MarginLeft, "10px");
                txtBoxPage.Style.Add(HtmlTextWriterStyle.MarginRight, "10px");

                txtBoxPage.Text = (theGrid.PageIndex + 1).ToString();

                //txtBoxPage.AutoPostBack = true;
                txtBoxPage.Attributes.Add("runat", "server");
                txtBoxPage.TextChanged += new EventHandler(txtBoxPage_TextChanged);
                tCellPageSelect.Controls.Add(txtBoxPage);

                if (this._pageCount == 1)
                    txtBoxPage.Enabled = false;
                else
                    txtBoxPage.Enabled = true;

                lbl = new Label();
                //if (this._isGroupedBy)
                //    lbl.Text = String.Format(" of {0} ", this._groupedPageCount);
                //else
                lbl.Text = String.Format(" of {0} ", this._pageCount);
                lbl.Style.Add(HtmlTextWriterStyle.PaddingRight, "20px");
                tCellPageSelect.Controls.Add(lbl);
                PagerTableRow.Cells.Add(tCellPageSelect);
            }

            //Render Buttons
            HtmlGenericControl divRight = new HtmlGenericControl("div");
            divRight.InnerText = "";
            divRight.ID = "divRight_" + tCell.ID;
            divRight.Style.Add("float", "right");
            divRight.Style.Add(HtmlTextWriterStyle.PaddingRight, "1px");
            divRight.Style.Add(HtmlTextWriterStyle.Width, "100%");
            divRight.Style.Add(HtmlTextWriterStyle.TextAlign, "right");

            //add first button
            if (theGrid.CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.FirstLast))
            {
                btnFirst.Text = "<<";
                btnFirst.CommandName = "Page";
                btnFirst.CommandArgument = "First";

                btnFirst.Enabled = !this._isFirstPage;
                if (theGrid.IsRowCommandPostBack)
                    if (theGrid.PageIndex == 1)
                        this.btnFirst.Enabled = false;

                if (btnFirst.Enabled)
                    btnFirst.ToolTip = "First page";
                btnFirst.Visible = true;

                btnFirst.Attributes.Add("runat", "server");
                btnFirst.CausesValidation = false;
                btnFirst.UseSubmitBehavior = false;

                divRight.Controls.Add(space);
                divRight.Controls.Add(btnFirst);
            }

            //add previous and next buttons
            if (theGrid.CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.PreviousNext))
            {
                btnPrev.Text = "<";
                btnPrev.CommandName = "Page";
                btnPrev.CommandArgument = "Prev";

                btnPrev.Enabled = !this._isFirstPage;
                if (theGrid.IsRowCommandPostBack)
                    if (theGrid.PageIndex == 1)
                        this.btnPrev.Enabled = false;

                if (btnPrev.Enabled)
                    btnPrev.ToolTip = "Previous page";
                btnPrev.Visible = true;

                btnPrev.UseSubmitBehavior = true;
                btnPrev.CausesValidation = false;
                btnPrev.Attributes.Add("runat", "server");

                divRight.Controls.Add(space);
                divRight.Controls.Add(btnPrev);

                //todo: setting usesubmitbehavior = false posts back to the page where the datagrid is rendered and actually bypasses RowCommand event 
                btnNext.Text = ">";
                btnNext.CommandName = "Page";  //"Next:
                btnNext.CommandArgument = "Next";

                btnNext.Enabled = !this._isLastPage;
                if (theGrid.IsRowCommandPostBack)
                    if (theGrid.PageIndex == 1)
                        this.btnNext.Enabled = false;

                if (btnNext.Enabled)
                    btnNext.ToolTip = "Next page";
                btnNext.Visible = true;

                btnNext.Attributes.Add("runat", "server");
                btnNext.UseSubmitBehavior = true;
                btnNext.CausesValidation = false;
                btnNext.Click += btnNext_Click;

                divRight.Controls.Add(space);
                divRight.Controls.Add(btnNext);
            }
            //add last button

            //add first button
            if (theGrid.CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.FirstLast))
            {
                btnLast.Text = ">>";
                btnLast.CommandName = "Page";
                btnLast.CommandArgument = "Last";

                if (theGrid.IsRowCommandPostBack)
                    if (theGrid.PageIndex == 1)
                        this.btnLast.Enabled = false;

                if (btnLast.Enabled)
                    btnLast.ToolTip = "Last page";
                btnLast.Visible = true;

                btnLast.UseSubmitBehavior = false;
                btnLast.CausesValidation = false;
                btnLast.Attributes.Add("runat", "server");
                btnLast.Enabled = !this._isLastPage;

                divRight.Controls.Add(space);
                divRight.Controls.Add(btnLast);
            }


            TableCell tCellPagerButtons = new TableCell();
            tCellPagerButtons.Style.Add(HtmlTextWriterStyle.Width, "auto");
            tCellPagerButtons.Controls.Add(divRight);
            PagerTableRow.Cells.Add(tCellPagerButtons);

            Table PagerTable = new Table();
            PagerTable.Width = Unit.Percentage(100);
            PagerTable.Rows.Add(PagerTableRow);

            tCell.Controls.Add(PagerTable);
        }

        /// <summary>
        /// Handles the TextChanged event of the tbPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtBoxPage_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb != null)
            {
                int page;
                if (int.TryParse(tb.Text, out page))
                {
                    if (page <= 0)
                        page = 1;

                    CffGenGridView theGrid = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffGenGridView)((((System.Web.UI.Control)(sender)).BindingContainer).BindingContainer));
                    if (page > this._pageCount)
                        theGrid.PageIndex = this._pageCount;
                    else
                        theGrid.PageIndex = page;
                }
            }
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            CffGenGridView theGrid = ((CffGenGridView)(((System.Web.UI.Control)(sender)).DataKeysContainer));
            theGrid.PageIndex += 1;

            //this._theGridView.PageIndex += 1;
            //this.txtBoxPage.Text = this._theGridView.PageIndex.ToString();
        }

        void _ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _ddlPageSizeValue = ((DropDownList)sender).SelectedValue.ToString();
            if (_ddlPageSizeValue == "All")
            {
                //this._theGridView.PageSize = this._rowsCount;
            }
            else
            {
                //this._theGridView.PageSize = Convert.ToInt32(_ddlPageSizeValue);
            }
            //this._theGridView.SelectedIndex = 0;
            //this._theGridView.PageIndex = 1;
        }

    }
}