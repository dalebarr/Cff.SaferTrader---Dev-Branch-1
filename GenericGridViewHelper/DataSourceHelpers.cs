using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace GenericGridViewHelper
{
    public class DataSourceHelpers
    {
        public static void PrepareDataSource(
            GridView gvGeneric,
            StateBag viewState,
            ref IEnumerable DataSource,
            ref int _totalRowCount,
            Func<GridViewSortEventArgs, IEnumerable> sorting)
        {
            
            var column = viewState[Const.SortExpression] as string;
            SortDirection sortDirection = SortDirection.Ascending;
            if (viewState[Const.SortDirection] != null)
            {
                sortDirection = (SortDirection)viewState[Const.SortDirection];
            }

            var temp = DataSource.Cast<object>();
            _totalRowCount = temp.Count();

            if (column != null)
            {
                var source = SortingHelper.GetSortedObjects(new GridViewSortEventArgs(column, sortDirection), temp, sorting);
                DataSource = source;
            }
            else if (DataSource.GetType().Name != Const.ListTypeName)
            {
                DataSource = temp.ToList();
            }

            gvGeneric.DataSource = DataSource;
            gvGeneric.DataBind();

        }

        public static void SelectDataSourceAndBind(CompositeDataBoundControl gvGeneric, IEnumerable sortedDataSource, IEnumerable orginalDataSource, GridViewSortEventArgs e)
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
    }
}
