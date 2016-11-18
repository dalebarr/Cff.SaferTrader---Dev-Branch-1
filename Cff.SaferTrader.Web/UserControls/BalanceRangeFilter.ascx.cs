using System;
using System.Collections.Generic;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class BalanceRangeFilter : System.Web.UI.UserControl
    {
        public BalanceRange BalanceRange
        {
            get
            {
                return new BalanceRange(int.Parse(MinBalanceDropDownList.SelectedValue),
                                        int.Parse(MaxBalanceDropDownList.SelectedValue));
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ConifgureDropDownList();
        }

        private void ConifgureDropDownList()
        {
            MinBalanceDropDownList.DataSource = LoadMinBanlanceDropDownListValues();
            MinBalanceDropDownList.DataTextField = "Value";
            MinBalanceDropDownList.DataValueField = "Key";
            MinBalanceDropDownList.DataBind();

            MaxBalanceDropDownList.DataSource = LoadMaxBanlanceDropDownListValues();
            MaxBalanceDropDownList.DataTextField = "Value";
            MaxBalanceDropDownList.DataValueField = "Key";
            MaxBalanceDropDownList.DataBind();
        }

        private static Dictionary<double, string> LoadMaxBanlanceDropDownListValues()
        {
            Dictionary<double, string> toDictionary = new Dictionary<double, string>();



            toDictionary.Add(25000000, "25,000,000");
            toDictionary.Add(5000000, "5,000,000");
            toDictionary.Add(2000000, "2,000,000");
            toDictionary.Add(1000000, "1,000,000");
            toDictionary.Add(500000, "500,000");
            toDictionary.Add(250000, "250,000");
            toDictionary.Add(100000, "100,000");
            toDictionary.Add(75000, "75,000");
            toDictionary.Add(50000, "50,000");
            toDictionary.Add(35000, "35,000");
            toDictionary.Add(25000, "25,000");
            toDictionary.Add(15000, "15,000");
            toDictionary.Add(10000, "10,000");
            toDictionary.Add(7500, "7,500");
            toDictionary.Add(5000, "5,000");
            toDictionary.Add(2500, "2,500");
            toDictionary.Add(1500, "1,500");
            toDictionary.Add(1000, "1,000");
            toDictionary.Add(750, "750");
            toDictionary.Add(500, "500");
            toDictionary.Add(250, "250");
            toDictionary.Add(100, "100");
            return toDictionary;
        }

        private static Dictionary<double, string> LoadMinBanlanceDropDownListValues()
        {
            Dictionary<double, string> toDictionary = new Dictionary<double, string>();
            toDictionary.Add(0, "0");
            toDictionary.Add(100, "100");
            toDictionary.Add(250, "250");
            toDictionary.Add(500, "500");
            toDictionary.Add(750, "750");
            toDictionary.Add(1000, "1,000");
            toDictionary.Add(1500, "1,500");
            toDictionary.Add(2500, "2,500");
            toDictionary.Add(5000, "5,000");
            toDictionary.Add(7500, "7,500");
            toDictionary.Add(10000, "10,000");
            toDictionary.Add(15000, "15,000");
            toDictionary.Add(25000, "25,000");
            toDictionary.Add(35000, "35,000");
            toDictionary.Add(50000, "50,000");
            toDictionary.Add(75000, "75,000");
            toDictionary.Add(100000, "100,000");
            toDictionary.Add(150000, "150,000");
            toDictionary.Add(200000, "200,000");
            toDictionary.Add(500000, "500,000");
            toDictionary.Add(1000000, "1,000,000");
            return toDictionary;
        }
    }
}