using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace Util
{
    public class JQueryUtils
    {
        public static void RegisterPageForDatePicker(Page page)
        {
            //page.ClientScript.RegisterClientScriptInclude(page.GetType(), "jquery", "./Scripts/JQuery/jquery.js");
            page.ClientScript.RegisterClientScriptInclude(page.GetType(), "jquery.ui.all", "./Scripts/JQuery/ui/jquery.ui.all.js");
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "datepickerCss", "<link  rel=\"stylesheet\" href=\"./Scripts/JQuery/themes/flora/flora.datepicker.css\" />");
        }

        public static void SetDatePickerFormat(Page page, string format, params TextBox[] textBoxes)
        {
            bool allTextBoxNull = true;
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox != null) allTextBoxNull = false;
            }

            if (allTextBoxNull) return;


            StringBuilder sb = new StringBuilder();

            sb.Append("$(document).ready(function() {");

          
            string dtString = DateTime.Now.ToShortDateString();

            foreach (TextBox textBox in textBoxes)
            {
                if (textBox != null)
                {
                    sb.Append("$('#" + textBox.ClientID + "').datepicker({ " + 
                        "dateFormat: '" + format + "',"
                        + "changeMonth: true,"
                        + "changeYear: true,"
                        + "yearRange: '-25:+0',"
                        + "maxDate:  '+0:+0',"
                        + "showOn:  'button',"
                        + "buttonImage: '../../images/calendar.png',"
                        + "buttonText: 'Calendar',"
                        + "buttonImageOnly: true,"
                        + "onSelect: function(date) { $('" + textBox.ClientID + "').datepicker('option', 'currentText', date); }"   
                        + "});");
                }
            }
            sb.Append("});");
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "jQueryScript", sb.ToString(), true);
        }
    }

}
