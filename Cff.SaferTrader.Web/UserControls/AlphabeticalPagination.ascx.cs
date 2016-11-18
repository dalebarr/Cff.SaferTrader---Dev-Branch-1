using System;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class AlphabeticalPagination : System.Web.UI.UserControl
    {
        public event EventHandler UpdatePageIndex;

        public bool IsClientContactsIndexing { get; set; }
        public string currentTextPage
        {
            get { return (string)this.ViewState["currentTextPage"]; }
            set { this.ViewState["currentTextPage"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsClientContactsIndexing)
            {
                AllLink.ToolTip = "View All Client Contacts";
                ALink.ToolTip = "Clients beginning with the letter 'A'";
                Blink.ToolTip = "Clients beginning with the letter 'B'";
                Clink.ToolTip = "Clients beginning with the letter 'C'";
                DLink.ToolTip = "Clients beginning with the letter 'D'";
                ELink.ToolTip = "Clients beginning with the letter 'E'";
                FLink.ToolTip = "Clients beginning with the letter 'F'";
                GLink.ToolTip = "Clients beginning with the letter 'G'";
                HLink.ToolTip = "Clients beginning with the letter 'H'";
                ILink.ToolTip = "Clients beginning with the letter 'I'";
                JLink.ToolTip = "Clients beginning with the letter 'J'";
                KLink.ToolTip = "Clients beginning with the letter 'K'";
                LLink.ToolTip = "Clients beginning with the letter 'L'";
                MLink.ToolTip = "Clients beginning with the letter 'M'";
                NLink.ToolTip = "Clients beginning with the letter 'N'";
                OLink.ToolTip = "Clients beginning with the letter 'O'";
                Plink.ToolTip = "Clients beginning with the letter 'P'";
                QLink.ToolTip = "Clients beginning with the letter 'Q'";
                RLink.ToolTip = "Clients beginning with the letter 'R'";
                Slink.ToolTip = "Clients beginning with the letter 'S'";
                TLink.ToolTip = "Clients beginning with the letter 'T'";
                ULink.ToolTip = "Clients beginning with the letter 'U'";
                VLink.ToolTip = "Clients beginning with the letter 'V'";
                WLink.ToolTip = "Clients beginning with the letter 'W'";
                XLink.ToolTip = "Clients beginning with the letter 'X'";
                YLink.ToolTip = "Clients beginning with the letter 'Y'";
                ZLink.ToolTip = "Clients beginning with the letter 'Z'";
                
            }
            else
            {
                AllLink.ToolTip = "View All Customer Contacts";
                ALink.ToolTip = "Customers beginning with the letter 'A'";
                Blink.ToolTip = "Customers beginning with the letter 'B'";
                Clink.ToolTip = "Customers beginning with the letter 'C'";
                DLink.ToolTip = "Customers beginning with the letter 'D'";
                ELink.ToolTip = "Customers beginning with the letter 'E'";
                FLink.ToolTip = "Customers beginning with the letter 'F'";
                GLink.ToolTip = "Customers beginning with the letter 'G'";
                HLink.ToolTip = "Customers beginning with the letter 'H'";
                ILink.ToolTip = "Customers beginning with the letter 'I'";
                JLink.ToolTip = "Customers beginning with the letter 'J'";
                KLink.ToolTip = "Customers beginning with the letter 'K'";
                LLink.ToolTip = "Customers beginning with the letter 'L'";
                MLink.ToolTip = "Customers beginning with the letter 'M'";
                NLink.ToolTip = "Customers beginning with the letter 'N'";
                OLink.ToolTip = "Customers beginning with the letter 'O'";
                Plink.ToolTip = "Customers beginning with the letter 'P'";
                QLink.ToolTip = "Customers beginning with the letter 'Q'";
                RLink.ToolTip = "Customers beginning with the letter 'R'";
                Slink.ToolTip = "Customers beginning with the letter 'S'";
                TLink.ToolTip = "Customers beginning with the letter 'T'";
                ULink.ToolTip = "Customers beginning with the letter 'U'";
                VLink.ToolTip = "Customers beginning with the letter 'V'";
                WLink.ToolTip = "Customers beginning with the letter 'W'";
                XLink.ToolTip = "Customers beginning with the letter 'X'";
                YLink.ToolTip = "Customers beginning with the letter 'Y'";
                ZLink.ToolTip = "Customers beginning with the letter 'Z'";
            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                this.currentTextPage = "ALL";
        }



#region Client Contacts Pagination
        protected void AllLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = "ALL";
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs("ALL"));
            }

        }

        protected void ALink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = ALink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(ALink.Text));
            }

        }

        protected void Blink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = Blink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(Blink.Text));
            }

        }

        protected void Clink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = Clink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(Clink.Text));
            }

        }

        protected void DLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = DLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(DLink.Text));
            }

        }

        protected void ELink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = ELink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(ELink.Text));
            }

        }

        protected void FLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = FLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(FLink.Text));
            }

        }


        protected void GLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = GLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(GLink.Text));
            }

        }


        protected void HLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = HLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(HLink.Text));
            }

        }


        protected void ILink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = ILink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(ILink.Text));
            }

        }

        protected void JLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = JLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(JLink.Text));
            }

        }

        protected void KLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = KLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(KLink.Text));
            }

        }

        protected void LLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = LLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(LLink.Text));
            }

        }

        protected void MLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = MLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(MLink.Text));
            }

        }

        protected void NLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = NLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(NLink.Text));
            }

        }

        protected void OLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = OLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(OLink.Text));
            }

        }

        protected void Plink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = Plink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(Plink.Text));
            }

        }

        protected void QLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = QLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(QLink.Text));
            }

        }

        protected void RLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = RLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(RLink.Text));
            }

        }

        protected void Slink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = Slink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(Slink.Text));
            }

        }

        protected void TLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = TLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(TLink.Text));
            }

        }

        protected void ULink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = ULink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(ULink.Text));
            }

        }

        protected void VLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = VLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(VLink.Text));
            }

        }

        protected void WLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = WLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(WLink.Text));
            }

        }

        protected void XLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = XLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(XLink.Text));
            }

        }

        protected void YLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = YLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(YLink.Text));
            }

        }

        protected void ZLink_Click(object sender, EventArgs e)
        {
            if (UpdatePageIndex != null)
            {
                this.currentTextPage = ZLink.Text;
                UpdatePageIndex(sender, new AlphabeticalPaginationEventArgs(ZLink.Text));
            }

        }

        #endregion
    }
}