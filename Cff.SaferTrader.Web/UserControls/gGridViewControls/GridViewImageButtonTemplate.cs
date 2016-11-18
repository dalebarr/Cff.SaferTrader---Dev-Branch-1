using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{
    public class GridViewImageButtonTemplate : System.Web.UI.ITemplate
    {
        public String _colName { get; set; }
        public String _imageUrl{ get; set; }
        public Unit _imageWidth;
  
        BorderStyle _imageBorderStyle;

        public GridViewImageButtonTemplate(string colname, string imageUrl, BorderStyle ImageBorderStyle = BorderStyle.None)
        {
            _colName = colname;
            _imageUrl = imageUrl;
            _imageBorderStyle = ImageBorderStyle;            

        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            var imageField = new System.Web.UI.WebControls.ImageButton();
            imageField.DataBinding  +=imageField_DataBinding;
            container.Controls.Add(imageField);
        }

        void System.Web.UI.ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            ImageButton im = new ImageButton();
            im.ID = "img_" + container.ID;
            im.CssClass = "dxgv";
            im.Width = _imageWidth;
            im.BorderStyle = _imageBorderStyle;
            im.ImageUrl = _imageUrl;
            container.Controls.Add(im);
        }

        void imageField_DataBinding(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
       
    }

}