using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;



namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{
    public class CffTemplateField : TemplateField
    {
        public Unit ItemStyleWidth { get; set; }
        public HorizontalAlign HorizontalAlignment { get; set; }
        public bool IsReadOnly { get; set; }
        public CffGridViewColumnType ColumnType { get; set; }
        public CffGroupBySettings GroupBySettings;
       
        public string DataBoundColumnName  { 
            get {
                object o = this.ViewState["DataBoundColumnName"];
                return (o==null)?"":o.ToString();
            }
            set {
                this.ViewState.Add("DataBoundColumnName", value); 
            }
        }

        public object ColumnViewState {
            get { 
                //stored here coz columntype field gets reset after postback
                object o = this.ViewState["ColumnType"];
                return o;
            }
        }

        public CffTemplateField()
        {
            HorizontalAlignment = HorizontalAlign.NotSet;
            ItemStyleWidth = Unit.Percentage(100);
            ItemStyle.BorderColor = System.Drawing.Color.LightGray;   //todo: we should be able to set this outside
            IsReadOnly = false;
            ColumnType = CffGridViewColumnType.Text;
        }


        public CffTemplateField(CffGridViewColumnType _colType)
        {
            HorizontalAlignment = HorizontalAlign.NotSet;
            ItemStyleWidth = Unit.Percentage(100);
            ItemStyle.BorderColor = System.Drawing.Color.LightGray;  //todo: we should be able to set this outside
            IsReadOnly = false;
            ColumnType = _colType;
            this.ViewState.Add("ColumnType", _colType);   //add on custom init
        }

    }

    //for queries refer to Mariper

}