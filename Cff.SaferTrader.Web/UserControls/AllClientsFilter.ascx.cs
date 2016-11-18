using System;
using System.Web.UI;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class AllClientsFilter : UserControl
    {
        private bool updateButtonVisible = true;
        private bool showAllKnownFacilityTypes = true;
        public event EventHandler Update;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                PopulateFacilityTypeDropdonw();
                UpdateButtonColumn.Visible = updateButtonVisible;
            }
            catch { }
        }

        private void PopulateFacilityTypeDropdonw()
        {
           try
           {
                FacilityTypeDropDownList.DataSource = FacilityType.KnownTypesAsListItems;

                if (!showAllKnownFacilityTypes)
                {
                    FacilityTypeDropDownList.DataSource = FacilityType.FactoringOrDiscountingTypesAsListItems;
                }

                FacilityTypeDropDownList.DataTextField = "Text";
                FacilityTypeDropDownList.DataValueField = "Value";
                FacilityTypeDropDownList.DataBind();
           } catch { }
        }

        protected void UpdateButton_Click(object sender, ImageClickEventArgs e)
        {
            if (Update != null)
            {
                Update(sender, EventArgs.Empty);
            }
        }

        public FacilityType FacilityType
        {
            get { return FacilityType.Parse(Convert.ToInt32(FacilityTypeDropDownList.SelectedValue)); }
        }

        public bool IsSalvageIncluded
        {
            get { return SalvageIncludedCheckBox.Checked; }
        }

        /// <summary>
        /// Shows or hides Update button - shown by default
        /// </summary>
        public bool UpdateButtonVisible
        {
            get { return updateButtonVisible; }
            set { updateButtonVisible = value; }
        }

        /// <summary>
        /// Shows all known FacilityTypes - shown by default. If false, only FactoringOrDiscountingFacilityTypes are shown
        /// </summary>
        public bool ShowAllKnownFacilityTypes
        {
            get { return showAllKnownFacilityTypes; }
            set { showAllKnownFacilityTypes = value; }
        }
    }
}