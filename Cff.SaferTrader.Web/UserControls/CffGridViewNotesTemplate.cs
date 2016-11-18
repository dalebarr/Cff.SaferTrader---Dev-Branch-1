using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Web.UserControls
{
    public class CffGridViewNotesTemplate : ITemplate
    {
        private DataControlRowType templateType;
        private string columnName;
        private CffGridView xDetail;

        public CffGridViewNotesTemplate(DataControlRowType type, string colname)
        {
            templateType = type;
            columnName = colname;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            // Create the content for the different row types.
            switch (templateType)
            {
                case DataControlRowType.Header:
                    // Create the controls to put in the header  section and set their properties.
                    Literal lc = new Literal();
                    lc.Text = "<b>" + columnName + "</b>";

                    // Add the controls to the Controls collection of the container.
                    container.Controls.Add(lc);
                    break;

                case DataControlRowType.DataRow:
                    xDetail = new CffGridView();
                    xDetail.InsertDataColumn("Created");
                    xDetail.InsertMemoColumn("Comment", "Comment");

                    // To support data binding, register the event-handling methods to perform the data binding. Each control needs its own event handler.
                    xDetail.DataBinding += new EventHandler(this.xDetail_DataBinding); //lastName.DataBinding += new EventHandler(this.field_DataBinding);

                    // Add the controls to the Controls collection  of the container.
                    container.Controls.Add(xDetail); //container.Controls.Add(field);
                    break;

                case DataControlRowType.Footer:
                    break;

                //Insert cases to create the content for the other row types, if desired.
                default:
                    // Insert code to handle unexpected values.
                    break;
            }
        }

    
         private void xDetail_DataBinding(Object sender, EventArgs e)
         {
            CffGridView masterGrid = (CffGridView)sender;
            int rIdx = masterGrid.FocusedRowIndex;
         
         }

         private void field_DataBinding(Object sender, EventArgs e)
         { 
              // Get the Label control to bind the value. The Label control
              // is contained in the object that raised the DataBinding 
              // event (the sender parameter).
              Label l = (Label)sender;

              // Get the GridViewRow object that contains the Label control.
              GridViewRow row = (GridViewRow)l.NamingContainer;

              // Get the field value from the GridViewRow object and 
              // assign it to the Text property of the Label control.
              l.Text = DataBinder.Eval(row.DataItem, "au_lname").ToString();
         }
    }
}
