using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{
    public class GridViewEditRowFormTemplate : System.Web.UI.ITemplate
    {
        GridViewRow _gridRow;
        CffGridViewEditingMode _theEditingMode;
        DataControlFieldCollection _rowColumns;


        public GridViewEditRowFormTemplate(GridViewRow theGridRow, CffGridViewEditingMode editingMode, DataControlFieldCollection  rowColumns)
        {
            _gridRow = theGridRow;
            _theEditingMode = editingMode;
            _rowColumns = rowColumns;       
        }

        void System.Web.UI.ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            container.DataBinding += container_DataBinding;
        }

        void container_DataBinding(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            DataControlFieldCell dcfCell = (DataControlFieldCell)sender;
            TableCell tCell = ((TableCell)(DataControlFieldCell)sender);
            HtmlGenericControl divLeft = new HtmlGenericControl("div");
            divLeft.Attributes.Add("align", "left");

            Label label = new Label();
            label.Text = "Editing Mode";
            label.BackColor = System.Drawing.Color.AliceBlue;
            label.ForeColor = System.Drawing.Color.DarkRed;
            divLeft.Controls.Add(label);

            int rIdx = _gridRow.RowIndex;

            if (_gridRow.DataItem != null)
            {
                int rCnt = 0;
                while (rCnt < _gridRow.Cells.Count)
                {
                    Label lblText = new Label();
                    lblText.Text = _rowColumns[rCnt].HeaderText;
                    lblText.Style.Add(HtmlTextWriterStyle.Padding, "5px 5px 5px 5px");
                    divLeft.Controls.Add(lblText);

                    TextBox tBox = new TextBox();
                    tBox.Text = _gridRow.Cells[rCnt].Text;
                    lblText.Style.Add(HtmlTextWriterStyle.Padding, "5px 5px 5px 5px");
                    divLeft.Controls.Add(tBox);
                }
            }

            tCell.Controls.Add(divLeft);
            
        }


    }
}