using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{
    public class CffCommandField : CommandField
    {
        private string _commandTag;

        public virtual bool ShowAddButton { get; set; }
        public virtual bool ShowUpdateButton { get; set; }
        public virtual bool ShowNewButton { get; set; }

        new public virtual bool ShowEditButton { get; set; }
        new public virtual bool ShowCancelButton { get; set; }

        public string DataBoundColumnName { get; set; }
        public HorizontalAlign HorizontalAlignment { get; set; }
        public Unit ItemStyleWidth { get; set; }
        public Unit ItemStyleHeight { get; set; }

        public bool isImageButton { get; set; }
        public bool isExpandingButton { get; set; }
        public bool isExpanded { get; set; }

        public string CommandTag { get { return this._commandTag; } }

        public int VisibleIndex { get; set; }

        public string AddImageUrl { get; set; }
        public string ImageButtonURL { get; set; }
        new public string EditImageUrl { get; set; }
        new public string CancelImageUrl { get; set; }

        public CffCommandField() : base()
        {
            HorizontalAlignment = HorizontalAlign.Center;
            isImageButton = false;
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.EditImageUrl = this.EditImageUrl;
            base.UpdateImageUrl = this.UpdateImageUrl;
            base.CancelImageUrl = this.CancelImageUrl;
            base.InsertImageUrl = this.InsertImageUrl;

            if (ShowAddButton)
            { //reuse the insert button
                base.ShowInsertButton = true;
                base.InsertImageUrl = this.AddImageUrl;
                this._commandTag = "Add";
            }

            if (ShowEditButton)
            {
                base.ShowEditButton = true;
                base.EditImageUrl = this.EditImageUrl;
                this._commandTag = "Edit";
            }

            if (ShowCancelButton)
            {
                base.ShowCancelButton = true;
                base.CancelImageUrl = this.CancelImageUrl;
                this._commandTag = "Cancel";
            }
                        
            if (ShowUpdateButton)
            {
                base.UpdateImageUrl= this.UpdateImageUrl;
                this._commandTag = "Update";
            }

            if (isImageButton)
            { //reuse the select button
                base.ShowSelectButton = true;
                base.SelectImageUrl = this.ImageButtonURL;
                this._commandTag = "Image";
            }

            if (isExpandingButton)
            { //reuse the select button
                base.ShowSelectButton = true;
                base.SelectImageUrl = this.ImageButtonURL;
                this._commandTag = "Expand";
            }

            if (ShowNewButton)
            { //reuse the select button
                base.ShowSelectButton = true;
                base.SelectImageUrl = this.NewImageUrl;
                this._commandTag = "New";
            }

            base.ItemStyle.CssClass = this.ControlStyle.CssClass;
            base.ItemStyle.Width = this.ItemStyle.Width;
            base.ControlStyle.BorderWidth = Unit.Pixel(0);
            base.InitializeCell(cell, cellType, rowState, rowIndex);
         }

         public Control GetControl
         {
            get { return base.Control; }
         }
 
    }
    
}