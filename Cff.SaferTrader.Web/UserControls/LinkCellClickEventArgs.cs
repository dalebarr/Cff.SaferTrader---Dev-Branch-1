using System;

namespace Cff.SaferTrader.Web.UserControls
{
    public class LinkCellClickEventArgs : EventArgs
    {
        private readonly string fieldName;
        private readonly int rowIndex;

        public LinkCellClickEventArgs(string fieldName, int rowIndex )
        {
            this.fieldName = fieldName;
            this.rowIndex = rowIndex;
        }

        public string FieldName
        {
            get { return fieldName; }
        }

        public int RowIndex
        {
            get { return rowIndex; }
        }
    }
}