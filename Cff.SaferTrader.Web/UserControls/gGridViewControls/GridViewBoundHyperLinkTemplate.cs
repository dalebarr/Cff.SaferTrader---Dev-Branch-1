using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls.gGridViewControls
{
    //ref.mariper - exclusively created for CFF requirements
    public class GridViewBoundHyperLinkTemplate : ITemplate
    {
        private CffGridViewColumnType _columnType;
        private string _navigateUrl;
        private String _boundFilterField;
        private String _boundFilterFieldValue;
        private bool _onOff;
        private bool _isReversed;
        private string _cssStyle;

        public bool OnOff { get { return _onOff; } }
        public bool IsReversed { get { return _isReversed; } }
        public String DataField { get; set; }
        public String DisplayField { get; set; }
        public String BoundFilterField { get { return _boundFilterField; } }
        public String BoundFilterFieldValue { get { return _boundFilterFieldValue; } }
        public string NavigateUrl { get { return _navigateUrl; } }

        /// <summary>
        /// Initialize Grid View Bound Hyper Link Template
        /// </summary>
        /// <param name="theColumnType">hyperlink</param>
        /// <param name="boundColumnText">display text</param>
        /// <param name="boundColumnValue">bound field name</param>
        /// <param name="hyperLinkFilterField">populate this column if you want to enable field filtering of hyperlink: format - "boundFilterField:boundFilterValue:on/off"</param>
        public GridViewBoundHyperLinkTemplate(CffGridViewColumnType theColumnType, string boundColumnText, string boundColumnValue, string hyperLinkFilterField = "", string cssStyle = "cffGGV_HyperLink")
        {
            string[] strArrayFilter = hyperLinkFilterField.Split(',');
            string[] strDummy = hyperLinkFilterField.Split(':');

            DataField = boundColumnValue;
            _columnType = theColumnType;
            
            _boundFilterField = strDummy[0];
            if (strDummy.Length > 1)
                _boundFilterFieldValue = strDummy[1];

            _onOff = false;
            if (strDummy.Length > 2)
                _onOff = (strDummy[2].ToLower() == "on") ? true : false;

            if (strDummy.Length > 3)
                _isReversed = true;
            else
                _isReversed = false;

            if (!string.IsNullOrEmpty(cssStyle))
                _cssStyle = cssStyle;

            DisplayField = boundColumnText;
            if (string.IsNullOrEmpty(boundColumnText))
                DisplayField = DataField;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            var linkField = new System.Web.UI.WebControls.HyperLink();
            linkField.DataBinding += linkField_DataBinding;
            linkField.Target = "_blank";
            linkField.CssClass = _cssStyle;
            container.Controls.Add(linkField);
        }

        void linkField_DataBinding(object sender, EventArgs e)
        {
            var linkField = (HyperLink)sender;
            var context = DataBinder.GetDataItem(linkField.NamingContainer);
            var boundFieldValue = (DataBinder.Eval(context, DataField)==null)?"":(DataBinder.Eval(context, DataField)).ToString();
            if (DisplayField == null)
                linkField.Text = "";
            else
                linkField.Text = CffGenGridViewCommon.FormatDataColumn(this._columnType, 
                                        (DataBinder.Eval(context, DisplayField) == null) ? "" : (DataBinder.Eval(context, DisplayField)).ToString());

            string strRowIndex = ((System.Web.UI.WebControls.GridViewRow)(((System.Web.UI.Control)(linkField)).DataItemContainer)).RowIndex.ToString();
            Cff.SaferTrader.Core.LinkHelper.SetRowIndex(strRowIndex);

            linkField.CssClass = this._cssStyle;

            //link helpers - careful when modifying as this needs to coincide with production version (ref: mariper)
            if (DataField.ToUpper().IndexOf("CUSTOMER") >= 0 || DataField.ToUpper().IndexOf("CUSTID") >= 0 || DataField.ToUpper().IndexOf("CUSTOMERID") >= 0)
            {
                if (string.IsNullOrEmpty(boundFieldValue))
                    linkField.NavigateUrl = Cff.SaferTrader.Core.LinkHelper.NavigateUrlFormatToDashboardForCustomer;
                else
                {
                    if (SessionWrapper.Instance.Get != null)
                    {
                        if (SessionWrapper.Instance.Get.Scope == Scope.AllClientsScope)
                        {
                            linkField.NavigateUrl = Cff.SaferTrader.Core.LinkHelper.NavigateUrlFormatToDashboardForGivenCustomerId(boundFieldValue, (int?)DataBinder.Eval(context, "ClientId"));
                        }
                        else
                            linkField.NavigateUrl = Cff.SaferTrader.Core.LinkHelper.NavigateUrlFormatToDashboardForGivenCustomerId(boundFieldValue);
                    }
                }
            }
            else if (DataField.ToUpper().IndexOf("CLIENT") >= 0)
            {
                linkField.NavigateUrl = Cff.SaferTrader.Core.LinkHelper.NavigateUrlFormatToDashboardForAGivenClientId(boundFieldValue);
            }
            else if (DataField.ToUpper().IndexOf("CLIENTID") >= 0)
            {
                linkField.NavigateUrl = Cff.SaferTrader.Core.LinkHelper.NavigateUrlFormatToDashboardForAGivenClientId(boundFieldValue);
            }
            else if ((DataField.ToUpper().IndexOf("BATCH") >= 0) || (DataField.ToUpper().IndexOf("BATCHNUMBER") >= 0))
            {
                string strCustID = "";
                string strClientID = "";
                try {
                    strClientID = (DataBinder.Eval(context, "ClientId") == null) ? "" : DataBinder.Eval(context, "ClientId").ToString();
                }
                catch (Exception) { }

                try {
                    strCustID = (DataBinder.Eval(context, "CustomerId") == null) ? "" : DataBinder.Eval(context, "CustomerId").ToString();
                } catch (Exception) { }


                bool doHyperLink = (_isReversed)?false:true;
                if (!string.IsNullOrEmpty(_boundFilterField)) {
                    if (_boundFilterFieldValue == ((DataBinder.Eval(context, _boundFilterField) == null) ? "" : (DataBinder.Eval(context, _boundFilterField)).ToString())) 
                    {
                        doHyperLink = _onOff;
                    }
                    //todo:: assign necessary filtering if arrayed filter fields
                }
              
                if (doHyperLink)
                {
                    if (string.IsNullOrEmpty(boundFieldValue))
                        linkField.NavigateUrl = Cff.SaferTrader.Core.LinkHelper.NavigateUrlFormatToInvoiceBatchesForClient;
                    else
                        linkField.NavigateUrl = Cff.SaferTrader.Core.LinkHelper.NavigateUrlFormatToInvoiceBatchesForClientWBatchID(boundFieldValue, strCustID, strClientID);

                    linkField.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
                    linkField.ToolTip = "Click to View Batch";
                    linkField.Attributes.Add("title", "Click to View Batch");
                    this._navigateUrl = linkField.NavigateUrl;
                }
                else 
                { //disable hyperlink
                    linkField.Style.Add(HtmlTextWriterStyle.TextDecoration, "none");
                    linkField.Style.Add(HtmlTextWriterStyle.Color, "black");
                }
            }
        }
    }
}