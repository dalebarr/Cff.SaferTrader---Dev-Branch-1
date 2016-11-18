using System;
using System.Windows.Forms;

namespace DataGridExtensions
{
	/// <summary>
	///     Derived to expose the CurrencyManager for the 
	///     ComboBox 
	/// </summary>
	internal class DataGridComboBox : ComboBox	{

		internal DataGridComboBox() : base() {
        }

        /// <summary>
        ///     Returns the CurrencyManager
        /// </summary>
        internal CurrencyManager ComboBoxCurrencyManager {
            get {
                return this.DataManager;
            }
        }

    }
}
