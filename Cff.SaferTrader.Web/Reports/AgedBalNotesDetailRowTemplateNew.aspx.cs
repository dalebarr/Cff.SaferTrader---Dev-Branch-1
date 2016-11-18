using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Web.UserControls;
using GenericGridViewHelper;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.Reports
{
    public partial class AgedBalNotesDetailRowTemplateNew : ITemplate
    {
        GenericGridView detailGridView; // nested grid
        object dataSource = null;
        string dtKeyFieldName;
        string ID;
        int currentRow;

        public AgedBalNotesDetailRowTemplateNew() {}

        public AgedBalNotesDetailRowTemplateNew(string strKFieldName)
        {

            this.dtKeyFieldName = strKFieldName;
            this.currentRow = 0;

            detailGridView = new GenericGridView();
            detailGridView.Visible = true;
            detailGridView.EnableCallBacks = false;
            detailGridView.KeyFieldName = this.dtKeyFieldName;

            detailGridView.AddRightAlignedDataColumnNested("CreatedByEmployeeName", "Created By","");
            detailGridView.AddRightAlignedDataColumnNested("Comment", "Comment", "");
            detailGridView.AddRightAlignedDataColumnNested("Created", "Created", "");            
            detailGridView.Enabled = false;

        }


        public AgedBalNotesDetailRowTemplateNew(string strKFieldName, object dSource, string pID, int cRow)
        {
            this.dtKeyFieldName = strKFieldName;
            this.dataSource = dSource;
            this.ID = pID;
            this.currentRow = cRow;

            detailGridView = new GenericGridView();
            detailGridView.KeyFieldName = this.dtKeyFieldName;

            detailGridView.AddRightAlignedDataColumnNested("CreatedByEmployeeName", "Created By", "");
            detailGridView.AddRightAlignedDataColumnNested("Comment", "Comment", "");
            detailGridView.AddRightAlignedDataColumnNested("Created", "Created", "");
            detailGridView.Visible = true;
            detailGridView.Enabled = false;
            detailGridView.EnableCallBacks = false;
           
        }

        public AgedBalNotesDetailRowTemplateNew(object dataSource, string strKFieldName)
        {
            this.dataSource = dataSource;
            this.currentRow = 0;
            this.dtKeyFieldName = strKFieldName;
            detailGridView = new GenericGridView();
            detailGridView.KeyFieldName = this.dtKeyFieldName;
            detailGridView.EnableCallBacks = false;

            detailGridView.AddRightAlignedDataColumnNested("CreatedByEmployeeName", "Created By", "");
            detailGridView.AddRightAlignedDataColumnNested("Comment", "Comment", "");
            detailGridView.AddRightAlignedDataColumnNested("Created", "Created", "");

            detailGridView.Enabled = false;
            
        }

        public void InstantiateIn(Control container)
        {
            try
            {
                //GenericGridView dGrid;
                if (this.dataSource != null)
                {
                    //detailGridView.DataSource = this.dataSource;
                    detailGridView.DataBind();

                    //dGrid = ((Cff.SaferTrader.Web.UserControls.GenericGridView)(((Cff.SaferTrader.Web.UserControls.gGridViewControls.GenericGridViewBaseTemplateContainer)(((Cff.SaferTrader.Web.UserControls.gGridViewControls.GenericGridViewDetailRowTemplateContainer)container))).Grid));
                    //dGrid.FocusedRowIndex = this.currentRow;
                    //dGrid.DetailGridID = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.GenericGridViewDetailRowTemplateContainer)container).ID;
                    //this.ID = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.GenericGridViewDetailRowTemplateContainer)container).ID;
                    //dGrid.FindDetailRowTemplateControl(dGrid.FocusedRowIndex, this.ID);
                    container.Controls.Add(detailGridView);

                }
                else
                {
                    //dGrid = ((Cff.SaferTrader.Web.UserControls.GenericGridView)(((Cff.SaferTrader.Web.UserControls.gGridViewControls.GenericGridViewBaseTemplateContainer)(((Cff.SaferTrader.Web.UserControls.gGridViewControls.GenericGridViewDetailRowTemplateContainer)container))).Grid));
                    //this.currentRow = dGrid.CurrentFocusedRow;
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
                GenericGridView masterGrid = sender as GenericGridView;
                //object kVal = masterGrid.GetMasterRowKeyValue();
                if (this.dataSource != null)
                {
                    IList<AgedBalancesReportRecord> xABR = dataSource as IList<AgedBalancesReportRecord>;
                    detailGridView.DataSource = (xABR[0] as AgedBalancesReportRecord).CustNoteList;
                }
                else
                {
                    AgedBalancesReportRecord dRow = masterGrid.GetRow(masterGrid.CurrentFocusedRow) as AgedBalancesReportRecord;
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
            GenericGridView masterGrid = sender as GenericGridView;
            //object kVal = masterGrid.GetMasterRowKeyValue();

            AgedBalancesReportRecord dRow = masterGrid.GetRow(masterGrid.CurrentFocusedRow) as AgedBalancesReportRecord;
            if (dRow != null) {
                detailGridView.DataSource = dRow.CustNoteList;
            } 
            
            //IList<AgedBalancesReportRecord> xABR = dataSource as IList<AgedBalancesReportRecord>;
            //detailGridView.DataSource = (xABR[0] as AgedBalancesReportRecord).CustNoteList;
        }

        void detailGridView_DataBound(object sender, EventArgs e)
        { //Template
            GenericGridView dGrid = sender as GenericGridView;
        }


        #region ITemplate Members

        public void InstantiateIn(System.Web.UI.Control container)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
