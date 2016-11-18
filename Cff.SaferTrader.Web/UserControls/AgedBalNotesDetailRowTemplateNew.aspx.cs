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
        CffGenGridView detailGridView; // nested grid
        object dataSource = null;
        string dtKeyFieldName;
        string ID;
        int currentRow;

        public AgedBalNotesDetailRowTemplateNew(string strKFieldName)
        {

            this.dtKeyFieldName = strKFieldName;
            this.currentRow = 0;

            detailGridView = new CffGenGridView();
            detailGridView.AutoGenerateColumns = false;
            detailGridView.CssClass = "scroll";
            detailGridView.Visible = true;

            detailGridView.InsertDataColumn("Created By", "CreatedByEmployeeName");
            detailGridView.InsertDataColumn("Comment", "Comment");
            detailGridView.InsertDataColumn("Created", "Created");
            detailGridView.Enabled = false;

        }


        public AgedBalNotesDetailRowTemplateNew(string strKFieldName, object dSource, string pID, int cRow)
        {
            this.dtKeyFieldName = strKFieldName;
            this.dataSource = dSource;
            this.ID = pID;
            this.currentRow = cRow;

            detailGridView = new CffGenGridView();

            detailGridView.InsertDataColumn("Created By", "CreatedByEmployeeName");
            detailGridView.InsertDataColumn("Comment", "Comment");
            detailGridView.InsertDataColumn("Created", "Created");      
            detailGridView.Visible = true;
            detailGridView.Enabled = false;
           
        }

        public AgedBalNotesDetailRowTemplateNew(object dataSource, string strKFieldName)
        {
            this.dataSource = dataSource;
            this.currentRow = 0;
            this.dtKeyFieldName = strKFieldName;
            detailGridView = new CffGenGridView();

            detailGridView.InsertDataColumn("Created By", "CreatedByEmployeeName");
            detailGridView.InsertDataColumn("Comment", "Comment");
            detailGridView.InsertDataColumn("Created", "Created"); 

            detailGridView.Enabled = false;
            
        }

        public void InstantiateIn(Control container)
        {
            try
            {
                CffGenGridView dGrid;
                if (this.dataSource != null)
                {
                    detailGridView.DataSource = this.dataSource;
                    detailGridView.DataBind();

                    dGrid = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffGenGridView)(((Cff.SaferTrader.Web.UserControls.gGridViewControls.GenericGridViewBaseTemplateContainer)(((Cff.SaferTrader.Web.UserControls.gGridViewControls.GenericGridViewDetailRowTemplateContainer)container))).Grid));
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
            detailGridView.DataBinding += new EventHandler(detailGridView_DataBinding);
            detailGridView.DataBound += new EventHandler(detailGridView_DataBound);
        } 

        void detailGridView_BeforePerformDataSelect(object sender, EventArgs e) 
        {//HttpContext.Current.Session[this.dtKeyFieldName] = (sender as ASPxGridView).GetMasterRowKeyValue();
         
            try
            {
                CffGenGridView masterGrid = sender as CffGenGridView;
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
            CffGenGridView masterGrid = sender as CffGenGridView;
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
            CffGenGridView dGrid = sender as CffGenGridView;
        }


        #region ITemplate Members

        public void InstantiateIn(System.Web.UI.Control container)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
