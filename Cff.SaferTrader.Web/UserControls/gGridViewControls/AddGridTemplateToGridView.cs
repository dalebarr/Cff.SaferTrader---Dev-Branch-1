using System;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Web.UserControls;

namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{
    public class AddGridTemplateToGridView : System.Web.UI.ITemplate
    {
        ListItemType gType;
        string gColName;

        public AddGridTemplateToGridView() { }

        public AddGridTemplateToGridView(ListItemType type, string colname)
        {
            gType = type;
            gColName = colname;
        }

        void System.Web.UI.ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            switch (gType)
            {
                case ListItemType.Item:

                    GridView gv = new GridView();
                    gv.ID = "gvChildGrid";
                    gv.CssClass = "scroll";
                    container.Controls.Add(gv);
                    break;

            }
        }
    }
}