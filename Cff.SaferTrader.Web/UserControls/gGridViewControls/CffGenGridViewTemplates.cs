using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;

namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{
    public class CffGenGridViewTemplate : ITemplate
    {
        protected CffGridViewColumnType _columnType;
        protected string DataHeader;
        protected string _columnWidth;
        protected string _textCss;
        
        public String DataField { get; set; }
        public bool ReadOnly { get; set; }

        public CffGenGridViewTemplate()
        { }

        public CffGenGridViewTemplate(CffGridViewColumnType theColumnType, string columnName, 
                                          string columnHeader = "", string colWidth = "100%", 
                                                string textCss="cffGGV_leftAlignedCell", bool readOnly = false)
        {
            DataField = columnName;
            DataHeader = columnHeader;
            ReadOnly = readOnly;
            _columnType = theColumnType;
            _columnWidth = colWidth;
            _textCss = textCss;
        }

        public virtual void InstantiateIn(Control container)
        {
            HtmlContainerControl divLeft = (HtmlContainerControl)new HtmlGenericControl("div");
            divLeft.InnerText = "";

            if (_textCss.ToLower().Contains("left"))
                divLeft.Style.Add("float", "left");

            if (_textCss.ToLower().Contains("right") || _textCss.ToLower().Contains("currency"))
                divLeft.Style.Add("float", "right");

            divLeft.Style.Add(HtmlTextWriterStyle.Width, "100%");
            divLeft.Style.Add(HtmlTextWriterStyle.Height, "100%");
            divLeft.DataBinding += divLeft_DataBinding;
            container.Controls.Add(divLeft);
        }

        protected virtual void divLeft_DataBinding(object sender, EventArgs e)
        {
        }
    }

    public class GridViewHeaderLabelTemplate : ITemplate
    {
        private string _textLabel;
        public string TextLabel
        {
            get { return this._textLabel; }
            set { this._textLabel = value; }
        }

        public GridViewHeaderLabelTemplate(string theTextLabel)
        {
            _textLabel = theTextLabel;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            var label = new Label();
            label.Text = _textLabel;
            label.CssClass = "left";
            container.Controls.Add(label);
        }
    }

    public class GridViewDataTemplate : CffGenGridViewTemplate
    {
        public GridViewDataTemplate(CffGridViewColumnType theColumnType, string columnName, string columnHeader = "", string colWidth = "100%", 
                                        string textCss = "cffGGV_leftAlignedCell", bool readOnly = false) :  base(theColumnType, columnName, columnHeader, colWidth, textCss, readOnly)
        {
        }

        public override void InstantiateIn(Control container)
        {
            base.InstantiateIn(container);
        }

        protected override void divLeft_DataBinding(object sender, EventArgs e)
        {
            HtmlContainerControl divLeft = (HtmlContainerControl)sender;
            var context = DataBinder.GetDataItem(divLeft.NamingContainer);
            //divLeft.ID = "TDIV_" + divLeft.NamingContainer.ID;

            CffGenGridView gV = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffGenGridView)(divLeft.NamingContainer.BindingContainer);
            GridViewRow gvRow = ((System.Web.UI.WebControls.GridViewRow)(divLeft.Controls[0].BindingContainer));

            if ((gV!=null) && (gV.IsInEditMode) && (gV.EditIndex == gvRow.RowIndex) && (gV.EditingMode == CffGridViewEditingMode.InLine))
            { //if is in editmode and edit index == current row and is inline edit
                HtmlGenericControl LineBreak = new HtmlGenericControl("br");   
                HtmlGenericControl par = new HtmlGenericControl("p");

                TextBox TBox = new TextBox();
                if (DataBinder.Eval(context, DataField) != null)
                    TBox.Text = DataBinder.Eval(context, DataField) == null ? "" : DataBinder.Eval(context, DataField).ToString();
                else
                    TBox.Text = "";

                TBox.ID = "Tbx" + DataField;
                TBox.ControlStyle.CssClass = "cffGV_CellTextBox";
                TBox.Style.Add(HtmlTextWriterStyle.BorderStyle, "none");
                
                TBox.Visible = true;
                TBox.ReadOnly = this.ReadOnly;
                TBox.Enabled = !this.ReadOnly;
                TBox.AutoPostBack = this.ReadOnly; ;

                Label label = new Label();
                label.ControlStyle.Width = Unit.Pixel((DataHeader.Length + 1) * 2);
                label.ControlStyle.CssClass = "cffGGV_CellLabel";
                label.Style.Add(HtmlTextWriterStyle.BorderStyle, "none");
                label.Style.Add(HtmlTextWriterStyle.Margin, "0px 0px 0px 0px;");
           
                label.Text = DataHeader + ":";
                par.Controls.Add(label);
                par.Controls.Add(LineBreak);  
                par.Controls.Add(TBox);
                par.Style.Add(HtmlTextWriterStyle.Padding, "0px 0px 0px 0px;");
                par.Style.Add(HtmlTextWriterStyle.Margin, "0px 0px 0px 0px;");
                par.Attributes.Add("align", "left");

                divLeft.Controls.Add(par);
                divLeft.Style.Add(HtmlTextWriterStyle.Width, "100%");
                divLeft.Style.Add(HtmlTextWriterStyle.Height, "150px");
                divLeft.Style.Add(HtmlTextWriterStyle.Padding, "0px 0px 0px 0px");
                divLeft.Style.Add(HtmlTextWriterStyle.Margin, "0px 0px 0px 0px");
                gvRow.Height = Unit.Pixel(150);
            }
            else
            {
                Label label = new Label();
                label.ControlStyle.CssClass = this._textCss;
                try {
                    label.Text = "";
                    if (this._columnType == CffGridViewColumnType.Date)  
                    {
                        object o = DataBinder.Eval(context, DataField);
                        if (DataBinder.Eval(context, DataField) != null) {
                            if (o != null)
                                label.Text = o.ToString();
                            else
                                label.Text = CffGenGridViewCommon.FormatDataColumn(this._columnType, DataBinder.Eval(context, DataField) == null ? "" : DataBinder.Eval(context, DataField).ToString());
                        }
                    }
                    else if (this._columnType == CffGridViewColumnType.DateTime)   // added by dbb
                    {
                        object ct = context.GetType().GetProperty("Created").GetValue(context, null);
                        object dt = ct.GetType().GetProperty("DateTime").GetValue(ct, null);

                        label.Text = CffGenGridViewCommon.FormatDataColumn(this._columnType, DataBinder.Eval(context, DataField) == null ? "" : dt.ToString());
                    }
                    else {
                        if (DataBinder.Eval(context, DataField) != null)
                            label.Text = CffGenGridViewCommon.FormatDataColumn(this._columnType, DataBinder.Eval(context, DataField) == null ? "" : DataBinder.Eval(context, DataField).ToString());
                        
                    }
                } catch (Exception exc) {
                    label.Text = exc.Message;
                }

                label.ControlStyle.BorderWidth = Unit.Pixel(0);
                if (this._columnType == CffGridViewColumnType.Currency) 
                        label.ControlStyle.CssClass = "cffGGV_currencyCell";
                
                divLeft.Controls.Add(label);
            }
        }
    }

    //MSarza -- added class
    /// <summary>
    //
    /// </summary>
    public class GridViewDropdownTemplate : CffGenGridViewTemplate
    {
        public GridViewDropdownTemplate(CffGridViewColumnType theColumnType, string columnName, string columnHeader = "",
                string colWidth = "100%", string textCss = "cffGGV_leftAlignedCell", bool readOnly = false)
            : base(theColumnType, columnName, columnHeader, colWidth, textCss, readOnly)
        {
        }

        public override void InstantiateIn(Control container)
        {
            base.InstantiateIn(container);
        }


        protected override void divLeft_DataBinding(object sender, EventArgs e)
        {
            CffGenGridView gV = null;
            GridViewRow gvRow = null;

            HtmlContainerControl divLeft = (HtmlContainerControl)sender;

            var context = DataBinder.GetDataItem(divLeft.NamingContainer);

            if ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffGenGridView)(divLeft.NamingContainer.BindingContainer) != null)
            {
                gV = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffGenGridView)(divLeft.NamingContainer.BindingContainer);
                gvRow = ((System.Web.UI.WebControls.GridViewRow)(divLeft.Controls[0].BindingContainer));
            }

            if (this.DataField == "emailReceipt")
            {
                Label label = new Label();
                label.ControlStyle.CssClass = this._textCss;

                foreach (EmailReceiptType rt in Enum.GetValues(typeof(EmailReceiptType)))
                {
                    if (Convert.ToInt16(DataBinder.Eval(context, DataField)) == (int)rt)
                        label.Text = rt.ToString();
                }

                divLeft.Controls.Add(label);
            }


            else
            {
                //implement other dropdown types here
            }



        }

        void DList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }



    public class GridViewMemoTemplate : CffGenGridViewTemplate
    {
        public GridViewMemoTemplate(CffGridViewColumnType theColumnType, string columnName, string columnHeader = "", 
                                            string colWidth = "100%", string textCss = "cffGGV_CellParagraph", bool readOnly=false) : 
                                                    base(theColumnType, columnName, columnHeader, colWidth, textCss,  readOnly)
        {
        }

        public override void InstantiateIn(Control container)
        {
            base.InstantiateIn(container);
        }
        
        protected override void divLeft_DataBinding(object sender, EventArgs e)
        {
            CffGenGridView gV = null;
            GridViewRow gvRow = null;

            HtmlContainerControl divLeft = (HtmlContainerControl)sender;
            var context = DataBinder.GetDataItem(divLeft.NamingContainer);
            //divLeft.ID = "TDIV_" + divLeft.NamingContainer.ID;
            
            if ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffGenGridView)(divLeft.NamingContainer.BindingContainer) != null)
            {
                gV = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffGenGridView)(divLeft.NamingContainer.BindingContainer);
                gvRow = ((System.Web.UI.WebControls.GridViewRow)(divLeft.Controls[0].BindingContainer));
            }

            if ((gV != null) && gV.IsInEditMode && (gV.EditIndex == gvRow.RowIndex) && (gV.EditingMode == CffGridViewEditingMode.InLine))
            {
                //if is in editmode and edit index == current row and is inline edit
                HtmlGenericControl LineBreak = new HtmlGenericControl("br");
                HtmlGenericControl par = new HtmlGenericControl("p");

                TextBox TBox = new TextBox();
                String strText = "";
                if (DataBinder.Eval(context, DataField) != null)
                {
                    strText = DataBinder.Eval(context, DataField) == null
                        ? ""
                        : DataBinder.Eval(context, DataField).ToString();
                    TBox.Text = strText; 

                }
                else
                    TBox.Text = "";

                TBox.ID = "Tbx" + DataField;

                TBox.CssClass = "cffGV_CellTextBox";
                TBox.Style.Add(HtmlTextWriterStyle.BorderStyle, "none");
                TBox.Style.Add(HtmlTextWriterStyle.Width, "100%");

                TBox.Width = Unit.Percentage(100);
                TBox.Height = Unit.Pixel(100);
                TBox.TextMode = TextBoxMode.MultiLine;
                //TBox.CssClass = "maxPopupScreenWidth";
                TBox.Attributes.Add("runat", "server");
                TBox.Attributes.Add("Rows", "100");

                TBox.Wrap = true;
                TBox.Enabled = !this.ReadOnly;
                TBox.AutoPostBack = false;

                Label label = new Label();
                //label.ControlStyle.Width = Unit.Pixel((DataHeader.Length + 1) * 2);  //dbb
                label.ControlStyle.Width = Unit.Pixel((DataHeader.Length) * 2);
                label.ControlStyle.CssClass = "cffGGV_CellLabel";
                label.Style.Add(HtmlTextWriterStyle.Margin, "0px 0px 0px 0px;");

                label.Text = DataHeader + ": ";
                par.Controls.Add(label);
                par.Controls.Add(LineBreak);   
                par.Controls.Add(TBox);

                par.Style.Add(HtmlTextWriterStyle.Padding, "0px 0px 0px 0px;");
                par.Style.Add(HtmlTextWriterStyle.Margin, "0px 0px 0px 0px;");
                par.Attributes.Add("align", "left");

                divLeft.Controls.Add(par);
                divLeft.Style.Add(HtmlTextWriterStyle.Width, "100%");
                divLeft.Style.Add(HtmlTextWriterStyle.Height, "150px");
                divLeft.Style.Add(HtmlTextWriterStyle.Padding, "0px 0px 0px 0px");
                divLeft.Style.Add(HtmlTextWriterStyle.Margin, "0px 0px 0px 0px");
                gvRow.Height = Unit.Pixel(150);
            }
            else
            {
                HtmlGenericControl MemoPar = new HtmlGenericControl("p");
                MemoPar.Attributes.CssStyle.Value = this._textCss;
                MemoPar.Attributes.Add("readonly", "true");
                MemoPar.Style.Add(HtmlTextWriterStyle.TextAlign, "justify");
                MemoPar.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                MemoPar.Style.Add(HtmlTextWriterStyle.BorderColor, "none");
                MemoPar.Style.Add(HtmlTextWriterStyle.MarginTop, "5px");
                MemoPar.Style.Add(HtmlTextWriterStyle.MarginBottom, "5px");
                MemoPar.Style.Add(HtmlTextWriterStyle.MarginLeft, "6px");
                MemoPar.Style.Add(HtmlTextWriterStyle.MarginRight, "10px");
                MemoPar.Style.Add(HtmlTextWriterStyle.BackgroundColor, "transparent");

                double rows = 1;
                double initRows;
                double finalRows;
                double preCol = 0;
                double finalCol = 0;

                if (DataBinder.Eval(context, DataField) != null)
                {
                    System.Text.StringBuilder strText = new System.Text.StringBuilder("");
                    strText = new System.Text.StringBuilder(CffGenGridViewCommon.FormatDataColumn(this._columnType, DataBinder.Eval(context, DataField) == null ? "" : DataBinder.Eval(context, DataField).ToString()));
                    strText.Replace("<br />", " ");

                    if (!string.IsNullOrEmpty(strText.ToString()))
                    {
                        MemoPar.InnerText = strText.ToString();

                        if (strText.Length > 120) //*todo: do a dynamic way of getting the max length
                        {
                            if (strText.Length >= 150)  //*todo: find a way dynamically adjust row column grids based on the length of memo data
                            {
                                initRows = ((double) (strText.Length/130.00));
                                finalRows = Math.Round(initRows, 2) + 1;
                                rows = finalRows;
                                string rowDec = rows.ToString();
                                string rowsFDec = rowDec.Substring(rowDec.Length - 1, 1);
                                int rowDisp = int.Parse(rowsFDec);
                                if (rowDisp > 0) rows = rows + 1;
                                preCol = (strText.Length / finalRows);
                                finalCol = (preCol + (130 - preCol)) - 10;
                                MemoPar.Attributes.Add("cols", strText.Length >= 120 ? "130" : finalCol.ToString());
                                //MemoPar.Attributes.Add("cols", finalCol.ToString());
                            }
                            else
                            {
                                MemoPar.Attributes.Add("cols", "115");
                                rows = ((int) (strText.Length/115)) + 1; 
                            }
                        }
                        else
                        {
                            MemoPar.Attributes.Add("cols", "120");
                            rows = ((int)(strText.Length) / 120);   // 50 - dbb
                        }

                        if (rows <= 0) rows = 1;
                        MemoPar.Attributes.Add("rows", rows.ToString());
                    }
                    else {
                        MemoPar.Attributes.Add("rows", "0");
                        MemoPar.Attributes.Add("cols", "140");
                    }
                }
                else
                {
                    MemoPar.Attributes.Add("rows", "0");
                    MemoPar.Attributes.Add("cols", "140");
                }
                MemoPar.Attributes.Add("wrap", "hard");   // dbb [20160804]
                MemoPar.Style.Add(HtmlTextWriterStyle.FontSize, "12px");
                MemoPar.Style.Add(HtmlTextWriterStyle.FontFamily, "Tahoma, Arial, Helvetica, sans-serif");
                MemoPar.Style.Add(HtmlTextWriterStyle.Overflow, "hidden");
                //if (rows < 6) { MemoPar.Style.Add(HtmlTextWriterStyle.Overflow, "hidden"); }  //dbb
                divLeft.Controls.Add(MemoPar);
                //divLeft.Style.Add(HtmlTextWriterStyle.Width, "100%");
            }
        }
    }


    public class GridViewBooleanTemplate : CffGenGridViewTemplate
    {
        public GridViewBooleanTemplate(CffGridViewColumnType theColumnType, string columnName, string columnHeader = "",
                                            string colWidth = "100%", string textCss = "cffGGV_CellParagraph", bool readOnly = false) :
            base(theColumnType, columnName, columnHeader, colWidth, textCss, readOnly)
        {

        }

        public override void InstantiateIn(Control container)
        {
            base.InstantiateIn(container);
        }

        protected override void divLeft_DataBinding(object sender, EventArgs e)
        {
            CffGenGridView gV = null;
            GridViewRow gvRow = null;

            HtmlContainerControl divLeft = (HtmlContainerControl)sender;
            divLeft.Style.Add(HtmlTextWriterStyle.Width, base._columnWidth);
            divLeft.Style.Add(HtmlTextWriterStyle.Padding, "2px 2px 2px 2px");
            divLeft.Style.Add(HtmlTextWriterStyle.Margin, "2px 2px 2px 2px");

            var context = DataBinder.GetDataItem(divLeft.NamingContainer);

            if ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffGenGridView)(divLeft.NamingContainer.BindingContainer) != null)
            {
                gV = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffGenGridView)(divLeft.NamingContainer.BindingContainer);
                gvRow = ((System.Web.UI.WebControls.GridViewRow)(divLeft.Controls[0].BindingContainer));
            }

            CheckBox CBox = new CheckBox();
            CBox.Width = Unit.Pixel(25);
            CBox.Height = Unit.Pixel(25);

            if (DataBinder.Eval(context, DataField) != null)
                CBox.Checked = Convert.ToBoolean(DataBinder.Eval(context, DataField));
            else
                CBox.Checked = false;

            if ((gV != null) && gV.IsInEditMode && (gV.EditIndex == gvRow.RowIndex))
                CBox.Enabled = true;
            else
                CBox.Enabled = false;

            CBox.CheckedChanged += CBox_CheckedChanged;
            CBox.Visible = true;
            divLeft.Controls.Add(CBox);
        }

        void CBox_CheckedChanged(object sender, EventArgs e)
        {
            //int x = 0;
            //throw new NotImplementedException();
        }
    }

}

/* for queries contact author: M.Santiago */