using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

namespace GenericGridViewHelper
{
    public class SortingHelper
    {
        public static SortDirection GetSortDirection(string column, StateBag viewState)
        {
            // By default, set the sort direction to ascending.
            var sortDirection = SortDirection.Ascending;

            // Retrieve the last column that was sorted.
            var sortExpression = viewState[Const.SortExpression] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    var lastDirection = (SortDirection)viewState[Const.SortDirection];
                    if (lastDirection == SortDirection.Ascending)
                    {
                        sortDirection = SortDirection.Descending;
                    }
                }
            }

            // Save new values in ViewState.
            viewState[Const.SortDirection] = sortDirection;
            viewState[Const.SortExpression] = column;

            return sortDirection;
        }

        public static IEnumerable<object> GetSortedObjects(GridViewSortEventArgs e, IEnumerable<object> source, Func<GridViewSortEventArgs, IEnumerable> sorting)
        {
            IEnumerable sortedSource = null;
            IEnumerable<object> newSource = null;

            if (sorting != null)
               sortedSource = sorting(e);
            
            if (sortedSource == null)
            {
                if (e.SortDirection == SortDirection.Ascending)
                {
                    newSource = (from x in source
                              orderby GetPropertyValue(x, e.SortExpression)
                              select x).ToList();
                }
                else
                {
                    newSource = (from x in source
                              orderby GetPropertyValue(x, e.SortExpression) descending
                              select x).ToList();
                }
                return newSource;
            }
            else
            {
                source = sortedSource.Cast<object>().ToList();
            }
            return source;
        }

        public static IEnumerable<object> GetSelectedObjects(GridViewSortEventArgs e, IEnumerable<object> source, Func<GridViewSortEventArgs, IEnumerable> sorting, string strColumnName, string colValue)
        {
            IEnumerable sortedSource = null;

            if (sorting != null)
                sortedSource = sorting(e);

            if (sortedSource == null)
            {
                if (e.SortDirection == SortDirection.Ascending)
                {
                    source = (from x in source
                              orderby GetPropertyValue(x, e.SortExpression)
                              select x).ToList();
                }
                else
                {
                    source = (from x in source
                              orderby GetPropertyValue(x, e.SortExpression) descending
                              select x).ToList();
                }
            }
            else
            {
                source = sortedSource.Cast<object>().ToList();
            }

            source = source.Cast<DataRow>().Where(row => row[strColumnName].ToString().Equals(colValue));
            return source;
        }


        // GridViewSortEventArgs.SortDirection won't get populated correctly when the GridView datasource is
        // a list of objects, so we have to manually correct it. 
        public static void PrepareGridViewSortEventArgs(GridViewSortEventArgs e, StateBag viewState)
        {
            var column = e.SortExpression;
            var direction = GetSortDirection(column, viewState);
            e.SortDirection = direction;
        }

        private static string GetPropertyValue(object obj, string property)
        {
            if (property == null)
                return string.Empty;

            var propertyInfo = obj.GetType().GetProperty(property);

            if (propertyInfo != null)
            {
                object value = propertyInfo.GetValue(obj, null);

                if (value != null)
                {
                    return value.ToString();
                }
            }

            return string.Empty;
        }
    }
}
