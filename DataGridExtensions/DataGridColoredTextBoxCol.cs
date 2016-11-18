using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace DataGridExtensions
{
    public class DataGridColoredTextBoxCol : DataGridTextBoxColumn
    {
        public string compareValue;
        
        protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, System.Windows.Forms.CurrencyManager source, 
                                            int rowNum, System.Drawing.Brush backBrush, System.Drawing.Brush foreBrush, bool alignToRight)
        {
            try
            {

                object o = this.GetColumnValueAtRow(source, rowNum);
                if (o != null)
                {
                    if (compareValue=="CLOSINGDATE")
                    {
                        if (Convert.ToDateTime(o.ToString()) < DateTime.Now)
                        {
                            //backBrush = new LinearGradientBrush(bounds, Color.FromArgb(255, 200, 200), Color.FromArgb(255, 20, 20), LinearGradientMode.BackwardDiagonal);
                            backBrush = new LinearGradientBrush(bounds, Color.Honeydew, Color.PaleGoldenrod, LinearGradientMode.BackwardDiagonal);
                            foreBrush = new SolidBrush(Color.OrangeRed);
                        }
                    }
                }
            }
            catch (Exception ex) {  /* empty catch */ }
            finally { base.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight); }


        }

    }
}
