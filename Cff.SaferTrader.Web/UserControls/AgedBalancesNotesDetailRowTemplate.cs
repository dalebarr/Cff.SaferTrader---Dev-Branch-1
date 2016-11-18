using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Web.UserControls
{
    //used for gridViews with detailRows
    public class AgedBalancesNotesDetailRowTemplate : ITemplate
    {
        object dataSource = null;
        string dtKeyFieldName;
        string ID;
        int currentRow;
        CffGridView detailGridView;

        public AgedBalancesNotesDetailRowTemplate(string strKFieldName)
        {
            this.dtKeyFieldName = strKFieldName;
            this.currentRow = 0;

            detailGridView = new CffGridView();
            detailGridView.SettingsBehavior.AllowFocusedRow = true;
            detailGridView.SettingsBehavior.AllowGroup = true;
            detailGridView.SettingsDetail.IsDetailGrid = true;
            detailGridView.SettingsDetail.ExportMode = DevExpress.Web.ASPxGridView.GridViewDetailExportMode.Expanded;
            detailGridView.KeyFieldName = this.dtKeyFieldName;
            detailGridView.Visible = true;

            detailGridView.Styles.Cell.Wrap =  DevExpress.Web.ASPxClasses.DefaultBoolean.True ;
            detailGridView.Styles.Cell.HorizontalAlign = HorizontalAlign.Justify;
            detailGridView.Settings.ShowFooter = true;
         
            detailGridView.EnableCallBacks = false;
            GridViewDataColumn colName = new GridViewDataColumn("CreatedByEmployeeName", "Created By");
            colName.VisibleIndex = detailGridView.Columns.Count;
            colName.CellStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            colName.Width = Unit.Percentage(10);
            colName.ReadOnly = true;
            detailGridView.Columns.Add(colName);

          
            GridViewDataMemoColumn column = new GridViewDataMemoColumn();
            column.Caption = "Comment";
            column.FieldName = "Comment";
            column.Width = Unit.Percentage(60);
            column.VisibleIndex = detailGridView.Columns.Count;
            column.CellStyle.HorizontalAlign = HorizontalAlign.Justify;
            column.CellStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
            column.EditCellStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
            column.ReadOnly = true; 
            detailGridView.Columns.Add(column);

            detailGridView.InsertDataColumn("Created", "Created");
            detailGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = false;
            detailGridView.SettingsBehavior.ProcessSelectionChangedOnServer = false;
            detailGridView.Enabled = false;
        }


        public AgedBalancesNotesDetailRowTemplate(string strKFieldName, object dSource, string pID, int cRow)
        {
            this.dtKeyFieldName = strKFieldName;
            this.dataSource = dSource;
            this.ID = pID;
            this.currentRow = cRow;

            detailGridView = new CffGridView();
            detailGridView.SettingsBehavior.AllowGroup = true;
            detailGridView.SettingsDetail.IsDetailGrid = true;
            detailGridView.SettingsDetail.ExportMode = DevExpress.Web.ASPxGridView.GridViewDetailExportMode.Expanded;
            detailGridView.KeyFieldName = this.dtKeyFieldName;
            detailGridView.Visible = true;

            detailGridView.Styles.Cell.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
            detailGridView.Styles.Cell.HorizontalAlign = HorizontalAlign.Justify;
            detailGridView.Settings.ShowFooter = true;

            GridViewDataColumn colName = new GridViewDataColumn("CreatedByEmployeeName", "Created By");
            colName.VisibleIndex = detailGridView.Columns.Count;
            colName.CellStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            colName.Width = Unit.Percentage(10);
            colName.ReadOnly = true;
            detailGridView.Columns.Add(colName);

           
            GridViewDataMemoColumn column = new GridViewDataMemoColumn();
            column.Caption = "Comment";
            column.FieldName = "Comment";
            column.Width = Unit.Percentage(60);
            column.VisibleIndex = detailGridView.Columns.Count;
            column.CellStyle.HorizontalAlign = HorizontalAlign.Justify;
            column.CellStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
            column.EditCellStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
            column.ReadOnly = true;
            detailGridView.Columns.Add(column);

            detailGridView.InsertDataColumn("Created", "Created");
            detailGridView.Enabled = false;
            detailGridView.EnableCallBacks = false;
        }

        public AgedBalancesNotesDetailRowTemplate(object dataSource, string strKFieldName)
        {
            this.dataSource = dataSource;
            this.currentRow = 0;
            this.dtKeyFieldName = strKFieldName;
            detailGridView = new CffGridView();
            detailGridView.SettingsDetail.IsDetailGrid = true;
            detailGridView.KeyFieldName = this.dtKeyFieldName;

            detailGridView.EnableCallBacks = false;
            detailGridView.InsertDataColumn("CreatedByEmployeeName", "Created By", 80);
            detailGridView.InsertDataColumn("Created", "Created", 80);

            GridViewDataMemoColumn column = new GridViewDataMemoColumn();
            column.Caption = "Comment";
            column.FieldName = "Comment";
            column.Width = Unit.Pixel(300);
            column.VisibleIndex = detailGridView.Columns.Count;
            column.CellStyle.HorizontalAlign = HorizontalAlign.Justify;
            column.CellStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
            column.EditCellStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
            column.ReadOnly = true;
            detailGridView.Columns.Add(column);

            detailGridView.Enabled = false;
        }

        public void InstantiateIn(Control container)
        {
            try
            {
                
                CffGridView dGrid;
                if (this.dataSource != null)
                {
                    detailGridView.DataSource = this.dataSource;
                    detailGridView.DataBind();

                    dGrid = ((Cff.SaferTrader.Web.UserControls.CffGridView)(((DevExpress.Web.ASPxGridView.GridViewBaseTemplateContainer)(((DevExpress.Web.ASPxGridView.GridViewDetailRowTemplateContainer)container))).Grid));
                    dGrid.FocusedRowIndex = this.currentRow;
                    dGrid.DetailGridID = ((DevExpress.Web.ASPxGridView.GridViewDetailRowTemplateContainer)container).ID;
                    this.ID = ((DevExpress.Web.ASPxGridView.GridViewDetailRowTemplateContainer)container).ID;
                    dGrid.FindDetailRowTemplateControl(dGrid.FocusedRowIndex, this.ID);
                    container.Controls.Add(detailGridView);
                }
                else
                {
                    dGrid = ((Cff.SaferTrader.Web.UserControls.CffGridView)(((DevExpress.Web.ASPxGridView.GridViewBaseTemplateContainer)(((DevExpress.Web.ASPxGridView.GridViewDetailRowTemplateContainer)container))).Grid));
                    this.currentRow = dGrid.CurrentFocusedRow;
                }
                
            }
            catch { }
           
            //detailGridView.BeforePerformDataSelect += new EventHandler(detailGridView_BeforePerformDataSelect);
            //detailGridView.DataBinding += new EventHandler(detailGridView_DataBinding);
            //detailGridView.DataBound += new EventHandler(detailGridView_DataBound);
        }

        void detailGridView_BeforePerformDataSelect(object sender, EventArgs e) 
        {//HttpContext.Current.Session[this.dtKeyFieldName] = (sender as ASPxGridView).GetMasterRowKeyValue();
         
            try
            {
                CffGridView masterGrid = sender as CffGridView;
                object kVal = masterGrid.GetMasterRowKeyValue();
                if (this.dataSource != null)
                {
                    IList<AgedBalancesReportRecord> xABR = dataSource as IList<AgedBalancesReportRecord>;
                    detailGridView.DataSource = (xABR[0] as AgedBalancesReportRecord).CustNoteList;
                }
                else
                {
                    AgedBalancesReportRecord dRow = masterGrid.GetRow(masterGrid.FocusedRowIndex) as AgedBalancesReportRecord;
                    detailGridView.DataSource = dRow.CustNoteList;
                }
                detailGridView.DataBind();
            }
            catch (Exception exc) {
                string Msg = exc.Message;
            }
            
        }

        void detailGridView_DataBinding(object sender, EventArgs e)
        { //Template
            CffGridView masterGrid = sender as CffGridView;
            object kVal = masterGrid.GetMasterRowKeyValue();

            AgedBalancesReportRecord dRow = masterGrid.GetRow(masterGrid.CurrentFocusedRow) as AgedBalancesReportRecord;
            if (dRow != null) {
                detailGridView.DataSource = dRow.CustNoteList;
            } 
            
            //IList<AgedBalancesReportRecord> xABR = dataSource as IList<AgedBalancesReportRecord>;
            //detailGridView.DataSource = (xABR[0] as AgedBalancesReportRecord).CustNoteList;
        }

        void detailGridView_DataBound(object sender, EventArgs e)
        { //Template
            CffGridView dGrid = sender as CffGridView;
        }
    }
}
