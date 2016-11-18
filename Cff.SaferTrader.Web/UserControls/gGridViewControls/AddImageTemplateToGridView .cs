using System;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Web.UserControls;

namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{
    public class AddImageTemplateToGridView : System.Web.UI.ITemplate
    {
        ListItemType _type;
        string _colName;

        public AddImageTemplateToGridView() { }

        public AddImageTemplateToGridView(ListItemType type, string colname)
        {
            _type = type;
            _colName = colname;
        }

        void System.Web.UI.ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            switch (_type)
            {
                case ListItemType.Item:

                    ImageButton im = new ImageButton();
                    im.ID = "img1";
                    im.CssClass = "scroll";
                    im.Width = Unit.Percentage(10);
                    im.BorderColor = System.Drawing.Color.Gray;
                    im.BorderStyle = BorderStyle.None;
                    im.ImageUrl = "/images/plus.gif";                    
                    container.Controls.Add(im);
                    break;

            }
        }
    }
}