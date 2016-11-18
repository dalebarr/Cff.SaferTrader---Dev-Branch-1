
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace GenericGridViewHelper
{
    public class ColumnHelper
    {
        public static void PrepareColumns(dynamic source, DataControlFieldCollection columns, Func<string, string> headerNameFormatter)
        {
            // if ViewState is enabled, the columns info will be persisted between requests and we only want to add the column 
            // the first time the grid is requested. 
            if (columns.Count == 0)
            {
                // at this point, the datasource must have been prepared and it must be a list
                var properties = source[0].GetType().GetProperties();
                foreach (var property in properties)
                {
                    var name = property.Name;

                    var field = new BoundField
                                    {
                                        DataField = name,
                                        HeaderText =
                                            headerNameFormatter == null
                                                ? SeperateNamesWithSpace(name)
                                                : headerNameFormatter(name),
                                        SortExpression = name,
                                        HtmlEncode = false
                                    };

                    if (property.PropertyType.Name == "DateTime")
                    {
                        field.DataFormatString = "{0:dd-MMM-yyyy}";
                    }

                    columns.Add(field);
                }
            }
        }

        // the default formatter
        private static string SeperateNamesWithSpace(string str)
        {
            var sb = new StringBuilder();

            foreach (var s in str)
            {
                if (StartsWithCapitalLetter(s.ToString()))
                {
                    sb.Append(" ");
                }

                sb.Append(s);
            }
            return sb.ToString().Trim();
        }

        private static bool StartsWithCapitalLetter(string str)
        {
            return Regex.IsMatch(str, @"^[A-Z]");
        }
    }
}
