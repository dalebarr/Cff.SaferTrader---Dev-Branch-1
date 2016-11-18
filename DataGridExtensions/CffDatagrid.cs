using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DataGridExtensions
{
    public partial class CffDatagrid : System.Windows.Forms.DataGrid
    {
        public CffDatagrid()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
            {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    //case Keys.Down:
                    //    this.Parent.Text = "Down Arrow Captured";
                    //    break;

                    //case Keys.Up:
                    //    this.Parent.Text = "Up Arrow Captured";
                    //    break;

                    //case Keys.Tab:
                    //    this.Parent.Text = "Tab Key Captured";
                    //    break;

                    //case Keys.Control | Keys.M:
                    //    this.Parent.Text = "<CTRL> + M Captured";
                    //    break;

                    case Keys.Alt | Keys.Z:
                        this.Parent.Text = "<ALT> + Z Captured";
                        break;

                    //case Keys.Alt | Keys.Down:
                    //    this.Parent.Text = "<ALT> + Down Arrow Captured";
                    //    break;

                    //case Keys.Alt:
                    //    this.Parent.Text = "<ALT>";
                    //    break;

                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }



    }
}
