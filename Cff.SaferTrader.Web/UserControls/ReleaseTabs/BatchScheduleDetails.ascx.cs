using System;
using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class BatchScheduleDetails : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void ShowNote()
        {
            noteRow.Visible = true;
        }

        public void HideNote()
        {
            noteRow.Visible = false;
        }

        public Label AdminFeeLabel
        {
            get { return _adminFeeLabel; }
            set { _adminFeeLabel = value; }
        }

        public Literal AdminFeeLiteral
        {
            get { return _adminFeeLiteral; }
            set { _adminFeeLiteral = value; }
        }

        public Label AdminFeeGstLabel
        {
            get { return _adminFeeGstLabel; }
            set { _adminFeeGstLabel = value; }
        }

        public Literal AdminFeeGstLiteral
        {
            get { return _adminFeeGstLiteral; }
            set { _adminFeeGstLiteral = value; }
        }

        public Label AdminFeeTotalLabel
        {
            get { return _adminFeeTotalLabel; }
            set { _adminFeeTotalLabel = value; }
        }

        public Literal AdminFeeTotalLiteral
        {
            get { return _adminFeeTotalLiteral; }
            set { _adminFeeTotalLiteral = value; }
        }

        public Label FactorFeeLabel
        {
            get { return _factorFeeLabel; }
            set { _factorFeeLabel = value; }
        }

        public Literal FactorFeeLiteral
        {
            get { return _factorFeeLiteral; }
            set { _factorFeeLiteral = value; }
        }

        public Label RetentionLabel
        {
            get { return _retentionLabel; }
            set { _retentionLabel = value; }
        }

        public Literal RetentionLiteral
        {
            get { return _retentionLiteral; }
            set { _retentionLiteral = value; }
        }

        public Label RepurchasesLabel
        {
            get { return _repurchasesLabel; }
            set { _repurchasesLabel = value; }
        }

        public Literal RepurchasesLiteral
        {
            get { return _repurchasesLiteral; }
            set { _repurchasesLiteral = value; }
        }

        public Label CreditLabel
        {
            get { return _creditLabel; }
            set { _creditLabel = value; }
        }

        public Literal CreditLiteral
        {
            get { return _creditLiteral; }
            set { _creditLiteral = value; }
        }

        public Label PostageLabel
        {
            get { return _postageLabel; }
            set { _postageLabel = value; }
        }

        public Literal PostageLiteral
        {
            get { return _postageLiteral; }
            set { _postageLiteral = value; }
        }

        public Label PostGstLabel
        {
            get { return _postGstLabel; }
            set { _postGstLabel = value; }
        }

        public Literal PostGstLiteral
        {
            get { return _postGstLiteral; }
            set { _postGstLiteral = value; }
        }

        public Label PostageTotalLabel
        {
            get { return _postageTotalLabel; }
            set { _postageTotalLabel = value; }
        }

        public Literal PostageTotalLiteral
        {
            get { return _postageTotalLiteral; }
            set { _postageTotalLiteral = value; }
        }

        public Label DeductionsLabel
        {
            get { return _deductionsLabel; }
            set { _deductionsLabel = value; }
        }

        public Literal DeductionsLiteral
        {
            get { return _deductionsLiteral; }
            set { _deductionsLiteral = value; }
        }

        public Label ChargesTotalLabel
        {
            get { return _chargesTotalLabel; }
            set { _chargesTotalLabel = value; }
        }

        public Literal ChargesTotalLiteral
        {
            get { return _chargesTotalLiteral; }
            set { _chargesTotalLiteral = value; }
        }

        public Label AvailableForReleaseLabel
        {
            get { 
                return _availableForReleaseLabel; 
            }
            set {
                _availableForReleaseLabel = value; 
            }
        }

        public Literal AvailableForReleaseLiteral
        {
            get { return _availableForReleaseLiteral; }
            set {
                _availableForReleaseLiteral = value;
            }
        }

        public Label StatusDescriptionLabel
        {
            get { return _statusDescriptionLabel; }
            set { _statusDescriptionLabel = value; }
        }

        public Literal StatusDescriptionLiteral
        {
            get { return _statusDescriptionLiteral; }
            set { _statusDescriptionLiteral = value; }
        }

       // public Label ReleasedLabel
       // {
       //     get { return _releasedLabel; }
       //     set { _releasedLabel = value; }
       // }

       public Literal ReleasedLiteral
       {
            get { return _releasedLiteral; }
            set {
                _releasedLiteral = value;
            }
       }

        public Label DateFinishedLabel
        {
            get { return _dateFinishedLabel; }
            set { _dateFinishedLabel = value; }
        }

        public Literal DateFinishedLiteral
        {
            get { return _dateFinishedLiteral; }
            set { _dateFinishedLiteral = value; }
        }
        public  Literal ScheduleBatchNoteLiteral
        {
            get { return _scheduleBatchNoteLiteral; }
            set { _scheduleBatchNoteLiteral = value; }
            
        }
        public Label ScheduleBatchNoteLabel
        {
            get { return _scheduleBatchNoteLabel; }
            set { _scheduleBatchNoteLabel = value; }
        }
        public Panel DivDeductions
        {
            get { return _DivDeductions; }
            set { _DivDeductions = value; }
        }

        public Label LabelLess
        {
            get { return _LabelLess; }
            set { _LabelLess = value; }
        }

        public Panel PanelCharges
        {
            get { return _PanelCharges; }
            set { _PanelCharges = value; }
        }

        public Panel PanelAssignmentCr 
        {
            get { return _PanelAssignmentCr; }
            set { _PanelAssignmentCr = value; }
        }


        public Literal LiteralAssignCr
        {
            get { return _LiteralAssignCr; }
            set { _LiteralAssignCr = value; }
        }


        public Panel PanelRetention 
        {
            get { return _PanelRetention; }
            set { _PanelRetention = value; }
        }

        public Panel PanelCredit 
        {
            get { return _panelCredit; }
            set { _panelCredit = value; }
        }


        public Panel PanelPrepayment 
        {
            get { return _panelPrepayment; }
            set { _panelPrepayment = value; }
        }

                

        public Literal LiteralResidual
        {
            get { return _LiteralResidual; }
            set { _LiteralResidual = value; }
        }

        public Literal RepurchLiteral100
        {
            get { return _repurchLiteral100; }
            set { _repurchLiteral100 = value; }
        }

        public Literal CreditLiteral100 
        {
            get { return _creditLiteral100; }
            set { _creditLiteral100 = value; }
        }
        public Label CreditResidual        
        {
            get { return _creditResidual; }
            set { _creditResidual = value; }
        }


        public Label RepurchResidual        
        {
            get { return _repurchResidual; }
            set { _repurchResidual = value; }
        }


        public Literal LiteralSumFeesCA        
        {
            get { return _literalSumFeesCA; }
            set { _literalSumFeesCA = value; }
        }

        public Panel PanelSumFeesForCA 
        {
            get { return _panelSumFeesForCA; }
            set { _panelSumFeesForCA = value; }
        }

        public Panel PanelAdminFeeGst 
        {
            get { return _panelAdminFeeGst; }
            set { _panelAdminFeeGst = value; }
        }

        public Panel PanelPostGst
        {
            get { return _panelPostGst; }
            set { _panelPostGst = value; }
        }
        

        public Literal NonCompliantFeeLiteral        
        {
            get { return _nonCompliantFeeLiteral; }
            set { _nonCompliantFeeLiteral = value; }
        }


        public Panel PanelNonCompliantFee
        {
            get { return _panelNonCompliantFee; }
            set { _panelNonCompliantFee = value; }
        }

        public Label NonCompliantFeeLabel
        {
            get { return _nonCompliantFeeLabel; }
            set { _nonCompliantFeeLabel = value; }
        }

        public Panel PanelViewRepurchases
        {
            get { return _panelViewRepurchases; }
            set { _panelViewRepurchases = value; }
        }
        
    }
}