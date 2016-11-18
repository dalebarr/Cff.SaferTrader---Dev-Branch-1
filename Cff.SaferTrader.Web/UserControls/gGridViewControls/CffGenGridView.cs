using System;
using System.Linq;
using System.Web;

using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Web.UserControls;
using GenericGridViewHelper;

using System.ComponentModel;
using System.Drawing.Design;
using System.Collections.Specialized;

using System.Text.RegularExpressions;

//customized exclusively for cff functionalities - please ref mariper before modifying this class thanks
namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{ 
    [Designer("System.Web.UI.Design.WebControls.GridViewDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [SupportsEventValidation]
    public partial class CffGenGridView : System.Web.UI.WebControls.GridView, IPrintableView
    {
        protected ImageButton btnAddNew;
        protected ImageButton btnCancel;
        protected ImageButton btnUpdate;

#region "Properties"
        public bool EnableRowSelect
        {
            get
            {
                bool ret = false;
                Object obj = this.ViewState["EnableRowSelect"];
                if (obj != null)
                    ret = (bool)obj;

                return ret;
            }

            set
            {
                this.ViewState["EnableRowSelect"] = value;
            }
        }

        public string RowCssClass 
        { 
             get 
             { 
               string rowClass = (string)ViewState["rowClass"]; 
               if (!string.IsNullOrEmpty(rowClass)) 
                 return rowClass; 
               else 
                 return string.Empty; 
             } 
             set 
             { 
               ViewState["rowClass"] = value; 
             } 
        } 

        public string HoverRowCssClass 
        { 
             get 
             { 
               string hoverRowClass = (string)ViewState["hoverRowClass"]; 
               if (!string.IsNullOrEmpty(hoverRowClass)) 
                 return hoverRowClass; 
               else 
                 return string.Empty; 
             } 
             set 
             { 
               ViewState["hoverRowClass"] = value; 
             } 
        } 

        public bool EnableCellClick
        {
            get
            {
                bool ret = false;
                Object obj = this.ViewState["EnableCellClick"];
                return (obj == null ? ret : (bool)obj);
            }

            set
            {
                this.ViewState["EnableCellClick"] = value;
            }
        }

        public int FocusedRowIndex { 
            get {
                int ret = 0;
                Object obj = this.ViewState["FocusedRowIndex"];
                int.TryParse((obj==null)?"-1":obj.ToString(), out ret);
                return ret;
            }

            set {
                this.ViewState["FocusedRowIndex"] = value;
            } 
        }

        public int DefaultPageSize
        {
            get
            {
                int ret = 0;
                Object obj = this.ViewState["DefaultPageSize"];
                int.TryParse((obj == null) ? "-1" : obj.ToString(), out ret);
                return ret;
            }

            set
            {
                this.ViewState["DefaultPageSize"] = value;
            }
        }

       
        /// <summary>
        /// use this when you want to enable/disable datagrid
        /// </summary>
        new public bool Enabled
        {
            get { return this.Enabled; }
            set { this.Enabled = value; }
        }


        /// <summary>
        /// <para>True when gridview is grouped by column</para>
        /// <para>Note: AllowSorting needs to be true for this to work with call to Sort() (see Notes.aspx)</para>
        /// </summary>
        public bool AllowGroupBy
        {
            get
            {
                Object obj = this.ViewState["AllowGroupBy"];
                return (obj == null ? false : (bool)obj);
            }

            set
            {
                this.ViewState["AllowGroupBy"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the columns in the grid can be re-sized in the UI
        /// </summary>
        /// <value><c>true</c> if  column resizing is allowed; otherwise, <c>false</c>.</value>
        public bool AllowColumnResizing
        {
            get
            {
                object o = this.ViewState["AllowColumnResizing"];
                return (o == null ? false : (bool)o);
            }
            set
            {
                this.ViewState["AllowColumnResizing"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the highlight colour for the row
        /// </summary>
        public System.Drawing.Color RowStyleHighlightColour
        {
            get
            {
                object o = this.ViewState["RowStyleHighlightColour"];
                return (o == null ? System.Drawing.Color.Empty : (System.Drawing.Color)o);
            }
            set
            {
                this.ViewState["RowStyleHighlightColour"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a sort graphic is shown in column headings
        /// </summary>
        /// <value><c>true</c> if sort graphic is displayed; otherwise, <c>false</c>.</value>
        public bool EnableSortGraphic
        {
            get
            {
                object o = this.ViewState["EnableSortGraphic"];
                return (o == null ? false : (bool)o);
            }
            set
            {
                this.ViewState["EnableSortGraphic"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the sort ascending image when <see cref="EnableSortGraphic"/> is <c>true</c>
        /// </summary>
        public string SortAscendingImage
        {
            get
            {
                object o = this.ViewState["SortAscendingImage"];
                return (o == null ? Page.ClientScript.GetWebResourceUrl(GetType(), Cff.SaferTrader.Web.App_GlobalResources.Cff_WebResource.ArrowUpImage) : (string)o);
            }
            set
            {
                this.ViewState["SortAscendingImage"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the sort descending image <see cref="EnableSortGraphic"/> is <c>true</c>
        /// </summary>
        public string SortDescendingImage
        {
            get
            {
                object o = ViewState["SortDescendingImage"];
                return (o == null ? Page.ClientScript.GetWebResourceUrl(GetType(), Cff.SaferTrader.Web.App_GlobalResources.Cff_WebResource.ArrowDownImage) : (string)o);
            }
            set
            {
                this.ViewState["SortDescendingImage"] = value;
            }
        }

        /// <summary>
        /// set sort expression -- use this when you want to issue default sorting or issue sort calls outside asp 
        /// </summary>
        public string SetSortExpression
        {
            get
            {
                object o = this.ViewState["SortExpression"];
                return (o == null ? "" : (string)o);
            }

            set { this.ViewState["SortExpression"] = value; }
        }

        /// <summary>
        /// Set Sort Direction as System.Web.UI.WebControls.SortDirection
        /// </summary>
        public SortDirection SetSortDirection
        {
            get
            {
                object o = this.ViewState["SortDirection"];
                return (o==null?SortDirection.Ascending:(SortDirection)o);
            }

            set { this.ViewState["SortDirection"] = value; }
        }

        /// <summary>
        /// Set when gridview contains images
        /// </summary>
        public bool IsImageContent
        {
            get
            {
                object o = this.ViewState["ImageContent"];
                return (o == null ? false : (bool)o);
            }
            set
            {
                this.ViewState["ImageContent"] = value;
            }
        }


        /// <summary>
        /// True when gridview is in add mode
        /// </summary>
        public bool IsInAddMode
        {
            get
            {
                object o = this.ViewState["IsInAddMode"];
                return (o == null ? false : (bool)o);
            }
            set
            {
                this.ViewState["IsInAddMode"] = value;
            }
        }

        /// <summary>
        /// True when gridview is in edit mode
        /// </summary>
        public bool IsInEditMode
        {
            get
            {
                object o = this.ViewState["IsInEditMode"];
                return (o == null ? false : (bool)o);
            }
            set
            {
                this.ViewState["IsInEditMode"] = value;
            }
        }

        public bool IsInUpdateMode
        {
            get
            {
                object o = this.ViewState["IsInUpdateMode"];
                return (o == null ? false : (bool)o);
            }
            set
            {
                this.ViewState["IsInUpdateMode"] = value;
            }
        }

        public bool IsUpdated
        {
            get
            {
                object o = this.ViewState["IsUpdated"];
                return (o == null ? false : (bool)o);
            }
            set
            {
                this.ViewState["IsUpdated"] = value;
            }
        }

        
        public bool IsCancelingEdit
        {
            get
            {
                object o = this.ViewState["IsCancelingEdit"];
                return (o == null ? false : (bool)o);
            }
            set
            {
                this.ViewState["IsCancelingEdit"] = value;
            }
        }

        /// <summary>
        /// True when gridview is in edit mode
        /// </summary>
        public bool IsRowEditing
        {
            get
            {
                object o = this.ViewState["IsRowEditing"];
                return (o == null ? false : (bool)o);
            }
            set
            {
                this.ViewState["IsRowEditing"] = value;
            }
        }

        /// <summary>
        /// True when gridview is editing new row
        /// </summary>
        public bool IsNewRowEditing
        {
            get
            {
                object o = this.ViewState["IsNewRowEditing"];
                return (o == null ? false : (bool)o);
            }
            set
            {
                this.ViewState["IsNewRowEditing"] = value;
            }
        }

        /// <summary>
        /// Row Count
        /// </summary>
        public int RowCount
        {
            get
            {
                if (this.DataSource == null)
                    return 0;

                IEnumerable origDataSource = (IEnumerable)this.DataSource;
                return ((origDataSource).Cast<object>().Count());
            }
        }


        public int CurrentPageIndex
        {
            get
            {
                object o = this.ViewState["CurrentPageIndex"];
                return (o == null ? 0 : (int)o);
            }
            set
            {
                this.ViewState["CurrentPageIndex"] = value;
            }
        }


        /// <summary>
        /// true if View All Button is required
        /// </summary>
        public bool IsViewAllButtonRequired
        {
            get
            {
                if (this.DataSource != null)
                    return (this.RowCount > this.PageSize);
                else
                    return false;
            }
        }


        /// <summary>
        /// true if row command postback
        /// </summary>
        public bool IsRowCommandPostBack
        {
            get
            {
                object o = this.ViewState["IsRowCommandPostBack"];
                return (o == null ? false : (bool)o);
            }
            set
            {
                this.ViewState["IsRowCommandPostBack"] = value;
            }
        }

        /// <summary>
        /// Use this for bagging needed objects to grid (such as saving object state/grid/data parameters  for later use)
        /// </summary>
        public object GridBag
        {
            get
            {
                object o = this.ViewState["GridBag"];
                return o;
            }

            set { this.ViewState["GridBag"] = value; }
        }


        /// <summary>
        /// Use this for retrieving the (unposted) edited values
        /// </summary>
        public object UpdatingValues
        {
            get
            {
                object o = this.ViewState["UpdateValues"];
                return o;
            }
        }

        //TODODO -- refer to mariper!!-- mariper can't remember why
        public object UpdatingCellHistory
        {
            get
            {
                object o = this.ViewState["UpdatingCellHistory"];
                return o;
            }
        }


        /// <summary>
        /// used and populated at grid nesting
        /// </summary>
        public int ExpandedRowIndex
        {
            get
            {
                object o = ViewState["ExpandedRowIndex"];
                return (o == null ? -1 : Convert.ToInt32(o));
            }

            set { this.ViewState["ExpandedRowIndex"] = value; }
        }

        public string PagerCommandSource
        {
            get
            {
                object o = ViewState["PagerCommandSource"];
                return (o == null ? "default" : o.ToString());
            }

            set { this.ViewState["PagerCommandSource"] = value; }
        }


        /// <summary>
        /// used and populated at grid nesting
        /// </summary>
        private object BoundPool
        {
            get
            {
                object o = this.ViewState["BoundPool"];
                return o;
            }

            set { this.ViewState["BoundPool"] = value; }
        }

        /// <summary>
        /// used and populated at grid nesting
        /// </summary>
        private object NestedPool
        {
            get
            {
                object o = this.ViewState["NestedPool"];
                return o;
            }

            set { this.ViewState["NestedPool"] = value; }
        }


        /// <summary>
        /// used to store grid's viewstatevalues passed thru postbacks
        /// used to contain List of CffGVViewStateValues
        /// </summary>
        public object ViewStateValues
        {
            get {
                object o = this.ViewState["ViewStateValues"];
                return o;
            }

            set { this.ViewState["ViewStateValues"] = value; }
        }


        public string FileNameRes {

            get {
                object o = this.ViewState["FileNameRes"];
                return ((o==null)?"":o.ToString());
            }

              set { this.ViewState["FileNameRes"] = value; }
        }

 #endregion Properties


#region "PropertySettings"
        /// <summary>
        /// Gets or sets the custom pager settings mode.
        /// </summary>
        public CffCustomPagerMode CustomPagerSettingsMode
        {
            get
            {
                object o = this.ViewState["CustomPagerSettingsMode"];
                return (o == null ? CffCustomPagerMode.None : (CffCustomPagerMode)o);
            }
            set
            {
                this.ViewState["CustomPagerSettingsMode"] = value;
            }
        }


        /// <summary>
        /// Gets or sets the custom pager rows per page increment (used when custompagermode has rows settings)
        /// </summary>
        public CffRowsPerPageIncrement RowsPerPageIncrement
        {
            get
            {
                object o = this.ViewState["RowsPerPageIncrement"];
                return (o == null ? CffRowsPerPageIncrement.PageSizeIncrement : (CffRowsPerPageIncrement)o);
            }
            set
            {
                this.ViewState["RowsPerPageIncrement"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom footer settings and mode.
        /// </summary>
        public CffCustomFooterMode CustomFooterSettings
        {
            get
            {
                object o = this.ViewState["CustomFooterSettings"];
                return (o == null ? CffCustomFooterMode.None : (CffCustomFooterMode)o);
            }
            set
            {
                this.ViewState["CustomFooterSettings"] = value;
            }
        }

        /// <summary>
        /// <para>Set this property when you want the grid to be editable</para>
        /// <para>Note: When InLine Editing  is enabled do not do a databind on page postback when IsInEditMode is true</para>
        /// <para>Todo: Allow editing in popup form </para>
        /// </summary>
        public CffGridViewEditingMode EditingMode
        {
            get
            {
                object o = this.ViewState["GridViewEditingMode"];
                return (o == null ? CffGridViewEditingMode.InLine : (CffGridViewEditingMode)o);
            }

            set { this.ViewState["GridViewEditingMode"] = value; }
        }


        /// <summary>
        ///  CFF GV Editing Settings
        /// </summary>
        public CffGVEditingSettings EditingSettings;

        /// <summary>
        /// Set this when enabling Editing in Form Mode
        /// </summary>
        public CffEditColumnSettings EditColumnSettings;    //TODO: refactor this in CffGVEditingSettings


        /// <summary>
        /// Set this when allowing group by column
        /// </summary>
        public CffGroupBySettings GroupBySettings;

        /// <summary>
        /// enable this for nested grids
        /// </summary>
        /// todo: put this in ViewState so we don't loose values on postbacks
        public CffNestedGridSettings NestedSettings;

        /// <summary>
        /// enable this for grouped by grids
        /// </summary>
        public CffCustomPagerGroupSettings CustomPagerGroupedBySettings;

        /// <summary>
        /// enable this for grids with totals summary
        /// </summary>
        public CffTotalsSummary TotalsSummarySettings;

        /// <summary>
        /// enable this for grids with caption header
        /// </summary>
        public CffCaptionHeaderSettings CaptionHeaderSettings;
        private string _caption;
 #endregion


#region IPrintableViewProperties
        public void Print()
        {
            if (this.PrintEventHandler != null)
                this.PrintEventHandler(this);
        }
#endregion

 #region "DeclaredEvents"
        public delegate void GridViewRowClicked(object sender, CffGridViewRowClickedEventArgs args);
        public delegate void GridViewPrintEvent(object sender);

        private readonly Object RowClickedEventKey = new Object();
        public event GridViewRowClicked RowClicked;
        public event GridViewPrintEvent PrintEventHandler;

        protected virtual void OnRowClicked(CffGridViewRowClickedEventArgs e) 
        { 
             if (RowClicked != null) 
                 RowClicked(this, e); 
        } 

        protected override void RaisePostBackEvent(string eventArgument) 
        { 
             if (eventArgument.StartsWith("rc")) 
             { 
               int index = Int32.Parse(eventArgument.Substring(2)); 
               CffGridViewRowClickedEventArgs args = new CffGridViewRowClickedEventArgs(Rows[index]); 
               OnRowClicked(args); 
             } 
             else 
               base.RaisePostBackEvent(eventArgument); 
         } 

       

         public class GridViewRowClickedEventArgs : EventArgs 
         { 
           private GridViewRow _row; 

           public GridViewRowClickedEventArgs(GridViewRow aRow) 
             : base() 
           { 
             _row = aRow; 
           } 

           public GridViewRow Row 
           { 
             get 
             { return _row; } 
           } 
         } 

        /// <summary>
        /// On Row Created Event Handler
        /// </summary>
        public new GridViewRowEventHandler RowCreated;


        /// <summary>
        /// On Row Command Event Handler
        /// </summary>
        public new GridViewCommandEventHandler RowCommand;

        /// <summary>
        /// Handle before performing grid view edit
        /// </summary>
        public GridViewEditEventHandler OnGridViewRowEditing;

        /// <summary>
        /// Handle before performing grid view update
        /// </summary>
        public GridViewUpdateEventHandler OnGridViewRowUpdating;
      
        /// <summary>
        /// On Row Data Bound Event Handler
        /// </summary>
        public new GridViewRowEventHandler RowDataBound;
        public new GridViewUpdatedEventHandler RowUpdated;
        public GridViewRowEventHandler OnGridViewRenderHeader;

        public event EventHandler RowUpdateSuccess;
        public event GridViewCommandEventHandler PagerCommand;
        public event EventHandler CellEditOrInitialize;
        public event EventHandler SelectionChanged;

        //public new EventHandler OnTextChanged;
       
        public object GetRow(int visibleIndex) {
            if (this.DataSource == null)
                return null;

            if (visibleIndex < this.RowCount)
                return ((IEnumerable)this.DataSource).Cast<object>().ToList()[visibleIndex];
            else 
                return null;
        }


        public int GetRowIndexByValue(string rowVal)
        {
            int rowIndex = -1;
            foreach (GridViewRow gr in this.Rows)
            {
                if (this.DataKeys[gr.RowIndex].Value.ToString().Equals(rowVal))
                {
                    rowIndex = gr.RowIndex;
                    if (this.PageIndex > 0)
                        rowIndex = (rowIndex + (this.PageSize * this.PageIndex));
                    break;
                }
            }
            return rowIndex;
        }

        //public object GetMasterRowKeyValue() { return this.GetMasterRowKeyValue(); }

        public int FindVisibleIndexByKeyValue(object keyValue) 
        {
            var val = ((string[])((keyValue==null)?"":keyValue))[0];
            int kVal = 0;
            if (int.TryParse(val, out kVal))
                return kVal;
            else
                return 0;
        }

        public void UnselectAll() {
            this.SelectedIndex = 0;
            this.FocusedRowIndex = 0;
        }

        public int VisibleRowCount { get; set; }

      
        public string KeyFieldName { get; set; }
        public int VisibleIndex { get; set; }

        #endregion


 #region "Constructor"
        public CffGenGridView() : base()
        {
            //enable this after call to constructor if you want to render a nested grid
            btnAddNew = new ImageButton();
            btnCancel = new ImageButton();
            btnUpdate = new ImageButton();

            this.NestedSettings.Enabled = false;
            this.NestedSettings.State = GridNestingState.Init;
            this.EditingMode = CffGridViewEditingMode.NotSet;
            this.PageIndex = 0;
          
            this.EditingSettings = new CffGVEditingSettings();
            this.EditingSettings.ColumnSettings.ColumnsPerRow = 2;
            this.EditingSettings.EditFormColumnCount = 2;
            this.EditingSettings.EditFormColumnSize = 50;
            this.EditingSettings.EditingFormHeight = Unit.Percentage(100);
            this.RowsPerPageIncrement = CffRowsPerPageIncrement.PageSizeIncrement;
       
            this.TotalsSummarySettings = new CffTotalsSummary();
            this.IsRowCommandPostBack = false;
        }
#endregion

#region "OverRidden Events"
        /// <summary>
        /// Initializes the pager row displayed when the paging feature is enabled.
        /// </summary>
        /// <param name="row">A <see cref="T:System.Web.UI.WebControls.GridViewRow"></see> that represents the pager row to initialize.</param>
        /// <param name="columnSpan">The number of columns the pager row should span.</param>
        /// <param name="pagedDataSource">A <see cref="T:System.Web.UI.WebControls.PagedDataSource"></see> that represents the data source.</param>
        protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        {
            if (this.CssClass == null)
                this.CssClass = "cffGGV";

            if (this.CustomPagerSettingsMode != CffCustomPagerMode.None && row.Cells.Count == 0)
            {
                this.PagerSettings.Mode = PagerButtons.Numeric;
            }

            if (this.DefaultPageSize==-1)
                this.DefaultPageSize = this.PageSize;

            base.InitializePager(row, columnSpan, pagedDataSource);
        }

        private void btnFirst_Command(object sender, CommandEventArgs e)
        {
            string cmd = e.CommandName;
        }

        private void CheckShowPager()
        {
            if (CustomPagerSettingsMode == CffCustomPagerMode.None && !AllowPaging)
                return; //do nothing

            if (CustomPagerSettingsMode != CffCustomPagerMode.None)
            {
                //this.AllowCustomPaging = true;
                this.AllowPaging = true;
                this.PagerSettings.Mode = PagerButtons.Numeric;
            }

            if (this.AllowPaging && CustomPagerSettingsMode == CffCustomPagerMode.None)
            {
                this.PagerSettings.Mode = PagerButtons.NumericFirstLast;
                this.PagerSettings.Position = PagerPosition.Bottom;
            }

            if (CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.Top) && TopPagerRow != null)
                TopPagerRow.Visible = true;
            else
            { //default as bottom pager
                if (BottomPagerRow != null)
                    BottomPagerRow.Visible = true;
            }
        }

        private void SelectDataSourceAndBind(CffGenGridView gvGeneric, IEnumerable orginalDataSource, GridViewSortEventArgs e, IEnumerable sortedDataSource = null)
        {
            if (sortedDataSource != null)
            {
                if (sortedDataSource.GetType().Name != Const.ListTypeName)
                {
                    gvGeneric.DataSource = sortedDataSource.Cast<object>().ToList();
                }
                else
                {
                    gvGeneric.DataSource = sortedDataSource;
                }
            }
            else
            {
                gvGeneric.DataSource = SortingHelper.GetSortedObjects(e, orginalDataSource.Cast<object>(), null);
            }

            gvGeneric.DataBind();
        }

        /// <summary>
        /// Creates the control hierarchy that is used to render a composite data-bound control based on the values that are stored in view state.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            CheckShowPager();
        }

        /// <summary>
        /// Creates the control hierarchy used to render the <see cref="T:System.Web.UI.WebControls.GridView"></see> control using the specified data source.
        /// </summary>
        /// <param name="dataSource">An <see cref="T:System.Collections.IEnumerable"></see> that contains the data source for the <see cref="T:System.Web.UI.WebControls.GridView"></see> control.</param>
        /// <param name="dataBinding">true to indicate that the child controls are bound to data; otherwise, false.</param>
        /// <returns>The number of rows created.</returns>
        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            try
            {
                int i = base.CreateChildControls(dataSource, dataBinding);
                CheckShowPager();
                return i;
            }
            catch (Exception exc)
            {
                string error = exc.Message;
                return 0;
            }
        }


        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        /// 
        
        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                if (AllowColumnResizing && Visible)
                {
                    string vars = String.Format("var _CffGenGridViewId = '{0}';\n", ClientID);

                    if (!Page.ClientScript.IsClientScriptBlockRegistered("CffGenGridView_GridViewVars"))
                        Page.ClientScript.RegisterClientScriptBlock(GetType(), "CffGenGridView_GridViewVars", vars, true);
                    else
                        Page.ClientScript.RegisterClientScriptInclude("CffGenGridView.js",
                                  Page.ClientScript.GetWebResourceUrl(GetType(), "CffGenGridView.WebControls.SharedWebResources.CffGenGridView_GridView_Resize.js"));
                }

              
                base.OnPreRender(e);
            }
            catch (Exception exc) {
                string msg = exc.Message; 
            
            }
        }


        /// <summary>
        /// Raises the System.Web.UI.WebControls.GridView.RowCreated  event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Web.UI.WebControls.GridViewRowEventArgs"></see> that contains event data.</param>
        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            if ((this.Page != null) && (!this.Page.IsPostBack))
            {
                if (EnableSortGraphic && IsImageContent)
                {
                    if (!((e.Row == null)) && e.Row.RowType == DataControlRowType.Header)
                    {
                        foreach (TableCell cell in e.Row.Cells)
                        {
                            if (cell.HasControls())
                            {
                                LinkButton button = ((LinkButton)(cell.Controls[0]));
                                if (!((button == null)))
                                {
                                    Image image = new Image();
                                    image.ImageUrl = "images/default.gif";
                                    image.ImageAlign = ImageAlign.Baseline;
                                    if (SortExpression == button.CommandArgument)
                                    {
                                        image.ImageUrl = SortDirection == SortDirection.Ascending ? SortAscendingImage : SortDescendingImage;
                                        Literal space = new Literal();
                                        space.Text = "&nbsp;";
                                        cell.Controls.Add(space);
                                        cell.Controls.Add(image);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (RowStyleHighlightColour != System.Drawing.Color.Empty)
            {
                if (e.Row != null)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Attributes.Add("onmouseover", String.Format("this.style.backgroundColor='{0}'", System.Drawing.ColorTranslator.ToHtml(RowStyleHighlightColour)));
                        e.Row.Attributes.Add("onmouseout", String.Format("this.style.backgroundColor='{0}'", System.Drawing.ColorTranslator.ToHtml(this.RowStyle.BackColor)));

                        if (this.EnableRowSelect && !this.IsInUpdateMode && !this.IsInEditMode && !this.IsInAddMode)
                        {
                            e.Row.Attributes.Add("onclick", String.Format("this.style.backgroundColor='{0}'", System.Drawing.ColorTranslator.ToHtml(RowStyleHighlightColour)));
                        }

                    }
                }
            }

            if (this.IsInEditMode && this.EditIndex == e.Row.RowIndex)
            {    //store the current value of this row
            }


            base.OnRowCreated(e);

            //let the user do something here
            if (this.RowCreated != null)
                RowCreated(this, e);
        }

  


        /// <summary>
        /// Handle the System.Web.UI.WebControls.GridView.DataBound event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                if (this.CustomPagerSettingsMode != CffCustomPagerMode.None)
                {
                    if (this.IsRowCommandPostBack && this.CurrentPageIndex != this.PageIndex && !this.IsInEditMode && !this.IsInAddMode && !this.IsInUpdateMode) 
                        this.PageIndex = (this.CurrentPageIndex>0)?(this.CurrentPageIndex-1):0;

                    DisplayCustomPager(e);
                }
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                DisplayCustomHeader(e);
            }

            if (e.Row != null && (e.Row.RowType == DataControlRowType.DataRow) && this.IsInEditMode  && (e.Row.RowIndex == this.EditIndex))
            {
                if (this.EditingMode == CffGridViewEditingMode.EditFormAndDisplayRow || this.EditingMode == CffGridViewEditingMode.EditForm)
                    DisplayEditingFormWithDataRow(e);
                else if (this.EditingMode == CffGridViewEditingMode.GroupByEditingForm)
                    DisplayGroupByEditingForm(e);
                else if (this.EditingMode == CffGridViewEditingMode.PopupEditForm)
                    DisplayPopupEditingForm(e);

                //else if inline -> handled by the template data bind
            }

            if (e.Row != null && (e.Row.RowType == DataControlRowType.DataRow) && this.IsInAddMode && (e.Row.RowIndex == this.SelectedIndex))
            {
                if (this.EditingMode == CffGridViewEditingMode.EditFormAndDisplayRow || this.EditingMode == CffGridViewEditingMode.EditForm)
                    DisplayAddNewFormWithDataRow(e);
                else if (this.EditingMode == CffGridViewEditingMode.PopupEditForm)
                    DisplayPopupAddNewForm(e);
                //else if inline -> handled by the template data bind
            }


            if (e.Row != null && (e.Row.RowType == DataControlRowType.DataRow) && this.IsInUpdateMode)
            {
                if ((e.Row.RowIndex == this.EditIndex) && this.EditingMode == CffGridViewEditingMode.EditFormAndDisplayRow || this.EditingMode == CffGridViewEditingMode.EditForm)
                {
                    this.EditIndex -= -1;
                    this.IsInUpdateMode = false;
                    RestoreEditingFormWithDataRow(e);
                }
            }

            else if (e.Row != null && (e.Row.RowType == DataControlRowType.DataRow) && this.IsInUpdateMode && (e.Row.RowIndex == this.EditIndex)) 
            {
                this.IsInUpdateMode = false;
                this.IsUpdated = true;
                //if (this.EditingMode == CffGridViewEditingMode.EditFormAndDisplayRow || this.EditingMode == CffGridViewEditingMode.EditForm)
                //    ClearEditingFormWithDataRow(e);
            }
            else if (e.Row != null && (e.Row.RowType == DataControlRowType.DataRow) && this.AllowGroupBy == true && this.GroupBySettings.State == CffGroupByState.Grouped)
                DisplayRowsInGroupBySettings(e);

           
            if (this.NestedSettings.Enabled && this.NestedSettings.State == GridNestingState.Init)
            {
                this.NestedSettings.State = GridNestingState.Nesting;
                if (e.Row != null && (e.Row.RowType == DataControlRowType.DataRow) && this.IsRowCommandPostBack) { 
                        DisplayNestedGrid(e); 
                }
            }

            if (e.Row != null && (e.Row.RowType == DataControlRowType.DataRow) && this.IsRowCommandPostBack && this.NestedSettings.Enabled && this.NestedSettings.State == GridNestingState.Nested)
                DisplayNestedGrid(e);
            else if (e.Row != null && (e.Row.RowType == DataControlRowType.DataRow) && this.NestedSettings.Enabled) 
                DisplayNestedGridDataAsIs(e);
            
            if (e.Row.RowType == DataControlRowType.Footer && this.ShowFooter)
            {
                if (this.CustomFooterSettings != CffCustomFooterMode.None)
                    DisplayCustomFooter(e);
            }

            //let the user do something here iff event handlers are present
            if (e.Row != null && e.Row.RowType == DataControlRowType.DataRow && this.CellEditOrInitialize != null)
                this.CellEditOrInitialize(this, e);

            if (this.RowDataBound != null)
                RowDataBound(this, e);

            if (this.IsUpdated)
            {
                this.IsUpdated = false;
                this.EditIndex = -1;
                if (this.RowUpdateSuccess != null)
                    RowUpdateSuccess(this, e);
            }

        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            if (this.CustomFooterSettings != CffCustomFooterMode.None)
                this.ShowFooter = true;

            if (this.NestedSettings.Enabled && this.NestedSettings.State == GridNestingState.Init)
            {
                //if (this.Columns[0].GetType().Name == "dd" && !this.IsRowCommandPostBack)
                //    this.NestedSettings.State = GridNestingState.Nesting;
                if (this.Columns[0].GetType().Name != "CffCommandField")
                    EnableExpandingButtons();
            }
        }


        /// <summary>
        /// Handles the Grid's Sorting Event:: to call a sort outside this class
        /// YourDefinedGrid.Sort("SortFieldName", System.Web.UI.WebControls.SortDirection)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSorting(GridViewSortEventArgs e)
        {
            if (this.AllowGroupBy)
            {
                this.GroupBySettings.State = CffGroupByState.Grouping;

                //iterate through the rows+columns and tag each datacolumn groupby settings
                int itr = 0;
                int colCtr = 0;
                string groupByExpression = this.GroupBySettings.GroupByExpression;
                string[] colGroupByExps = new string[] { };

                if (this.GroupBySettings.GroupByExpression.IndexOf(",") > 0)
                    colGroupByExps = this.GroupBySettings.GroupByExpression.Split(',');

                while (colCtr < this.Columns.Count - 1)
                {
                    if ((this.Columns[colCtr]).GetType().Name == "CffTemplateField")
                    {
                        if (colGroupByExps.Length > 0)
                        {
                            if (itr >= colGroupByExps.Length) { itr = 0; }
                            groupByExpression = colGroupByExps[itr];
                            itr++;
                        }

                        if (((CffTemplateField)(this.Columns[colCtr])).DataBoundColumnName == groupByExpression)
                            ((CffTemplateField)(this.Columns[colCtr])).GroupBySettings.IsGroupedByColumn = true;
                    }
                    colCtr++;
                }

                this.GroupBySettings.State = CffGroupByState.Grouped;
            }

            e.SortDirection = this.SetSortDirection;
            e.SortExpression = this.SetSortExpression;
            if (this.DataSource != null)
            {
                IEnumerable origDataSource = (IEnumerable)this.DataSource;
                SelectDataSourceAndBind(this, origDataSource, e);
            }
        }


        /// <summary>
        /// Page Index Changed event handler
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            if (this.CustomPagerSettingsMode != CffCustomPagerMode.None)
            {
                int pageIndex = e.NewPageIndex;
                if ((this.PagerCommandSource != "Numeric") && (this.PagerCommandSource != "GoToPage") && (this.PagerCommandSource != "Last"))  // dbb
                {
                    pageIndex = e.NewPageIndex - 1;
                }

                if (this.PagerCommandSource == "Prev") pageIndex = e.NewPageIndex + 1;  // dbb

                this.PageIndex = (pageIndex < 0 ? 0 : pageIndex == this.PageCount ? this.PageCount : pageIndex);

                CffCommandEventArgs cx = new CffCommandEventArgs("Page", this.PageIndex.ToString());
                if (this.PagerCommand != null)
                    PagerCommand(this, new CffGridViewCommandEventArgs(e, ((CommandEventArgs) cx)));

                if (this.IsRowCommandPostBack)
                {
                    this.GridBag = this.DataSource;
                    this.CurrentPageIndex = this.PageIndex;
                }

                this.DataBind();
            }
            else //for use of default pager
                this.PageIndex = e.NewPageIndex;
        }

        /// <summary>
        /// <para>To Handle this OnRowCommand Event outside of this Generic Grid View Class simply declare a callback event handler</para>
        /// <para>example: yourpage_gridview.RowCommand += yourpage_rowcommand_eventHandler();</para>
        /// </summary>
        /// <param name="e"></param>
        private void UpdateViewStateValues(string viewstateID)
        {
            string gID = this.ID;
            if (viewstateID == "UpdateValues")
            {
                IList<CffGVUpdStruct> xNRPK = new List<CffGVUpdStruct>();
                int colCtr = 0;
                for (int iCtr = 0; iCtr < this.Page.Request.Params.Count; iCtr++)
                {
                    if (this.Page.Request.Params.GetKey(iCtr) != null) {
                        if (this.Page.Request.Params.GetKey(iCtr).Contains(gID))
                        {
                            CffGVUpdStruct xPar;
                            xPar.type = this.Page.Request.Params.GetKey(iCtr).GetType();
                            xPar.value = this.Page.Request.Params[iCtr];

                            if (colCtr < this.Columns.Count)
                            {
                                if (this.Columns[colCtr].GetType().Name == "CffTemplateField")
                                    xPar.name = (this.Columns[colCtr] as CffTemplateField).DataBoundColumnName;
                                else
                                    xPar.name = this.Columns[colCtr].GetType().Name;

                                colCtr++;
                                xNRPK.Add(xPar);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                this.ViewState.Add("UpdateValues", (object)xNRPK);
            }
        }


        /// <summary>
        /// <para>To Handle this OnRowCommand Event outside of this Generic Grid View Class simply declare a callback event handler</para>
        /// <para>example: yourpage_gridview.RowCommand += yourpage_rowcommand_eventHandler();</para>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowCommand(GridViewCommandEventArgs e)
        {
            int rowIndex;
            bool doUserRowCommand = true;
            string cmdArg = "Page";
            this.IsRowCommandPostBack = true;

            switch (e.CommandName)
            {
                case "Edit":
                    {
                        if (!this.IsInEditMode)
                        {
                            rowIndex = Convert.ToInt32(e.CommandArgument);
                            this.SelectedIndex = rowIndex;
                            this.EnableCellClick = false;
                            this.EditRowStyle.BackColor = System.Drawing.Color.LightYellow;
                            this.EditRowStyle.ForeColor = System.Drawing.Color.Navy;

                            this.IsInAddMode = false;
                            this.IsInEditMode = true;
                            this.SetEditRow(rowIndex);
                        }
                        else
                        {
                            doUserRowCommand = false;
                        }
                    }
                    break;

                case "Update":
                    {
                        if (e.CommandArgument.ToString().IndexOf("$") >= 0)
                        {
                            string[] cargs = e.CommandArgument.ToString().Split('$');
                            rowIndex = Convert.ToInt32(cargs[1]);
                        }
                        else
                            rowIndex = Convert.ToInt32(e.CommandArgument);

                        if (this.IsInEditMode)
                        {
                            this.IsInEditMode = false;
                            this.IsInUpdateMode = true;
                            this.IsInAddMode = false;
                            UpdateViewStateValues("UpdateValues");
                        }
                    }
                    break;

                case "New":
                    { //set grid view state to add new row
                        if (this.IsInAddMode)
                        {
                            rowIndex = Convert.ToInt32(e.CommandArgument);
                            this.SelectedIndex = rowIndex;
                            this.EnableCellClick = true;
                            this.EditRowStyle.BackColor = System.Drawing.Color.LightYellow;
                            this.EditRowStyle.ForeColor = System.Drawing.Color.Navy;

                            this.IsInAddMode = false;  // true;  
                            this.IsInEditMode = false;
                            this.SetEditRow(rowIndex);
                            //note: handle add values @rowcommand event handler
                        }
                        else

                            doUserRowCommand = false;  
                    }
                    break;

                case "ADD":
                    {

                        if (e.CommandArgument.ToString().IndexOf("$") >= 0)
                        {
                            string[] cargs = e.CommandArgument.ToString().Split('$');
                            rowIndex = Convert.ToInt32(cargs[1]);
                        }
                        else
                            rowIndex = Convert.ToInt32(e.CommandArgument);
                        
                        if (this.IsInAddMode)
                        {
                            this.IsInEditMode = false;
                            this.IsInUpdateMode = true;
                            UpdateViewStateValues("UpdateValues");

                        }
                    }
                    break;

                case "Select":
                    { 
                    //reuse this to perform other data grid task such as expand/minimize nested grid or route to another page 
                    this.IsInAddMode = false;

                    this.EditIndex = -1;
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    this.SelectedIndex = rowIndex;
                    this.IsInUpdateMode = false;

                    if (this.NestedSettings.Enabled && this.PageIndex > 0)
                    { //user must've clicked on nextpage/gotopage
                        rowIndex = (this.PageSize*this.PageIndex) + rowIndex;
                    }

                    bool isExpanded = isNestedRowExpanded(rowIndex);
                    if (this.IsInEditMode==false && this.IsInAddMode==false && this.NestedSettings.Enabled && this.NestedSettings.State == GridNestingState.Nesting)
                    {
                        if (rowIndex != this.ExpandedRowIndex)
                        { //just clear out previously expanded row as we don't want to retain too many child datasources on viewstate/gridbag
                            if (this.ExpandedRowIndex >= 0)
                            {
                                this.RemoveExpandedIndex(this.ExpandedRowIndex);
                                this.ExpandedRowIndex = rowIndex;
                                this.SetNestedRowExpanded(rowIndex, false);
                            }
                            else 
                            {
                                string strExpandButtons = this.Columns[0].GetType().Name;

                                if (this.DataSource == null)
                                    this.DataSource = this.GridBag;
                                isExpanded = isNestedRowExpanded(rowIndex);
                                this.ExpandedRowIndex = rowIndex;
                            }
                        }
                        else
                            isExpanded = isNestedRowExpanded(rowIndex);

                        this.NestedSettings.RowIndex = rowIndex;
                        if (!isExpanded)
                        {
                            CffCommandEventArgs cx = new CffCommandEventArgs("Expand", rowIndex.ToString());
                            if (this.RowCommand != null)
                                RowCommand(this, new CffGridViewCommandEventArgs(e.CommandSource, ((CommandEventArgs)cx)));
                            return;
                        }
                        else
                        {
                            this.NestedSettings.State = GridNestingState.Nested;
                            doUserRowCommand = false;
                            if (this.DataSource == null)
                                this.DataSource = this.GridBag;
                            this.DataBind();
                        }
                    }
                    else
                        this.IsInEditMode = false;
                    }
                    break;

                case "Page":
                    { //pager command - for default pager settings
                        //int pageCount = (int)(this.RowCount / this.PageSize) + ((this.RowCount % this.PageSize) > 5 ? 1 : 0);
                        int pageIndex = -1;
                        this.IsInAddMode = false;

                        if (e.CommandArgument != null) {
                            switch (e.CommandArgument.ToString())
                            {
                                case "First":
                                    {
                                        pageIndex = 0;
                                        cmdArg += "First"; //todo: clean this up as this has no further use here
                                        this.PagerCommandSource = "First";
                                    }
                                    break;

                                case "Prev":
                                    {
                                        pageIndex = this.PageIndex - 1;
                                        cmdArg += "Prev";
                                        if (pageIndex < 0)
                                            pageIndex = 0;
                                        this.PagerCommandSource = "Prev";
                                    } break;

                                case "Next":
                                    {
                                        pageIndex = this.PageIndex + 1;
                                        cmdArg += "Next";
                                        if (pageIndex > this.PageCount)
                                            pageIndex = this.PageCount;
                                        this.PagerCommandSource = "Next";
                                    }
                                    break;

                                case "Last":
                                    {
                                        cmdArg += "Last";
                                        pageIndex = this.PageCount;
                                        this.PagerCommandSource = "Last";
                                    }
                                    break;



                                default:
                                    {//this may be a different pager command - eg. pagetext change, dropdownlist change or the default numeric pager settings
                                        int.TryParse(e.CommandArgument.ToString(), out pageIndex);
                                        if (pageIndex < 0) { pageIndex = 0; }
                                        this.PagerCommandSource = "Numeric";
                                    }
                                    break;
                            }

                            this.EditIndex = -1;
                            this.IsInEditMode = false;
                            this.IsInUpdateMode = false;
                            this.PageIndex = pageIndex;


                            if (this.AllowCustomPaging)
                            { //enable custom pager command only for custom paging
                                CffCommandEventArgs cx = new CffCommandEventArgs(cmdArg, this.PageIndex.ToString());
                                if (this.PagerCommand != null)
                                    PagerCommand(this, new CffGridViewCommandEventArgs(e, ((CommandEventArgs)cx)));
                            }

                            doUserRowCommand = false;
                        }
                    } 
                    break;

                case "Page1":
                    { //command comes from "gotopage" in custompager
                        int pageIndex = 0;

                        this.IsInAddMode = false;
                        this.PageIndex = 0;
                        TextBox tBox = ((System.Web.UI.WebControls.TextBox)(((System.Web.UI.WebControls.WebControl)((((System.Web.UI.WebControls.Table)((((System.Web.UI.WebControls.TableRow)(((System.Web.UI.Control)(e.CommandSource)).BindingContainer)).Cells[0].Controls[0])))).Rows[0].Cells[2])).FindControl("PageTextBox")));
                        int.TryParse(tBox.Text, out pageIndex);
                        this.EditIndex = -1;
                        this.IsInEditMode = false;
                        this.IsInUpdateMode = false;

                        cmdArg += pageIndex.ToString();
                        this.PageIndex = (pageIndex==0)?0:pageIndex-1;
                        doUserRowCommand = false;
                        this.PagerCommandSource = "GoToPage";
                        this.OnPageIndexChanging(new GridViewPageEventArgs(this.PageIndex));
                    } break;

                case "Page2":
                    { //command comes from "rowsperpage" in custompager
                        int pageSize = 0;
                        ListControl ddlPageSize = ((System.Web.UI.WebControls.ListControl)(((System.Web.UI.WebControls.WebControl)(((System.Web.UI.WebControls.Table)(((System.Web.UI.WebControls.Table)(((System.Web.UI.WebControls.TableRow)(((System.Web.UI.Control)(e.CommandSource)).BindingContainer)).Cells[0].Controls[0])))).Rows[0].Cells[1])).FindControl("PageSizeList")));
                        int.TryParse(ddlPageSize.SelectedValue.ToString(), out pageSize);
                        this.PageSize = pageSize;
                        this.IsInAddMode = false;


                        cmdArg += "0";
                        this.EditIndex = -1;
                        this.IsInEditMode = false;
                        this.IsInUpdateMode = false;

                        doUserRowCommand = false;
                        this.PagerCommandSource = "RowsPerPage";
                        if (this.NestedSettings.Enabled && this.PagerCommand != null && this.IsRowCommandPostBack)
                        {
                            this.PageIndex = 0;
                            if (this.DataSource!=null)
                                this.GridBag = this.DataSource;
                            CffCommandEventArgs cx = new CffCommandEventArgs(this.PagerCommandSource, this.PageIndex.ToString());
                            if (this.PagerCommand != null)
                                PagerCommand(this, new CffGridViewCommandEventArgs(e, ((CommandEventArgs)cx)));
                        }
                        else
                        {
                            this.PageIndex = 1;
                            this.OnPageIndexChanging(new GridViewPageEventArgs(this.PageIndex));
                        }
                    } break;

                case "Bound": 
                    { //row command from InsertBoundCommandButtonColumn()
                        if (this.DataSource != null)
                            this.GridBag = this.DataSource;

                        if (!string.IsNullOrEmpty(e.CommandArgument.ToString())) {
                            string cArg = e.CommandArgument.ToString().Substring(0, 3);
                            if (cArg == "Row")
                            {
                                cArg = e.CommandArgument.ToString().Substring(3);
                                rowIndex = Convert.ToInt32(cArg);
                                doUserRowCommand = false;
                                CffCommandEventArgs cx = new CffCommandEventArgs("Bound", rowIndex.ToString());
                                if (this.RowCommand != null)
                                    RowCommand(this, new CffGridViewCommandEventArgs(e.CommandSource, ((CommandEventArgs)cx)));
                            }
                        }
                    }
                    break;

                default:
                    {
                        if (this.IsInEditMode || this.IsInUpdateMode || this.IsInAddMode)
                            this.IsCancelingEdit = true;

                        this.IsInAddMode = false;
                        
                        if (this.EditingSettings.Mode != CffGridViewEditingMode.EditFormAndDisplayRow)
                        { //remove the editindex, isineditmode, isinupdatemode @RowDataBinding for editformanddisplayrow
                            this.EditIndex = -1;
                            this.IsInEditMode = false;
                            this.IsInUpdateMode = false;
                        }
                        this.DataBind();
                    }
                    break;
            }

            //let the user do something else
            if (this.RowCommand != null && doUserRowCommand)
                RowCommand(this, e);
        }

        /// <summary>
        /// Triggered when datagrid is set in editing mode
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowEditing(GridViewEditEventArgs e)
        {

            if (this.OnGridViewRowEditing != null)
                this.OnGridViewRowEditing(this, e);

            if (this.IsInUpdateMode)
            {
                if (this.Caption.Length > 0)
                    this._caption = this.Caption;
                this.EditIndex = e.NewEditIndex;

                this.SelectedIndex = 0;
                this.SelectRow(0);
            }
            else
                //this.SelectedIndex = e.NewEditIndex;  //dbb
                //this.SelectRow(e.NewEditIndex);   //dbb
                
            if (this.EditIndex == -1)
                {
                    this.IsInEditMode = false;
                    this.IsInAddMode = false;
                    //this.IsInUpdateMode = false;
                }
                else
                {
                    this.IsInEditMode = true;
                    this.EditIndex = SelectedIndex;
                    
                }
                
            this.SelectRow(SelectedIndex);
            this.DataBind();

        }

        /// <summary>
        /// Triggered when data grid's editing is cancelled @edit mode
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowCancelingEdit(GridViewCancelEditEventArgs e)
        {
            this.EditIndex = -1;
            this.IsInEditMode = false;
            this.IsInAddMode = false;
            string strSortExp = this.SortExpression;

            if (this.AllowSorting == true)
            {
                this.DataBind();
                if (!string.IsNullOrEmpty(strSortExp))
                    this.Sort(strSortExp, this.SortDirection);
                else if (this.ViewState["SortExpression"] != null)
                {
                     strSortExp = this.ViewState["SortExpression"].ToString();
                    this.Sort(strSortExp, this.SortDirection);
                }
                else if (this.ViewStateValues != null)
                { //look for the sort expression here 
                    if ((this.ViewStateValues as IList<CffGVViewStateValues>).Count > 0)
                    {
                        IList<CffGVViewStateValues> xVS = this.ViewStateValues as IList<CffGVViewStateValues>;
                        foreach (CffGVViewStateValues xSub in xVS)
                        {
                            if (xSub.name == "SetSortExpression")
                            {
                                strSortExp = xSub.value;
                                this.SetSortExpression = xSub.value;
                                this.Sort(xSub.value, this.SortDirection);
                                break;
                            }
                        }

                    }
                }
            }

            try
            {
                this.RaisePostBackEvent("PostBack");
            }
            catch { }
        }


        /// <summary>
        /// Triggered when data grid's update button is pressed @edit mode
        /// good to use when editing in-line
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowUpdating(GridViewUpdateEventArgs e)
        {
            try
            {
                if (this.OnGridViewRowUpdating != null)
                    this.OnGridViewRowUpdating(this, e);

                if (this.IsInUpdateMode)
                {
                    this.EditIndex = -1;
                    this.SelectedIndex = e.RowIndex;
                    this.IsInEditMode = false;
                    this.IsInAddMode = false;
                    this.IsInUpdateMode = false;
                    this.GridBag = this.DataSource;

                    this.DataBind();
                    this.IsCancelingEdit = true;
                    this.OnRowCancelingEdit(new GridViewCancelEditEventArgs(e.RowIndex));
                }
                else
                    this.EditIndex = e.RowIndex;

            }
            catch { }
        }


        protected override void OnRowUpdated(GridViewUpdatedEventArgs e)
        {
            base.OnRowUpdated(e);

            this.IsInUpdateMode = false;
            this.IsInEditMode = false;
            this.IsInAddMode = false;

            if (this.RowUpdated != null)
                RowUpdated(this, e);
        }

        /// <summary>
        /// Triggered when data grid's selected index is changing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectedIndexChanging(GridViewSelectEventArgs e)
        {
            this.SelectedIndex = (e.NewSelectedIndex<0)?0:e.NewSelectedIndex;
        }

        /// <summary>
        /// Use this when you want to call SelectionChanged
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (!this.IsInEditMode && !this.IsInUpdateMode)
            {
                //do whatever

            }

            try
            {
                if (this.SelectionChanged != null)
                    this.SelectionChanged(this, e);

            }
            catch { }
        }


        protected override void PrepareControlHierarchy() 
        { 
             base.PrepareControlHierarchy();
             
            if (EnableRowSelect && !this.IsInEditMode && !this.IsInUpdateMode && !this.IsInAddMode)
            {
                 for (int i = 0; i < Rows.Count; i++)
                 {
                     string argsData = "rc" + Rows[i].RowIndex.ToString();

                     if (this.EditingMode == CffGridViewEditingMode.NotSet)
                         Rows[i].Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this, argsData));
                     else {
                         for (int j = 0; j < Rows[i].Cells.Count; j++)
                         {
                             if ((this.Columns[j] as object).GetType().Name != "CffCommandField") {
                                 Rows[i].Cells[j].Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this, argsData));
                             }
                         }
                     }
                 }
             }
        } 


        #endregion


        #region "Methods"
        public void InitExpandedIndex()
        {
            if (this.NestedPool == null)
                return;

            Dictionary<int, bool> nPool = (Dictionary<int, bool>)this.NestedPool;
            if (nPool.Count > 0) {
                nPool.Clear();
            }
        }

        public void RemoveExpandedIndex(int rowIndex)
        {
            Dictionary<int, bool> nPool = (Dictionary<int, bool>)this.NestedPool;
            try
            {
                nPool.Remove(rowIndex);
                this.NestedPool = nPool;
            }
            catch { }
        }

        public void SetNestedRowExpanded(int rowIndex, bool value)
        {
            Dictionary<int, bool> nPool = (Dictionary<int, bool>)this.NestedPool;
            try
            {
                if (nPool == null)
                {
                    nPool = new Dictionary<int, bool>();
                    nPool.Add(rowIndex, value);
                    this.NestedPool = nPool;
                }
                else
                {
                    if (nPool.Remove(rowIndex)) 
                    {
                        nPool.Add(rowIndex, value);
                        this.NestedPool = nPool;
                    }
                }
            }
            catch { }
        }

        public bool isNestedRowExpanded(int rowIndex)
        {
            bool isExpanded = false;
            Dictionary<int, bool> nPool = (Dictionary<int, bool>)this.NestedPool;
            try
            {
                if (this.NestedPool == null)
                    nPool = new Dictionary<int, bool>();

                if (nPool.TryGetValue(rowIndex, out isExpanded) == false)
                {
                    nPool.Add(rowIndex, false);
                    this.NestedPool = nPool;
                }
            }
            catch { }
            return isExpanded;
        }

        private void RenderToPreviousState(EventArgs e)
        {
            int rEditIdx = this.EditIndex;
            if (this.EditingSettings.Mode == CffGridViewEditingMode.EditFormAndDisplayRow)
            {  //just remove the edit form 
                ControlCollection cC = this.Rows[rEditIdx].Cells[this.Columns.Count - 1].Controls;
            }
            //else handle other editing mode cases here

            this.EditIndex = -1;
            this.IsInUpdateMode = false;
            this.IsInEditMode = false;
            this.IsInAddMode = false;
        }


        private void EnableExpandingButtons()
        { //careful when modifying this. ref mariper.
            Dictionary<int, object> boundcols = new Dictionary<int, object>();
            int colIdx = 0;
            double totalColumnsWidth = 0;
            while (colIdx < this.Columns.Count)
            {
                if (this.Columns[colIdx].GetType().Name == "CffTemplateField")
                    boundcols.Add(colIdx, (object)((CffTemplateField)(this.Columns[colIdx])).DataBoundColumnName);

                //todo: else handle other types here
                totalColumnsWidth += ((CffTemplateField)this.Columns[colIdx]).ItemStyleWidth.Value;
                colIdx++;
            }
            this.BoundPool = boundcols;

            CffCommandField ImageColumnButton = new CffCommandField();
            ImageColumnButton.ButtonType = System.Web.UI.WebControls.ButtonType.Image;
            ImageColumnButton.HeaderText = "";
            ImageColumnButton.HeaderStyle.CssClass = "cffGGVHeader";
            if (this.NestedSettings.Expanded)
            {
                ImageColumnButton.ImageButtonURL = "~/images/minus.gif";
                ImageColumnButton.isExpanded = true;
            }
            else
            {
                ImageColumnButton.ImageButtonURL = "~/images/plus.gif";
                ImageColumnButton.isExpanded = false;
            }
            ImageColumnButton.isImageButton = true;

            if (this.NestedSettings.ExpandingButtonWidth != null)
            {
                ImageColumnButton.ItemStyle.Width = this.NestedSettings.ExpandingButtonWidth;
                ImageColumnButton.ItemStyleWidth = this.NestedSettings.ExpandingButtonWidth;
                ImageColumnButton.ControlStyle.Width = this.NestedSettings.ExpandingButtonWidth;
            }

            if (this.NestedSettings.ExpandingButtonHeight != null)
            {
                ImageColumnButton.ItemStyle.Height = this.NestedSettings.ExpandingButtonHeight;
                ImageColumnButton.ItemStyleHeight = this.NestedSettings.ExpandingButtonHeight;
            }

            if (string.IsNullOrEmpty(this.NestedSettings.ExpandingButtonCssStyle))
            {
                ImageColumnButton.ItemStyle.BorderWidth = Unit.Pixel(0);
                ImageColumnButton.ItemStyle.BorderColor = System.Drawing.Color.LightGray;
                ImageColumnButton.ItemStyle.VerticalAlign = VerticalAlign.Top;
            }
            else
                ImageColumnButton.ItemStyle.CssClass = this.NestedSettings.ExpandingButtonCssStyle;
            

            ImageColumnButton.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            HtmlTextWriter writer = new HtmlTextWriter(null);
            writer.AddAttribute("onmouseover", "document.body.style.cursor='pointer';");
            writer.AddAttribute("onmouseout", "document.body.style.cursor='default';");
    
            if (!string.IsNullOrEmpty(this.NestedSettings.ExpandingButtonAltText))
                writer.AddAttribute("alt", this.NestedSettings.ExpandingButtonAltText);

            ImageColumnButton.ItemStyle.AddAttributesToRender(writer);

            this.Columns.Insert(0, ImageColumnButton);

            if (string.IsNullOrEmpty(this.NestedSettings.ExpandingButtonCssStyle))
                this.Columns[0].ControlStyle.CssClass = "cffGGV_centerAlignedCell";
            else
                this.Columns[0].ControlStyle.CssClass = this.NestedSettings.ExpandingButtonCssStyle;

            if (this.NestedSettings.ExpandingColumnWidth == null)
                this.Columns[0].ItemStyle.Width = Unit.Percentage(Convert.ToInt32(100 - totalColumnsWidth));
            else
                this.Columns[0].ItemStyle.Width = this.NestedSettings.ExpandingColumnWidth;
           
            this.NestedSettings.State = GridNestingState.Nesting;
        }


        private void DisplayCustomHeader(GridViewRowEventArgs e)
        {
            //let the user control the header display
            if (!string.IsNullOrEmpty(this.Caption))
            {
                if (string.IsNullOrEmpty(this._caption))
                    this._caption = this.Caption; 

                if (!string.IsNullOrEmpty(this.CaptionHeaderSettings.CssStyle))
                    this.Caption = "<span class='" + this.CaptionHeaderSettings.CssStyle + "'>" + this._caption + "</span>";
                else {

                    int hasSpan = this.Caption.IndexOf("span");   // dbb

                    if (hasSpan == -1)    // dbb
                    {
                        string spanStyle = "<span style='";
                        if (this.CaptionHeaderSettings.BoldCaption)
                            spanStyle += "font-weight:bold;font-size:15px;color:#88422b;";

                        if (this.CaptionHeaderSettings.HorizontalAlignment != System.Web.UI.WebControls.HorizontalAlign.NotSet)
                            spanStyle += "text-align:" + this.CaptionHeaderSettings.HorizontalAlignment.ToString() + ";";

                        if (this.CaptionHeaderSettings.VerticalAlignment != VerticalAlign.NotSet)
                            spanStyle += "vertical-align:" + this.CaptionHeaderSettings.VerticalAlignment.ToString() + ";";

                        // spanStyle += "'>" + this._caption + "</span>";
                        spanStyle += "'><i class='fa fa-pencil-square-o' aria-hidden='true'></i>" + this.Caption + "</span>";
                        this.Caption = "";
                        this.Caption = spanStyle;
                    }
                }
            }

            int iCtr = 0;
            e.Row.Width = Unit.Percentage(100);
            while (iCtr < e.Row.Cells.Count)
            {
                if ((this.Columns[iCtr] as object).GetType().Name == "CffCommandField") {
                    e.Row.Cells[iCtr].CssClass = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffCommandField)(this.Columns[iCtr]))).ItemStyle)).CssClass;
                    e.Row.Cells[iCtr].Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffCommandField)(this.Columns[iCtr]))).ItemStyle)).Width;
                    e.Row.Cells[iCtr].HorizontalAlign = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffCommandField)(this.Columns[iCtr])).HorizontalAlignment;
                }
                else {
                    if (!this.NestedSettings.Enabled && !this.NestedSettings.Expanded)
                    {
                        if (this.Columns[iCtr].HeaderStyle.HorizontalAlign == HorizontalAlign.NotSet)
                        {
                            e.Row.Cells[iCtr].CssClass = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[iCtr]))).ItemStyle)).CssClass;
                            e.Row.Cells[iCtr].Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[iCtr]))).ItemStyle)).Width;
                            e.Row.Cells[iCtr].HorizontalAlign = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[iCtr])).HorizontalAlignment;
                            if (e.Row.Cells[iCtr].Controls.Count > 0)
                                ((System.Web.UI.WebControls.Label)(e.Row.Cells[iCtr].Controls[0])).CssClass = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[iCtr]))).ItemStyle)).CssClass;
                            if (string.IsNullOrEmpty( e.Row.Cells[iCtr].CssClass))
                                e.Row.Cells[iCtr].BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        }
                        else
                        {
                            e.Row.Cells[iCtr].HorizontalAlign = this.Columns[iCtr].HeaderStyle.HorizontalAlign;
                            e.Row.Cells[iCtr].CssClass = string.IsNullOrEmpty(this.Columns[iCtr].HeaderStyle.CssClass) ? "cffGGVHeader" : this.Columns[iCtr].HeaderStyle.CssClass;
                            if (string.IsNullOrEmpty(e.Row.Cells[iCtr].CssClass))
                                e.Row.Cells[iCtr].Style.Add(HtmlTextWriterStyle.TextAlign, this.Columns[iCtr].HeaderStyle.HorizontalAlign.ToString().ToLower());
                            e.Row.Cells[iCtr].Text = this.Columns[iCtr].HeaderText;

                            if (string.IsNullOrEmpty(e.Row.Cells[iCtr].CssClass))
                            {
                                if (e.Row.Cells[iCtr].Controls.Count > 0)
                                {
                                    if (e.Row.Cells[iCtr].HorizontalAlign == System.Web.UI.WebControls.HorizontalAlign.Right)
                                        ((System.Web.UI.WebControls.Label)(e.Row.Cells[iCtr].Controls[0])).CssClass = "cffGGV_rightAlignedCell";
                                    else if (e.Row.Cells[iCtr].HorizontalAlign == System.Web.UI.WebControls.HorizontalAlign.Center)
                                        ((System.Web.UI.WebControls.Label)(e.Row.Cells[iCtr].Controls[0])).CssClass = "cffGGV_centerAlignedCell";
                                    else
                                        ((System.Web.UI.WebControls.Label)(e.Row.Cells[iCtr].Controls[0])).CssClass = string.IsNullOrEmpty(this.Columns[iCtr].HeaderStyle.CssClass) ? "cffGGVHeader" : this.Columns[iCtr].HeaderStyle.CssClass;
                                    ((System.Web.UI.WebControls.Label)(e.Row.Cells[iCtr].Controls[0])).Style.Add(HtmlTextWriterStyle.TextAlign, this.Columns[iCtr].HeaderStyle.HorizontalAlign.ToString().ToLower());
                                }
                              
                            }
                        }
                    }
                    else {
                        if (e.Row.Cells[iCtr].HorizontalAlign == HorizontalAlign.NotSet) {
                            e.Row.Cells[iCtr].Text = this.Columns[iCtr].HeaderText;
                            
                            if (string.IsNullOrEmpty(e.Row.Cells[iCtr].CssClass)) {
                                e.Row.Cells[iCtr].CssClass = string.IsNullOrEmpty(this.Columns[iCtr].HeaderStyle.CssClass) ? "cffGGVHeader" : this.Columns[iCtr].HeaderStyle.CssClass;
                            }

                            if (string.IsNullOrEmpty(e.Row.Cells[iCtr].CssClass)) {
                                e.Row.Cells[iCtr].HorizontalAlign = this.Columns[iCtr].HeaderStyle.HorizontalAlign;
                                e.Row.Cells[iCtr].Style.Add(HtmlTextWriterStyle.TextAlign, this.Columns[iCtr].HeaderStyle.HorizontalAlign.ToString().ToLower());
                            }
                        }
                    }
               }
               iCtr++;
            }

            if (this.OnGridViewRenderHeader != null)
                this.OnGridViewRenderHeader(this, e);
        }

        private Dictionary<string, string> ComputeTotalSummaryPool(Dictionary<int, string> columnsPool, int rowCount, IEnumerable dSource)
        {
            Dictionary<string, string> TotalsPool = new Dictionary<string, string>();
            Dictionary<string, string> SummaryPool = new Dictionary<string, string>();
            try
            {
                int iCtr = 0;
                decimal TotalValue = 0;

                foreach (KeyValuePair<int, string> colTotals in columnsPool)
                {
                    while (iCtr < rowCount)
                    {
                        object xS = dSource.Cast<object>().ToList()[iCtr];
                        object objectValue = CffGenGridViewCommon.GetObjectValue(xS, colTotals.Value);
                        TotalValue += Convert.ToDecimal((objectValue == null) ? "0" : (string.IsNullOrEmpty(objectValue.ToString())) ? "0" : objectValue.ToString());
                        iCtr++;
                    }
                    TotalsPool.Add(colTotals.Value, TotalValue.ToString("C"));
                    TotalValue = 0;
                    iCtr = 0;
                }
            }
            catch { }
            return TotalsPool;
        }

        private void DisplayCustomFooter(GridViewRowEventArgs e)
        { //careful when modifying this. ref mariper.
            Table FooterTable = new Table();
            FooterTable.CssClass = this.TotalsSummarySettings.TableClass; 
            if (string.IsNullOrEmpty(FooterTable.CssClass))
                FooterTable.Width = Unit.Percentage(100);

            TableRow FooterRow = (System.Web.UI.WebControls.TableRow)e.Row;
            FooterRow.CssClass = this.TotalsSummarySettings.TableClass;
            if (string.IsNullOrEmpty(FooterRow.CssClass))
                FooterRow.Width = Unit.Percentage(100);

            Dictionary<string, string> TotalsPool = new Dictionary<string, string>();
            Dictionary<string, string> SummaryPool = new Dictionary<string, string>();
            int cellCount = FooterRow.Cells.Count;
            int iCtr = 0;
            int colFiller = 1;
            
            bool bIsNestedGrid = this.NestedSettings.Enabled;
            TableCell cellTotals = new TableCell();
                
            if (FooterRow.Cells.Count > 0)
            {
                CffTotalsSummary xTS = (this.TotalsSummarySettings == null) ? (new CffTotalsSummary("Total", 
                        ((string.IsNullOrEmpty(this.TotalsSummarySettings.TotalsSummaryText))?"Totals Summary":this.TotalsSummarySettings.TotalsSummaryText)))
                            : this.TotalsSummarySettings;
                //todo: if both has showtotal and show summary span this and create a table instead
                //1. create a totals row with cell columns = cellcount;
                //2. create a summary row cell columns = 1 and put the summary text there
                FooterRow.Width = Unit.Percentage(100);
                if (bIsNestedGrid && (this.CustomFooterSettings.HasFlag(CffCustomFooterMode.ShowSummary)))
                {
                    FooterRow.Cells[0].ColumnSpan = this.Columns.Count;
                    int x = 1;
                    while (x < cellCount) {
                        FooterRow.Cells.RemoveAt(1);
                        x++;
                    }
                } else if  (this.CustomFooterSettings.HasFlag(CffCustomFooterMode.ShowSummary))
                {
                    FooterRow.Cells[0].ColumnSpan = this.Columns.Count-1;
                    int x = 0;
                    while (x < cellCount) {
                        FooterRow.Cells.RemoveAt(0);
                        x++;
                    }
                }

                if (this.CustomFooterSettings.HasFlag(CffCustomFooterMode.ShowTotals))
                {
                    iCtr = 0;
                    decimal TotalValue = 0;
                    try
                    {
                        foreach (KeyValuePair<int, string> colTotals in this.TotalsSummarySettings.ColumnTotals.ColumnsPool)
                        {
                            IEnumerable dSource = (IEnumerable)this.DataSource;
                            while (iCtr < this.RowCount) {
                                object xS = dSource.Cast<object>().ToList()[iCtr];
                                object objectValue = CffGenGridViewCommon.GetObjectValue(xS, colTotals.Value);
                                TotalValue += Convert.ToDecimal((objectValue == null) ? "0" : (string.IsNullOrEmpty(objectValue.ToString())) ? "0" : objectValue.ToString());
                                iCtr++;
                            }
                            TotalsPool.Add(colTotals.Value, TotalValue.ToString("C"));
                            TotalValue = 0;
                            iCtr = 0;
                        }
                    }
                    catch { }

                    CffTemplateField xGridColumnField;
                    CffCommandField xGridCommandField;
        
                    Label lblTotalsText = new Label();
                    lblTotalsText.Width = Unit.Percentage(100);
                    lblTotalsText.Text = xTS.TotalsText;
                    lblTotalsText.CssClass = xTS.ColumnTotals.cssStyle;
                    lblTotalsText.ControlStyle.BorderWidth = Unit.Pixel(0);
                    lblTotalsText.Font.Bold = true;
                    lblTotalsText.BorderWidth = Unit.Pixel(0);

                    Label lblFillerText = new Label();
                    lblFillerText.Width = Unit.Percentage(100);
                    lblFillerText.Text = " ";
                    lblFillerText.CssClass = xTS.ColumnTotals.cssStyle;
                    lblFillerText.ControlStyle.BorderWidth = Unit.Pixel(0);
                    lblFillerText.Font.Bold = true;
                    lblFillerText.BorderWidth = Unit.Pixel(0);

                    if (string.IsNullOrEmpty(xTS.TotalColumnsRowStyle))
                    {
                        if (string.IsNullOrEmpty(FooterRow.CssClass))
                        {
                            FooterRow.Cells[0].BorderWidth = Unit.Pixel(0);
                        }
                    }
                    else
                        FooterRow.CssClass = xTS.TotalColumnsRowStyle;

                    
                    if ((this.Columns[0] as object).GetType().Name == "CffCommandField")
                    {
                        xGridCommandField = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffCommandField)(this.Columns[0]);
                        if (this.NestedSettings.ExpandingColumnWidth.Value <= 0)
                        {
                            FooterRow.Cells[0].ControlStyle.CssClass = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridCommandField)).ItemStyle)).CssClass;
                            FooterRow.Cells[0].Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridCommandField)).ItemStyle)).Width;
                            FooterRow.Cells[0].HorizontalAlign = xGridCommandField.HorizontalAlignment;
                        }
                        else {
                            FooterRow.Cells[0].Width = this.NestedSettings.ExpandingColumnWidth;
                        }
                        if (string.IsNullOrEmpty(FooterRow.CssClass))
                              FooterRow.Cells[0].BorderWidth = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridCommandField)).ItemStyle)).BorderWidth;


                        if (this.NestedSettings.Enabled == true && FooterRow.Cells[0].Width.Value == 0.0) {
                            FooterRow.Cells[0].Style.Add(HtmlTextWriterStyle.Width, "auto");
                        }

                        FooterRow.Cells[0].Controls.Clear();
                        FooterRow.Cells[0].Controls.Add(lblTotalsText);

                        if (bIsNestedGrid) {
                            colFiller = 2;
                            if (string.IsNullOrEmpty(xTS.TotalsTextStyle))
                            {
                                if (string.IsNullOrEmpty(FooterRow.CssClass))
                                    FooterRow.Cells[0].BorderWidth = Unit.Pixel(0);
                            }
                            else
                                FooterRow.Cells[0].CssClass = xTS.TotalsTextStyle;
                        }
                        else {
                            colFiller = 1;
                            FooterRow.Cells[0].Controls.Clear();
                            FooterRow.Cells[0].Controls.Add(lblTotalsText);
                            if (string.IsNullOrEmpty(FooterRow.Cells[0].CssClass)) {
                                FooterRow.Cells[0].BorderWidth = Unit.Pixel(0);
                            }
                        }

                        xGridColumnField = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[1]);
                        FooterRow.Cells[1].Controls.Clear();
                        FooterRow.Cells[1].Controls.Add(lblFillerText);
                        FooterRow.Cells[1].ControlStyle.CssClass = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).CssClass;
                        FooterRow.Cells[1].Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).Width;
                        FooterRow.Cells[1].HorizontalAlign = (((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle).HorizontalAlign;
                        if (string.IsNullOrEmpty(FooterRow.CssClass))
                            FooterRow.Cells[1].BorderWidth = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).BorderWidth;
                    }
                    else
                    {
                        colFiller = 0;
                        FooterRow.Cells[0].Controls.Clear();
                        FooterRow.Cells[0].Controls.Add(lblTotalsText);

                        xGridColumnField = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[0]);
                        if (bIsNestedGrid)
                        {
                            FooterRow.Cells[0].ControlStyle.CssClass = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).CssClass;
                            FooterRow.Cells[0].HorizontalAlign = xGridColumnField.HorizontalAlignment;
                        }
                        else {
                            FooterRow.Cells[0].ControlStyle.CssClass = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).CssClass;
                            FooterRow.Cells[0].ControlStyle.Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).Width;
                            FooterRow.Cells[0].HorizontalAlign = xGridColumnField.HorizontalAlignment;
                            if (string.IsNullOrEmpty(FooterRow.Cells[0].ControlStyle.CssClass))
                                FooterRow.Cells[0].BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        }
                    }

                    try
                    {
                        iCtr = colFiller;
                        string colName = "";
                        string totValue = "";
                        string totCssStyle = "cffGGV_leftAlignedColumn";

                        while (iCtr < this.Columns.Count) {
                            if (((this.Columns[iCtr] as object) as CffTemplateField).DataBoundColumnName != null)
                                colName = ((this.Columns[iCtr] as object) as CffTemplateField).DataBoundColumnName;
                            else
                            {
                                try {
                                    colName = this.TotalsSummarySettings.ColumnTotals.ColumnsPool[iCtr].ToString();
                                } catch (Exception) {  //try the header name
                                    colName = ((this.Columns[iCtr] as object) as CffTemplateField).HeaderText;
                                }
                            }

                            TotalsPool.TryGetValue(colName, out totValue);
                            xGridColumnField = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[iCtr]);

                            Label lblValue = new Label();
                            lblValue.Width = Unit.Percentage(100);
                            lblValue.Text = (totValue==null) ? "" : totValue;
                            lblValue.CssClass = (this.Columns[iCtr] as CffTemplateField).ControlStyle.CssClass;
                            lblValue.Style.Add(HtmlTextWriterStyle.TextAlign, (this.Columns[iCtr] as CffTemplateField).HorizontalAlignment.ToString());
                            lblValue.Style.Add(HtmlTextWriterStyle.Display,"inline");   //dbb

                            if (bIsNestedGrid) {
                                FooterRow.Cells[iCtr].BorderWidth = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).BorderWidth;
                                FooterRow.Cells[iCtr].CssClass = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).CssClass;
                                FooterRow.Cells[iCtr].Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).Width;
                                FooterRow.Cells[iCtr].HorizontalAlign = xGridColumnField.HorizontalAlignment;
                                FooterRow.Cells[iCtr].Controls.Clear();
                                if (string.IsNullOrEmpty(lblValue.CssClass))
                                    lblValue.CssClass = FooterRow.Cells[iCtr].CssClass;

                                if (FooterRow.Cells[iCtr].HorizontalAlign == HorizontalAlign.NotSet && lblValue.CssClass.ToLower().Contains("currency"))
                                    FooterRow.Cells[iCtr].HorizontalAlign = HorizontalAlign.Right;

                                FooterRow.Cells[iCtr].Text = (totValue == null) ? "" : totValue;
                            } else {
                                if (this.TotalsSummarySettings.ColumnTotals.CssStylePool.Count == 0) 
                                    totCssStyle = "";
                                else 
                                    this.TotalsSummarySettings.ColumnTotals.CssStylePool.TryGetValue(colName, out totCssStyle);

                                if (string.IsNullOrEmpty(totCssStyle))
                                {
                                    FooterRow.Cells[iCtr].CssClass = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).CssClass;
                                    FooterRow.Cells[iCtr].BorderWidth = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).BorderWidth;
                                    FooterRow.Cells[iCtr].HorizontalAlign = xGridColumnField.HorizontalAlignment;

                                }
                                else 
                                {
                                    FooterRow.Cells[iCtr].CssClass = totCssStyle;
                                    if ((totCssStyle.ToUpper().Contains("CURRENCY")) && (FooterRow.Cells[iCtr].HorizontalAlign == HorizontalAlign.NotSet))
                                        FooterRow.Cells[iCtr].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;    
                                }

                                if (string.IsNullOrEmpty(FooterRow.Cells[iCtr].CssClass))
                                    FooterRow.Cells[iCtr].BorderStyle = System.Web.UI.WebControls.BorderStyle.None;

                                FooterRow.Cells[iCtr].Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).Width;
                                FooterRow.Cells[iCtr].Controls.Add(lblValue);

                            }
                            colName = "";
                            totValue = "";
                            iCtr++;
                        }
                    }
                    catch { }
               }

                if (this.CustomFooterSettings.HasFlag(CffCustomFooterMode.ShowSummary))
                {
                    decimal SummaryValue = 0;
                    try
                    {
                        string strValue = "";
                        foreach (KeyValuePair<int, string> colTotals in this.TotalsSummarySettings.SummaryTotals.ColumnsPool)
                        {

                            TotalsPool.TryGetValue(colTotals.Value, out strValue);
                            SummaryValue += string.IsNullOrEmpty(strValue) ? 0 : Convert.ToDecimal(strValue);
                        }

                        //SummaryTotal 
                        SummaryPool.Add("SummaryTotal", SummaryValue.ToString("C"));
                    }
                    catch { }

                    string SummaryText = "Error summarizing column Total;";
                    if (TotalsPool.Count > 0)
                    {
                        SummaryText = xTS.TotalsSummaryText;
                    }

                    Label labelSummary = new Label();
                    labelSummary.Text = SummaryText;
                    labelSummary.CssClass = xTS.SummaryTotals.cssStyle;

                    TableCell cellTotalSummary = new TableCell();
                    cellTotalSummary.Controls.Add(labelSummary);

                    TableRow SummaryTotalsRow = new TableRow();
                    SummaryTotalsRow.Cells.Add(cellTotalSummary);

                    CffTemplateField xGridColumnField;

                    try
                    {
                        iCtr = 0;
                        string totValue = "";
                        foreach (KeyValuePair<string, string> sumTotals in SummaryPool)
                        {
                            SummaryPool.TryGetValue(sumTotals.Key, out totValue);

                            xGridColumnField = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[iCtr]);

                            Label lblValue = new Label();
                            lblValue.Text = totValue;

                            //todo:: refactor this - we need to be able to let the user set this outside
                            lblValue.ControlStyle.Width = Unit.Percentage(100);
                            lblValue.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5px");
                            lblValue.Style.Add(HtmlTextWriterStyle.TextAlign, "Left");
                            //todo:: end refactor

                            TableCell cellValue = new TableCell();
                            cellValue.Controls.Add(lblValue);
                            SummaryTotalsRow.Cells.Add(cellValue);
                            totValue = "";
                            iCtr++;
                        }
                    }
                    catch { }

                    FooterTable.Rows.Add(FooterRow);
                    FooterTable.Rows.Add(SummaryTotalsRow);
                }

                if (string.IsNullOrEmpty(FooterTable.CssClass)) 
                {
                    FooterTable.Width = Unit.Percentage(100);
                    FooterTable.BorderWidth = Unit.Pixel(1);
                    FooterTable.BackColor = System.Drawing.Color.LightGray;

                    FooterRow.BackColor = System.Drawing.Color.LightGray;
                    FooterRow.BorderWidth = Unit.Pixel(1);
                }

                if (FooterTable.Rows.Count > 0)
                { //Summary Table Exists
                    e.Row.Controls.Clear();
                    e.Row.Controls.Add(FooterTable);
                }
                else {
                    e.Row.Width = Unit.Percentage(100);
                    e.Row.Height = Unit.Percentage(100);
                }
            
            }
        }

        private void DisplayCustomPager(GridViewRowEventArgs e)
        {
            TableRow PagerRow;
            if (this.CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.Top)) //todo: top pager row may be in a different location
                PagerRow = ((System.Web.UI.WebControls.Table)((((System.Web.UI.WebControls.TableRow)(e.Row)).Cells[0]).Controls[0])).Rows[0];
            else
                PagerRow = ((System.Web.UI.WebControls.Table)((((System.Web.UI.WebControls.TableRow)(e.Row)).Cells[0]).Controls[0])).Rows[0];

            PagerRow.Width = Unit.Percentage(100);
            PagerRow.BorderWidth = Unit.Pixel(0);
            Literal space = new Literal();
            space.Text = "&nbsp;";

            TableCell cellRecordsSum = new TableCell();
            Label recordsLabel = new Label();
            recordsLabel.Text = String.Format("Found {0} record{1}", this.RowCount, (this.RowCount <= 1) ? String.Empty : "s");
            recordsLabel.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5px");
            recordsLabel.Style.Add(HtmlTextWriterStyle.PaddingRight, "5px");
            recordsLabel.Style.Add(HtmlTextWriterStyle.TextAlign, "Left");
            cellRecordsSum.Controls.Add(recordsLabel);

            PagerRow.Cells.AddAt(0, cellRecordsSum);
            PagerRow.Cells[0].Width = Unit.Percentage(25);

            int customControlCounter = 1;
            //int customPageCount = (int)(this.RowCount / this.PageSize);
            //if ((this.RowCount % this.PageSize) > 0)
            //    customPageCount += 1;

            TableCell cellRowsPerPage = new TableCell();
            Button btnPageSize = new Button();
            if (this.CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.Rows))
            {
                cellRowsPerPage.Width = Unit.Percentage(15);
                Label lb = new Label();
                lb.Text = "Rows Per Page: ";
                lb.ID = "lbl_RowsPerPage";
                cellRowsPerPage.Controls.Add(lb);

                int i;
                int max = this.RowCount;
                int increment = (this.PageSize==this.RowCount) ? this.PageSize : ((this.RowCount < this.PageSize) ? this.RowCount : this.PageSize); //const int increment = 5;

                //int i2 = (customPageCount * increment);  // dbb

                bool alreadySelected = false;
                if (this.RowsPerPageIncrement == CffRowsPerPageIncrement.DefaultPageSizeIncrement)
                    increment = (this.PageSize == this.RowCount) ? this.PageSize : ((this.RowCount < this.PageSize) ? this.RowCount : this.DefaultPageSize);
                
                DropDownList _ddlPageSize = new DropDownList();
                _ddlPageSize.ID = "PageSizeList";
                _ddlPageSize.ToolTip = "Select number of rows per page";

                ListItem item;
                //for (i = increment; (i <= max || i <= i2); i += increment)  // dbb
                for (i = increment; i <= max; i += increment)
                {
                    item = new ListItem(i.ToString());
                    if (i == this.PageSize)
                    {
                        item.Selected = true;
                        alreadySelected = true;
                    }
                    _ddlPageSize.Items.Add(item);
                }

                item = new ListItem("All", this.RowCount.ToString());
                if (this.RowCount == this.PageSize && alreadySelected == false)
                {
                    item.Selected = true;
                    alreadySelected = true;
                }

                if (this.RowCount > (i - increment) && alreadySelected == false)
                {
                    item.Selected = true;
                }

                _ddlPageSize.Items.Add(item);
                _ddlPageSize.Attributes.Add("runat", "server");

                btnPageSize.ID = "btnPSGo";
                btnPageSize.Text = "Go";
                btnPageSize.ToolTip = "Click to refresh grid";
                btnPageSize.CommandName = "Page2";
                btnPageSize.CommandArgument = "0";
                btnPageSize.UseSubmitBehavior = true;
                btnPageSize.CausesValidation = false;

                cellRowsPerPage.Controls.Add(_ddlPageSize);
                cellRowsPerPage.Controls.Add(btnPageSize);
                PagerRow.Cells.AddAt(customControlCounter, cellRowsPerPage);
                PagerRow.Cells[customControlCounter].Width = Unit.Percentage(20);
                customControlCounter++;
            }

            int customPageCount = (int)(this.RowCount / this.PageSize);
            if ((this.RowCount % this.PageSize) > 0)
                customPageCount += 1;

            Button btnGo = new Button();
            if (this.CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.Page))
            {
                TableCell tCellPageSelect = new TableCell();
                tCellPageSelect.Width = Unit.Percentage(25);

                Label lbl = new Label();
                lbl.Text = "Page";
                lbl.Style.Add(HtmlTextWriterStyle.PaddingLeft, "10px");
                lbl.Style.Add(HtmlTextWriterStyle.PaddingRight, "3px");
                tCellPageSelect.Controls.Add(lbl);
                tCellPageSelect.Controls.Add(space);

                TextBox txtBoxPage = new TextBox();
                txtBoxPage.ToolTip = "Enter page number";
                txtBoxPage.ID = "PageTextBox";
                txtBoxPage.MaxLength = 4;
                txtBoxPage.Width = Unit.Pixel(30);
                txtBoxPage.Style.Add(HtmlTextWriterStyle.MarginLeft, "10px");
                txtBoxPage.Style.Add(HtmlTextWriterStyle.MarginRight, "10px");

                //if ((customPageCount - (this.PageIndex + 1)) == 1)   // dbb
                //{
                //    this.PageIndex = customPageCount;
                //    txtBoxPage.Text = (this.PageIndex).ToString();
                //}
                //else
                //{
                    txtBoxPage.Text = (this.PageIndex + 1).ToString();
                //}

                txtBoxPage.Attributes.Add("runat", "server");
                tCellPageSelect.Controls.Add(txtBoxPage);
                if (this.PageCount == 1)
                    txtBoxPage.Enabled = false;
                else
                    txtBoxPage.Enabled = true;

                lbl = new Label();
             
                lbl.Text = String.Format(" of {0} ", customPageCount);
                lbl.Style.Add(HtmlTextWriterStyle.PaddingRight, "10px");
                tCellPageSelect.Controls.Add(lbl);

                btnGo.ID = "btnPagerGo";
                btnGo.Text = "Go To Page";
                btnGo.ToolTip = "Click to go to page number";
                btnGo.CommandName = "Page1";
                btnGo.CommandArgument = "0";
                btnGo.UseSubmitBehavior = true;
                btnGo.CausesValidation = false;
                if (customPageCount == 1)
                    btnGo.Enabled = false;
                else
                    btnGo.Enabled = true;
                tCellPageSelect.Controls.Add(btnGo);
                PagerRow.Cells.AddAt(customControlCounter, tCellPageSelect);
                PagerRow.Cells[customControlCounter].Width = Unit.Percentage(25);
                customControlCounter++;
            }

            Button btnNext = new Button();
            Button btnPrev = new Button();
            Button btnFirst = new Button();
            Button btnLast = new Button();

            TableCell cellButtons = new TableCell();
            if (this.CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.FirstLast))
            {
                btnFirst.Text = "|<";
                btnFirst.CommandName = "Page";
                btnFirst.CommandArgument = "First";
                btnFirst.ToolTip = "Navigate to first page";
                btnNext.Style.Add(HtmlTextWriterStyle.MarginLeft, "5px");
                if (customPageCount == 1)
                    btnFirst.Enabled = false;
                else
                    btnFirst.Enabled = true;

                cellButtons.Controls.Add(btnFirst);
            }

            if (this.CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.PreviousNext))
            {
                btnPrev.Text = "<";
                btnPrev.CommandName = "Page";
                btnPrev.CommandArgument = "Prev";
                btnPrev.ToolTip = "Navigate to previous page";
                btnPrev.Style.Add(HtmlTextWriterStyle.MarginLeft, "2px");
                if (customPageCount == 1)
                    btnPrev.Enabled = false;
                else
                    btnPrev.Enabled = true;

                cellButtons.Controls.Add(space);
                cellButtons.Controls.Add(btnPrev);

                btnNext.Text = ">";
                btnNext.CommandName = "Page";
                btnNext.CommandArgument = "Next";
                btnNext.ToolTip = "Navigate to next page";
                btnNext.Style.Add(HtmlTextWriterStyle.MarginLeft, "2px");
                if (customPageCount == 1)
                    btnNext.Enabled = false;
                else
                    btnNext.Enabled = true;

                cellButtons.Controls.Add(space);
                cellButtons.Controls.Add(btnNext);
            }

            if (this.CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.FirstLast))
            {
                btnLast.Text = ">|";
                btnLast.CommandName = "Page";
                btnLast.CommandArgument = "Last";
                btnLast.ToolTip = "Navigate to last page";
                btnLast.Style.Add(HtmlTextWriterStyle.MarginLeft, "2px");
                if (customPageCount == 1)
                    btnLast.Enabled = false;
                else
                    btnLast.Enabled = true;

                cellButtons.Controls.Add(space);
                cellButtons.Controls.Add(btnLast);
            }

            if (cellButtons.Controls.Count > 0)
            {
                PagerRow.Cells.AddAt(customControlCounter, cellButtons);
                PagerRow.Cells[customControlCounter].Style.Add(HtmlTextWriterStyle.MarginLeft, "5px");
                PagerRow.Cells[customControlCounter].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                PagerRow.Cells[customControlCounter].Width = Unit.Percentage(20);
                customControlCounter++;  
            }

            Button btnExport = new Button();
            if (this.CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.WithExport))
            { //display viewallbutton here
                TableCell cellExportButton = new TableCell();

                btnExport.ID = "btnPgrExport";
                btnExport.Text = "Export";
                btnExport.ToolTip = "Click to Export";
                btnExport.CommandName = "Export";
                btnExport.CommandArgument = this.PageIndex.ToString();
                btnExport.UseSubmitBehavior = true;
                btnExport.CausesValidation = false;
                btnExport.BackColor = System.Drawing.Color.Green;
                btnExport.ForeColor = System.Drawing.Color.White;
                btnExport.Width = Unit.Pixel(60);
                btnExport.Height= Unit.Pixel(30);
                cellExportButton.Controls.Add(btnExport);

                PagerRow.Cells.AddAt(customControlCounter, cellExportButton);
                PagerRow.Cells[customControlCounter].Style.Add(HtmlTextWriterStyle.MarginLeft, "10px");
                PagerRow.Cells[customControlCounter].Style.Add(HtmlTextWriterStyle.MarginRight, "0px");
                PagerRow.Cells[customControlCounter].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                PagerRow.Cells[customControlCounter].Width = Unit.Percentage(15);
                customControlCounter++;
            }

            PagerRow.Cells[customControlCounter].Style.Add(HtmlTextWriterStyle.PaddingLeft, "10px");    
            PagerRow.Cells[customControlCounter].Style.Add(HtmlTextWriterStyle.TextAlign, "right");
            PagerRow.BorderWidth = Unit.Pixel(0);
            PagerRow.Width = Unit.Percentage(100);
   
            if (this.CustomPagerSettingsMode.HasFlag(CffCustomPagerMode.Top))
                //e.Row.CssClass = "cffGGV_PagerTopPanel";   //if pager is being displayed in top of page
                e.Row.CssClass = "dxgvPagerTopPanel";   //if pager is being displayed in top of page
            else
                //e.Row.CssClass = "cffGGV_PagerBottomPanel";
               e.Row.CssClass = "dxgvPagerBottomPanel";

            if ((((System.Web.UI.WebControls.TableRow)(e.Row)).Cells[0]).ColumnSpan > 1)
            {
                ((System.Web.UI.WebControls.WebControl)(((System.Web.UI.WebControls.TableRow)(e.Row)).Cells[0])).Width = Unit.Percentage(100);
                ((System.Web.UI.WebControls.WebControl)(e.Row.Cells[0])).CssClass = "dxgvPagerBottomPanel";
                if (((System.Web.UI.WebControls.WebControl)(e.Row.Cells[0])).Controls.Count > 0)
                {
                    if (((System.Web.UI.WebControls.WebControl)(e.Row.Cells[0])).Controls[0].GetType().Name == "PagerTable")
                    {
                        ((System.Web.UI.WebControls.Table)(((System.Web.UI.WebControls.WebControl)(e.Row.Cells[0])).Controls[0])).Width = Unit.Percentage(100);
                        ((System.Web.UI.WebControls.Table)(((System.Web.UI.WebControls.WebControl)(e.Row.Cells[0])).Controls[0])).BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ((System.Web.UI.WebControls.Table)(((System.Web.UI.WebControls.WebControl)(e.Row.Cells[0])).Controls[0])).BorderWidth = Unit.Pixel(0);
                    }
                }
                e.Row.Cells[0].Attributes.Add("colspan", (((System.Web.UI.WebControls.TableRow)(e.Row)).Cells[0]).ColumnSpan.ToString());
            }
        }

        /// <summary>
        /// Displays elements in nested grid.
        /// </summary>
        /// <param name="e"></param>
        private void DisplayNestedGrid(GridViewRowEventArgs e)
        { //careful when modifying this. ref mariper.
            int colIdx;
            int rowIndex = e.Row.RowIndex;
            int dtaIndex = e.Row.DataItemIndex;

            if (e.Row.DataItemIndex != e.Row.RowIndex) //user must've clicked the nextpage/gotopage
                rowIndex = e.Row.DataItemIndex;

            CffCommandField expandingButton;
            CffTemplateField firstColumnField;
            if (this.Columns[0].GetType().Name == "CffCommandField")
                expandingButton = ((CffCommandField)this.Columns[0]);
            else if (this.Columns[0].GetType().Name == "CffTemplateField") {
                //??? why are the expanding buttons missing
                firstColumnField = ((CffTemplateField)this.Columns[0]);
                EnableExpandingButtons();
                expandingButton = ((CffCommandField)this.Columns[0]);
            }

            if (e.Row.Cells[0].Controls[0].GetType().Name.Contains("Image")) {
                ((System.Web.UI.WebControls.Image)(e.Row.Cells[0].Controls[0])).Attributes.Add("onmouseover", "document.body.style.cursor='pointer';");                
                ((System.Web.UI.WebControls.Image)(e.Row.Cells[0].Controls[0])).Attributes.Add("onmouseout", "document.body.style.cursor='default';");

                if (!string.IsNullOrEmpty(this.NestedSettings.ExpandingButtonAltText)) {
                    ((System.Web.UI.WebControls.Image)(e.Row.Cells[0].Controls[0])).Attributes.Add("title", this.NestedSettings.ExpandingButtonAltText);
                }

                ((System.Web.UI.WebControls.Image)(e.Row.Cells[0].Controls[0])).Attributes.Add("ToolTip", "Click to expand");  // dbb
            }

            if (rowIndex == this.NestedSettings.RowIndex)
            {
                bool isRowExpanded = isNestedRowExpanded(rowIndex);
                if (!isRowExpanded)
                { //expand the child grid
                    SetNestedRowExpanded(rowIndex, true);
                    if (e.Row.Cells[0].Controls[0].GetType().Name.Contains("Image")) 
                    {
                        ((System.Web.UI.WebControls.Image)(e.Row.Cells[0].Controls[0])).ImageUrl = "~/images/minus.gif";
                        ((System.Web.UI.WebControls.Image)(e.Row.Cells[0].Controls[0])).ToolTip = "Click to minimise";
                    } 
                    else
                    {  //??? why are the expanding buttons missing -- add a new expanding button image to current row
                        System.Web.UI.WebControls.ImageButton ImageColumnButton = new System.Web.UI.WebControls.ImageButton();
                        ImageColumnButton.ImageUrl = "~/images/minus.gif";
                        ImageColumnButton.BorderWidth = Unit.Pixel(0);
                        ImageColumnButton.ImageAlign = ImageAlign.AbsMiddle;
                        ImageColumnButton.ToolTip = "Click to minimise";
                        TableCell imgCell = new TableCell();
                        imgCell.ControlStyle.Width = this.NestedSettings.ExpandingButtonWidth;
                        imgCell.ControlStyle.Height = this.NestedSettings.ExpandingButtonHeight;
                        imgCell.Controls.AddAt(0, ImageColumnButton);
                        e.Row.Cells.AddAt(0, imgCell);
                        e.Row.Cells[0].Width = this.NestedSettings.ExpandingColumnWidth;
                    }
    
                    //start populating the Parent Grid
                    int cellCount = e.Row.Cells.Count;
                    colIdx = 1;
                    while (e.Row.Cells.Count > 2) {
                        e.Row.Cells.RemoveAt(e.Row.Cells.Count - 1);
                    }

                    if (this.Columns[0].GetType().Name == "CffCommandField") {
                        e.Row.Cells[0].Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffCommandField)(this.Columns[0]))).ItemStyle)).Width;
                    }
                    e.Row.Cells[1].ColumnSpan = this.Columns.Count - 1;
                    e.Row.Cells[1].Width = Unit.Percentage(100);

                    /// RESIZE THE TABLE HEADER - for some reason this gets expanded due to the columnspan above
                    Table tRowTable = new Table();
                    tRowTable.Width = Unit.Percentage(100);

                    TableRow tRowHeader = new TableRow();
                    TableCell xFillerCell = new TableCell();
                    if ((this.Columns[0] as object).GetType().Name == "CffCommandField")
                        xFillerCell.Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffCommandField)(this.Columns[0]))).ItemStyle)).Width;
                    else
                        xFillerCell.Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[0]))).ItemStyle)).Width;

                    tRowHeader.Controls.Add(xFillerCell);

                    for (int iCtr = 1; iCtr < this.HeaderRow.Cells.Count; iCtr++)
                    {
                        if (!this.Columns[iCtr].Visible)
                            continue;

                        if (this.Columns[iCtr].GetType().Name != "CffTemplateField")
                            continue;

                        CffTemplateField xGCF = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[iCtr]);
                        TableCell xHdrCell = new TableCell();
                        xHdrCell.CssClass = (string.IsNullOrEmpty(this.Columns[iCtr].HeaderStyle.CssClass)) ?
                                                ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGCF)).ItemStyle)).CssClass
                                                    : this.Columns[iCtr].HeaderStyle.CssClass;
                        // xHdrCell.BorderWidth = Unit.Pixel(0);  // dbb
                        xHdrCell.Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGCF)).ItemStyle)).Width;
                        xHdrCell.HorizontalAlign = this.Columns[iCtr].HeaderStyle.HorizontalAlign;
                        if (xHdrCell.HorizontalAlign!=System.Web.UI.WebControls.HorizontalAlign.NotSet)
                            xHdrCell.Style.Add(HtmlTextWriterStyle.TextAlign, xHdrCell.HorizontalAlign.ToString().ToLower());

                        xHdrCell.Text = ((System.Web.UI.WebControls.TableCell)(this.HeaderRow.Cells[iCtr])).Text;
                        if (string.IsNullOrEmpty(xHdrCell.Text)){
                            xHdrCell.Text = this.Columns[iCtr].HeaderText;
                        }
                        tRowHeader.Controls.Add(xHdrCell);
                    }
                    tRowHeader.Width = Unit.Percentage(100);
                    tRowTable.Rows.Add(tRowHeader);

                    if (this.HeaderRow.Cells.Count > 0) {
                        for (int iCtr = this.HeaderRow.Cells.Count - 1; iCtr > 0; iCtr--) {
                            this.HeaderRow.Cells.RemoveAt(iCtr);
                        }

                        this.HeaderRow.Cells[0].ColumnSpan = this.Columns.Count;
                        this.HeaderRow.Cells[0].Controls.Clear();
                        this.HeaderRow.Cells[0].Controls.AddAt(0, tRowTable);
                        this.HeaderRow.Cells[0].Width = Unit.Percentage(100);
                    }
                    this.HeaderRow.Width = Unit.Percentage(100);
                    /// RESIZE THE TABLE HEADER - for some reason this gets expanded due to the columnspan above

                    //start populating parent table
                    Table theParentTable = new Table();
                    theParentTable.Width = Unit.Percentage(100);

                    TableRow theParentTableRow = new TableRow();
                    theParentTableRow.Width = Unit.Percentage(100);
                    
                    CffTemplateField xGridColumnField;

                    while (colIdx < cellCount) 
                    {
                        TableCell tPDta = new TableCell();
                        object o;
                        if (((Dictionary<int, object>)(this.BoundPool)).Count == 0)
                        { //TODO
                            //int x = 0; //???? why is this missing???
                        }

                        if (((Dictionary<int, object>)(this.BoundPool)).TryGetValue(colIdx - 1, out o))
                        {
                            if (!this.Columns[colIdx].Visible) {
                                colIdx++;
                                continue;
                            }

                            if (this.Columns[colIdx].GetType().Name != "CffTemplateField") {
                                colIdx++;
                                continue;
                            }

                            xGridColumnField = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[colIdx]);
                            
                            string colTypeName = xGridColumnField.GetType().Name;
                            if (xGridColumnField.ItemTemplate != null)
                                colTypeName = xGridColumnField.ItemTemplate.GetType().Name;
                            else if (xGridColumnField.ColumnViewState != null) {
                                colTypeName = ((CffGridViewColumnType)(xGridColumnField.ColumnViewState)).ToString();
                            }
                            else if (xGridColumnField.ColumnType == CffGridViewColumnType.HyperLink)
                                colTypeName = "HyperLink";


                            tPDta.CssClass = xGridColumnField.ItemStyle.CssClass;
                            if (colTypeName.Contains("HyperLink")) 
                            {
                                HyperLink tLink = new HyperLink();
                                tLink.CssClass = xGridColumnField.ControlStyle.CssClass;
                                tLink.Width = Unit.Percentage(100);

                                if (o == null)
                                    tLink.Text = "";
                                else
                                    tLink.Text = DataBinder.Eval(e.Row.DataItem, o.ToString()).ToString();

                                bool doHyperLink = true;
                                if (xGridColumnField.ItemTemplate != null)
                                {
                                    GridViewBoundHyperLinkTemplate hyperTemplate = xGridColumnField.ItemTemplate as GridViewBoundHyperLinkTemplate;
                                    doHyperLink = hyperTemplate.IsReversed ? false:true;

                                    string navigateUrl = hyperTemplate.NavigateUrl;
                                    if (!string.IsNullOrEmpty(navigateUrl))
                                        tLink.NavigateUrl = navigateUrl;
                                    else
                                    { //check if there are filter fields
                                        if (!string.IsNullOrEmpty(hyperTemplate.BoundFilterField))
                                        {
                                            string fBoundField = hyperTemplate.BoundFilterField;
                                            string fBoundFieldValue = hyperTemplate.BoundFilterFieldValue;
                                            bool onOff = hyperTemplate.OnOff;

                                            object xS = ((IEnumerable)this.NestedSettings.childGrid.DataSource).Cast<object>().ToList()[rowIndex];
                                            object fObj = CffGenGridViewCommon.GetObjectValue(xS, fBoundField);
                                            if (fObj != null)
                                            {
                                                if (fObj.ToString() == fBoundFieldValue)
                                                    doHyperLink = onOff;
                                            }
                                        }
                                    }
                                } 

                                if (doHyperLink && string.IsNullOrEmpty(tLink.NavigateUrl))
                                {
                                    if (xGridColumnField.ToString().ToUpper().Contains("CUSTOMER"))
                                    {
                                        object oValue = CffGenGridViewCommon.GetObjectValue((e.Row.DataItem as object), "CustomerId");
                                        if (oValue != null)
                                        { //todo:  check if client or allclient scope
                                            Cff.SaferTrader.Core.LinkHelper.SetRowIndex(rowIndex.ToString());
                                            tLink.NavigateUrl = Cff.SaferTrader.Core.LinkHelper.NavigateUrlFormatToDashboardForGivenCustomerId(oValue.ToString());
                                        }
                                    }
                                }

                                if (doHyperLink && !string.IsNullOrEmpty(tLink.NavigateUrl))
                                {
                                    if (!string.IsNullOrEmpty(xGridColumnField.ControlStyle.CssClass)) {
                                        tLink.CssClass = xGridColumnField.ControlStyle.CssClass;
                                    }
                                    tLink.Attributes.Add("runat", "server");
                                    tLink.Attributes.Add("Target", "_blank");
                                    tLink.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
                                }
                                
                                if (!doHyperLink)
                                    tLink.Style.Add(HtmlTextWriterStyle.TextDecoration, "none");
                                
                                tPDta.Controls.Add(tLink);
                            }
                            else 
                            {
                                string textData = "";
                                if (o != null)
                                    textData = DataBinder.Eval(e.Row.DataItem, o.ToString()).ToString();

                                if (xGridColumnField.ColumnViewState.ToString() == "Memo") 
                                {
                                    HtmlGenericControl tMemo = new HtmlGenericControl("textarea");
                                    tMemo.Attributes.CssStyle.Value = xGridColumnField.ItemStyle.CssClass;
                                    tMemo.Attributes.Add("readonly", "true");
                                    tMemo.Style.Add(HtmlTextWriterStyle.TextAlign, "justify");
                                    tMemo.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                                    tMemo.Style.Add(HtmlTextWriterStyle.BorderColor, "none");
                                    tMemo.Style.Add(HtmlTextWriterStyle.MarginTop, "5px");
                                    tMemo.Style.Add(HtmlTextWriterStyle.MarginBottom, "5px");
                                    tMemo.Style.Add(HtmlTextWriterStyle.BackgroundColor, "transparent");
                                    tMemo.Style.Add(HtmlTextWriterStyle.Width, "80%");
                                    tMemo.Style.Add(HtmlTextWriterStyle.FontFamily, "Tahoma, Arial, Helvetica, sans-serif");
                                    tMemo.Style.Add(HtmlTextWriterStyle.FontSize, "11px");
                                    tMemo.Style.Add(HtmlTextWriterStyle.Overflow, "scroll");  
                                    tMemo.Attributes.Add("wrap", "hard"); //takes care of newlines if any

                                    tMemo.InnerText = textData;
                                    tPDta.Controls.Add(tMemo);
                                }
                                else {
                                    if (colTypeName.ToUpper().Contains("CURRENCY") || xGridColumnField.ColumnViewState.ToString().ToUpper()=="CURRENCY")
                                        tPDta.Text = string.IsNullOrEmpty(textData) ? "0.00" : Convert.ToDecimal(textData).ToString("C");
                                    else
                                        tPDta.Text = textData;
                                }
                               
                                if (tPDta.HorizontalAlign == HorizontalAlign.NotSet)
                                {
                                    tPDta.HorizontalAlign = (xGridColumnField as CffTemplateField).ItemStyle.HorizontalAlign;
                                    tPDta.Style.Add(HtmlTextWriterStyle.TextAlign, (xGridColumnField as CffTemplateField).ItemStyle.HorizontalAlign.ToString().ToLower());
                                }
                            }

                            if ((xGridColumnField.ItemStyleWidth.Value.ToString() == "100") || (xGridColumnField.ItemStyleWidth.Value.ToString() == "0"))
                                tPDta.Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).Width;
                            else
                                tPDta.Width = xGridColumnField.ItemStyleWidth;

                            
                            //tPDta.BorderWidth = Unit.Pixel(0);     // dbb
                            //theParentTable.BorderWidth = Unit.Pixel(0); 
                            theParentTableRow.Cells.Add(tPDta);
                            if (theParentTableRow.Cells[theParentTableRow.Cells.Count-1].HorizontalAlign == System.Web.UI.WebControls.HorizontalAlign.NotSet)
                            {
                                if (colTypeName.ToUpper().Contains("CURRENCY")) { 
                                    theParentTableRow.Cells[colIdx + 1].HorizontalAlign  = (xGridColumnField as CffTemplateField).ItemStyle.HorizontalAlign;
                                }
                            }
                            else if (theParentTableRow.Cells[theParentTableRow.Cells.Count - 1].HorizontalAlign == System.Web.UI.WebControls.HorizontalAlign.Right) { 
                                theParentTableRow.Cells[theParentTableRow.Cells.Count-1].Style.Add(HtmlTextWriterStyle.TextAlign,  System.Web.UI.WebControls.HorizontalAlign.Right.ToString().ToLower());
                            }

                        }
                        colIdx++;
                    }
                    
                    theParentTableRow.Width = Unit.Percentage(100);
                    theParentTableRow.BorderWidth = Unit.Point(1);   
                    theParentTableRow.BorderColor = System.Drawing.Color.LightGray;

                    theParentTable.Rows.Add(theParentTableRow);
                    theParentTable.BorderWidth = Unit.Pixel(1);
                    theParentTable.BorderColor = System.Drawing.Color.LightGray;
                    //end populating the Parent Grid

                    //start populating the child Grid
                    Table theChildTable = new Table();
                    theChildTable.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
                    //theChildTable.BorderWidth = Unit.Point(1);  // dbb
                    theChildTable.BorderColor = System.Drawing.Color.LightGray;

                    if (this.NestedSettings.childGrid.DataSource != null) 
                    {
                        IEnumerable childSource = (IEnumerable)this.NestedSettings.childGrid.DataSource;
                        int rowCtr = 0;
                        int dtaCount = childSource.Cast<object>().ToList().Count();

                        //start child table header
                        TableRow theChildTableHeaderRow = new TableRow();
                        theChildTableHeaderRow.CssClass = "dxdxgvHeader td";   // "cffGGVHeader"; // dbb

                        colIdx = 0;
                        while (colIdx < this.NestedSettings.childGrid.Columns.Count)
                        {
                            if (this.NestedSettings.childGrid.Columns[colIdx].GetType().Name != "CffTemplateField")
                                continue;

                            xGridColumnField = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.NestedSettings.childGrid.Columns[colIdx]);

                            TableCell childCell = new TableCell();
                            childCell.CssClass = xGridColumnField.ControlStyle.CssClass;   

                            Label HeaderTxt = new Label();
                            HeaderTxt.Text = xGridColumnField.HeaderText;

                            HeaderTxt.Width = Unit.Percentage(100);
                            HeaderTxt.Style.Add(HtmlTextWriterStyle.BackgroundImage, "url(../images/header_bg.png)");
                            HeaderTxt.BackColor = System.Drawing.Color.Green;
                            HeaderTxt.ForeColor = System.Drawing.Color.White;
                            HeaderTxt.Font.Bold = true;

                            childCell.Controls.Add(HeaderTxt);
                            childCell.BackColor = System.Drawing.Color.Green;
                            // childCell.BorderWidth = Unit.Pixel(1); // dbb
                            childCell.Width = xGridColumnField.ItemStyle.Width;

                            theChildTableHeaderRow.Cells.Add(childCell);
                            colIdx++;
                        }
                        theChildTable.Rows.Add(theChildTableHeaderRow);
                        //end child table header

                        //start child table rows
                        theChildTable.Attributes.Add("align", "left");
                        theChildTable.Style.Add(HtmlTextWriterStyle.MarginTop, "3px");
                        theChildTable.Style.Add(HtmlTextWriterStyle.MarginBottom, "5px");
                        theChildTable.Style.Add(HtmlTextWriterStyle.Width, "70%");
                        theChildTable.Attributes.Add("border","1");

                        while (rowCtr < dtaCount)
                        {
                            object xS = childSource.Cast<object>().ToList()[rowCtr];
                            colIdx = 0;

                            //just make these guys auto size
                            TableRow theChildRow = new TableRow();
                            //theChildRow.Width = Unit.Percentage(100);    // dbb

                            while (colIdx < this.NestedSettings.childGrid.Columns.Count)
                            {
                                if (this.NestedSettings.childGrid.Columns[colIdx].GetType().Name != "CffTemplateField")
                                    continue;

                                xGridColumnField = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.NestedSettings.childGrid.Columns[colIdx]);
                                TableCell childCell = new TableCell();
                                childCell.BackColor = System.Drawing.Color.AliceBlue;
                                childCell.ControlStyle.CssClass = xGridColumnField.ControlStyle.CssClass;
                                childCell.CssClass = xGridColumnField.ControlStyle.CssClass;

                                object objectValue = CffGenGridViewCommon.GetObjectValue(xS, xGridColumnField.DataBoundColumnName);

                                string colTemplateName = xGridColumnField.ItemTemplate.GetType().Name;
                                if (colTemplateName == "GridViewBoundHyperLinkTemplate")
                                {
                                    HyperLink tLink = new HyperLink();
                                    tLink.CssClass = xGridColumnField.ControlStyle.CssClass;
                                    tLink.Width = Unit.Percentage(100);

                                    if (objectValue != null)
                                        tLink.Text = (objectValue == null) ? "" : objectValue.ToString();
                                    else
                                        tLink.Text = "";

                                    bool doHyperLink = true;
                                    if (xGridColumnField.ItemTemplate != null)
                                    {
                                        GridViewBoundHyperLinkTemplate hyperTemplate = xGridColumnField.ItemTemplate as GridViewBoundHyperLinkTemplate;
                                        doHyperLink = hyperTemplate.IsReversed ? false : true;

                                        string navigateUrl = hyperTemplate.NavigateUrl;
                                        if (!string.IsNullOrEmpty(navigateUrl))
                                            tLink.NavigateUrl = navigateUrl;
                                        else
                                        { //check if there are filter fields
                                            if (!string.IsNullOrEmpty(hyperTemplate.BoundFilterField))
                                            {
                                                string fBoundField = hyperTemplate.BoundFilterField;
                                                string fBoundFieldValue = hyperTemplate.BoundFilterFieldValue;
                                                bool onOff = hyperTemplate.OnOff;

                                                object fObj = CffGenGridViewCommon.GetObjectValue(xS, fBoundField);
                                                if (fObj != null)
                                                {
                                                    if (fObj.ToString() == fBoundFieldValue)
                                                        doHyperLink = onOff;
                                                }
                                            }
                                        }
                                    }

                                    if (string.IsNullOrEmpty(tLink.NavigateUrl) && doHyperLink)
                                    {
                                        if (xGridColumnField.DataBoundColumnName.ToUpper().IndexOf("BATCH") >= 0)
                                        {
                                            string strCustID = SessionWrapper.Instance.Get.CustomerFromQueryString.Id.ToString();
                                            Cff.SaferTrader.Core.LinkHelper.SetRowIndex(rowIndex.ToString());

                                            if (string.IsNullOrEmpty(objectValue.ToString()))
                                                tLink.NavigateUrl = Cff.SaferTrader.Core.LinkHelper.NavigateUrlFormatToInvoiceBatchesForClient;
                                            else
                                                tLink.NavigateUrl = Cff.SaferTrader.Core.LinkHelper.NavigateUrlFormatToInvoiceBatchesForClientWBatchID(objectValue.ToString(), strCustID);
                                        }
                                    }


                                    if (!string.IsNullOrEmpty(tLink.NavigateUrl))
                                    {
                                        if (!string.IsNullOrEmpty(xGridColumnField.ControlStyle.CssClass)) {
                                            tLink.CssClass = xGridColumnField.ControlStyle.CssClass;
                                        }

                                        tLink.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
                                        tLink.Attributes.Add("runat", "server");
                                        tLink.Attributes.Add("Target", "_blank");
                                    }

                                    if (!doHyperLink)
                                    {
                                        tLink.Style.Add(HtmlTextWriterStyle.TextDecoration, "none");
                                        tLink.Style.Add(HtmlTextWriterStyle.Color, "black");
                                    }

                                    childCell.Controls.Add(tLink);
                                }
                                else if (colTemplateName == "GridViewMemoTemplate")
                                {
                                    HtmlTextArea dtaText = new HtmlTextArea();
                                    dtaText.Attributes.CssStyle.Value = xGridColumnField.ControlStyle.CssClass;
                                    dtaText.Attributes.Add("Multiline", "true");
                                    dtaText.Attributes.Add("AutoSize", "true");
                                    dtaText.Style.Add("width", "98%");
                                    dtaText.Style.Add("background-color", "AliceBlue");
                                    dtaText.Style.Add("border", "none");
                                    dtaText.Style.Add("overflow", "hidden");                               
                                    if (objectValue != null)
                                    {
                                        dtaText.InnerText = objectValue.ToString();
                                    }
                                    else
                                        dtaText.InnerText = "";

                                    if (dtaText.InnerText.Length < 170)
                                    {
                                        System.Drawing.SizeF sF = GetDrawingSize(dtaText.InnerText);
                                        dtaText.Style.Add("height", sF.Height.ToString() + "px");
                                        dtaText.Attributes.Add("cols", "170");
                                    }
                                    else {
                                        int xRow = (int)(dtaText.InnerText.Length / 170);
                                        dtaText.Style.Add("word-wrap", "break-word");
                                        dtaText.Style.Add("white-space", "pre-wrap");
                                        dtaText.Style.Add("height", "100%");
                                        dtaText.Attributes.Add("cols", "160");
                                        dtaText.Attributes.Add("rows", xRow.ToString());
                                    }
                                    dtaText.Attributes.Add("wrap", "hard");
    
                                    if (xGridColumnField.IsReadOnly)
                                        dtaText.Attributes.Add("readonly", "true");
                                    
                                    childCell.Controls.Add(dtaText);
                                }
                                else
                                {
                                    childCell.Text = "";
                                    childCell.CssClass = xGridColumnField.ControlStyle.CssClass;
                                    if (objectValue != null)
                                    {
                                        if (xGridColumnField.ColumnType == CffGridViewColumnType.Currency)
                                        {
                                            childCell.Text = (objectValue == null) ? "0.00" : (Convert.ToDecimal(objectValue)).ToString("C");
                                            if (xGridColumnField.ItemStyle.HorizontalAlign == System.Web.UI.WebControls.HorizontalAlign.NotSet)
                                                childCell.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
                                            else
                                                childCell.Style.Add(HtmlTextWriterStyle.TextAlign, xGridColumnField.ItemStyle.HorizontalAlign.ToString().ToLower());
                                        }
                                        else
                                        {
                                            childCell.Text = (objectValue == null) ? "" : objectValue.ToString();
                                        }

                                        childCell.HorizontalAlign = xGridColumnField.HorizontalAlignment;
                                    }
                                }
                                childCell.Width = xGridColumnField.ItemStyleWidth;
                                theChildRow.Cells.Add(childCell);
                                colIdx++;
                            }

                            theChildTable.Rows.Add(theChildRow);
                            theChildTable.Width = (this.NestedSettings.ChildTableWidth.Value > 0) ? this.NestedSettings.ChildTableWidth : Unit.Percentage(100);
                            rowCtr++;
                        }

                        if (this.NestedSettings.childGrid.TotalsSummarySettings.Enabled
                                && this.NestedSettings.childGrid.CustomFooterSettings.HasFlag(CffCustomFooterMode.ShowTotals))
                        { //The ChildGrid Has Totals Summary Settings - Let's Compute
                            Dictionary<string, string> ChildSummaryTotalsPool = ComputeTotalSummaryPool(this.NestedSettings.childGrid.TotalsSummarySettings.ColumnTotals.ColumnsPool, dtaCount, childSource);
                            TableRow theChildRow = new TableRow();
                            theChildRow.Width = Unit.Percentage(100);
                            theChildRow.Height = Unit.Percentage(100);

                            TableCell totalHeader = new TableCell();
                            totalHeader.Width = Unit.Percentage(100);
                            totalHeader.Height = Unit.Percentage(100);
                            totalHeader.BackColor = System.Drawing.Color.LightGray;
                            totalHeader.ForeColor = System.Drawing.Color.Black;

                            Label txtTotalHeader = new Label();

                            if (this.NestedSettings.childGrid.Columns[0].GetType().Name == "CffTemplateField")
                            {
                                txtTotalHeader.CssClass = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.NestedSettings.childGrid.Columns[0])).ControlStyle.CssClass;
                                if (string.IsNullOrEmpty(this.TotalsSummarySettings.TotalsText))
                                    txtTotalHeader.Text = "Total :";
                                else
                                    txtTotalHeader.Text = this.TotalsSummarySettings.TotalsText;
                                totalHeader.Controls.Add(txtTotalHeader);

                                if (string.IsNullOrEmpty(this.TotalsSummarySettings.TotalsTextStyle))
                                    totalHeader.CssClass = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.NestedSettings.childGrid.Columns[0])).ControlStyle.CssClass;
                                else
                                    totalHeader.CssClass = this.TotalsSummarySettings.TotalsTextStyle;

                                totalHeader.Width = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.NestedSettings.childGrid.Columns[0])).ItemStyleWidth;
                            }
                            theChildRow.Cells.Add(totalHeader);

                            for (colIdx = 1; colIdx < this.NestedSettings.childGrid.Columns.Count; colIdx++)
                            {
                                TableCell childCellCol = new TableCell();
                                childCellCol.Height = Unit.Percentage(100);
                                childCellCol.BackColor = System.Drawing.Color.LightGray;
                                childCellCol.ForeColor = System.Drawing.Color.Black;

                                Label dtaText = new Label();
                                dtaText.ForeColor = System.Drawing.Color.Black;

                                string strValue = "";

                                if (this.NestedSettings.childGrid.Columns[colIdx].GetType().Name == "CffTemplateField")
                                {
                                    ChildSummaryTotalsPool.TryGetValue(((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.NestedSettings.childGrid.Columns[colIdx])).DataBoundColumnName, out strValue);
                                    //dtaText.Text = (strValue == null) ? "" : strValue;
                                    //dtaText.CssClass = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.NestedSettings.childGrid.Columns[colIdx])).ItemStyle.CssClass;
                                    //childCellCol.Controls.Add(dtaText);

                                    childCellCol.Width = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.NestedSettings.childGrid.Columns[colIdx])).ItemStyle.Width;
                                    childCellCol.ControlStyle.CssClass = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.NestedSettings.childGrid.Columns[colIdx])).ControlStyle.CssClass;
                                    childCellCol.CssClass = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.NestedSettings.childGrid.Columns[colIdx])).ItemStyle.CssClass;
                                    childCellCol.Text = (strValue == null) ? " " : strValue;
                                    childCellCol.HorizontalAlign = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.NestedSettings.childGrid.Columns[colIdx])).ItemStyle.HorizontalAlign;
                                }
                                theChildRow.Cells.Add(childCellCol);
                            }

                            theChildTable.Rows.Add(theChildRow);
                        }
                        //end child table rows

                        //start styling the child table cells
                        rowCtr = 0;
                        while (rowCtr < theChildTable.Rows.Count)
                        {
                            colIdx = 0;
                            if (this.Columns[0].GetType().Name.ToUpper() == "CFFCOMMANDFIELD")
                                colIdx = 1;

                            while (colIdx < theChildTable.Rows[rowCtr].Cells.Count)
                            {
                                if (this.NestedSettings.childGrid.Columns[colIdx].GetType().Name == "CffTemplateField")
                                {
                                    xGridColumnField = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.NestedSettings.childGrid.Columns[colIdx]);
                                    theChildTable.Rows[rowCtr].Cells[colIdx].CssClass = xGridColumnField.ControlStyle.CssClass;
                                    theChildTable.Rows[rowCtr].Cells[colIdx].Width = xGridColumnField.ItemStyleWidth;
                                }
                                colIdx++;
                            }
                            rowCtr++;
                        }
                    }
                    //end styling the child table cells
                 
                    theParentTable.Width = Unit.Percentage(100);
                    theParentTable.Style.Add(HtmlTextWriterStyle.MarginBottom, "2px");
                    if (e.Row.Cells.Count == 1)
                        e.Row.Cells.Add(new TableCell());

                    if (this.NestedSettings.isDisplayChildrenInPanel)
                    {
                        System.Web.UI.WebControls.Panel FloatingDivTablePanel = new System.Web.UI.WebControls.Panel();
                        FloatingDivTablePanel.Controls.AddAt(0, theChildTable);
                        FloatingDivTablePanel.Controls.AddAt(0, theParentTable);

                        theChildTable.Width = Unit.Percentage(90);
                        theParentTable.Width = Unit.Percentage(90);
                        theChildTable.Height = Unit.Percentage(100);
                        theParentTable.Height = Unit.Percentage(100);

                        FloatingDivTablePanel.Width = Unit.Percentage(90);
                        FloatingDivTablePanel.Height = Unit.Percentage(100);

                        e.Row.Cells[1].Controls.Clear();
                        e.Row.Cells[1].Controls.AddAt(0, FloatingDivTablePanel);
                        e.Row.Cells[1].Height = Unit.Pixel(200);
                    }
                    else 
                    {
                        //put these guys in seperate divs
                        HtmlGenericControl FloatingDivChild = new HtmlGenericControl("div");
                        FloatingDivChild.ID = "NestedChildDiv";
                        //FloatingDivChild.Style.Add(HtmlTextWriterStyle.Width, "98%");   // dbb
                        FloatingDivChild.Style.Add(HtmlTextWriterStyle.Height, "100%");
                        FloatingDivChild.Style.Add(HtmlTextWriterStyle.Padding, "2px");
                        FloatingDivChild.Controls.Add(theChildTable);


                        HtmlGenericControl FloatingDivParent = new HtmlGenericControl("div");
                        FloatingDivParent.ID = "NestedParentDiv";
                        //FloatingDivParent.Style.Add(HtmlTextWriterStyle.Width, "100%");  // dbb
                        //FloatingDivParent.Style.Add(HtmlTextWriterStyle.Height, "100%");  //dbb
                        FloatingDivParent.Controls.Add(theParentTable);

                        HtmlGenericControl FloatingDiv = new HtmlGenericControl("div");
                        FloatingDiv.ID = "NestedGridsDiv";
                        //FloatingDiv.Style.Add(HtmlTextWriterStyle.Width, "100%");   // dbb
                        FloatingDiv.Style.Add(HtmlTextWriterStyle.Height, "100%");

                        FloatingDiv.Controls.AddAt(0, FloatingDivChild);
                        FloatingDiv.Controls.AddAt(0, FloatingDivParent);

                        e.Row.Cells[1].Controls.AddAt(0, FloatingDiv);
                        e.Row.Cells[1].Height = Unit.Percentage(100);
                    }

                    e.Row.Cells[1].Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                    //e.Row.Cells[1].BorderWidth = Unit.Pixel(0);    // dbb


                    if (this.FooterRow !=null) {
                        if (this.FooterRow.Cells.Count > 0)
                        {
                            int footerRowCellCount = this.FooterRow.Cells.Count;
                        }
                    }

                    if ((this.Columns[0] as object).GetType().Name == "CffCommandField")
                    {
                        if (this.NestedSettings.ExpandingColumnWidth.Value > 0)
                            e.Row.Cells[0].Width = this.NestedSettings.ExpandingColumnWidth;
                        else
                            e.Row.Cells[0].Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffCommandField)(this.Columns[0]))).ItemStyle)).Width;
                        // e.Row.Cells[0].BorderWidth = Unit.Pixel(0);  // dbb
                    }
                    else
                        e.Row.Cells[0].Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[0]))).ItemStyle)).Width;

                    if (this.NestedSettings.ExpandingColumnWidth.Value < 0)
                        e.Row.Cells[0].CssClass = this.Columns[0].ControlStyle.CssClass;

                    if (this.FooterRow != null) {
                        if (this.FooterRow.Cells.Count > 0)
                        {
                            if ((this.Columns[0] as object).GetType().Name == "CffCommandField")
                                this.FooterRow.Cells[0].Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffCommandField)(this.Columns[0]))).ItemStyle)).Width;
                            if (this.NestedSettings.ExpandingColumnWidth.Value <= 0)
                                this.FooterRow.Cells[0].CssClass = this.Columns[0].ControlStyle.CssClass;
                        }
                    }

                    ((System.Web.UI.WebControls.TableItemStyle)(((System.Web.UI.WebControls.WebControl)(e.Row)).ControlStyle)).Height = Unit.Percentage(100);
                    if (this.NestedSettings.isDisplayChildrenInPanel)
                        ((System.Web.UI.WebControls.TableItemStyle)(((System.Web.UI.WebControls.WebControl)(e.Row)).ControlStyle)).Height = Unit.Pixel(200);
                }
                else
                {
                    SetNestedRowExpanded(rowIndex, false);
                    if (e.Row.Cells[0].Controls.Count == 0) {
                        if (this.Columns[0].GetType().Name == "CffCommandField") {
                            expandingButton = ((CffCommandField)this.Columns[0]);
                            expandingButton.ImageButtonURL = "~/images/plus.gif";
                            expandingButton.isImageButton = true;
                            expandingButton.isExpanded = false;
                            expandingButton.ItemStyle.Width = this.NestedSettings.ExpandingButtonWidth;
                            expandingButton.ItemStyleWidth = this.NestedSettings.ExpandingButtonWidth;
                            expandingButton.ControlStyle.Width = this.NestedSettings.ExpandingButtonWidth;
                        }
                    }
                    else
                        ((System.Web.UI.WebControls.Image)(e.Row.Cells[0].Controls[0])).ImageUrl = "~/images/plus.gif";

                    DisplayNestedGridDataAsIs(e);
                }
            }
            else
            {
                DisplayNestedGridDataAsIs(e);
            }
        }

        private void DisplayNestedGridDataAsIs(GridViewRowEventArgs e)
        {
            //just display data as is
            if ((e.Row.Cells.Count > 1) && (e.Row.RowIndex >= 0))
            {
                //start repopulating the Parent Grid - for some reason this is not rendered when databind is called after rowbound;
                //also respan the columns, as everything gets moved to the right....
                CffTemplateField xGridColumnField;
                int colIdx = 1;
                int rowIdx = e.Row.RowIndex;
                
                if (e.Row.RowIndex != e.Row.DataItemIndex) //user must've clicked next page/goto page
                    rowIdx = e.Row.DataItemIndex;

                object xS = ((IEnumerable)this.DataSource).Cast<object>().ToList()[rowIdx];
                int cellCount = e.Row.Cells.Count;
                string boundColumnName = this.NestedSettings.BoundColumnName;
                object fObj = (!string.IsNullOrEmpty(boundColumnName))?CffGenGridViewCommon.GetObjectValue(xS, boundColumnName):null;

                if (e.Row.Cells[0].Controls[0].GetType().Name.Contains("Image"))  
                {
                     bool EnablePointer = true;
                     if (this.NestedSettings.Enabled && !string.IsNullOrEmpty(this.NestedSettings.BoundColumnName))
                     {
                            string[] strDummy = boundColumnName.Split(',');
                            if (strDummy.Length > 1) {
                                //do your thing for multiple filters here...
                            } 
                            else {
                                if (fObj != null)
                                {
                                    if (string.IsNullOrEmpty(fObj.ToString()))
                                    {  //todo: this would be nice if we could put this on a callback function
                                        if (this.NestedSettings.BoundColumnFilterObject != null)
                                        {
                                            //modify this part if you want to add more functionalities
                                            if (this.NestedSettings.BoundColumnFilterObject.GetType().Name == "HtmlTextWriterStyle")
                                            { //todo:: hardcode this for now - tbd - we need to find a way make this type of calls mutable
                                                ((System.Web.UI.WebControls.Image)(e.Row.Cells[0].Controls[0])).Style.Add(HtmlTextWriterStyle.Display, this.NestedSettings.BoundColumnFilterValue.ToString());
                                                 EnablePointer = false;
                                            }
                                        }
                                    }
                                }
                            }
                     }

                     if (!string.IsNullOrEmpty(this.NestedSettings.ExpandingButtonCssStyle))
                         e.Row.Cells[0].CssClass = this.NestedSettings.ExpandingButtonCssStyle;

                     if (EnablePointer) {
                        ((System.Web.UI.WebControls.Image)(e.Row.Cells[0].Controls[0])).Attributes.Add("onmouseover", "document.body.style.cursor='pointer';");
                        ((System.Web.UI.WebControls.Image)(e.Row.Cells[0].Controls[0])).Attributes.Add("onmouseout", "document.body.style.cursor='default';");

                        if (!string.IsNullOrEmpty(this.NestedSettings.ExpandingButtonAltText))
                            ((System.Web.UI.WebControls.Image)(e.Row.Cells[0].Controls[0])).Attributes.Add("title", this.NestedSettings.ExpandingButtonAltText);
                     }
                }

                while (colIdx < e.Row.Cells.Count)
                {
                        TableCell tPDta = e.Row.Cells[colIdx];
                        xGridColumnField = (Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[colIdx]);
                        tPDta.CssClass = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).CssClass;
                        tPDta.Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)(xGridColumnField)).ItemStyle)).Width;

                        object o;
                        if (((Dictionary<int, object>)(this.BoundPool)).Count == 0)
                        { ///for investigation: 
                            //where did it all go??? (check modifier types, COM exposures etc)
                            //int x = 0;
                        }

                        if (((Dictionary<int, object>)(this.BoundPool)).TryGetValue(colIdx - 1, out o))
                        {
                            string colTypeName = xGridColumnField.GetType().Name;

                            if (xGridColumnField.ItemTemplate != null)
                                colTypeName = xGridColumnField.ItemTemplate.GetType().Name;
                            else if (xGridColumnField.ColumnViewState != null)
                                colTypeName = xGridColumnField.ColumnViewState.ToString();

                            if (colTypeName == "GridViewBoundHyperLinkTemplate" || colTypeName == "HyperLink")
                            {
                                HyperLink tLink = new HyperLink();
                                tLink.CssClass = xGridColumnField.ControlStyle.CssClass;
                                tLink.Width = Unit.Percentage(100);
                                tLink.Text = "";
                                if (e.Row.DataItem == null)
                                {
                                    tLink.Text = ((o == null) ? "" : CffGenGridViewCommon.GetObjectValue(xS, o.ToString()).ToString());
                                }
                                else
                                    tLink.Text = ((o == null) ? "" : DataBinder.Eval(e.Row.DataItem, o.ToString()).ToString());

                                bool doHyperLink = true;
                                if (xGridColumnField.ItemTemplate != null)
                                {
                                    GridViewBoundHyperLinkTemplate hyperTemplate = xGridColumnField.ItemTemplate as GridViewBoundHyperLinkTemplate;
                                    doHyperLink = hyperTemplate.IsReversed ? false : true;

                                    string navigateUrl = hyperTemplate.NavigateUrl;
                                    if (!string.IsNullOrEmpty(navigateUrl))
                                        tLink.NavigateUrl = navigateUrl;
                                    else
                                    { //check if there are filter fields
                                        if (!string.IsNullOrEmpty(hyperTemplate.BoundFilterField))
                                        {
                                            string fBoundField = hyperTemplate.BoundFilterField;
                                            string fBoundFieldValue = hyperTemplate.BoundFilterFieldValue;
                                            bool onOff = hyperTemplate.OnOff;

                                            fObj = CffGenGridViewCommon.GetObjectValue(xS, fBoundField);
                                            if (fObj != null)
                                            {
                                                if (fObj.ToString() == fBoundFieldValue)
                                                    doHyperLink = onOff;
                                            }
                                        }
                                    }
                                }

                                if (doHyperLink && string.IsNullOrEmpty(tLink.NavigateUrl))
                                {
                                    if (xGridColumnField.ToString().ToUpper().IndexOf("CUSTOMER") >= 0)
                                    {
                                        object oValue = CffGenGridViewCommon.GetObjectValue((e.Row.DataItem as object), "CustomerId");
                                        if (oValue != null)
                                        { //todo:  check if client or allclients scope
                                            Cff.SaferTrader.Core.LinkHelper.SetRowIndex(e.Row.RowIndex.ToString());
                                            tLink.NavigateUrl = Cff.SaferTrader.Core.LinkHelper.NavigateUrlFormatToDashboardForGivenCustomerId(oValue.ToString());
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(tLink.NavigateUrl))
                                {
                                    if (!string.IsNullOrEmpty(xGridColumnField.ControlStyle.CssClass)) {
                                        tLink.CssClass = xGridColumnField.ControlStyle.CssClass;
                                    }

                                    tLink.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
                                    tLink.Target = "_blank";
                                }

                                if (!doHyperLink)
                                    tLink.Style.Add(HtmlTextWriterStyle.TextDecoration, "none");

                                tPDta.Controls.Clear();
                                tPDta.Controls.Add(tLink);
                            }
                            else
                            {

                                if (e.Row.DataItem == null)
                                {
                                    tPDta.Text = ((o == null) ? "" : CffGenGridViewCommon.GetObjectValue(xS, o.ToString()).ToString());
                                }
                                else
                                    tPDta.Text = ((o == null) ? "" : DataBinder.Eval(e.Row.DataItem, o.ToString()).ToString());

                                if (tPDta.CssClass.ToUpper().Contains("CURRENCY"))
                                    tPDta.Text = string.IsNullOrEmpty(tPDta.Text) ? "0.00" : Convert.ToDecimal(tPDta.Text).ToString("C");
                            }

                            if (tPDta.HorizontalAlign == HorizontalAlign.NotSet)
                            {
                                tPDta.HorizontalAlign = xGridColumnField.ItemStyle.HorizontalAlign;
                                tPDta.Style.Add(HtmlTextWriterStyle.TextAlign, xGridColumnField.ItemStyle.HorizontalAlign.ToString().ToLower());
                            }
                        }

                        // tPDta.BorderWidth = Unit.Pixel(0);  // dbb
                        colIdx++;
                    }


                    if (((this.Columns[0] as object).GetType().Name == "CffCommandField")
                                || (this.NestedSettings.Enabled && this.NestedSettings.childGrid != null))
                    {

                        if (e.Row.Cells[0].Controls.Count > 0)
                        {
                            if (this.NestedSettings.Enabled && !string.IsNullOrEmpty(this.NestedSettings.BoundColumnName))
                            {
                                if (fObj != null)
                                {
                                    if (string.IsNullOrEmpty(fObj.ToString()))
                                    {
                                        if (this.NestedSettings.BoundColumnFilterObject != null)
                                        {
                                            //modify this part if you want to add more functionalities
                                            if (this.NestedSettings.BoundColumnFilterObject.GetType().Name == "HtmlTextWriterStyle")
                                            {
                                                if (this.NestedSettings.BoundColumnFilterValue.ToString().Contains("none"))
                                                    e.Row.Cells[0].Controls[0].Visible = false;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (((System.Web.UI.WebControls.DataControlFieldCell)(e.Row.Cells[0])).ContainingField.GetType().Name == "CffTemplateField")
                                {
                                    EnableExpandingButtons();
                                }
                            }
                        }
                        else
                        {
                            e.Row.Cells[0].Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffCommandField)(this.Columns[0]))).ItemStyle)).Width;
                            if (e.Row.Cells[0].Controls.Count == 0)
                            {
                                System.Web.UI.WebControls.ImageButton ImageColumnButton = new System.Web.UI.WebControls.ImageButton();
                                ImageColumnButton.ImageUrl = "~/images/plus.gif";
                                ImageColumnButton.BorderWidth = Unit.Pixel(0);
                                ImageColumnButton.ImageAlign = ImageAlign.Left;
                                ImageColumnButton.ToolTip = "Click to expand";
                                ImageColumnButton.Click += ImageColumnButton_Click;
                                e.Row.Cells[0].Controls.AddAt(0, ImageColumnButton);
                                e.Row.Cells[0].Width = this.NestedSettings.ExpandingColumnWidth;
                                e.Row.Cells[0].BorderWidth = Unit.Pixel(0);
                            }
                        }

                    }
                    else
                        e.Row.Cells[0].Width = ((System.Web.UI.WebControls.Style)(((System.Web.UI.WebControls.DataControlField)((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffTemplateField)(this.Columns[0]))).ItemStyle)).Width;
                }
    
            // e.Row.BorderWidth = Unit.Pixel(0);  // dbb
            //end repopulating the Parent Grid - for some reason this is not rendered when databind is called after rowbound
        }

        void ImageColumnButton_Click(object sender, ImageClickEventArgs e)
        {
            if (this.RowCommand != null)
            {
                CffCommandEventArgs cx = new CffCommandEventArgs("Expand", this.NestedSettings.RowIndex.ToString());
                if (this.RowCommand != null)
                    RowCommand(this, new CffGridViewCommandEventArgs(this, ((CommandEventArgs)cx)));
                return;
            }
        }

        private void DisplayGridDataAsIs(GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1 && e.Row.RowIndex >= 0)
            {
                //start repopulating the Parent Grid - for some reason this is not rendered when databind is called after rowbound
                int colIdx = 1;
                object xS = ((IEnumerable)this.DataSource).Cast<object>().ToList()[0];
                while (colIdx < e.Row.Cells.Count)
                {
                    TableCell tCell = e.Row.Cells[colIdx];
                    int celCount = tCell.Controls.Count;
                    
                     e.Row.Cells[colIdx].CssClass = this.Columns[colIdx].ControlStyle.CssClass;
                     //e.Row.Cells[colIdx].BorderWidth = Unit.Pixel(0);  // dbb
                    colIdx++;
                }

                //e.Row.BorderWidth = Unit.Pixel(0);   // dbb
            }
        }
      

        /// <summary>
        /// Display Rows in Group By Expression; triggered by rowdatabound event
        /// </summary>
        /// <param name="e"></param>
        private void DisplayRowsInGroupBySettings(GridViewRowEventArgs e)
        { //careful when modifying this. ref mariper.
            int rowIdx = e.Row.RowIndex;
            int colIdx = 0;
            bool isGrouped = false;

            while (colIdx < this.Columns.Count - 1)
            {
                CffTemplateField xColumn = ((CffTemplateField)this.Columns[colIdx]);
                if (xColumn.GroupBySettings.IsGroupedByColumn)
                {
                    if (rowIdx == 0)
                    {
                        this.GroupBySettings.PreviousColumnValue = (DataBinder.Eval(e.Row.DataItem, xColumn.DataBoundColumnName)==null)?"":
                                                                        (DataBinder.Eval(e.Row.DataItem, xColumn.DataBoundColumnName)).ToString();
                        //this.GroupBySettings.SpannedColumnRow = rowIdx; //removed for now
                    }
                    else
                    {
                        string xColValue = (DataBinder.Eval(e.Row.DataItem, xColumn.DataBoundColumnName)==null)?null:(DataBinder.Eval(e.Row.DataItem, xColumn.DataBoundColumnName)).ToString();
                        if (!string.IsNullOrEmpty(xColValue))
                        {
                            if (xColValue == this.GroupBySettings.PreviousColumnValue)
                            {
                                //this.Rows[this.GroupBySettings.SpannedColumnRow].Cells[colIdx].RowSpan += 1;
                                TableCell tCell = e.Row.Cells[colIdx];
                                tCell.Text = "";
                                tCell.Width = xColumn.ItemStyleWidth;
                                isGrouped = true;
                            }
                            else
                            {
                                //this.GroupBySettings.SpannedColumnRow = rowIdx;
                                this.GroupBySettings.PreviousColumnValue = (DataBinder.Eval(e.Row.DataItem, xColumn.DataBoundColumnName) == null) ? "" :
                                                                                (DataBinder.Eval(e.Row.DataItem, xColumn.DataBoundColumnName)).ToString();
                            }
                        }
                    }
                }
                colIdx++;
            }

            if (this.CustomPagerSettingsMode != CffCustomPagerMode.None)
            {
                if (!isGrouped)
                {
                    this.CustomPagerGroupedBySettings.RowCounter += 1;
                    this.CustomPagerGroupedBySettings.GroupedRowCount += 1;

                    if (this.CustomPagerGroupedBySettings.RowCounter == this.PageSize)
                    {
                        this.CustomPagerGroupedBySettings.RowCounter += 1;
                        this.CustomPagerGroupedBySettings.PageCount += 1;
                    }
                }
            }
        }


        private System.Drawing.SizeF GetDrawingSize(string inStr)
        {
            System.Drawing.SizeF sizeF = new System.Drawing.SizeF();
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(1, 1));
            sizeF = graphics.MeasureString(inStr, new System.Drawing.Font("Arial,Verdana,Helvetica,sans-serif", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            return sizeF;
        }


        private void ClearEditingFormWithDataRow(GridViewRowEventArgs e)
        {
            Control RowControl = e.Row.Controls[0];
            int rIdx = e.Row.RowIndex;

            //TODO: REFACTOR THIS! as the location of the command buttons may not be always the last column
            CffCommandField cmdCol = (CffCommandField)this.Columns[this.Columns.Count - 1];
            cmdCol.ShowUpdateButton = this.EditingSettings.ShowUpdateButtonOnEdit;
            cmdCol.ShowCancelButton = this.EditingSettings.ShowCancelButtonOnEdit;

            TableCell tCell = e.Row.Cells[this.Columns.Count - 1];
            Table DataTable = new Table();
            TableRow tRow = new TableRow();

            tCell.ColumnSpan = this.Columns.Count;
            int iCtr = 0;
            int maxHeaderLen = 0;

            //Start Rendering the DataRow Table
            while (iCtr < this.Columns.Count - 1)
            {
                TableCell tdta = new TableCell();
                Label lblData = new Label();
                lblData.Text = DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName).ToString();
                tdta.Controls.Add(lblData);

                tdta.Style.Add(HtmlTextWriterStyle.Width, ((CffTemplateField)this.Columns[iCtr]).ItemStyleWidth.ToString());
                tdta.Style.Add(HtmlTextWriterStyle.BackgroundColor, "HoneyDew");
                //tdta.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");  // dbb

                tRow.Cells.Add(tdta);
                maxHeaderLen = ((this.Columns[iCtr].HeaderText.Length < maxHeaderLen) ? maxHeaderLen : this.Columns[iCtr].HeaderText.Length);
                iCtr++;
            }

            DataTable.Rows.Add(tRow);
            DataTable.Style.Add(HtmlTextWriterStyle.Padding, "0px 0px 0px 0px");
            DataTable.Style.Add(HtmlTextWriterStyle.Margin, "0px 0px 0px 0px");
            DataTable.Style.Add(HtmlTextWriterStyle.Width, "100%");
            //End Rendering the DataRow Table

            iCtr = 0;
            while (iCtr < this.Columns.Count - 1)
            {
                e.Row.Cells.RemoveAt(0); 
                iCtr++;
            }

            tCell.Controls.AddAt(0, DataTable);

            tCell.Width = Unit.Percentage(100);
            tCell.VerticalAlign = VerticalAlign.Middle;
            tCell.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
            tCell.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            tCell.BorderColor = System.Drawing.Color.LightBlue;

            ((System.Web.UI.WebControls.TableItemStyle)(((System.Web.UI.WebControls.WebControl)(e.Row)).ControlStyle)).Height = Unit.Percentage(100);
        }


        private void RestoreEditingFormWithDataRow(GridViewRowEventArgs e)
        {
            Control RowControl = e.Row.Controls[0];
            int rIdx = e.Row.RowIndex;


            CffCommandField cmdCol = (CffCommandField)this.Columns[this.Columns.Count - 1];
            cmdCol.ShowUpdateButton = this.EditingSettings.ShowUpdateButtonOnEdit;
            cmdCol.ShowCancelButton = this.EditingSettings.ShowCancelButtonOnEdit;

            TableCell tCell = e.Row.Cells[this.Columns.Count - 1];

            //note: carefull on modifying tCell; removing the command buttons does not do RowCommand PostBack
            Table DataTable = new Table();
            TableRow tRow = new TableRow();
        }


        private void DisplayAddNewFormWithDataRow(GridViewRowEventArgs e)
        {
            Control RowControl = e.Row.Controls[0];
            int rIdx = e.Row.RowIndex;

            CffCommandField cmdCol = (CffCommandField)this.Columns[this.Columns.Count - 1];
            cmdCol.ShowUpdateButton = this.EditingSettings.ShowUpdateButtonOnEdit;
            cmdCol.ShowCancelButton = this.EditingSettings.ShowCancelButtonOnEdit;

            TableCell tCell = e.Row.Cells[this.Columns.Count - 1];

            //note: carefull on modifying tCell; removing the command buttons does not do RowCommand PostBack
            Table DataTable = new Table();
            TableRow tRow = new TableRow();


            tCell.ColumnSpan = this.Columns.Count;
            int iCtr = 0;
            string strDummy = "";
            string maxHeaderString = "";
            int maxHeaderLen = 0;

            //START RENDERING THE READONLY DATA ROW TABLE
            while (iCtr < this.Columns.Count - 1)
            {
                TableCell tdta = new TableCell();
                if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Boolean)
                {
                    CheckBox cbxData = new CheckBox();
                    if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)) == true)
                        cbxData.Checked = true;

                    cbxData.Enabled = false; //set to readonly
                    tdta.Controls.Add(cbxData);
                }
                else if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Text ||
                            ((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Currency ||
                                 ((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Date ||
                                     ((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.DateTime ||
                                        ((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Time)
                {
                    Label lblData = new Label();
                    lblData.Text = DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)==null?"" :
                                        DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName).ToString();
                    tdta.Controls.Add(lblData);
                }

                else if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Memo)
                {
                    TextBox memoData = new TextBox();
                    memoData.Text = DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)==null?"" :
                                        DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName).ToString();
                    memoData.TextMode = TextBoxMode.MultiLine;
                    memoData.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                    memoData.Width = Unit.Percentage(80);
                    memoData.Height = Unit.Pixel(80);
                    memoData.Enabled = false; //set to readonly
                    tdta.Controls.Add(memoData);
                }

                //MSarza -- added code block
                else if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Dropdown)
                {

                    if (((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName == "emailReceipt")
                    {
                        Label lblData = new Label();

                        foreach (EmailReceiptType rt in Enum.GetValues(typeof(EmailReceiptType)))
                        {
                            if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)) == (int)rt)
                                lblData.Text = rt.ToString();
                        }
                        tdta.Controls.Add(lblData);
                    }

                    else
                    {
                        // implement other enum types here
                    }

                }
                else
                {
                    //TODO: handle other column types here: remember to set to readonly for textbox, checkbox or radiobutton fields etc
                }

                tdta.Style.Add(HtmlTextWriterStyle.Width, ((CffTemplateField)this.Columns[iCtr]).ItemStyleWidth.ToString());
                tdta.Style.Add(HtmlTextWriterStyle.BackgroundColor, "HoneyDew");
                tdta.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");

                tRow.Cells.Add(tdta);
                //maxHeaderLen = ((this.Columns[iCtr].HeaderText.Length < maxHeaderLen) ? maxHeaderLen : this.Columns[iCtr].HeaderText.Length);
                if (this.Columns[iCtr].HeaderText.Length > maxHeaderString.Length)
                {
                    maxHeaderString = this.Columns[iCtr].HeaderText;
                }
                
                iCtr++;
            }

            DataTable.Rows.Add(tRow);
            DataTable.Style.Add(HtmlTextWriterStyle.Padding, "0px 0px 0px 0px");
            DataTable.Style.Add(HtmlTextWriterStyle.Margin, "0px 0px 0px 0px");
            DataTable.Style.Add(HtmlTextWriterStyle.Width, "100%");
            //END RENDERING THE READONLY DATA ROW TABLE

          
            //clear the cell controls
            iCtr = 0;
            while (iCtr < this.Columns.Count - 1)
            {
                e.Row.Cells.RemoveAt(0);
                iCtr++;
            }

            //START RENDERING THE ADD NEW FORM TABLE
            Table EditFormTable = new Table();
            TableRow EditFormRow = new TableRow();

            if (this.EditColumnSettings.ColumnsPerRow == 0)
            {
                iCtr = 0;
                while (iCtr < (this.Columns.Count - 1))
                {
                    TableCell tdta = new TableCell();
                    Label label = new Label();
                    strDummy = this.Columns[iCtr].HeaderText;
                    strDummy += ": ";
                    label.Text = strDummy;

                    System.Drawing.SizeF sF = GetDrawingSize(label.Text);
                    if (sF.Width < maxHeaderLen)
                    {
                        float nHLen = 0;
                        nHLen += maxHeaderLen;
                        sF.Width = nHLen;
                    }

                    label.Width = Unit.Pixel(Convert.ToInt32(sF.Width));
                    label.Height = Unit.Pixel(Convert.ToInt32(sF.Height));

                    label.Style.Add(HtmlTextWriterStyle.PaddingLeft, "2px");
                    label.Style.Add(HtmlTextWriterStyle.MarginLeft, "0px");
                    label.Style.Add(HtmlTextWriterStyle.MarginRight, "0px");
                    label.Style.Add(HtmlTextWriterStyle.PaddingRight, "0px");
                    label.Style.Add(HtmlTextWriterStyle.MarginBottom, "0px");
                    label.Style.Add(HtmlTextWriterStyle.PaddingBottom, "0px");

                    switch (((CffTemplateField)this.Columns[iCtr]).ColumnType)
                    {
                        case CffGridViewColumnType.Boolean:
                            CheckBox CBox = new CheckBox();
                            CBox.Checked = false;
                            CBox.Enabled = true;
                            CBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                            CBox.Width = ((CffTemplateField)this.Columns[iCtr]).ItemStyle.Width;
                            CBox.Height = ((CffTemplateField)this.Columns[iCtr]).ItemStyle.Height;
                            CBox.TextAlign = TextAlign.Left;
                            CBox.Text = label.Text;

                            CBox.ID = "Cbx" + ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                            if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                                CBox.Enabled = false;

                            CBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "center");
                            CBox.Style.Add(HtmlTextWriterStyle.PaddingLeft, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.MarginLeft, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.PaddingBottom, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.MarginBottom, "0px");

                            //tdta.Controls.Add(label);
                            tdta.Controls.Add(CBox);

                            if (this.EditingSettings.EditFormColumnSize > 0)
                                tdta.Style.Add(HtmlTextWriterStyle.Width, this.EditingSettings.EditFormColumnSize.ToString() + "%");
                            break;
                                                  

                        default:
                            TextBox tBox = new TextBox();
                            tBox.ID = "Tbx" + ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                            tBox.Text = "[add new]";
                            if (tBox.Text.Length < 20)
                                tBox.Text = tBox.Text.PadRight(20);

                            if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                            {
                                tBox.ReadOnly = true;
                                tBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                            }

                            if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Memo)
                            {
                                tBox.TextMode = TextBoxMode.MultiLine;
                                tBox.Style.Add(HtmlTextWriterStyle.Width, "80%");
                                tBox.Style.Add(HtmlTextWriterStyle.Height, "80px");
                                tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                            }
                            else
                            {
                                sF = GetDrawingSize(tBox.Text);
                                tBox.Height = Unit.Pixel(Convert.ToInt32(sF.Height));
                                tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "center");

                                if (this.EditingSettings.EditFormColumnSize == 0)
                                    tBox.Style.Add(HtmlTextWriterStyle.Width, "80%");
                                else
                                    tBox.Width = Unit.Pixel(Convert.ToInt32(sF.Width));
                            }

                            tBox.Style.Add(HtmlTextWriterStyle.PaddingLeft, "0px;");
                            tBox.Style.Add(HtmlTextWriterStyle.MarginLeft, "0px;");
                            tBox.Style.Add(HtmlTextWriterStyle.PaddingBottom, "0px;");
                            tBox.Style.Add(HtmlTextWriterStyle.MarginBottom, "0px;");
                            tdta.Controls.Add(label);
                            tdta.Controls.Add(tBox);

                            if (this.EditingSettings.EditFormColumnSize > 0)
                                tdta.Style.Add(HtmlTextWriterStyle.Width, this.EditingSettings.EditFormColumnSize.ToString() + "%");
                            break;
                    }

                    EditFormRow.Controls.Add(tdta); //EditFormRow.Cells.Add(tdta); 
                    iCtr++;
                }
            }
            else
            {
                rIdx = 0;
                iCtr = 0;
                while (iCtr < (this.Columns.Count - 1))
                {
                    if (rIdx == this.EditColumnSettings.ColumnsPerRow)
                    {
                        EditFormTable.Rows.Add(EditFormRow);
                        EditFormRow = new TableRow();
                        rIdx = 0;
                    }

                    TableCell tdta = new TableCell();
                    Label label = new Label();

                    strDummy = this.Columns[iCtr].HeaderText;
                    label.Text = strDummy + " : ";


                    System.Drawing.SizeF sF = GetDrawingSize(label.Text);
                    label.Height = Unit.Pixel(Convert.ToInt32(sF.Height));
                    label.Style.Add(HtmlTextWriterStyle.VerticalAlign, "center");
                    label.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
                    sF = GetDrawingSize(maxHeaderString);
                    label.ControlStyle.Width = new Unit(sF.Width);                    

                    switch (((CffTemplateField)this.Columns[iCtr]).ColumnType)
                    {
                        case CffGridViewColumnType.Boolean:
                            CheckBox CBox = new CheckBox();
                            CBox.Checked = false;
                            CBox.Enabled = true;
                            CBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                            CBox.Width = ((CffTemplateField)this.Columns[iCtr]).ItemStyle.Width;
                            CBox.Height = ((CffTemplateField)this.Columns[iCtr]).ItemStyle.Height;
                            CBox.TextAlign = TextAlign.Left;
                            CBox.Text = label.Text;
                            CBox.ID = "Cbx" + ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                            if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                                CBox.Enabled = false;

                            CBox.Style.Add(HtmlTextWriterStyle.PaddingLeft, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.MarginLeft, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.PaddingBottom, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.MarginBottom, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.WhiteSpace, "nowrap");
                            //tdta.Controls.Add(label);
                            tdta.Controls.Add(CBox);
                            break;

                        //MSarza Added block
                        case CffGridViewColumnType.Dropdown:

                            DropDownList dDL = new DropDownList();
                            dDL.ID = "dDL" + ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                            string dDLItemLength = "";

                            if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                            {
                                dDL.Enabled = false;
                                dDL.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                            }

                            if (((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName == "emailReceipt")
                            {
                                ListItem item;

                                foreach (EmailReceiptType rt in Enum.GetValues(typeof(EmailReceiptType)))
                                {
                                    item = new ListItem(rt.ToString(), ((int)rt).ToString());
                                    dDL.Items.Add(item);
                                    if (item.Text.Length > dDLItemLength.Length){dDLItemLength = item.Text;}
                                }
                                dDL.SelectedIndex = 2;
                            }

                            dDL.Height = label.Height;

                            System.Drawing.SizeF sDDLItem = GetDrawingSize(dDLItemLength);
                            dDL.Width = Unit.Pixel(Convert.ToInt32(sDDLItem.Width)+ 30);

                            dDL.Style.Add(HtmlTextWriterStyle.VerticalAlign, "center");
                            dDL.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
                            dDL.Style.Add(HtmlTextWriterStyle.PaddingLeft, "0px");

                            tdta.Controls.Add(label);
                            tdta.Controls.Add(dDL);

                            break;

                        case CffGridViewColumnType.Memo:
                        default:
                            TextBox tBox = new TextBox();
                            tBox.ID = "Tbx" + ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                            tBox.Text = "[add new]";
                            tBox.Style.Add(HtmlTextWriterStyle.TextAlign, "left");

                            if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                            {
                                tBox.ReadOnly = true;
                                tBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                            }

                            if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Memo)
                            {
                                tBox.TextMode = TextBoxMode.MultiLine;
                                tBox.Style.Add(HtmlTextWriterStyle.Width, "80%");
                                tBox.Style.Add(HtmlTextWriterStyle.Height, "80px");
                                tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                            }
                            else
                            {
                                sF = GetDrawingSize(tBox.Text);
                                tBox.Height = Unit.Pixel(Convert.ToInt32(sF.Height));
                                tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "center");

                                if (tBox.Height.Value < 10)
                                    tBox.Height = label.Height;

                                if (this.EditingSettings.EditFormColumnSize == 0)
                                    tBox.Style.Add(HtmlTextWriterStyle.Width, "70%");
                                else
                                {
                                    tBox.Width = Unit.Pixel(Convert.ToInt32(sF.Width));
                                    if (tBox.Width.Value < 100)
                                        tBox.Width = Unit.Percentage(30);       // Unit.Percentage(50);   // dbb
                                }
                            }
                            tBox.Style.Add(HtmlTextWriterStyle.PaddingLeft, "0px;");
                            tBox.Style.Add(HtmlTextWriterStyle.MarginLeft, "0px;");
                            tBox.Style.Add(HtmlTextWriterStyle.PaddingBottom, "0px;");
                            tBox.Style.Add(HtmlTextWriterStyle.MarginBottom, "0px;");
                            //tBox.Attributes.Add("onKeyUp", "javascript:alert('ops');");
                            //tBox.TextChanged += new EventHandler(OnTextChanged); 

                            tdta.Controls.Add(label);
                            tdta.Controls.Add(tBox);
                            break;

                        //todo: handle other CffTemplateField data types here
                    }

                    //TODO: we should be able to set this padding properties outside
                    if (this.EditingSettings.EditFormColumnSize > 0)
                        tdta.Style.Add(HtmlTextWriterStyle.Width, this.EditingSettings.EditFormColumnSize.ToString() + "%");

                    tdta.Style.Add(HtmlTextWriterStyle.PaddingTop, "2px");
                    tdta.Style.Add(HtmlTextWriterStyle.PaddingLeft, "2px");
                    tdta.Style.Add(HtmlTextWriterStyle.PaddingBottom, "2px");
                    tdta.VerticalAlign = VerticalAlign.Middle;
                    tdta.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;

                    EditFormRow.Controls.Add(tdta);
                    rIdx++;
                    iCtr++;
                }
            }


            EditFormTable.Rows.Add(EditFormRow);

            EditFormTable.Style.Add(HtmlTextWriterStyle.BackgroundColor, "AliceBlue");
            EditFormTable.Style.Add(HtmlTextWriterStyle.MarginTop, "2px");
            EditFormTable.Style.Add(HtmlTextWriterStyle.MarginBottom, "2px");
            EditFormTable.Style.Add(HtmlTextWriterStyle.MarginLeft, "1px");
            EditFormTable.Style.Add(HtmlTextWriterStyle.PaddingBottom, "3px");
            EditFormTable.Style.Add(HtmlTextWriterStyle.PaddingTop, "3px");
            EditFormTable.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5px");


            EditFormTable.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            EditFormTable.BorderWidth = Unit.Pixel(1);
            EditFormTable.BorderColor = System.Drawing.Color.LightGray;
            EditFormTable.Width = Unit.Percentage(100);
            EditFormTable.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;

            EditFormTable.Visible = true;
            //END RENDERING THE ADD NEW FORM TABLE

            if (this.EditingMode == CffGridViewEditingMode.EditFormAndDisplayRow)
            {
                tCell.Controls.AddAt(0, DataTable);
                tCell.Controls.AddAt(1, EditFormTable);
                iCtr = 2;
            }
            else
            {
                tCell.Controls.AddAt(0, EditFormTable);
                iCtr = 1;
            }


            tCell.Width = Unit.Percentage(100);
            tCell.VerticalAlign = VerticalAlign.Middle;
            tCell.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
            tCell.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            tCell.BorderColor = System.Drawing.Color.LightBlue;

           
            //render save and cancel buttons
            ((System.Web.UI.WebControls.ImageButton)((e.Row.Cells[e.Row.Cells.Count - 1]).Controls[iCtr])).Visible = false;
            iCtr += 2;
            ((System.Web.UI.WebControls.ImageButton)((e.Row.Cells[e.Row.Cells.Count - 1]).Controls[iCtr])).Visible = false;
            
            /*((System.Web.UI.WebControls.ImageButton)((e.Row.Cells[e.Row.Cells.Count - 1]).Controls[iCtr])).ImageUrl = "~/images/btn_sm_add.png";
            ((System.Web.UI.WebControls.ImageButton)((e.Row.Cells[e.Row.Cells.Count - 1]).Controls[iCtr])).CommandName = "AddNew";
            ((System.Web.UI.WebControls.ImageButton)((e.Row.Cells[e.Row.Cells.Count - 1]).Controls[iCtr])).CommandArgument = e.Row.RowIndex.ToString();
            ((System.Web.UI.WebControls.ImageButton)((e.Row.Cells[e.Row.Cells.Count - 1]).Controls[iCtr])).OnClientClick = this.Page.ClientScript.GetPostBackEventReference(this, e.Row.RowIndex.ToString());
            */
            iCtr += 1;
            btnAddNew = new ImageButton();
            btnAddNew.ImageUrl = "~/images/btn_sm_add.png";
            btnAddNew.CommandArgument = e.Row.RowIndex.ToString();
            btnAddNew.CommandName = "ADD";
            btnAddNew.OnClientClick = this.Page.ClientScript.GetPostBackEventReference(this, "ADD$" + e.Row.RowIndex.ToString());

            //MSarza raise alert to support validation in Contacts.aspx.xs
            btnAddNew.Attributes.Add("onclick", "ValidationAlert();");

            (e.Row.Cells[e.Row.Cells.Count - 1]).Controls.AddAt(iCtr, btnAddNew);
            iCtr += 1;

            btnCancel = new ImageButton();
            btnCancel.ImageUrl = "~/images/btn_sm_cancel.png";
            btnCancel.CommandArgument =  e.Row.RowIndex.ToString();
            btnCancel.CommandName = "Cancel";
            btnCancel.OnClientClick = this.Page.ClientScript.GetPostBackEventReference(this, "Cancel$" + e.Row.RowIndex.ToString()); 
            (e.Row.Cells[e.Row.Cells.Count - 1]).Controls.AddAt(iCtr, btnCancel);

            ((System.Web.UI.WebControls.TableItemStyle)(((System.Web.UI.WebControls.WebControl)(e.Row)).ControlStyle)).Height = Unit.Percentage(100);

        }

      
        /// <summary>
        /// Display Editing Form With Data Row
        /// </summary>
        /// <param name="e"></param>
        /// TODO: THIS FUNCTION NEEDS REFACTORING! ref: mariper
        ///     1. The location of the command buttons may not be always the last column
        ///     2. Refactor hardcoded control style properties 
        ///             we should be able to set control style properties from outside of this function
        ///             we may need to put these on EditingSettings property of this grid (see CffGenGridViewCommon class)
        private void DisplayEditingFormWithDataRow(GridViewRowEventArgs e)
        {
            double rowHeight = this.EditingSettings.EditingFormHeight.Value;
            if (!e.Row.Height.IsEmpty)
                rowHeight = e.Row.Height.Value;

            Control RowControl = e.Row.Controls[0];
            int rIdx = e.Row.RowIndex;
          
            CffCommandField cmdCol = (CffCommandField)this.Columns[this.Columns.Count - 1];
            cmdCol.ShowUpdateButton = this.EditingSettings.ShowUpdateButtonOnEdit;
            cmdCol.ShowCancelButton = this.EditingSettings.ShowCancelButtonOnEdit;

            e.Row.Enabled = true;
            TableCell tCell = e.Row.Cells[this.Columns.Count - 1];
            
            //note: carefull on modifying tCell; removing the command buttons does not do RowCommand PostBack
            Table DataTable = new Table();
            TableRow tRow = new TableRow();

            tCell.ColumnSpan = this.Columns.Count;
            int iCtr = 0;
            string strDummy = "";
            string maxHeaderString = "";
            int maxHeaderLen = 0;

            //START RENDERING THE READONLY DATA ROW TABLE
            while (iCtr < this.Columns.Count - 1)
            {
                TableCell tdta = new TableCell();
                if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Boolean)
                {
                    CheckBox cbxData = new CheckBox();
                    if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)) == true)
                        cbxData.Checked = true;

                    cbxData.Enabled = false; //set to readonly
                    tdta.Controls.Add(cbxData);
                }
                else if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Text ||
                            ((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Currency ||
                                 ((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Date ||
                                     ((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.DateTime ||
                                        ((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Time)
                {
                    Label lblData = new Label();

                    lblData.Text = (DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName) == null) ? "" :
                                        (DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)).ToString();

                    tdta.Controls.Add(lblData);
                }

                else if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Memo)
                {
                    TextBox memoData = new TextBox();
                    memoData.Text = (DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName) == null) ? "" :
                                        (DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)).ToString();
                    memoData.TextMode = TextBoxMode.MultiLine;
                    memoData.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                    memoData.Width = Unit.Percentage(80);
                    memoData.Height = Unit.Pixel(80);
                    memoData.Enabled = false; //set to readonly
                    tdta.Controls.Add(memoData);
                }

                //MSarza -- added code block
                else if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Dropdown)
                {
                    if (((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName == "emailReceipt")
                    {
                        Label lblData = new Label();

                        foreach (EmailReceiptType rt in Enum.GetValues(typeof(EmailReceiptType)))
                        {
                            if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)) == (int)rt)
                                lblData.Text = rt.ToString();
                        }
                        //lblData.Text = (Convert.ToString(DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName));
                        tdta.Controls.Add(lblData);
                    }
                }

                else
                {
                    //TODO: handle other column types here: remember to set to readonly for textbox, checkbox or radiobutton fields etc
                }

                tdta.Style.Add(HtmlTextWriterStyle.BackgroundColor, "HoneyDew");
                tdta.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");

                if (((CffTemplateField)this.Columns[iCtr]).ItemStyleWidth == null)
                    tdta.Width = Unit.Percentage(100);
                else
                    tdta.Width = ((CffTemplateField)this.Columns[iCtr]).ItemStyleWidth;

                tdta.Height = Unit.Percentage(100);
               
               tRow.Cells.Add(tdta); 
                //maxHeaderLen = ((this.Columns[iCtr].HeaderText.Length < maxHeaderLen) ? maxHeaderLen : this.Columns[iCtr].HeaderText.Length);
                if (this.Columns[iCtr].HeaderText.Length > maxHeaderString.Length)
                {
                    maxHeaderString = this.Columns[iCtr].HeaderText;
                }
                iCtr++;
            }

            DataTable.Rows.Add(tRow);
            DataTable.Style.Add(HtmlTextWriterStyle.Padding, "0px 0px 0px 0px");
            DataTable.Style.Add(HtmlTextWriterStyle.Margin, "0px 0px 0px 0px");
            DataTable.Style.Add(HtmlTextWriterStyle.Width, "98%");
            //System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
            //if (HttpBrowserCapabilities == "Firefox")
            if (((System.Web.Configuration.HttpCapabilitiesBase)(HttpContext.Current.Request.Browser)).Browser == "Firefox") 
                DataTable.Height = Unit.Percentage(0);
            else DataTable.Height = Unit.Percentage(100);

            DataTable.Visible = true;
            ////END RENDERING THE READONLY DATA ROW TABLE
            

            //clear the cell controls
            iCtr = 0;
            while (iCtr < this.Columns.Count - 1)
            { 
                e.Row.Cells.RemoveAt(0); 
                iCtr++;
            }

            //START RENDERING THE EDITING FORM TABLE
            Table EditFormTable = new Table(); 
            TableRow EditFormRow = new TableRow(); 
            
            if (this.EditColumnSettings.ColumnsPerRow == 0)
            {
                iCtr = 0;
                while (iCtr < (this.Columns.Count - 1))
                {
                    TableCell tdta = new TableCell();  
                    Label label = new Label();
                    strDummy = this.Columns[iCtr].HeaderText;
                    strDummy += ": ";
                    label.Text = strDummy;

                    System.Drawing.SizeF sF = GetDrawingSize(label.Text);
                    if (sF.Width < maxHeaderLen) 
                    {
                        float nHLen = 0;
                        nHLen += maxHeaderLen;
                        sF.Width = nHLen;
                    }
                    
                    label.Width = Unit.Pixel(Convert.ToInt32(sF.Width));
                    label.Height = Unit.Pixel(Convert.ToInt32(sF.Height));
                    
                    label.Style.Add(HtmlTextWriterStyle.PaddingLeft, "2px");
                    label.Style.Add(HtmlTextWriterStyle.MarginLeft, "0px");
                    label.Style.Add(HtmlTextWriterStyle.MarginRight, "0px");
                    label.Style.Add(HtmlTextWriterStyle.PaddingRight, "0px");
                    label.Style.Add(HtmlTextWriterStyle.MarginBottom, "0px");
                    label.Style.Add(HtmlTextWriterStyle.PaddingBottom, "0px");

                    switch (((CffTemplateField)this.Columns[iCtr]).ColumnType)
                    {
                        case CffGridViewColumnType.Boolean:
                            CheckBox CBox = new CheckBox();
                            CBox.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName));
                            CBox.Enabled = true;
                            CBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                            CBox.Width = ((CffTemplateField)this.Columns[iCtr]).ItemStyle.Width;
                            CBox.Height = ((CffTemplateField)this.Columns[iCtr]).ItemStyle.Height;
                            CBox.TextAlign = TextAlign.Left;
                            CBox.Text = label.Text;

                            CBox.ID = "Cbx" + ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                            if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                                CBox.Enabled = false;
                            
                            CBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "center");
                            CBox.Style.Add(HtmlTextWriterStyle.PaddingLeft, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.MarginLeft, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.PaddingBottom, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.MarginBottom, "0px");
                            
                            //tdta.Controls.Add(label);
                            tdta.Controls.Add(CBox);

                            if (this.EditingSettings.EditFormColumnSize > 0)
                                tdta.Style.Add(HtmlTextWriterStyle.Width, this.EditingSettings.EditFormColumnSize.ToString() + "%");
                            break;
                    
                        default:
                            TextBox tBox = new TextBox();
                            tBox.ID = "Tbx" + ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                            tBox.Text = (DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)==null)?"":
                                               (DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)).ToString();
                            if (tBox.Text.Length < 20)
                                tBox.Text = tBox.Text.PadRight(20);

                            if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                            {
                                tBox.ReadOnly = true;
                                tBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                            }
                            else {
                                tBox.ReadOnly = false;
                                tBox.Enabled = true;
                            }

                            if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Memo)
                            {
                                tBox.TextMode = TextBoxMode.MultiLine;
                                tBox.Style.Add(HtmlTextWriterStyle.Width, "80%");
                                tBox.Style.Add(HtmlTextWriterStyle.Height, "80px");
                                tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                            }
                            else {
                                sF = GetDrawingSize(tBox.Text);
                                tBox.Height = Unit.Pixel(Convert.ToInt32(sF.Height));
                                tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "center");
                                
                                if (this.EditingSettings.EditFormColumnSize == 0)
                                    tBox.Style.Add(HtmlTextWriterStyle.Width, "80%");
                                else
                                    tBox.Width = Unit.Pixel(Convert.ToInt32(sF.Width));
                            }

                           tBox.Style.Add(HtmlTextWriterStyle.PaddingLeft, "0px;");
                           tBox.Style.Add(HtmlTextWriterStyle.MarginLeft, "0px;");
                           tBox.Style.Add(HtmlTextWriterStyle.PaddingBottom, "0px;");
                           tBox.Style.Add(HtmlTextWriterStyle.MarginBottom, "0px;");

                        
                           tdta.Controls.Add(label);
                           tdta.Controls.Add(tBox);

                           if (((CffTemplateField)this.Columns[iCtr]).ItemStyleWidth == null)
                               tdta.Width = Unit.Percentage(100);
                           else
                               tdta.Width = ((CffTemplateField)this.Columns[iCtr]).ItemStyleWidth;

                           tdta.Height = Unit.Percentage(100);

                           if (this.EditingSettings.EditFormColumnSize > 0)
                               tdta.Style.Add(HtmlTextWriterStyle.Width, this.EditingSettings.EditFormColumnSize.ToString() + "%");
                            
                           break;
                    }

                    EditFormRow.Controls.Add(tdta); //EditFormRow.Cells.Add(tdta); 
                    iCtr++;
                }
            }
            else
            {
                rIdx = 0;
                iCtr = 0;
                while (iCtr < (this.Columns.Count - 1))
                {
                    if (rIdx == this.EditColumnSettings.ColumnsPerRow)
                    {
                        EditFormTable.Rows.Add(EditFormRow);
                        EditFormRow = new TableRow();
                        rIdx = 0;
                    }

                    TableCell tdta = new TableCell(); 
                    Label label = new Label();
                    strDummy = this.Columns[iCtr].HeaderText;
                    //if (strDummy.Length < maxHeaderLen)
                    //    strDummy = strDummy.PadRight(maxHeaderLen, ' ');
                    label.Text = strDummy +  " : ";

                    System.Drawing.SizeF sF = GetDrawingSize(label.Text);
                    label.Height = Unit.Pixel(Convert.ToInt32(sF.Height));
                    label.Style.Add(HtmlTextWriterStyle.VerticalAlign, "center");
                    label.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
                    sF = GetDrawingSize(maxHeaderString);
                    label.ControlStyle.Width = new Unit(sF.Width);
                    
                    switch (((CffTemplateField)this.Columns[iCtr]).ColumnType)
                    {
                        case CffGridViewColumnType.Boolean:
                            CheckBox CBox = new CheckBox();
                            CBox.Checked = Convert.ToBoolean((DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)==null)?"0":
                                                                (DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)));
                            CBox.Enabled = true;
                            CBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                            CBox.Width = ((CffTemplateField)this.Columns[iCtr]).ItemStyle.Width;
                            CBox.Height = ((CffTemplateField)this.Columns[iCtr]).ItemStyle.Height;
                            CBox.TextAlign = TextAlign.Left;
                            CBox.Text = label.Text;
                            CBox.ID = "Cbx" + ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                            if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                                CBox.Enabled = false;

                            CBox.Style.Add(HtmlTextWriterStyle.PaddingLeft, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.MarginLeft, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.PaddingBottom, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.MarginBottom, "0px");
                            CBox.Style.Add(HtmlTextWriterStyle.WhiteSpace, "nowrap");
                            //tdta.Controls.Add(label);
                            tdta.Controls.Add(CBox);
                            break;

                        //MSarza Added block
                        case CffGridViewColumnType.Dropdown:

                            DropDownList dDL = new DropDownList();
                            dDL.ID = "dDL" + ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                            string dDLItemLength = "";

                            if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                            {
                                dDL.Enabled = false;
                                dDL.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                            }

                            if (((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName == "emailReceipt")
                            {
                                ListItem item;

                                foreach (EmailReceiptType rt in Enum.GetValues(typeof(EmailReceiptType)))
                                {
                                    item = new ListItem(rt.ToString(), ((int)rt).ToString());
                                    dDL.Items.Add(item);
                                    if (item.Text.Length > dDLItemLength.Length) {dDLItemLength = item.Text;}
                                }

                                dDL.SelectedIndex = Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName));


                            }

                            dDL.Height = label.Height;

                            System.Drawing.SizeF sDDLItem = GetDrawingSize(dDLItemLength);
                            dDL.Width = Unit.Pixel(Convert.ToInt32(sDDLItem.Width)+ 30);

                            dDL.Style.Add(HtmlTextWriterStyle.VerticalAlign, "center");
                            dDL.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
                            dDL.Style.Add(HtmlTextWriterStyle.PaddingLeft, "0px");

                            tdta.Controls.Add(label);
                            tdta.Controls.Add(dDL);

                            break;

                        case CffGridViewColumnType.Memo:
                        default:
                            TextBox tBox = new TextBox();
                            tBox.ID = "Tbx" +  ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                            tBox.Text = (DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)==null)?"":
                                                (DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName)).ToString();
                            tBox.Style.Add(HtmlTextWriterStyle.TextAlign, "left");

                            if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                            {
                                tBox.ReadOnly = true;
                                tBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                            }
                            else
                            {
                                tBox.ReadOnly = false;
                                tBox.Enabled = true;
                            }

                            if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Memo)
                            {
                                tBox.TextMode = TextBoxMode.MultiLine;
                                tBox.Style.Add(HtmlTextWriterStyle.Width, "80%");
                                tBox.Style.Add(HtmlTextWriterStyle.Height, "80px");
                                tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                            }
                            else {
                                sF = GetDrawingSize(tBox.Text);
                                tBox.Height = Unit.Pixel(Convert.ToInt32(sF.Height));
                                tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "center");

                                if (tBox.Height.Value < 10)
                                    tBox.Height = label.Height;

                                if (this.EditingSettings.EditFormColumnSize == 0)
                                    tBox.Style.Add(HtmlTextWriterStyle.Width, "70%");
                                else
                                {
                                  tBox.Width = Unit.Pixel(Convert.ToInt32(sF.Width));
                                  //if (tBox.Width.Value < 100)       // dbb
                                      tBox.Width = Unit.Percentage(30);    // previously Unit.Percentage(50);   // dbb
                                }
                            }

                            tBox.Style.Add(HtmlTextWriterStyle.PaddingLeft, "0px;");
                            tBox.Style.Add(HtmlTextWriterStyle.MarginLeft, "0px;");
                            tBox.Style.Add(HtmlTextWriterStyle.PaddingBottom, "0px;");
                            tBox.Style.Add(HtmlTextWriterStyle.MarginBottom, "0px;");
                            tdta.Controls.Add(label);
                            tdta.Controls.Add(tBox);
                            break;

                        //todo: handle other CffTemplateField data types here
                    }

                    //TODO: we should be able to set this padding properties outside
                    if (this.EditingSettings.EditFormColumnSize > 0)
                        tdta.Style.Add(HtmlTextWriterStyle.Width, this.EditingSettings.EditFormColumnSize.ToString() + "%");
                    else 
                        tdta.Width = Unit.Percentage(100);

                    tdta.Style.Add(HtmlTextWriterStyle.PaddingTop, "2px");
                    tdta.Style.Add(HtmlTextWriterStyle.PaddingLeft, "2px");
                    tdta.Style.Add(HtmlTextWriterStyle.PaddingBottom, "2px");
                    tdta.VerticalAlign = VerticalAlign.Middle;
                    tdta.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    
                    EditFormRow.Controls.Add(tdta);
                    rIdx++;
                    iCtr++;
                }
            }


            EditFormTable.Rows.Add(EditFormRow);

            EditFormTable.Style.Add(HtmlTextWriterStyle.BackgroundColor, "AliceBlue");
            EditFormTable.Style.Add(HtmlTextWriterStyle.MarginTop, "2px");
            EditFormTable.Style.Add(HtmlTextWriterStyle.MarginBottom, "2px");
            EditFormTable.Style.Add(HtmlTextWriterStyle.MarginLeft, "1px");
            EditFormTable.Style.Add(HtmlTextWriterStyle.PaddingBottom, "3px");
            EditFormTable.Style.Add(HtmlTextWriterStyle.PaddingTop, "3px");
            EditFormTable.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5px");

            EditFormTable.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid; 
            EditFormTable.BorderWidth = Unit.Pixel(1);
            EditFormTable.BorderColor = System.Drawing.Color.LightGray; 
            EditFormTable.Width = Unit.Percentage(98);
            EditFormTable.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
            //EditFormTable.Height = Unit.Percentage(98);   //MSarza: commenting out as this breaks firfox and has no effect on other browsers
            
            EditFormTable.Visible = true;
            //END RENDERING THE EDITING FORM TABLE

            iCtr = 1;
            if (this.EditingMode == CffGridViewEditingMode.EditFormAndDisplayRow)
            {
                tCell.Controls.AddAt(0, DataTable);
                tCell.Controls.AddAt(1, EditFormTable); 
                iCtr++;
            }
            else
                tCell.Controls.AddAt(0, EditFormTable);
 
            tCell.Width = Unit.Percentage(100);
            tCell.Height = Unit.Percentage(100);
            tCell.VerticalAlign = VerticalAlign.Middle;
            tCell.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
            tCell.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            tCell.BorderColor = System.Drawing.Color.LightBlue;

            //render update and cancel buttons  // MSarza added block
            ((System.Web.UI.WebControls.ImageButton)((e.Row.Cells[e.Row.Cells.Count - 1]).Controls[iCtr])).Visible = false;
            iCtr += 2;
            ((System.Web.UI.WebControls.ImageButton)((e.Row.Cells[e.Row.Cells.Count - 1]).Controls[iCtr])).Visible = false;

            iCtr += 1;
            btnUpdate = new ImageButton();
            btnUpdate.ImageUrl = "~/images/btn_sm_update.png";
            btnUpdate.CommandArgument = e.Row.RowIndex.ToString();
            btnUpdate.CommandName = "Update";
            btnUpdate.OnClientClick = this.Page.ClientScript.GetPostBackEventReference(this, "Update$" + e.Row.RowIndex.ToString());

            //MSarza raise alert to support  validation in Contacts.aspx.xs
            //btnUpdate.Attributes.Add("onclick", "return InvalidEmailAlert('" + strInvalidEmail + "');");
            //btnUpdate.Attributes.Add("onclick", "InvalidEmailAlert('" + strInvalidEmail + "');");
            btnUpdate.Attributes.Add("onclick", "ValidationAlert();");

            (e.Row.Cells[e.Row.Cells.Count - 1]).Controls.AddAt(iCtr, btnUpdate);
            iCtr += 1;

            btnCancel = new ImageButton();
            btnCancel.ImageUrl = "~/images/btn_sm_cancel.png";
            btnCancel.CommandArgument = e.Row.RowIndex.ToString();
            btnCancel.CommandName = "Cancel";
            btnCancel.OnClientClick = this.Page.ClientScript.GetPostBackEventReference(this, "Cancel$" + e.Row.RowIndex.ToString());
            (e.Row.Cells[e.Row.Cells.Count - 1]).Controls.AddAt(iCtr, btnCancel);

            ((System.Web.UI.WebControls.TableItemStyle)(((System.Web.UI.WebControls.WebControl)(e.Row)).ControlStyle)).Height = Unit.Percentage(100);
        }

        private void DisplayPopupAddNewForm(GridViewRowEventArgs e)
        {
        
            Control RowControl = e.Row.Controls[0];
            int rIdx = e.Row.RowIndex;
            CommandField cmdCol = (CommandField)this.Columns[this.Columns.Count - 1];
            TableCell tCell = e.Row.Cells[this.Columns.Count - 1];
            string rowStyleCollection = e.Row.Style.Value;


            HtmlGenericControl FloatingDiv = new HtmlGenericControl("div");
            Label label1 = new Label();
            label1.Text = "EDIT FORM";
            label1.Width = Unit.Percentage(100);
            label1.Style.Add(HtmlTextWriterStyle.BackgroundImage, "url(../images/header_bg.png)");
            label1.ForeColor = System.Drawing.Color.White;
            label1.Font.Bold = true;

            //label1.BackColor = System.Drawing.Color.AliceBlue;
            //label1.Attributes.Add("onmousedown", "selectmouse");
            //label1.Attributes.Add("onmouseup", "new Function(isdrag = false;)");
            FloatingDiv.Controls.Add(label1);

            int iCtr = 0;
            Table EditingFormTable = new Table();
            //start Generate data columns for edit 
            if (this.EditColumnSettings.ColumnsPerRow == 0)
            {
                while (iCtr < (this.Columns.Count - 1))
                {
                    TableRow EditingFormTableRow = new TableRow();
                    TableCell tdta = new TableCell();
                    Label label = new Label();
                    label.Text = this.Columns[iCtr].HeaderText + ":  ";
                    TextBox tBox = new TextBox();
                    tBox.Text = DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName).ToString();
                    if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                    {
                        tBox.ReadOnly = true;
                        tBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                    }

                    if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Memo)
                    {
                        tBox.TextMode = TextBoxMode.MultiLine;
                        tBox.Style.Add(HtmlTextWriterStyle.Height, "80px");
                        tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                    }

                    tBox.Style.Add(HtmlTextWriterStyle.Width, "90%");
                    tdta.Controls.Add(label);
                    tdta.Controls.Add(tBox);
                    EditingFormTableRow.Cells.Add(tdta);
                    iCtr++;
                }
            }
            else
            {
                rIdx = 0;
                TableRow EditingFormTableRow = new TableRow();
                EditingFormTableRow.Height = Unit.Percentage(100);
                EditingFormTableRow.Width = Unit.Percentage(100);

                while (iCtr < (this.Columns.Count - 1))
                {
                    TableCell tdta = new TableCell();
                    tdta.Height = Unit.Percentage(100);
                    tdta.Style.Add(HtmlTextWriterStyle.Width, "auto");

                    Label label = new Label();
                    label.Text = this.Columns[iCtr].HeaderText + ":  ";
                    label.Width = Unit.Percentage(98);

                    TextBox tBox = new TextBox();
                    if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                    {
                        tBox.ReadOnly = true;
                        tBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                    }

                    if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Memo)
                    {
                        tBox.TextMode = TextBoxMode.MultiLine;
                        tBox.Attributes.Add("rows", "50");
                        tBox.Style.Add(HtmlTextWriterStyle.Height, "80px");
                        tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                        tBox.Visible = true;
                        tBox.Enabled = true;
                    }

                    tBox.Text = "[add new]"; 
                    tBox.Width = Unit.Percentage(98);
                    tdta.Controls.Add(label);
                    tdta.Controls.Add(tBox);
                    EditingFormTableRow.Cells.Add(tdta);
                    EditingFormTable.Rows.Add(EditingFormTableRow);
                    iCtr++;
                    rIdx++;

                    if (rIdx == this.EditColumnSettings.ColumnsPerRow)
                    {
                        EditingFormTableRow = new TableRow();
                        EditingFormTableRow.Height = Unit.Percentage(100);
                        EditingFormTableRow.Width = Unit.Percentage(100);
                        rIdx = 0;
                    }

                }
            }
            //END GENERATE DATA COLUMNS FOR EDIT

            FloatingDiv.Controls.Add(EditingFormTable);
            ImageButton imgButton = ((ImageButton)(tCell.Controls[0]));
            FloatingDiv.Controls.Add(imgButton);
            FloatingDiv.Controls.Add(tCell.Controls[1]);

            //TODO:: WE NEED TO REFACTOR THIS - we must be be able to set control style properties from outside of this method
            //          we may need to make use of editingsettings (see CffGenGridViewCommon)
            FloatingDiv.Style.Add(HtmlTextWriterStyle.Width, "auto");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.Height, "auto");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.BackgroundColor, "HoneyDew");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.BorderWidth, "1px");

            FloatingDiv.Style.Add(HtmlTextWriterStyle.Top, "400px");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.Left, "400px");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.Position, "absolute");
            FloatingDiv.Attributes.Add("draggable", "true");
            FloatingDiv.Attributes.Add("id", "draggable");
            FloatingDiv.Attributes.Add("class", "ui-widget-content");  // FloatingDiv.Attributes.Add("class", "ui-widget-content"); 

            tCell.Controls[0].Focus();
            tCell.Controls[0].Visible = false;

            (this.Parent.FindControl(this.ID)).Controls.Add(FloatingDiv);

        }


        private void DisplayGroupByEditingForm(GridViewRowEventArgs e)
        {
            Control RowControl = e.Row.Controls[0];
            int rIdx = e.Row.RowIndex;
            CommandField cmdCol = (CommandField)this.Columns[this.Columns.Count - 1];
            int removeCellCount = e.Row.Cells.Count - 2;
              
            TableCell tCell = e.Row.Cells[this.Columns.Count - 1];
            tCell.ColumnSpan = this.Columns.Count+2;
            for (int ix = 0; ix < removeCellCount; ix++) {
                e.Row.Cells.RemoveAt(0);
            }
            tCell.Attributes.Add("colspan", tCell.ColumnSpan.ToString());
            e.Row.Cells[0].Width = Unit.Percentage(1);
            e.Row.Cells[0].Visible  = false;
            e.Row.Cells[1].Width = Unit.Percentage(100);
        
            string rowStyleCollection = e.Row.Style.Value;
            HtmlGenericControl FloatingDiv = new HtmlGenericControl("div");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.Width, "90%");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.Height, "auto");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.BackgroundColor, "HoneyDew");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.BorderWidth, "1px");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.Position, "relative");


            Label label1 = new Label();
            label1.Text = "*** EDITING ROW ***";
            label1.Width = Unit.Percentage(100);
            label1.Style.Add(HtmlTextWriterStyle.BackgroundImage, "url(../images/header_bg.png)");
            label1.ForeColor = System.Drawing.Color.White;
            label1.Font.Bold = true;

            FloatingDiv.Controls.Add(label1);
            int iCtr = 0;
            Table EditingFormTable = new Table();

            //start Generate data columns for edit 
            if (this.EditColumnSettings.ColumnsPerRow == 0)
            {
                while (iCtr < (this.Columns.Count - 1))
                {
                    TableRow EditingFormTableRow = new TableRow();
                    EditingFormTableRow.Width = Unit.Percentage(100);

                    TableCell tdta = new TableCell();
                    tdta.Height = Unit.Percentage(100);
                    tdta.Width = Unit.Percentage(98);

                    Label label = new Label();
                    label.Width = Unit.Percentage(98);
                    label.Text = this.Columns[iCtr].HeaderText + ":  ";

                    TextBox tBox = new TextBox();
                    tBox.Width = Unit.Percentage(78);
                    tBox.ID = "Tbx" + ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                    tBox.Text = DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName).ToString();
                    if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                    {
                        tBox.ReadOnly = true;
                        tBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                    }


                    if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Memo)
                    {
                        tBox.TextMode = TextBoxMode.MultiLine;
                        tBox.Style.Add(HtmlTextWriterStyle.Height, "80px");
                        tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                    }

                    tdta.Controls.Add(label);
                    tdta.Controls.Add(tBox);
                    EditingFormTableRow.Cells.Add(tdta);
                    //EditingFormTableRow.Cells[iCtr].Width = Unit.Percentage(100);
                    EditingFormTable.Rows.Add(EditingFormTableRow);
                    iCtr++;
                }
            }
            else
            {
                rIdx = 0;
                int CellPWidth = (int)(100 / (this.EditColumnSettings.ColumnsPerRow-1));
                TableRow EditingFormTableRow = new TableRow();
                EditingFormTableRow.Height = Unit.Percentage(100);
                EditingFormTableRow.Width = Unit.Percentage(90);

                while (iCtr < (this.Columns.Count - 1))
                {
                    TableCell tdtaCell = new TableCell();
                    tdtaCell.Height = Unit.Percentage(100);
                    tdtaCell.Width = Unit.Percentage(CellPWidth);
                    tdtaCell.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");

                    TextBox tBoxHdr = new TextBox();
                    
                    tBoxHdr.Text = this.Columns[iCtr].HeaderText + ":  ";
                    tBoxHdr.ReadOnly = true;
                    tBoxHdr.BorderWidth = Unit.Pixel(0);
                    tBoxHdr.Width = Unit.Percentage(28);
                    tBoxHdr.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                    
                    TextBox tBox = new TextBox();
                    if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                    {
                        tBox.ReadOnly = true;
                        tBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                        tBox.Width = Unit.Percentage(68);
                    }

                    if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Memo)
                    {
                        tBox.TextMode = TextBoxMode.MultiLine;
                        tBox.Attributes.Add("rows", "50");
                        tBox.Style.Add(HtmlTextWriterStyle.Height, "80px");
                        tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                        tBox.Visible = true;
                        tBox.Enabled = true;
                        tBox.Width = Unit.Percentage(100);
                    }

                    tBox.ID = "Tbx" + ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                    tBox.Text = DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName).ToString();
                    

                    tdtaCell.Controls.Add(tBoxHdr);
                    tdtaCell.Controls.Add(tBox);
                    tdtaCell.Style.Add(HtmlTextWriterStyle.Display, "table-cell");
                    if (iCtr == (this.Columns.Count - 2) && (rIdx <= (this.EditColumnSettings.ColumnsPerRow))) {
                        tdtaCell.ColumnSpan = (this.EditColumnSettings.ColumnsPerRow - (rIdx+1)); 
                        tdtaCell.Attributes.Add("colspan", iCtr.ToString());
                    }
                    EditingFormTableRow.Cells.Add(tdtaCell);
                    iCtr++;
                    rIdx++;

                    if (rIdx == (this.EditColumnSettings.ColumnsPerRow-1) || iCtr == (this.Columns.Count-1))
                    {
                        EditingFormTable.Rows.Add(EditingFormTableRow);
                        EditingFormTableRow = new TableRow();
                        EditingFormTableRow.Height = Unit.Percentage(100);
                        EditingFormTableRow.Width = Unit.Percentage(100);
                        rIdx = 0;
                    }
                }
            }
            //END GENERATE DATA COLUMNS FOR EDIT

            FloatingDiv.Controls.Add(EditingFormTable);
            ImageButton imgButton = ((ImageButton)(tCell.Controls[0]));
            FloatingDiv.Controls.Add(imgButton);
            FloatingDiv.Controls.Add(tCell.Controls[1]);

            //TODO:: WE NEED TO REFACTOR THIS - we must be be able to set control style properties from outside of this method
            //          we may need to make use of editingsettings (see CffGenGridViewCommon)
            FloatingDiv.Attributes.Add("draggable", "true");
            FloatingDiv.Attributes.Add("id", "draggable");
            FloatingDiv.Attributes.Add("class", "ui-widget-content");  // FloatingDiv.Attributes.Add("class", "ui-widget-content"); 

            tCell.Controls.AddAt(0, FloatingDiv);
            //(((System.Web.UI.WebControls.ImageButton)(tCell.Controls[0].Controls[2]))).ImageAlign = ImageAlign.Right;
            //((System.Web.UI.WebControls.ImageButton)(tCell.Controls[0].Controls[3])).ImageAlign = ImageAlign.Right;
            tCell.Controls[0].Controls[0].Focus();
            e.Row.Height = Unit.Percentage(100);

        }

        private void DisplayPopupEditingForm(GridViewRowEventArgs e)
        {
            Control RowControl = e.Row.Controls[0];
            int rIdx = e.Row.RowIndex;
            CommandField cmdCol = (CommandField)this.Columns[this.Columns.Count - 1];
            TableCell tCell = e.Row.Cells[this.Columns.Count - 1];
            string rowStyleCollection = e.Row.Style.Value;


            HtmlGenericControl FloatingDiv = new HtmlGenericControl("div");
            Label label1 = new Label();
            label1.Text = "EDIT FORM";
            label1.Width = Unit.Percentage(100);
            label1.Style.Add(HtmlTextWriterStyle.BackgroundImage, "url(../images/header_bg.png)");
            label1.ForeColor = System.Drawing.Color.White;
            label1.Font.Bold = true;

            FloatingDiv.Controls.Add(label1);
            int iCtr = 0;
            Table EditingFormTable = new Table();

            //start Generate data columns for edit 
            if (this.EditColumnSettings.ColumnsPerRow == 0)
            {
                while (iCtr < (this.Columns.Count - 1))
                {
                    TableRow EditingFormTableRow = new TableRow();
                    TableCell tdta = new TableCell();
                    Label label = new Label();
                    label.Text = this.Columns[iCtr].HeaderText + ":  ";
                    TextBox tBox = new TextBox();
                    tBox.ID = "Tbx" + ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                    tBox.Text = DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName).ToString();
                    if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                    {
                        tBox.ReadOnly = true;
                        tBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                    }

                    if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Memo)
                    {
                        tBox.TextMode = TextBoxMode.MultiLine;
                        tBox.Style.Add(HtmlTextWriterStyle.Height, "80px");
                        tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                    }

                    tBox.Style.Add(HtmlTextWriterStyle.Width, "90%");
                    tdta.Controls.Add(label);
                    tdta.Controls.Add(tBox);
                    EditingFormTableRow.Cells.Add(tdta);
                    EditingFormTable.Rows.Add(EditingFormTableRow);
                    iCtr++;
                }
            }
            else
            {
                rIdx = 0;
                TableRow EditingFormTableRow = new TableRow();
                EditingFormTableRow.Height = Unit.Percentage(100);
                EditingFormTableRow.Width = Unit.Percentage(100);

                while (iCtr < (this.Columns.Count - 1))
                {
                    TableCell tdta = new TableCell();
                    tdta.Height = Unit.Percentage(100);
                    tdta.Style.Add(HtmlTextWriterStyle.Width, "auto");

                    Label label = new Label();
                    label.Text = this.Columns[iCtr].HeaderText + ":  ";
                    label.Width = Unit.Percentage(98);

                    TextBox tBox = new TextBox();
                    if (((CffTemplateField)this.Columns[iCtr]).IsReadOnly)
                    {
                        tBox.ReadOnly = true;
                        tBox.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                    }

                    if (((CffTemplateField)this.Columns[iCtr]).ColumnType == CffGridViewColumnType.Memo)
                    {
                        tBox.TextMode = TextBoxMode.MultiLine;
                        tBox.Attributes.Add("rows", "50");
                        tBox.Style.Add(HtmlTextWriterStyle.Height, "80px");
                        tBox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                        tBox.Visible = true;
                        tBox.Enabled = true;
                    }

                    tBox.ID = "Tbx" + ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName;
                    tBox.Text = DataBinder.Eval(e.Row.DataItem, ((CffTemplateField)this.Columns[iCtr]).DataBoundColumnName).ToString();
                    tBox.Width = Unit.Percentage(98);
                    tdta.Controls.Add(label);
                    tdta.Controls.Add(tBox);
                    EditingFormTableRow.Cells.Add(tdta);
                    EditingFormTable.Rows.Add(EditingFormTableRow);
                    iCtr++;
                    rIdx++;

                    if (rIdx == this.EditColumnSettings.ColumnsPerRow)
                    {
                        EditingFormTableRow = new TableRow();
                        EditingFormTableRow.Height = Unit.Percentage(100);
                        EditingFormTableRow.Width = Unit.Percentage(100);
                        rIdx = 0;
                    }

                }
            }
            //END GENERATE DATA COLUMNS FOR EDIT
            
            FloatingDiv.Controls.Add(EditingFormTable);
            ImageButton imgButton = ((ImageButton)(tCell.Controls[0]));
            FloatingDiv.Controls.Add(imgButton);
            FloatingDiv.Controls.Add(tCell.Controls[1]);

            //TODO:: WE NEED TO REFACTOR THIS - we must be be able to set control style properties from outside of this method
            //          we may need to make use of editingsettings (see CffGenGridViewCommon)
            //FloatingDiv.Style.Add(HtmlTextWriterStyle.Width, "auto");
            //FloatingDiv.Style.Add(HtmlTextWriterStyle.Height, "auto");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.Width, "800px");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.Height, "400px");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.BackgroundColor, "HoneyDew");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.BorderWidth, "1px");

            //FloatingDiv.Style.Add(HtmlTextWriterStyle.Top, "400px");
            //FloatingDiv.Style.Add(HtmlTextWriterStyle.Left, "400px");
            FloatingDiv.Style.Add(HtmlTextWriterStyle.Position, "relative");
            FloatingDiv.Attributes.Add("draggable", "true");
            FloatingDiv.Attributes.Add("id", "draggable");
            FloatingDiv.Attributes.Add("class", "ui-widget-content");  // FloatingDiv.Attributes.Add("class", "ui-widget-content"); 

            tCell.Controls.AddAt(0, FloatingDiv);
            tCell.Controls[1].Focus();

            e.Row.Cells.RemoveAt(0);

            //(this.Parent.FindControl(this.ID)).Controls.Add(FloatingDiv);
            //((System.Web.UI.WebControls.TableItemStyle)(((System.Web.UI.WebControls.WebControl)(e.Row)).ControlStyle)).Height = this.EditingSettings.EditingFormHeight;
        }


        #endregion

        #region "PublicFunctions"
        /// <summary>
        /// <para>Insert Bound Data Column</para>
        /// <para>use parameter CssStyle to create or specify data cell alignment (@ css file use "text-align" )</para>
        /// <para>You need to set ReadOnly = true when you do not want this column to be edited in edit mode</para>
        /// </summary>
        public void InsertDataColumn(string HeaderName, string ColumnName, CffGridViewColumnType ColumnType = CffGridViewColumnType.Text,
                                           string ColumnWidth = "100%", string CssStyle = "cffGGV_leftAlignedCell",
                                            HorizontalAlign ColumnDataHorizontalAlignment = HorizontalAlign.NotSet,
                                            HorizontalAlign HeaderHorizontalAlignment = HorizontalAlign.NotSet,
                                                    bool ReadOnly = false, string HeaderCssStyle = "cffGGVHeader")
        {
            CffTemplateField theColumn = new CffTemplateField(ColumnType);
            theColumn.HeaderText = HeaderName;
            theColumn.DataBoundColumnName = ColumnName;
            theColumn.HeaderTemplate = new GridViewHeaderLabelTemplate(HeaderName);
            theColumn.HeaderStyle.CssClass = HeaderCssStyle;
            theColumn.ControlStyle.CssClass = CssStyle;
            theColumn.ItemStyle.CssClass = CssStyle;
          
            if (HeaderHorizontalAlignment != System.Web.UI.WebControls.HorizontalAlign.NotSet)
                theColumn.HeaderStyle.HorizontalAlign = HeaderHorizontalAlignment;
            else
                theColumn.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;


            if (ColumnDataHorizontalAlignment != System.Web.UI.WebControls.HorizontalAlign.NotSet)
            {
                theColumn.HorizontalAlignment = ColumnDataHorizontalAlignment;
                theColumn.ItemStyle.HorizontalAlign = ColumnDataHorizontalAlignment;
            }
            else
            {
                theColumn.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                theColumn.ItemStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            }

            switch (ColumnType)
            { 
                case CffGridViewColumnType.Memo:
                    theColumn.ItemTemplate = new GridViewMemoTemplate(ColumnType, ColumnName, HeaderName, ColumnWidth, CssStyle, ReadOnly);
                    break;

                case CffGridViewColumnType.Boolean:
                    theColumn.ItemTemplate = new GridViewBooleanTemplate(ColumnType, ColumnName, HeaderName, ColumnWidth, CssStyle, ReadOnly);
                    break;

                //MSarza -- added case block 
                case CffGridViewColumnType.Dropdown:
                    theColumn.ItemTemplate = new GridViewDropdownTemplate(ColumnType, ColumnName, HeaderName, ColumnWidth, CssStyle, ReadOnly);
                    break;

                default:
                    theColumn.ItemTemplate = new GridViewDataTemplate(ColumnType, ColumnName, HeaderName, ColumnWidth, CssStyle, ReadOnly);
                    theColumn.ItemStyle.Wrap = false;
                    break;
            }

            if (ColumnWidth.IndexOf("%") >= 0)
            {
                ColumnWidth = ColumnWidth.Replace("%", "");
                theColumn.ItemStyle.Width = Unit.Percentage(Convert.ToDouble(ColumnWidth));
            }
            else
                theColumn.ItemStyle.Width = Unit.Pixel(Convert.ToInt32(ColumnWidth));

            theColumn.ControlStyle.BorderWidth = Unit.Pixel(0);
            theColumn.ItemStyleWidth = theColumn.ItemStyle.Width;
            theColumn.IsReadOnly = ReadOnly;

            this.Columns.Add(theColumn);

        }


        /// <summary>
        /// <para>Insert Right Aligned Data Column</para>
        /// <para>use parameter CssStyle to create or specify data cell alignment (@ css file use "text-align" )</para>
        /// <para>You need to set ReadOnly = true when you do not want this column to be edited in edit mode</para>
        /// </summary>
        public void InsertRightAlignedDataColumn(string HeaderName, string ColumnName, CffGridViewColumnType ColumnType = CffGridViewColumnType.Text,
                                                     string ColumnWidth = "100%", string CssStyle = "cffGGV_leftAlignedCell", bool ReadOnly = false,
                                                                         HorizontalAlign HeaderHorizontalAlignment = HorizontalAlign.Center)
        {
            CffTemplateField theColumn = new CffTemplateField();
            theColumn.HeaderText = HeaderName;
            theColumn.DataBoundColumnName = ColumnName;
            theColumn.ControlStyle.CssClass = CssStyle;
            theColumn.HeaderTemplate = new GridViewHeaderLabelTemplate(HeaderName);
            theColumn.HeaderStyle.CssClass = "cffGGVHeader";
            theColumn.HeaderStyle.HorizontalAlign = HeaderHorizontalAlignment;

            theColumn.ItemTemplate = new GridViewDataTemplate(ColumnType, ColumnName, HeaderName, ColumnWidth);
            theColumn.ItemStyle.CssClass = CssStyle;
            theColumn.ItemStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;

            theColumn.ColumnType = ColumnType;
            theColumn.IsReadOnly = ReadOnly;

            if (ColumnWidth.IndexOf("%") >= 0)
            {
                ColumnWidth = ColumnWidth.Replace("%", "");
                theColumn.ItemStyle.Width = Unit.Percentage(Convert.ToDouble(ColumnWidth));
            }
            else
                theColumn.ItemStyle.Width = Unit.Pixel(Convert.ToInt32(ColumnWidth));

            theColumn.ItemStyleWidth = theColumn.ItemStyle.Width;
            theColumn.ControlStyle.BorderWidth = Unit.Pixel(0);

            this.Columns.Add(theColumn);
        }

        /// <summary>
        /// <para>Insert Currency Column</para>
        /// <para>Currency format is defaulted to display with $sign and 2 decimal digit</para>
        /// <para>use parameter CssStyle to create or specify data cell alignment (@ css file use "text-align" )</para>
        /// </summary>
        public void InsertCurrencyColumn(string HeaderName,  string ColumnName, string ColumnWidth = "100%", 
                string ColumnCssStyle= "cffGGV_leftAlignedCell", bool ReadOnly = false,
                    HorizontalAlign ColumnHorizontalAlignment = HorizontalAlign.Right,  
                            HorizontalAlign HeaderHorizontalAlignment = HorizontalAlign.Center,
                                string HeaderCssStyle = "cffGGVHeader")
        {
            CffTemplateField theColumn = new CffTemplateField(CffGridViewColumnType.Currency);
            theColumn.HeaderText = HeaderName;
            theColumn.DataBoundColumnName = ColumnName;
            theColumn.ControlStyle.CssClass = ColumnCssStyle;

            theColumn.HeaderTemplate = new GridViewHeaderLabelTemplate(HeaderName);
            theColumn.HeaderStyle.CssClass = HeaderCssStyle;
            theColumn.HeaderStyle.HorizontalAlign = HeaderHorizontalAlignment;

            theColumn.HorizontalAlignment = ColumnHorizontalAlignment;
            theColumn.IsReadOnly = ReadOnly;

            theColumn.ItemStyle.HorizontalAlign = ColumnHorizontalAlignment;
            theColumn.ItemTemplate = new GridViewDataTemplate(CffGridViewColumnType.Currency, ColumnName, HeaderName, ColumnWidth, ColumnCssStyle);
            theColumn.ItemStyle.CssClass = ColumnCssStyle;
            if (string.IsNullOrEmpty(ColumnCssStyle))
                theColumn.ItemStyle.HorizontalAlign = ColumnHorizontalAlignment;


            if (ColumnWidth.IndexOf("%") >= 0)
            {
                ColumnWidth = ColumnWidth.Replace("%", "");
                theColumn.ItemStyle.Width = Unit.Percentage(Convert.ToDouble(ColumnWidth));
            }
            else
                theColumn.ItemStyle.Width = Unit.Pixel(Convert.ToInt32(ColumnWidth));

            theColumn.ControlStyle.BorderWidth = Unit.Pixel(0);
            theColumn.ItemStyleWidth = theColumn.ItemStyle.Width;
            this.Columns.Add(theColumn);
        }

        /// <summary>
        /// <para>Insert Bound Hyperlink Column</para>
        /// <para>Display Fieldname is the text displayed in the HyperLink</para>
        /// <para>Bound Fieldname is the actual data value of the HyperLink</para>
        /// <para>CssStyle:  use parameter CssStyle to create or specify data cell alignment (@css file use "text-align" )</para>
        /// <para>Column and Header Alignment: Use HorizontalAlign to manually set column alignment)</para>
        /// <para>Hyper Link Filters: populate this parameter to turn on/off the hyperlink for certain fieldvalues @databind ("{FieldName:FieldValue:On/Off}:{'Reversed'/'Arrayed'}")</para>
        /// <para>when 'Reversed' is set, default hyperlink navigation is disabled by default and will be turned on/off @ hyperlinkFilter settings's fieldname and value</para>
        /// <para>when 'Arrayed' is set, default hyperlink navigation is enabled by default and filtered settings are arrayed by {} separated with ',' eg "{fieldname1:fieldvalue1:off},{fieldname2:fieldvalue2:off}"</para>
        /// </summary>
        public void InsertBoundHyperLinkColumn(string HeaderName, string DisplayFieldName, string BoundFieldName,
                                                     string ColumnWidth = "100%", string CssStyle = "cffGGV_HyperLink",
                                                                     HorizontalAlign ColumnAlignment = HorizontalAlign.NotSet,
                                                                         HorizontalAlign HeaderAlignment = HorizontalAlign.NotSet,
                                                                                string hyperLinkFilters="", string HeaderCssStyle = "cffGGVHeader")
        {
            CffTemplateField theColumn = new CffTemplateField(CffGridViewColumnType.HyperLink);
            theColumn.HeaderText = HeaderName;
            theColumn.DataBoundColumnName = DisplayFieldName;
            theColumn.ControlStyle.CssClass = CssStyle;
            theColumn.ItemStyle.CssClass = CssStyle;

            theColumn.HeaderTemplate = new GridViewHeaderLabelTemplate(HeaderName);
            theColumn.ItemTemplate = new GridViewBoundHyperLinkTemplate(CffGridViewColumnType.HyperLink, DisplayFieldName, BoundFieldName, hyperLinkFilters, CssStyle);
            theColumn.HeaderStyle.CssClass = HeaderCssStyle;
            theColumn.HeaderStyle.VerticalAlign = VerticalAlign.Middle;

            if (HeaderAlignment == System.Web.UI.WebControls.HorizontalAlign.NotSet)
                theColumn.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
            else
                theColumn.HeaderStyle.HorizontalAlign = HeaderAlignment;

            if (ColumnAlignment == HorizontalAlign.NotSet)
                theColumn.HorizontalAlignment = System.Web.UI.WebControls.HorizontalAlign.Left;
            else
                theColumn.HorizontalAlignment = ColumnAlignment;

            theColumn.ColumnType = CffGridViewColumnType.HyperLink;
     
            if (ColumnWidth.IndexOf("%") >= 0)
            {
                ColumnWidth = ColumnWidth.Replace("%", "");
                theColumn.ControlStyle.Width = Unit.Percentage(Convert.ToDouble(ColumnWidth));
            }
            else
                theColumn.ControlStyle.Width = Unit.Pixel(Convert.ToInt32(ColumnWidth));

            theColumn.ItemStyle.Wrap = false;
            theColumn.ControlStyle.BorderWidth = Unit.Pixel(0);
            theColumn.ItemStyle.Width = theColumn.ControlStyle.Width;
            theColumn.ItemStyleWidth = theColumn.ControlStyle.Width;
            theColumn.HeaderStyle.BorderWidth = Unit.Pixel(0);
            theColumn.IsReadOnly = true;
            
            this.Columns.Add(theColumn);
        }


        ///<summary>
        ///<para>Insert Unbound Image Command Button for specialised requirements</para> 
        ///<para>ButtonType = "Image_batch", "Image_estimate", "Image_retention"</para>
        ///<para>OnClick += YourClickEventHandler()</para>
        ///</summary>
        public void InsertImageCommandButtonColumn(string HeaderName, CffGridViewButtonType BtnType, string CssStyle = "cffGV_CommandButtons",
                                                                     HorizontalAlign HorizontalAlignment = HorizontalAlign.NotSet)
        {
            CffCommandField theColumn = new CffCommandField();
            theColumn.ButtonType = System.Web.UI.WebControls.ButtonType.Image;
            theColumn.ControlStyle.CssClass = CssStyle;

            theColumn.HeaderText = HeaderName;
            theColumn.HeaderStyle.CssClass = "dxgvHeader";   //"cffGGVHeader";  //dbb
            
            theColumn.InsertImageUrl = "~/images/btn_view_batch.png";
            theColumn.EditImageUrl = "~/images/btn_view_estimate.png";
            theColumn.CancelImageUrl = "~/images/btn_view_retention.png";

            //NOTE: TO REFACTOR THIS!
            // insert, edit and cancel will behave differently as:
            // (1) edit - will put the row in edit state and fire an edit event before going to the RowCommand event handler
            // (2) insert - inserts a new row and fires an insert event before going to the RowCommand event handler
            // (3) cancel - cancels row edit
            // if you just want to do an edit and display different images for each type, just change the ImageURL and CffCommandField.commandTag value
            // if you want to reuse this command button to fire up an event, set the CffCommandField.isImageButton to true and handle the "Image" command tag inside your aspx
            if (BtnType.HasFlag(CffGridViewButtonType.Image_batch))
                theColumn.ShowInsertButton = true;

            if (BtnType.HasFlag(CffGridViewButtonType.Image_estimate))
                theColumn.ShowEditButton = true;

            if (BtnType.HasFlag(CffGridViewButtonType.Image_retention))
                theColumn.ShowCancelButton = true;

            theColumn.ItemStyle.CssClass = CssStyle;
            theColumn.ItemStyle.VerticalAlign = VerticalAlign.Middle;
            theColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            theColumn.ControlStyle.BorderWidth = Unit.Pixel(0);

            if (HorizontalAlignment == HorizontalAlign.NotSet)
                theColumn.HorizontalAlignment = HorizontalAlign.Center;
            else
                theColumn.HorizontalAlignment = HorizontalAlignment;

            this.Columns.Add(theColumn);
        }


        /// <summary>
        ///<para>Usage: Insert an Unbound Command Button Column</para>
        ///<para>ButtonType = "Add/New/Insert", "Edit", "Update", "Cancel", "Delete"</para>
        ///<para>OnClick += YourClickEventHandler() OR use the RowCommand event and handle manually</para>
        ///<para>To Display Multiple Buttons XOR button types with each other - see Contacts.aspx.cs for reference</para>
        ///</summary>
        public void InsertCommandButtonColumn(string HeaderName, CffGridViewButtonType BtnType, 
                                                    string CssStyle = "cffGGV_CommandButtons",
                                                            HorizontalAlign HorizontalAlignment = HorizontalAlign.NotSet,
                                                                string ColumnWidth = "10%")
        {
            CffCommandField theColumn = new CffCommandField();
            theColumn.ButtonType = System.Web.UI.WebControls.ButtonType.Image;
            theColumn.ControlStyle.CssClass = CssStyle;

            theColumn.HeaderText = HeaderName;
            theColumn.HeaderStyle.CssClass = "cffGGVHeader";
            theColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

            if (ColumnWidth.IndexOf("%") >= 0)
            {
                ColumnWidth = ColumnWidth.Replace("%", "");
                theColumn.ItemStyle.Width = Unit.Percentage(Convert.ToDouble(ColumnWidth));
            }
            else
                theColumn.ItemStyle.Width = Unit.Pixel(Convert.ToInt32(ColumnWidth));
            theColumn.ItemStyleWidth = theColumn.ItemStyle.Width;

            if (HorizontalAlignment == HorizontalAlign.NotSet)
                theColumn.HorizontalAlignment = HorizontalAlign.Center;
            else
                theColumn.HorizontalAlignment = HorizontalAlignment;

            theColumn.ItemStyle.CssClass = CssStyle;
            theColumn.ItemStyle.VerticalAlign = VerticalAlign.Top;  //.Middle; // dbb
            theColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            theColumn.ControlStyle.BorderWidth = Unit.Pixel(0);


            theColumn.AddImageUrl    = "~/images/btn_sm_add.png";
            theColumn.EditImageUrl   = "~/images/btn_sm_edit.png";
            theColumn.CancelImageUrl = "~/images/btn_sm_cancel.png";
            theColumn.UpdateImageUrl = "~/images/btn_sm_update.png";
            theColumn.NewImageUrl    = "~/images/btn_sm_add.png";
           
            if (BtnType.HasFlag(CffGridViewButtonType.Add))
                theColumn.ShowAddButton = true;

            if (BtnType.HasFlag(CffGridViewButtonType.Edit))
                theColumn.ShowEditButton = true;

            if (BtnType.HasFlag(CffGridViewButtonType.Cancel))
                theColumn.ShowCancelButton = true;

            if (BtnType.HasFlag(CffGridViewButtonType.Update))
                theColumn.ShowUpdateButton = true;

            if (BtnType.HasFlag(CffGridViewButtonType.New))
                theColumn.ShowNewButton = true;

            this.Columns.Add(theColumn);
        }


        /// <summary>
        /// Insert Bound Command Button 
        /// OnClick += YourClickEventHandler()
        /// for columnwidth include: '%' or 'px' or 'em'
        /// </summary>
        public void InsertBoundCommandButtonColumn(string HeaderName, string BoundColumnName, string columnWidth="5%",
                                                         string CssStyle = "scroll", HorizontalAlign HorizontalAlignment = HorizontalAlign.NotSet,
                                                                string HeaderCssStyle = "cffGGVHeader", string alt="", bool isAutoPostBack = true, bool isReturnRowIndex=false)
        {
            CffTemplateField theColumn = new CffTemplateField();
            theColumn.HeaderText = HeaderName;
            theColumn.DataBoundColumnName = BoundColumnName;
            theColumn.HorizontalAlignment = HorizontalAlignment;
            theColumn.HeaderTemplate = new GridViewHeaderLabelTemplate(HeaderName);
            theColumn.HeaderStyle.CssClass = HeaderCssStyle;

            theColumn.ControlStyle.CssClass = CssStyle;
            theColumn.ControlStyle.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;

            theColumn.ItemTemplate = new GridViewCommandButtonTemplate(HeaderName, BoundColumnName, CffGridViewButtonType.Bound, isAutoPostBack, CssStyle, alt, isReturnRowIndex);
            theColumn.ItemStyle.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
          
            int colWidth = 5;
            if (columnWidth.Contains("%"))
            {
                colWidth = Convert.ToInt32(columnWidth.Replace("%", "").Trim());
                theColumn.ItemStyleWidth = Unit.Percentage(colWidth);
                theColumn.HeaderStyle.Width = Unit.Percentage(colWidth);

            }
            else if (columnWidth.Contains("px"))
            {
                colWidth = Convert.ToInt32(columnWidth.Replace("px", "").Trim());
                theColumn.ItemStyleWidth = Unit.Pixel(colWidth);
                theColumn.HeaderStyle.Width = Unit.Pixel(colWidth);

            }
            else if (columnWidth.Contains("em"))
            {
                colWidth = Convert.ToInt32(columnWidth.Replace("em", "").Trim());
                theColumn.ItemStyleWidth = Unit.Point(colWidth);
                theColumn.HeaderStyle.Width = Unit.Point(colWidth);
            }
            else
            { //default to pixel
                colWidth = Convert.ToInt32(columnWidth);
                theColumn.ItemStyleWidth = Unit.Pixel(colWidth);
                theColumn.HeaderStyle.Width = Unit.Pixel(colWidth);
            }

            
            this.Columns.Add(theColumn);
        }

        /// <summary>
        /// Insert Image Button Column
        /// </summary>
        public void InsertImageButtonColumn(string HeaderName = "", string ColumnWidth = "5%", string CssStyle = "cffGV_CommandButtons")
        {
            CffTemplateField theColumn = new CffTemplateField();
            theColumn.HeaderText = HeaderName;
            theColumn.HeaderTemplate = new GridViewHeaderLabelTemplate(HeaderName);
            theColumn.HeaderStyle.CssClass = "cffGGVHeader";
            theColumn.ItemTemplate = new GridViewImageButtonTemplate("", "");
            theColumn.ControlStyle.CssClass = CssStyle;
            theColumn.ItemStyle.CssClass = CssStyle;

            if (ColumnWidth.IndexOf("%") >= 0)
            {
                ColumnWidth = ColumnWidth.Replace("%", "");
                theColumn.ItemStyle.Width = Unit.Percentage(Convert.ToDouble(ColumnWidth));
            }
            else
                theColumn.ItemStyle.Width = Unit.Pixel(Convert.ToInt32(ColumnWidth));

            theColumn.ControlStyle.BorderWidth = Unit.Pixel(0);
            theColumn.ItemStyleWidth = theColumn.ItemStyle.Width;
            this.Columns.Add(theColumn);

        }

        /// <summary>
        /// Insert Image Command Column
        /// </summary>
        public void InsertImageCommandColumn(string HeaderName = "", string ColumnWidth = "5%", string ColumnHeight = "5%", string CssStyle = "cffGV_CommandButtons")
        {
            CffCommandField ImageColumn = new CffCommandField();
            ImageColumn.ButtonType = System.Web.UI.WebControls.ButtonType.Image;
            ImageColumn.HeaderText = HeaderName;
            ImageColumn.HeaderStyle.CssClass = "cffGGVHeader";
            ImageColumn.ImageButtonURL = "/images/plus.gif";
            ImageColumn.isImageButton = true;
            ImageColumn.ControlStyle.CssClass = CssStyle;

            if (ColumnWidth.IndexOf("%") >= 0)
            {
                ColumnWidth = ColumnWidth.Replace("%", "");
                ColumnHeight = ColumnHeight.Replace("%", "");
                ImageColumn.ItemStyle.Width = Unit.Percentage(Convert.ToDouble(ColumnWidth));
                ImageColumn.ItemStyle.Height = Unit.Percentage(Convert.ToDouble(ColumnHeight));
            }
            else
            {
                ColumnHeight = ColumnHeight.Replace("%", "");
                ImageColumn.ItemStyle.Width = Unit.Pixel(Convert.ToInt32(ColumnWidth));
                ImageColumn.ItemStyle.Height = Unit.Pixel(Convert.ToInt32(ColumnHeight));
            }

            ImageColumn.ItemStyle.Width = Unit.Pixel(5);
            ImageColumn.ItemStyle.Height = Unit.Pixel(5);
            ImageColumn.ItemStyle.VerticalAlign = VerticalAlign.Middle;

            ImageColumn.ControlStyle.BorderWidth = Unit.Pixel(0);
            this.Columns.Add(ImageColumn);
        }


        
        //todo:: add other insert data column types here
        /// <summary>
        /// call this function to trigger binding of nested grid and display nested data to selected row
        /// </summary>
        public void BindNestedGrid(object sender, object ds, bool isRaisePostBack=false)
        {
            this.NestedSettings.RowIndex = ((CffGenGridView)sender).NestedSettings.RowIndex;
            this.NestedSettings.State = GridNestingState.Nested;
            this.NestedSettings.childGrid.DataSource = ds; //this.NestedSettings.childGrid.DataBind();
            this.DataSource = ((CffGenGridView)sender).DataSource;
            this.IsRowCommandPostBack = true;
            this.DataBind();
            if (isRaisePostBack)
                this.RaisePostBackEvent("PostBack");
        }

        /// <summary>
        /// Excel export construction functions
        /// </summary>

        public void WriteToExcelDocument(ExcelDocument document)
        {
            ExcelDocument xDoc = document;
            WriteHeaderRow(ref xDoc);

            if (RowCount > 0)
            {
                WriteBodyRows(xDoc);
                WriteFooterRow(ref xDoc);
                document = xDoc;
            }
            else
            {
                document.MoveToNextRow();
                document.AddCell("No data to display");
            }
        }

        public void WriteToExcelDocumentWithReplaceField(ExcelDocument document, Hashtable hashtable)
        {
            ExcelDocument xDoc = document;
            WriteHeaderRow(ref xDoc);

            if (RowCount > 0)
            {
                WriteBodyRowsWithReplacedFieldName(xDoc, hashtable);
                WriteFooterRow(ref xDoc);
                document = xDoc;
            }
            else
            {
                document.MoveToNextRow();
                document.AddCell("No data to display");
            }

        }

        /// <summary>
        /// This needs to be refactored. Perhaps moved into a customer notes panel or another object of that sort
        /// </summary>
        public void ExportAsNote(string title)
        {
            var document = new ExcelDocument(true);
            string fileName = "Notes";
            if (!string.IsNullOrEmpty(title))
            {
                DateTime dateViewed = DateTime.Now;
                fileName = string.Format("{0}_{1}", title.Replace(" ", "_"), dateViewed.ToString("yyyyMMddhhmm"));
                document.WriteTitle(string.Format("{0} as at {1}", title, dateViewed.ToString("dd/MM/yyyy")));
            }

            WriteToExcelDocument(document);
            WriteToResponse(document.WriteToStream(), fileName);
        }

        public void Export(string documentTitle, DateRange dateRange,string rptFilePath = "./reports")
        {
            string descriptor = string.Format("from {0} to {1}", dateRange.StartDate.ToMonthYearString(), dateRange.EndDate.ToMonthYearString());
            Export(documentTitle, descriptor, rptFilePath);
        }

        /// <summary>
        /// Export to excel
        /// </summary>
        /// <param name="documentTitle">Title of document including name and client name</param>
        public void Export(string documentTitle)
        {
            this.Export(documentTitle, string.Empty);
        }

        /// <summary>
        /// Export to excel
        /// </summary>
        /// <param name="documentTitle">Title of document including name and client name</param>
        /// <param name="descriptor">Parameters used to generate document e.g. date as at</param>
        private void Export(string documentTitle, string descriptor, string rptFilePath = "./reports")
        {
            DateTime dateViewed = DateTime.Now;
            var document = new ExcelDocument();

            string fileName = string.Format("{0}_{1}", documentTitle.Replace(" ", "_"), dateViewed.ToString("yyyyMMddhhmm"));
            if (string.IsNullOrEmpty(descriptor)) {
                descriptor = string.Format("as at {0}", dateViewed.ToString("dd/MM/yyyy"));
            }
            documentTitle = string.Format("{0} {1}", documentTitle, descriptor);
            document.WriteTitle(documentTitle);

            WriteToExcelDocument(document);
            WriteToResponse(document.WriteToStream(), fileName, rptFilePath);
        }

        private void WriteToResponse(MemoryStream stream, string fileName, string rptFilePath="./reports")
        {
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/vnd.ms-excel"));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Page.Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));

            try 
            {
                Page.Response.BinaryWrite(stream.ToArray());
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                //Page.Response.End();
            }
            catch (Exception exc) {
                string errmsg = exc.Message;
                //copy to default report folder first
                if (!System.IO.Directory.Exists(rptFilePath))
                    System.IO.Directory.CreateDirectory(rptFilePath);
                rptFilePath = rptFilePath + "\\" + fileName ;
                var fileStream = new FileStream(rptFilePath, FileMode.CreateNew, FileAccess.ReadWrite);
                stream.Position = 0;
                stream.CopyTo(fileStream);
                stream.Flush();
                stream.Close();
                this.FileNameRes = rptFilePath;
            }

            try
            {
                stream.Dispose();
            }
            catch { }
        }

        private void WriteToResponse(byte[] dta, string fileName)
        {
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/vnd.openxmlformat-officedocument.spreadsheet.sheet"));
            Page.Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", fileName));

            Page.Response.BinaryWrite(dta);
            Page.Response.End();
        }

        private NPOI.SS.UserModel.HorizontalAlignment NPOIAlignment(HorizontalAlign xAlignment)
        {
            switch (xAlignment) { 
                case System.Web.UI.WebControls.HorizontalAlign.Left:
                    return NPOI.SS.UserModel.HorizontalAlignment.LEFT;

                case System.Web.UI.WebControls.HorizontalAlign.Right:
                    return NPOI.SS.UserModel.HorizontalAlignment.RIGHT;

                default:
                    break;
            }

            return NPOI.SS.UserModel.HorizontalAlignment.CENTER;
        }

        private void WriteHeaderRow(ref ExcelDocument document)
        {
            for (int columnIndex = 0; columnIndex < Columns.Count; columnIndex++) 
            {
                // Create headers for GridViewDataColumn only - i.e. exclude GridViewCommandColumn
                string columnType = (this.Columns[columnIndex]).GetType().Name;
                if (columnType.Contains("CffTemplateField")) 
                {
                    var gridViewDataColumn = ((CffTemplateField)this.Columns[columnIndex]).DataBoundColumnName;
                    if (gridViewDataColumn == null && this.NestedSettings.Enabled) {
                        gridViewDataColumn = ((CffTemplateField)this.Columns[columnIndex]).ToString();
                    }

                    if (gridViewDataColumn != null && ((CffTemplateField)this.Columns[columnIndex]).Visible)
                    {
                        string caption = string.IsNullOrEmpty(((CffTemplateField)this.Columns[columnIndex]).HeaderText)
                                             ? ((CffTemplateField)this.Columns[columnIndex]).ToString()
                                             : ((CffTemplateField)this.Columns[columnIndex]).HeaderText;

                        HorizontalAlign xAlign = ((CffTemplateField)this.Columns[columnIndex]).HeaderStyle.HorizontalAlign;
                        if (xAlign == HorizontalAlign.NotSet)
                            xAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                        document.AddHeaderCell(caption, NPOIAlignment(xAlign));
                    }
                }
            }
        }

        private void WriteBodyRows(ExcelDocument document)
        {
            //write the data section
            decimal dAmt = 0;
            string columnRowText = string.Empty;
            bool hasCommandField = false;

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                document.MoveToNextRow();
                object xS = ((IEnumerable)(this.DataSource)).Cast<object>().ToList()[rowIndex];
                for (int columnIndex = 0; columnIndex < Columns.Count; columnIndex++)
                {
                    string columnType = (this.Columns[columnIndex]).GetType().ToString();
                    if (columnType.Contains("CffCommandField")) {
                        hasCommandField = true;
                    } else if (columnType.Contains("CffTemplateField")) {
                        CffTemplateField gvRow = ((CffTemplateField)this.Columns[columnIndex]);
                        var gridViewDataColumn = gvRow.DataBoundColumnName;

                        if (gridViewDataColumn == null && this.NestedSettings.Enabled) { //get the bound column from boundpool
                            object oC = null;
                            if (((System.Collections.Generic.Dictionary<int, object>)(this.BoundPool)).TryGetValue((columnIndex-(hasCommandField?1:0)), out oC)) {
                                gridViewDataColumn = oC.ToString();
                            }
                        }

                        if (gridViewDataColumn != null && ((CffTemplateField)this.Columns[columnIndex]).Visible)
                        {
                            object objectValue = CffGenGridViewCommon.GetObjectValue(xS,gridViewDataColumn);
                            if (objectValue != null)
                            {
                                columnRowText = objectValue.ToString();
                                CffGridViewColumnType colType = gvRow.ColumnType;
                                if (this.NestedSettings.Enabled) //so we could support nested grids
                                    colType = (CffGridViewColumnType)(gvRow.ColumnViewState);

                                //todo: should be able to add nested rows here if nested grid is expanded
                                if (colType == CffGridViewColumnType.Currency)
                                {
                                    if (decimal.TryParse(columnRowText, out dAmt))
                                    {
                                        document.AddCurrencyCell(dAmt, NPOI.SS.UserModel.HorizontalAlignment.RIGHT);  //document.AddCell(string.Format("{0:C}", dAmt));
                                    }
                                    else
                                        document.AddCell((columnRowText == null) ? "" : columnRowText, -1, NPOIAlignment(((CffTemplateField)this.Columns[columnIndex]).HorizontalAlignment)); //autoadd
                                }
                                else
                                    document.AddCell(columnRowText, -1, NPOIAlignment(((CffTemplateField)this.Columns[columnIndex]).HorizontalAlignment)); //autoadd
                            }
                            else
                                document.AddCell("");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// when generate report replace certain column field value with the one specified in hashtable
        /// </summary>
        /// <param name="document"></param>
        /// <param name="hashtable"></param>
        private void WriteBodyRowsWithReplacedFieldName(ExcelDocument document, Hashtable hashtable)
        {
            bool bHasCommandField = false;
            CffTemplateField gridViewDataColumn = null;
            IEnumerable dSource = (IEnumerable)this.DataSource;

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                document.MoveToNextRow();

                for (int columnIndex = 0; columnIndex < Columns.Count; columnIndex++)
                {
                    object xS = dSource.Cast<object>().ToList()[rowIndex];
                    if (this.Columns[columnIndex].GetType().Name == "CffCommandField") {
                        bHasCommandField = true;
                        continue;
                    }
                    
                    if (this.Columns[columnIndex].GetType().Name == "CffTemplateField")
                    {
                        gridViewDataColumn  = ((CffTemplateField)this.Columns[columnIndex]);
                        if (gridViewDataColumn.DataBoundColumnName == null && this.NestedSettings.Enabled)
                        { //get the bound column from boundpool
                            object oC = null;
                            if (((System.Collections.Generic.Dictionary<int, object>)(this.BoundPool)).TryGetValue((columnIndex - (bHasCommandField ? 1 : 0)), out oC))
                            {
                                gridViewDataColumn.DataBoundColumnName = oC.ToString();
                            }
                        }

                    }
                  

                    if (xS != null  &&  gridViewDataColumn.Visible) 
                    {
                        object rowValue = CffGenGridViewCommon.GetObjectValue(xS, gridViewDataColumn.DataBoundColumnName);
                        string rowText = string.Empty;
                        if (rowValue != null)
                        {
                            rowText = rowValue.ToString();
                        }

                        if (((CffTemplateField)this.Columns[columnIndex]).ColumnType == CffGridViewColumnType.Currency && (!string.IsNullOrEmpty(rowText)))
                        {
                            decimal amount = decimal.Parse(rowText);
                            document.AddCurrencyCell(amount, NPOI.SS.UserModel.HorizontalAlignment.RIGHT);
                        }
                        else
                        {
                            if (document.RemoveBr)
                            {
                                rowText = CustomerNotesParser.RemoveBr(rowText);
                            }
                            document.AddCell(rowText, -1, NPOIAlignment(((CffTemplateField)this.Columns[columnIndex]).HorizontalAlignment));
                        }
                    }
                }
            }
        }

        private int DataColumnCount
        {
            get
            {
                int colCtr = 0;
                int count = 0;
                foreach (var column in Columns)
                {
                    string columnType = this.Columns[colCtr].GetType().Name;
                    if (columnType.Contains("CffTemplateField")) {
                        count++;    
                    }
                    colCtr++;
                }
                return count;
            }
        }

        private void WriteFooterRow(ref ExcelDocument document)
        {  //document.StartFooterRow(DataColumnCount);
            int footerCells = this.FooterRow.Cells.Count;
            int cellCount = FooterRow.Cells.Count;
            int iCtr = 0;
            bool bHasCommandField = false;

            document.MoveToNextRow();
            if (FooterRow.Cells.Count > 0)
            {
                CffTotalsSummary xTS = (this.TotalsSummarySettings == null) ? (new CffTotalsSummary()) : this.TotalsSummarySettings;

                iCtr = 0;
                int iCol = 0;
                decimal TotalValue = 0;
                bool bHit = false;
                int offset = 0;          
    
                try {
                    IEnumerable dSource = (IEnumerable)this.DataSource;
                    foreach (DataControlField dx in this.Columns) 
                    {
                        HorizontalAlign ha = dx.HeaderStyle.HorizontalAlign;
                        var columnType = dx.GetType().Name;
                        if (!dx.Visible)
                        {
                            offset++;
                            iCol++;
                            continue;
                        }

                        if (iCol == 0 || iCol==offset)
                        {
                            document.AddFooterCellToCurrentRow("Total", (iCol-offset), NPOIAlignment(ha));
                            if (columnType.Contains("CffCommandField"))
                                bHasCommandField = true;
                        }
                        else
                        {
                            if (columnType.Contains("CffTemplateField"))
                            {
                                var dbColumnName = ((CffTemplateField)this.Columns[iCol]).DataBoundColumnName;
                                if (dbColumnName == null && this.NestedSettings.Enabled)
                                {
                                    object oC = null;
                                    if (((System.Collections.Generic.Dictionary<int, object>)(this.BoundPool)).TryGetValue((iCol - (bHasCommandField ? 1 : 0)), out oC))
                                    {
                                        dbColumnName = oC.ToString();
                                    }
                                }

                                bHit = false;
                                foreach (KeyValuePair<int, string> colTotals in this.TotalsSummarySettings.ColumnTotals.ColumnsPool)
                                {
                                    if (colTotals.Value == dbColumnName)
                                    {
                                        bHit = true;
                                        while (iCtr < this.RowCount)
                                        {
                                            object xS = dSource.Cast<object>().ToList()[iCtr];
                                            object objectValue = CffGenGridViewCommon.GetObjectValue(xS, colTotals.Value);
                                            TotalValue += Convert.ToDecimal((objectValue == null) ? "0" : (string.IsNullOrEmpty(objectValue.ToString())) ? "0" : objectValue.ToString());
                                            iCtr++;
                                        }
                                        break;
                                    }
                                }

                                if (bHit) {
                                    document.AddCurrencyFooterCellToCurrentRow(TotalValue, (bHasCommandField ? (iCol - 1 - offset) : iCol - offset));
                                }
                                else
                                    document.AddFooterCellToCurrentRow(" ", (bHasCommandField ? (iCol - 1 - offset) : iCol - offset), NPOIAlignment(ha));
                                TotalValue = 0;
                            }
                            else if (columnType.Contains("CffCommandField"))
                                bHasCommandField = true;
                        }
                        iCol++;
                        iCtr = 0;
                    }
                }
                catch { }
            }
        }

        public void ResetPaginationAndFocus()
        {
            FocusedRowIndex = -1;
            PageIndex = 0;
        }

        public void ResetPaginationAndFocus(int focusedRow, int pgIdx)
        {
            FocusedRowIndex = focusedRow;
            PageIndex = pgIdx;
        }

        public int GetColumnIndexByName(GridView grid, object name)
        {
            foreach (DataControlField col in grid.Columns)
            {
                if (col.HeaderText.ToLower().Trim() == name.ToString().ToLower().Trim())
                {
                    return grid.Columns.IndexOf(col);
                }
            }
            return -1;
        }

        #endregion


        #region "ControlEvents"
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((this.AllowSorting) && (ViewState["SortExpression"] != null))
            {
                object o = ViewState["SortDirection"];
                this.Sort(ViewState["SortExpression"].ToString(), (SortDirection)o);
            }

        }

        public static Control GetPostBackControl(Page page)
        {
            Control control = null;
            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != String.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    if (!string.IsNullOrEmpty(ctl)) {
                        Control c = page.FindControl(ctl);
                        if (c is System.Web.UI.WebControls.Button)
                        {
                            control = c;
                            break;
                        }
                    }
                }
            }
            return control;
        }
        #endregion

    }
}