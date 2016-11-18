using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Web.App_GlobalResources;

namespace Cff.SaferTrader.Web.UserControls
{
    /// <summary>
    /// Decorates a WebControl with onCellClick which performs callback to redirect
    /// </summary>
    public class LinkDecorator
    {
        private readonly CallbackParameter parameter;
        private readonly WebControl link;
        private readonly ISecurityManager securityManager;
        private readonly CffGridView grid;

        public LinkDecorator(CffGridView grid, CallbackParameter parameter, WebControl link, ISecurityManager securityManager)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(grid.ClientInstanceName, "grid");

            this.grid = grid;
            this.parameter = parameter;
            this.link = link;
            this.securityManager = securityManager;
        }

        public void Decorate()
        {
            string fieldName = parameter.FieldName;

            if (fieldName.Equals("Batch") && securityManager.CanViewReleaseTab())
            {
                BatchRecord record = grid.GetRow(parameter.RowIndex) as BatchRecord;
                if (record != null && record.IsInvoice)
                {
                    Decorate(Cff_WebResource.batchNumberLinkTooltip);
                }
            }
            else if (fieldName.Equals("ClientName") && securityManager.CanChangeSelectedClient())
            {
                Decorate(Cff_WebResource.clientNameLinkTooltip);
            }
            else if (fieldName.Equals("CustomerName") && securityManager.CanChangeSelectedCustomer())
            {
                Decorate(Cff_WebResource.customerNameLinkTooltip);
            }
        }

        private void Decorate(string tooltip)
        {
            link.Attributes.Add("onclick", string.Format("onCellClick({0}, '{1}');return false;", grid.ClientInstanceName, parameter));
            link.CssClass += " linkCell";
            link.ToolTip = tooltip;
        }
    }
}