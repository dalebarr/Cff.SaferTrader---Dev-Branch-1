using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace GenericGridViewHelper
{
    public class StyleHelper
    {
        public static void SetHeaderStyle(
            StateBag viewState,
            DataControlFieldCollection columns,
            string AscendingClassName,
            string DescendingClassName)
        {
            SortDirection sortDirection = SortDirection.Ascending;

            if (viewState[Const.SortDirection] != null)
            {
                sortDirection = (SortDirection)viewState[Const.SortDirection];
            }

            var column = viewState[Const.SortExpression] as string;

            if (typeof(BoundField) == columns.GetType())
            {
                foreach (BoundField field in columns)
                {
                    if (field.DataField == column)
                    {
                        field.HeaderStyle.CssClass = sortDirection == SortDirection.Ascending ? AscendingClassName : DescendingClassName;
                    }
                    else
                    {
                        field.HeaderStyle.CssClass = string.Empty;
                    }
                }
            }

        }

                 
        private static int activeRowIndex;
        public static void SetRowStyle(GridViewRowEventArgs e, string ROW_STYLE, string ALTERNATIVE_ROW_STYLE)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var style = activeRowIndex % 2 == 0 ? ROW_STYLE : ALTERNATIVE_ROW_STYLE;
                e.Row.CssClass = style;
                activeRowIndex++;
            }
        }
    }
}
