using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{
    public class GridViewCommandButtonTemplate : System.Web.UI.ITemplate
    {
        string _HeaderText;
        string _BoundFieldName;
        string _customCssStyle;
        string _alt;


        int _buttonCount;
        bool _isAutoPostBack;
        bool _isReturnRowIndex;

        CffGridViewButtonType _buttonType;
        CffGridViewEditingMode _gvEditRowType;
        public CffGridViewEditingMode EditRowType { set { _gvEditRowType = value; } }

        public GridViewCommandButtonTemplate(string headerText, string boundFieldName, 
                                                CffGridViewButtonType buttonType, bool isAutoPostBack = true, 
                                                    string customCssStyle= "cffGGV_underlineBoundButton",
                                                        string alt="", bool isReturnRowIndex=false)
        {
            _HeaderText = headerText;
            _BoundFieldName = boundFieldName;
            _buttonType = buttonType;
            _buttonCount = 0;
            _gvEditRowType = CffGridViewEditingMode.EditFormAndDisplayRow;
            _isAutoPostBack = isAutoPostBack;
            _customCssStyle = customCssStyle;
            _alt = alt;
            _isReturnRowIndex = isReturnRowIndex;
        }

        void System.Web.UI.ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            if (_buttonType == CffGridViewButtonType.Bound)
            {
                Button boundButton = new Button();
                boundButton.DataBinding += boundButton_DataBinding;
                boundButton.Attributes.Add("autopostback", (_isAutoPostBack)?"true":"false");
                boundButton.BorderStyle = BorderStyle.None;
                boundButton.CssClass = this._customCssStyle;
                if (!string.IsNullOrEmpty(this._alt))
                {
                    boundButton.ToolTip = this._alt;
                }
                container.Controls.Add(boundButton);
            }
            else
            {
                if (_buttonType.HasFlag(CffGridViewButtonType.Add))
                { //insert an add button in this container
                    ImageButton addButton = new ImageButton();
                    addButton.ImageUrl = "~/images/btn_sm_add.png";
                    addButton.Click += addButton_Click;
                    container.Controls.Add(addButton);
                    _buttonCount += 1;
                }

                if (_buttonType.HasFlag(CffGridViewButtonType.Edit))
                { //insert an edit button in this container
                    ImageButton editButton = new ImageButton();
                    editButton.ImageUrl = "~/images/btn_sm_edit.png";
                    editButton.CssClass = "cffGV_CommandButtons";
                    editButton.Width = Unit.Pixel(25);
                    editButton.Height = Unit.Pixel(25);
                    editButton.ImageAlign = ImageAlign.Middle;
                    editButton.Click += editButton_Click;
                    container.Controls.Add(editButton);
                    _buttonCount += 1;
                }

                if (_buttonType.HasFlag(CffGridViewButtonType.Delete))
                { //insert a delete button in this container
                    ImageButton deleteButton = new ImageButton();
                    deleteButton.ImageUrl = "~/images/btn_sm_delete.png";
                    deleteButton.Click += deleteButton_Click;
                    container.Controls.Add(deleteButton);
                    _buttonCount += 1;
                }

                if (_buttonType.HasFlag(CffGridViewButtonType.Cancel))
                { //insert a cancel button in this container
                    ImageButton cancelButton = new ImageButton();
                    cancelButton.ImageUrl = "~/images/btn_sm_cancel.png";
                    cancelButton.Click += cancelButton_Click;
                    container.Controls.Add(cancelButton);
                    _buttonCount += 1;
                }

                if (_buttonType.HasFlag(CffGridViewButtonType.Update))
                { //insert a cancel button in this container
                    ImageButton updateButton = new ImageButton();
                    updateButton.ImageUrl = "~/images/btn_sm_update.png";
                    updateButton.Click += updateButton_Click;
                    container.Controls.Add(updateButton);
                    _buttonCount += 1;
                }

                if (_buttonType.HasFlag(CffGridViewButtonType.Image_batch))
                { //insert a cancel button in this container
                    ImageButton viewBatchButton = new ImageButton();
                    viewBatchButton.ImageUrl = "~/images/btn_view_batch.png";
                    viewBatchButton.Click +=viewBatchButton_Click;
                    container.Controls.Add(viewBatchButton);
                    _buttonCount += 1;
                }

                if (_buttonType.HasFlag(CffGridViewButtonType.Image_estimate))
                { //insert a cancel button in this container
                    ImageButton viewEstimateButton = new ImageButton();
                    viewEstimateButton.ImageUrl = "~/images/btn_view_estimate.png";
                    viewEstimateButton.Click += viewEstimateButton_Click;
                    container.Controls.Add(viewEstimateButton);
                    _buttonCount += 1;
                }

                if (_buttonType.HasFlag(CffGridViewButtonType.Image_retention))
                { //insert a cancel button in this container
                    ImageButton viewRetentionButton = new ImageButton();
                    viewRetentionButton.ImageUrl = "~/images/btn_view_retention.png";
                    viewRetentionButton.Click += viewRetentionButton_Click;
                    container.Controls.Add(viewRetentionButton);
                    _buttonCount += 1;
                }
            }
        }

        void viewRetentionButton_Click(object sender, ImageClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        void viewEstimateButton_Click(object sender, ImageClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        void viewBatchButton_Click(object sender, ImageClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        void boundButton_DataBinding(object sender, EventArgs e)
        {
            var bButton = (Button)sender;
            var context = DataBinder.GetDataItem(bButton.NamingContainer);
            if (DataBinder.Eval(context, _BoundFieldName) != null)
            {
                bButton.Text = DataBinder.Eval(context, _BoundFieldName).ToString();
                if (this._isReturnRowIndex)
                    bButton.CommandArgument = "Row" + ((System.Web.UI.WebControls.GridViewRow)(((System.Web.UI.Control)(sender)).BindingContainer)).RowIndex.ToString();
                else
                    bButton.CommandArgument = bButton.Text;
                bButton.CommandName = "Bound";
            }
            else
                bButton.Text = "";
        }

        void editButton_Click(object sender, ImageClickEventArgs e)
        {
            var eButton = (ImageButton)sender;
            //todo figure out how to transfer control to calling program
            //throw new NotImplementedException();
        }

        void addButton_Click(object sender, ImageClickEventArgs e)
        {
            //todo figure out how to transfer control to calling program
            //throw new NotImplementedException();
        }

        void updateButton_Click(object sender, ImageClickEventArgs e)
        {
            //throw new NotImplementedException();
            //todo figure out how to transfer control to calling program
        }

        void cancelButton_Click(object sender, ImageClickEventArgs e)
        {
            //todo figure out how to transfer control to calling program
            //throw new NotImplementedException();
        }

        void deleteButton_Click(object sender, ImageClickEventArgs e)
        {
            //todo figure out how to transfer control to calling program
            //throw new NotImplementedException();
        }
    }
}