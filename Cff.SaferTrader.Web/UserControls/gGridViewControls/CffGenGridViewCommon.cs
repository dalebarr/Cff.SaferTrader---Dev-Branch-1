using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel;

using System.Web;
using System.Web.UI.WebControls;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;


namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{
    //following is used by CffGenericGridView exclusively - please refer to mariper before modifying thanks
    /********************/
    /**  Common Enums  **/
    /********************/

    /// <summary>
    /// Represents additional custom paging modes
    /// <para>sample usage:  (CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.PreviousNext) => displays Rows,Page and Previous and Next Buttons on Pager</para>
    /// <para>Values: None = no custom pager settings set</para>
    /// <para>Rows = Display Rows drop down selection list</para>
    /// <para>Page = Display Page number</para>
    /// <para>PreviousNext = Display Previous and Next buttons</para>
    /// <para>FirstLast = Display First and Last Buttons</para>
    /// <para>Top = Display custom pager on top of gridview</para>
    /// <para>Bottom = Display custom pager on bottom of gridview</para>
    /// </summary>
    [Flags]
    public enum CffCustomPagerMode
    {
        None = 0, 
        Rows = 2,
        Page = 4,
        PreviousNext = 8,
        FirstLast= 16,
        Top = 32,
        Bottom= 64,
        WithExport = 128
    }

 
   
    /// <summary>
    /// Cff Generic Grid View Button Types
    /// Use CffGenGridView.InsertCommandButton with these flags 
    /// (use XOR to put multiple command buttons inside column with exceptions noted below)
    /// </summary>
    [Flags]
    public enum CffGridViewButtonType
    {
        Add = 1,
        Edit = 2,                 //Note: Edit cannot be xored with Update
        Delete = 4,
        Cancel = 8,
        Update = 16,              //Note: Update cannot be xored with Edit
        New = 32,                 //Note: New cannot be xored with Image
        Bound = 64,               //button that is bound to a datafield - default display is text
        Image = 128,              //button that displays an image; note that Image button type cannot be xored with New

        //Following are specific for CFF usage
        Image_batch = 256,        //button that displays a view batch image
        Image_retention = 512,    //button that displays a view retention image 
        Image_estimate = 1024     //button that displays a view estimate image  
    }

    /// <summary>
    /// Cff Generic Grid View Footer Modes
    /// </summary>
    [Flags]
    public enum CffCustomFooterMode
    {
        None = 0,
        ShowTotals = 2,
        ShowSummary = 4,
        DefaultSettings = 8,       //default settings fors totals and/or footer summary
        UserDefinedSettings = 16   //user defained settings for totals and/or footer summary
    }


    /// <summary>
    /// Used for custom pager settings
    /// </summary>
    public enum CffRowsPerPageIncrement
    {
        PageSizeIncrement = 0,
        DefaultPageSizeIncrement = 1
    }


    /// <summary>
    /// Cff Generic Grid View New Row Position
    /// </summary>
    public enum CffGridViewNewRowPosition
    {
        Top = 0,
        Bottom = 1
    }

    /// <summary>
    /// Cff Generic Grid View Column Types
    /// </summary>
    public enum CffGridViewColumnType
    {
        Text = 1,                   //text format data column
        Currency = 2,               //currency format data column (2 decimal)
        Date = 3,                   //date format data column
        Time = 4,                   //time format data column
        DateTime = 5,               //dattime format data column 
        Memo = 6 ,                  //memo format data column
        Boolean = 7,                //boolean data column
        HyperLink = 8,              //HyperLink data column
        Dropdown = 9                //Dropdown data column
    }

    /// <summary>
    /// <para>Use this setting for Editing Rows:</para>
    /// <para>InLine         => Edit row data inside cell</para>
    /// <para>EditForm       => Edit row data in a form embedded inside the gridview with data row hidden</para>
    /// <para>PopupEditForm  => Edit row data in a popup form</para>
    /// <para>EditFormAndDisplayRow => Edit row data in a form embedded inside the gridview with data row displayed</para>
    /// </summary>
    public enum CffGridViewEditingMode
    {
        NotSet = 0,
        InLine = 1,
        EditForm = 2,
        PopupEditForm = 3,
        EditFormAndDisplayRow = 4,
        GroupByEditingForm=5
    }


    /// <summary>
    ///  Cff Generic Grid View Popup Form Horizontal Alignment Settings
    /// </summary>
    public enum CffGridViewPopupHorizontalAlign
    {
        NotSet = 0,
        OutsideLeft = 1,
        LeftSides = 2,
        Center = 3,
        RightSides = 4,
        OutsideRight = 5,
        WindowCenter = 6,
    }


    /// <summary>
    /// Cff Generic Grid View Group By States
    /// </summary>
    public enum CffGroupByState
    {
        Init = 0,
        Grouping = 1,
        Grouped = 2
    }


    /// <summary>
    /// used by cff generic grid view only
    /// </summary>
    public enum GridNestingState
    {
        None = -1,
        Init = 0,
        Nesting = 1,
        Nested = 2,
    }


    /**************************/
    /**  Common Structures  **/
    /*************************/
    
    /// <summary>
    /// use this setting when loading images on gridview
    /// </summary>
    public struct CffImageStyleSettings
    {
        public Unit ImageWidth;
        public Unit ImageHeight;
    }

    /// <summary>
    /// set this setting when editing in Form mode
    /// </summary>
    public struct CffEditColumnSettings
    {
        public int ColumnsPerRow;
        public Unit SizePerColumn;
        public HorizontalAlign HorizontalAlignment;
    }

    /// <summary>
    /// set this setting when enabling groupby on grid view 
    /// </summary>
    public struct CffGroupBySettings
    {
        public string GroupByExpression; //columnname separated by comma (eg: Customer, Date)
        public string OrderByExpression; //set when group expression is more than 1 column
        
        public string PreviousColumnValue;
        public int SpannedColumnRow;
        
        public bool IsGroupedByColumn;
        public CffGroupByState State;
    }


    /// <summary>
    /// custom pager grouped by settings
    /// </summary>
    public struct CffCustomPagerGroupSettings
    {
        public int PageCount;
        public int RowCounter;
        public int GroupedRowCount;

    }

   
    /// <summary>
    /// caption header settings
    /// </summary>
    public struct CffCaptionHeaderSettings
    {
        public string CssStyle;
        public bool BoldCaption;
        public HorizontalAlign HorizontalAlignment;
        public VerticalAlign VerticalAlignment;
    }


    //ref2 mariper - see if we can support multiple nesting levels
    public struct CffNestedGridSettings
    { //currently supports 1 level nesting only
        public bool   Enabled;
        public bool   Expanded;

        public GridNestingState State;
        public int    RowIndex;
  
        public CffGenGridView childGrid;
        public Unit   ChildTableWidth;
        public bool   isDisplayChildrenInPanel;
        
        public Unit   ExpandingButtonWidth;
        public Unit   ExpandingButtonHeight;
        public Unit   ExpandingColumnWidth;
        public string ExpandingButtonCssStyle;
        public string ExpandingButtonAltText;

        public string BoundColumnName;           //populate this if you want the expanding buttons bound to a field
        public object BoundColumnFilterObject;   //populate this if you want to set object filters for the bound field
        public object BoundColumnFilterValue;    //populate this to set the object filter value
    }


    /// <summary>
    /// Totals Settings - used by cff generic grid view only
    /// </summary>
    public struct CffGenericTotalsStruct
    {
        public string cssStyle;
        public string text;
        public Dictionary<int, string> ColumnsPool;   //column index, column name
        public Dictionary<string, string> CssStylePool;
    }

    /// <summary>
    /// Cff Generic GridView Update Structure - holds the grid's updated values 
    /// </summary>
    [Serializable]
    public struct CffGVUpdStruct
    {
        public Type type;
        public String value;
        public String name;
    }

    /// <summary>
    /// Cff Generic Grid View View State Values - stores grid's viewstate values
    /// </summary>
    public struct CffGVViewStateValues
    {
        public String name;
        public String value;
    }

    /********************/
    /** Common Classes **/
    /********************/

    /// <summary>
    /// Command Event Arguments - used by cff generic grid view only
    /// </summary>
    public class CffCommandEventArgs : CommandEventArgs
    {
        public string CommandArgs { get; set; }
        public string CommandTag { get; set; }

        public CffCommandEventArgs(string commandName, string argument)
            : base(commandName, argument)
        {
        }
    }

    /// <summary>
    /// Command Event Arguments Extended - used by cff generic grid view only
    /// </summary>
    public class CffGridViewCommandEventArgs : GridViewCommandEventArgs
    {
        public string CommandArgs { get; set; }
        public string CommandTag { get; set; }

        public CffGridViewCommandEventArgs(object commandSource, CommandEventArgs e)
            : base(commandSource, e)
        {
        }
    }


    /// <summary>
    /// Totals Summary Settings - used by cff generic grid view only
    /// </summary>
    public  class CffTotalsSummary
    {
        private CffGenericTotalsStruct _columnTotals;
        private CffGenericTotalsStruct _summaryTotals;
        private int _totalsSumRows;

        public string TableClass;
        
        public int TotalsSummaryRows 
        {
            set { this._totalsSumRows = value; }
        }

        public CffGenericTotalsStruct ColumnTotals
        {
            get { return this._columnTotals; } 
        }

        public CffGenericTotalsStruct SummaryTotals
        {
            get { return this._summaryTotals; }
        }

        public bool Enabled {
            get { return (this._columnTotals.ColumnsPool==null)?false:(this._columnTotals.ColumnsPool.Count==0)?false:true; }
        }

        public  string TotalsText  {
            get { return this._columnTotals.text;  }
            set { this._columnTotals.text = value; } 
        }

        public  string TotalsTextStyle
        {
            get { return this._columnTotals.cssStyle; }
            set { this._columnTotals.cssStyle = value; }
        }

        public string TotalColumnsRowStyle
        {
            get { return this._columnTotals.cssStyle; }
            set { this._columnTotals.cssStyle = value; }
        }

        /// <summary>
        /// If TotalsSummaryRows > 1, Seperate totals summary text with comma
        /// </summary>
        public string TotalsSummaryText
        {
            get { return this._summaryTotals.text;  }
            set { this._summaryTotals.text = value; }
        }

        public string TotalsSummaryTextStyle
        {
            get { return this._summaryTotals.cssStyle; }
            set { this._summaryTotals.cssStyle = value; }
        }

       
        public  CffTotalsSummary(string totalsText="Total",
                                    string totalsSummaryText="Total Summary")
        {
            TotalsText = totalsText;
            TotalsSummaryText = totalsSummaryText;
            TotalsTextStyle = "";
            TotalsSummaryTextStyle = "";

            this._columnTotals.ColumnsPool = new Dictionary<int, string>();
            this._columnTotals.CssStylePool = new Dictionary<string, string>();

            this._summaryTotals.ColumnsPool = new Dictionary<int, string>();
            this._summaryTotals.CssStylePool = new Dictionary<string, string>();
        }

        public void SetColumnTotals(string TheTotalsColumnsSeparatedByComma)
        {
            string[] strDummy = TheTotalsColumnsSeparatedByComma.Split(',');
            int colIdx = 0;
            if (this._columnTotals.ColumnsPool == null) {
                this._columnTotals.ColumnsPool = new Dictionary<int, string>();
            } 
            else 
                this._columnTotals.ColumnsPool.Clear();
            foreach (string s in strDummy)
            {
                this._columnTotals.ColumnsPool.Add(colIdx, s.Trim());
                colIdx++;
            }
        }

        public void SetSummaryTotals(string TheSummaryColumnTotalsSeparatedByComma)
        {
            string[] strDummy = TheSummaryColumnTotalsSeparatedByComma.Split(',');
            int colIdx = 0;
            if (this._summaryTotals.ColumnsPool == null)
            {
                this._summaryTotals.ColumnsPool = new Dictionary<int, string>();
            }
            else    
                this._summaryTotals.ColumnsPool.Clear();
            foreach (string s in strDummy)
            {
                this._summaryTotals.ColumnsPool.Add(colIdx, s.Trim());
                colIdx++;
            }
        }

        public void SetTotalsColumnCssStyle(string columnName, string columnStyle)
        {
            if (this._columnTotals.CssStylePool == null)
                this._columnTotals.CssStylePool = new Dictionary<string, string>();

            try {
                string style;
                if (this._columnTotals.ColumnsPool.Count > 0)
                {
                    if (this._columnTotals.CssStylePool.TryGetValue(columnName, out style))
                        this._columnTotals.CssStylePool.Remove(columnName.Trim());
                }

                this._columnTotals.CssStylePool.Add(columnName, columnStyle.Trim());
            } catch {}
        }

        public void SetSummaryTotalColumnCssStyle(string columnName, string columnStyle)
        {
            if (this._summaryTotals.CssStylePool == null)
                this._summaryTotals.CssStylePool = new Dictionary<string, string>();

            try
            {
                string style;
                if (this._summaryTotals.ColumnsPool.Count > 0)
                {
                    if (this._summaryTotals.CssStylePool.TryGetValue(columnName, out style))
                        this._summaryTotals.CssStylePool.Remove(columnName.Trim());
                }

                this._summaryTotals.CssStylePool.Add(columnName, columnStyle.Trim());
            }
            catch { }
        }
    }

    /// <summary>
    /// CFF Generic Grid View Editing Settings
    /// </summary>
    public class CffGVEditingSettings
    {
        public CffGridViewNewRowPosition NewItemRowPosition;
        public CffGridViewEditingMode Mode;

        public bool ShowUpdateButtonOnEdit { get; set; }
        public bool ShowCancelButtonOnEdit { get; set; }
        public bool ShowDeleteButtonOnEdit { get; set; }
        
        [Description("Sets the column settings in the Edit Form.")]
        public CffEditColumnSettings ColumnSettings;
        
        [Description("Gets or sets the maximum size of columns allowed in Edit Form")]
        [DefaultValue(25)]
        [NotifyParentProperty(true)]
        public int EditFormColumnSize { get; set; }

        [Description("Gets or sets the maximum number of columns allowed in the Edit Form.")]
        [DefaultValue(2)]
        [NotifyParentProperty(true)]
        public int EditFormColumnCount { get; set; }


        [Description("Gets or sets the Editing Form's height.")]
        [DefaultValue(typeof(Unit), "")]
        [NotifyParentProperty(true)]
        public Unit EditingFormHeight { get; set; }

        //Popup Form Settings
        [DefaultValue(typeof(Unit), "")]
        [Description("Gets or sets the Popup Edit Form's height.")]
        [NotifyParentProperty(true)]
        public Unit PopupEditFormHeight { get; set; }

        [Description("Gets or sets the popup edit form's horizontal alignment.")]
        [NotifyParentProperty(true)]
        public CffGridViewPopupHorizontalAlign PopupEditFormHorizontalAlign { get; set; }

        [DefaultValue(0)]
        [Description("Gets or sets the offset from the left or right border of the popup edit form's container.")]
        [NotifyParentProperty(true)]
        public int PopupEditFormHorizontalOffset { get; set; }

        [NotifyParentProperty(true)]
        [Description("Gets or sets whether the Popup Edit Form is displayed as a modal dialog.")]
        [DefaultValue(false)]
        public bool PopupEditFormModal { get; set; }

        [NotifyParentProperty(true)]
        [Description("Gets or sets whether the Popup Edit Form's header is displayed.")]
        [DefaultValue(true)]
        public bool PopupEditFormShowHeader { get; set; }

        [NotifyParentProperty(true)]
        [DefaultValue(typeof(Unit), "")]
        [Description("Gets or sets the Popup Edit Form's width.")]
        public Unit PopupEditFormWidth { get; set; }
    }


    /// <summary>
    /// CFF Gen Grid View Common Static Methods 
    /// </summary>
    public static class CffGenGridViewCommon
    {
        public static string FormatDataColumn(CffGridViewColumnType colType, string Data)
        {
            string colData = Data;
            try
            {
                switch (colType)
                {
                    case CffGridViewColumnType.Currency:
                        decimal value = Convert.ToDecimal(Data);
                        colData = String.Format("{0:C}", value);
                        break;

                    case CffGridViewColumnType.Date:
                        System.DateTime dt = Convert.ToDateTime(Data);
                        colData = dt.ToString("dd/MM/yyyy");    // dt.ToString("mm/dd/yyyy"); //dbb
                        break;

                    case CffGridViewColumnType.DateTime:
                        System.DateTime dttm = Convert.ToDateTime(Data);
                        colData = dttm.ToString("dd/MM/yyyy h:mm tt");      //dttm.ToString("mm/dd/yyyy H:mm:ss zzz");  //dbb

                        break;

                    case CffGridViewColumnType.Text:
                        break;
                    case CffGridViewColumnType.Memo:
                        colData = Data.Replace("<br />", " ");  //todo: for some reason the next line break in this field causes the grid to throw an exception; we replace next line with ' ' for now
                        break;
                    default:
                        //todo :: add other data format handlers here
                        break;
                }
            }
            catch { }
            return colData;
        }

        public static int GetGridViewColumnIndex(DataControlFieldCollection Columns, string DataColumnName)
        {
            int idx = -1;
            
            foreach (DataControlField field in Columns)
            {
                if (field.GetType() == Type.GetType("System.Web.UI.Controls.BoundField"))
                {
                    if (((BoundField)field).DataField == DataColumnName)
                    {
                        idx = Columns.IndexOf(field);
                        break;
                    }
                }
            }

            return idx;
        }

        //todo: tbdw mariper: find a way to optimize this
        public static object GetObjectValue(object o, string propertyName)
        {
            object propValue = null;

            Type oType = o.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(oType.GetProperties());

            //begin search for the propertyValue
            foreach (PropertyInfo prop in props)
            { 
                if (((System.Reflection.MemberInfo)(prop)).Name.ToLower() == propertyName.ToLower().ToLower())
                {
                    propValue = prop.GetValue(o, null);
                    break;
                }
            }

            Type obType = o.GetType().BaseType;
            IList<PropertyInfo> baseProps = new List<PropertyInfo>(obType.GetProperties());
            if (propValue == null)
            { //try finding it on the base object   
                foreach (PropertyInfo prop in baseProps)
                {
                    if (((System.Reflection.MemberInfo)(prop)).Name == propertyName)
                    {
                        propValue = prop.GetValue(o, null);
                        break;
                    }
                }
            }

            return propValue;
        }
    }



    /********************/
    /** Misc **/
    /********************/

    //MSarza -- added block
    /// <summary>
    /// emailReceipt 
    /// </summary>
    public enum EmailReceiptType
    {
        Never = 0,
        Always = 1,
        Inherited = 2
    }






}

/*for queries contact author: Mariper Santiago*/