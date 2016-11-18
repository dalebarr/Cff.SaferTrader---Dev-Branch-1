using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{
    public class GridViewCellClickedEventArgs: EventArgs
    {
        private TableCell mobjCell;
        private GridViewRow mobjRow;
        private int mintColumnIndex;

        public GridViewCellClickedEventArgs(TableCell tCell, GridViewRow gRow, int colIndex) 
        {
            mobjCell = tCell;
            mobjRow = gRow;
            mintColumnIndex = colIndex;
        }

        public TableCell GridCell
        { 
            get { return mobjCell; }
        }

        public GridViewRow GridRow
        {
            get { return mobjRow; }
        }

        public int GridColIndex
        {
            get { return mintColumnIndex; }
        }
    }
}