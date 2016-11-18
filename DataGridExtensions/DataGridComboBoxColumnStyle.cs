using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;


//TODO: Formatting
//TODO: Better keyboard handling
//TODO: DBNull needs improving
//TODO: Cut & Paste

namespace DataGridExtensions {

    // A sample DataGrid ComboBox column.
    public class DataGridComboBoxColumnStyle : DataGridColumnStyle {

        // UI Constants
        private int              xMargin =           2;
        private int              yMargin =           1;
        
        //The combobox we are going to bind to
        private DataGridComboBox combo;

        // These variables track the editing state.
        private  int  oldIndex = 0;
        private int F_DIndex = -1;
        //Mart 20080613 . was originally false!
        private bool inEdit = false;
        
        //private bool inEdit = true;
        //Mart 20080613 

        //Cache property descriptor values to make text lookup faster
        PropertyDescriptor valueMemberProperty            = null ; 
        bool               valueMemberPropertyInitialized = false ;

        PropertyDescriptor displayMemberProperty            = null ; 
        bool               displayMemberPropertyInitialized = false ;

        /// <summary>
        ///   Creates an new DataGridComboBoxColumnStyle
        /// </summary>
        public DataGridComboBoxColumnStyle() {
            combo = new DataGridComboBox();
            combo.Visible = false;
            

            //Hooked to cache ValueMember and DisplayMember PropertyDescriptors
            combo.ValueMemberChanged += new EventHandler(comboValueMemberChanged);
            combo.DisplayMemberChanged += new EventHandler(comboDisplayMemberChanged);
        }

        // ----------------------------------------------------------
        // Lookup table properties
        // ----------------------------------------------------------

        /// <summary>
        /// The DataSource for list of values displayed 
        /// in the ComboBox. 
        /// </summary>
        public object DataSource {
            get {
                return combo.DataSource;
            }
            set {
                combo.DataSource = value;
            }
        }

        /// <summary>
        /// The name of the property to display for each 
        /// item in the Combobox. This is optional
        /// e.g. State.LongName
        /// </summary>
        public string DisplayMember {
            get {
                return combo.DisplayMember;
            }
            set {
                combo.DisplayMember = value;
            }
        }

        /// <summary>
        ///  The name of the property that will be pushed into
        ///  the current row
        ///  e.g. State.ShortName
        /// </summary>
        public string ValueMember {
            get {
                return combo.ValueMember;
            }
            set {
				combo.ValueMember = value;
            }
        }

        // ----------------------------------------------------------
        // Methods overridden from DataGridColumnStyle
        // ----------------------------------------------------------

        /// <summary>
        ///     Called when the user or the Grid aborts an edit 
        ///     For example hits the Escape key
        /// </summary>
        protected override void Abort(int rowNum) {
            //Debug.WriteLine("Start Abort: " + rowNum);
            
            RollBack();
            HideComboBox();
            EndEdit();
        }

        /// <summary>
        /// Called for the 'from' cell, when the current cell changes. 
        /// This is called whether there are changes to commit or not. 
        /// If the user has not edited the value we simply hide the combobox. 
        /// InEdit is used to track whether the cell has been edited or not.
        /// </summary>
        protected override bool Commit(CurrencyManager dataSource, int rowNum) { 
            //Debug.WriteLine("Start Commit: " + rowNum);

            HideComboBox();

            if (F_DIndex > -1)
            {

                combo.SelectedIndex = F_DIndex;
                F_DIndex = -1;
                    //InEdit = true;
            }

            if (combo.SelectedIndex < 0)
            {
                InEdit = false;
            }


            

            // If we are not in an edit, simply return.
            if (!InEdit) return true; 

            //Now get the value out of the ComboBox and push it back
            //into the datamodel
            try {
                string comboText = combo.Text;
                object value = null;

                // If the Combo Text is equal to the NullText, then set the value 
                // to DBNull otherwise set the selected value back into the grid
                // Done like this because the selected value may not be a string
                if (NullText.Equals(comboText)) {
                    value = Convert.DBNull;
                } else {
                    value = combo.SelectedValue;

                    //If the user types in a value we don't automatically pick it up 
                    // as the selected value so force the best fit value
                    if (value == null) {
                        int s = combo.FindString(comboText);
                        combo.SelectedIndex = s ;
                        value = combo.SelectedValue;
                    }
                }

                //Push the value back into the DataGrid 

                
                SetColumnValueAtRow(dataSource,rowNum,value);
            }
            catch (Exception) {
                RollBack();
                //Should possibly EndEdit here
                return false;
            }
            EndEdit();
            return true;
        }


        /// <summary>
        ///     Called when the Column (not the Grid just the column) is
        ///     readonly to take focus away from the Combobox
        /// </summary>
        protected override void ConcedeFocus() {
            //Debug.WriteLine("Start ConcedeFocus");

            combo.Visible = false;
        }


        /// <summary>
        ///     Edit is called when a cell column is made the active cell
        ///     We don't put the Grid into edit mode here - we wait for 
        ///     the user to change something before going into edit mode
        ///     because we don't want to dirty the data model unecessarily
        /// </summary>
        protected override void Edit( CurrencyManager source
                                    , int rowNum
                                    , Rectangle bounds
                                    , bool readOnly
                                    , string instantText
                                    , bool cellIsVisible
                                    ) 
        {
            //Debug.WriteLine("Start Edit - RN:" + rowNum + ", RO:" + readOnly + ", IT:" + instantText + ", CV:" + cellIsVisible);

            //Unhook the combo events while we set up the ComboBox

            combo.SelectedIndexChanged -= new EventHandler(comboSelectedIndexChanged);
            //combo.SelectionChangeCommitted -= new EventHandler(SelectionChangeCommitted);
 
            combo.KeyDown -= new KeyEventHandler(comboKeyDown);

            //Reset the ComboBox 
            combo.Text = null ;

            //Store the real bounds
            Rectangle originalBounds = bounds;

            //Set the selected value for the Combobox
            //This attempts to deal with null values
            //and fails
            object value = GetColumnValueAtRow(source,rowNum);
            if (!value.Equals(DBNull.Value)) {
                //if (!value.Equals(-1)) {
                    combo.SelectedValue = value;
                //} else {
                //    combo.SelectedIndex = -1;
                //}
            } else {
                combo.SelectedIndex = -1;
            }

            // If we have instanceText push this into the Combobox
            // I'm unclear on when this happens also very inefficient
            // because it causes a scan across the combobox values
            if (instantText != null) {
                combo.Text = instantText;
            }

            // Store the old value in case of a rollback
            // Store the index because index based lookups are more efficient
            oldIndex = combo.SelectedIndex;

            //If the cell is visible then adjust the size of the
            //combobox otherwise leave it as is
            if (cellIsVisible) {
                bounds.Offset(xMargin, yMargin);
                bounds.Width  -= xMargin*2;
                bounds.Height -= yMargin;
                combo.Bounds = bounds;
                combo.Visible = true;
            }
            else {
                combo.Bounds = originalBounds;
                combo.Visible = false;
            }

            //If the cell is readonly don't allow editing of the value
            //this.ReadOnly only reflects readonly on the Column not on the Grid 
            //or the TableStyle. The readonly parameter also reflects 
            //IBindingList.AllowEdit 
            //RealReadOnly takes all of these things into account and so I ignore 
            //the readonly param passed into this method
            combo.Enabled  = !this.RealReadOnly;

            combo.RightToLeft = this.DataGridTableStyle.DataGrid.RightToLeft;

            //Set the selection
            if (instantText == null)
            {
                combo.SelectAll();
            }
            else {
                int end = combo.Text.Length;
                combo.Select(end, 0);
            }

            //If necessary repaint the grid
            if (combo.Visible) {
                DataGridTableStyle.DataGrid.Invalidate(originalBounds);
            }

            //Hook up the change events to determine when the user 
            //starts to edit the value either by typing or by 
            //changing the selected value

            combo.SelectedIndexChanged += new EventHandler(comboSelectedIndexChanged);
            //combo.SelectionChangeCommitted += new EventHandler(SelectionChangeCommitted);


            combo.KeyDown += new KeyEventHandler(comboKeyDown);

            //Finally make sure the combobox has focus
            combo.Focus();
            //Mart 20080613 
        }

        /// <summary>
        ///     Called when the user presses Ctrl+0 to enter a null
        /// </summary>
        protected override void EnterNullValue() {
            //Debug.WriteLine("Start EnterNullValue");
            
            //Do nothing if the grid/column/datamodel is readonly
            if (this.RealReadOnly) return;

            //Tell the combo its in Edit Mode
            InEdit = true;

            //Set the combo box value to the null text value
            combo.Text = NullText; //BUG?Fails?
        }


        /// <summary>
        ///     Called to get the minimum height that rows this column 
        ///     can be
        /// </summary>
        protected override int GetMinimumHeight() {
            //Debug.WriteLine("Start GetMinimumHeight");

            // Sets the minimum height to the height of the combobox.
            return combo.PreferredHeight + yMargin;
        }


        /// <summary>
        ///     Called to get the preferred height of the cell 
        ///     
        ///     This called to calculate the preferred height for a row when a 
        ///     row is auto-resized by double-clicking on the row border in 
        ///     the row header 
        ///
        ///     Its also used to paint the parent row when drilled into a child, however 
        ///     in both cases only the width is used never the height
        /// </summary>
        protected override int GetPreferredHeight(Graphics g, object value) {
            //Debug.WriteLine("Start GetPreferredHeight " + value);
            
            return Math.Min(this.GetPreferredSize(g, value).Height, this.GetMinimumHeight());
        }

        /// <summary>
        ///     Called to get the preferred size of the cell 
        ///     
        ///     This called to calculate the preferred width for a column when a 
        ///     column is auto-resized by double-clicking on the column border in 
        ///     the header 
        ///
        ///     Its also used to paint the parent row when drilled into a child, however 
        ///     in both cases only the width is used never the height
        /// </summary>
        protected override Size GetPreferredSize(Graphics g, object value) {
            //Debug.WriteLine("Start GetPreferredSize " + value);
            
            string text = LookupDisplayText(value, -1);
            Font textFont = this.DataGridTableStyle.DataGrid.Font;
            int dataGridLineWidth = this.DataGridTableStyle.GridLineStyle == DataGridLineStyle.Solid ? 1 : 0 ;

            Size preferredSize = Size.Ceiling(g.MeasureString(text, textFont));
            preferredSize.Width += xMargin*2 + dataGridLineWidth;
            preferredSize.Height += yMargin;
            return preferredSize;
        }
       
        /// <summary>
        ///     Paint 1: Called to paint the contents of a particular cell. 
        ///     
        ///     This is the rountine that is most commonly called to paint the contents 
        ///     of the cell. 
        ///     
        ///     Paint 2 and Paint 3 are funnelled into this routine
        /// </summary>
        protected override void Paint( Graphics g
                                     , Rectangle bounds
                                     , CurrencyManager currencyManager
                                     , int rowNum
                                     , Brush backBrush
                                     , Brush foreBrush
                                     , bool alignToRight) 
        {
            //Debug.WriteLine("Start Paint 1 (g, bounds, cm, rowNum, backBrush, foreBrush, alignToRight) " + rowNum);

            //Look up the text to display for this cell
            string text = "";
            bool doPaint = false;
            if (combo.Parent.Name == "dgCrJnlRep")
            {
                if (combo.Parent.Tag != null){
                    if (Convert.ToInt16(combo.Parent.Tag)== -1)
                    {
                        doPaint = true;
                    }
                    else if (Convert.ToInt16(combo.Parent.Tag) == Convert.ToInt16(rowNum))//(convert.tonumber(combo.Tag) = rowNum)
                    {
                        text = LookupDisplayText(GetColumnValueAtRow(currencyManager,rowNum), rowNum);
                        doPaint = true;
                    }
                }

                else //if Null
                {
                    text = LookupDisplayText(GetColumnValueAtRow(currencyManager, rowNum), rowNum);
                    doPaint = true;
                }
            }

            else if (combo.Parent.Name == "dgTrnsInputting") 
            {
                text = LookupDisplayText(GetColumnValueAtRow(currencyManager, rowNum), rowNum);
                doPaint = true;
            }

            else //if (combo.Parent.Name == "DgRetnCharges") ETC
            {
                text = LookupDisplayText(GetColumnValueAtRow(currencyManager, rowNum), rowNum);
                doPaint = true;
            }

            if (doPaint == true)
            {

                //text = Trim(text);
                Rectangle rect = bounds;

                using (StringFormat format = new StringFormat()) {
                    
                    if (alignToRight) {
                        format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                    }

                    if (this.Alignment == HorizontalAlignment.Left) {
                        format.Alignment = StringAlignment.Near ;
                    } else if (this.Alignment == HorizontalAlignment.Center) {
                        format.Alignment = StringAlignment.Center ;
                    } else {
                        format.Alignment = StringAlignment.Far ;
                    }

                    format.FormatFlags |= StringFormatFlags.NoWrap;

                    g.FillRectangle(backBrush, rect);

                    // We want painting to leave a little padding around the rectangle,
                    // so reduce the size of rectangle by the margin
                    rect.Offset(0,yMargin);
                    rect.Height -= yMargin;

                    g.DrawString( text
                                , this.DataGridTableStyle.DataGrid.Font
                                , foreBrush
                                , rect
                                , format);
                }
            }
        }
     

        /// <summary>
        ///     Paint 2: Called to paint the contents of a particular cell. 
        ///     This simply calls Paint 3.
        /// </summary>
        protected override void Paint( Graphics g
                                     , Rectangle bounds
                                     , CurrencyManager currencyManager
                                     , int rowNum) 
        {
            //Debug.WriteLine("Start Paint 2 (g, rect, cm, rowNum) " + rowNum);
            Paint(g, bounds, currencyManager, rowNum, false);
        }

        /// <summary>
        ///     Paint 3: Called to paint the contents of a particular cell. 
        ///     This simply calls Paint 1.
        /// </summary>
        protected override void Paint( Graphics g
            , Rectangle bounds
            , CurrencyManager currencyManager
            , int rowNum
            , bool alignToRight) 
        {
            //Debug.WriteLine("Start Paint 3 (g, rect, cm, rowNum, alignToRight) " + rowNum);

            using (Brush backBrush = new SolidBrush(this.DataGridTableStyle.BackColor)) {
                using (Brush foreBrush = new SolidBrush(this.DataGridTableStyle.ForeColor)) {
                    Paint(g, bounds, currencyManager, rowNum, backBrush, foreBrush, alignToRight);
                }
            }
        }

        /// <summary>
        ///     Called to parent the column to the datagrid
        /// </summary>
        protected override void SetDataGridInColumn(DataGrid value) {
            
            //Debug.WriteLine("Start SetDataGridInColumn");

            base.SetDataGridInColumn(value);
            if (combo.Parent != value) {
                if (combo.Parent != null) {
                    combo.Parent.Controls.Remove(combo);
                    combo.Parent = null;
                }
            }
            if (value != null) {
                value.Controls.Add(combo);
            }
        }

        /// <summary>
        ///     Called when the datasource is updated programatically
        /// </summary>
        protected override void UpdateUI( CurrencyManager source
                                        , int rowNum
                                        , string instantText) 
        {
            //Debug.WriteLine("Start UpdateUI");

            //Set the selected value for the Combobox
            combo.SelectedValue = GetColumnValueAtRow(source,rowNum);
            
            if (instantText != null) {
                combo.Text = instantText;
            }
        }

        // ----------------------------------------------------------
        // Helper Methods 
        //----------------------------------------------------------
        
        /// <summary>
        ///     Event Handler for the ComboBox key down event - when the user presses an input
        ///     key tell the grid to go into edit mode. We only go into edit mode when the user 
        ///     starts to type in to avoid marking the datasource value as dirty when it has 
        ///     not been updated
        /// </summary>
        private void comboKeyDown(object source, KeyEventArgs ev){

            //These keys should not cause an edit - this is not an exhaustive list - I
            //need to think of a better way to do this

            //************SEE DataGridKeyCatch.vb      Partial Class DataGridKeyCatch**********

            if (ev.Alt == true)
            {
                //InEdit = true;
                combo.DroppedDown = true ;
          
                //combo.SelectedIndexChanged -= new EventHandler(comboSelectedIndexChanged);
                //return;
            }
            else
            {
                if (combo.Parent.Name == "dgTrnsInputting")
                {
                    switch (ev.KeyData & Keys.KeyCode)
                    {
                        case Keys.F:
                            {
                                //object value = null;
                                //value = combo.SelectedValue;
                                //InEdit = true;
                                //combo.Text = "Fund";
                                combo.SelectedIndex = 0;
                                F_DIndex = 0;         
                                //Paint!!!
                                //combo.Text = combo.SelectedText;
                                //SetColumnValueAtRow(dataSource, rowNum, value);
                                //EndEdit();

                                break;
                            }
                        case Keys.D:
                            {
                        //        InEdit = true;
                                combo.SelectedIndex = 1;
                                F_DIndex = 1;         
                        //        Paint!!!
                        //        //combo.Text = "Don't Fund";//combo.SelectedText;
                        //        //EndEdit();
                                break;
                            }
                        case Keys.C:
                            {
                                //        InEdit = true;
                                combo.SelectedIndex = 3;
                                F_DIndex = 3;
                                //        Paint!!!
                                //        //combo.Text = "Don't Fund";//combo.SelectedText;
                                //        //EndEdit();
                                break;
                            }

                        case Keys.Tab:
                        case Keys.Left:
                        case Keys.Right:
                        case Keys.Up:
                        case Keys.Down:
                            //{
                            //    if (combo.DroppedDown == true)
                            //    {
                            //        InEdit = true;
                            //        SendKeys.Send("{DOWN}");

                            //        //combo.SelectedIndex += 1;
                            //    }
                            //    break;
                            //}

                        case Keys.Enter:
                        case Keys.Escape:
                        //case Keys.ShiftKey:
                        case Keys.ControlKey:
                        //case Keys.Menu:
                        case Keys.Home:
                        case Keys.End:
                        case Keys.F4:
                            break;

                        default:
                            InEdit = true;
                            break;

                    }
                }
            //ccc
                else if (combo.Parent.Name == "dgPayersCrJnlRep")
                {
                //if (combo.Parent.Name == "dgPayersCrJnlRep")
                //{
                    switch (ev.KeyData & Keys.KeyCode)
                    {
                        case Keys.O:
                            {
                                combo.SelectedIndex = 4;
                                F_DIndex = 4;         
                                break;
                            }
                        case Keys.C:
                            {
                                combo.SelectedIndex = 1;
                                F_DIndex = 1;         
                                break;
                            }
                        case Keys.D:
                            {
                                combo.SelectedIndex = 0;
                                F_DIndex = 0;
                                break;
                            }

                        case Keys.Tab:
                        case Keys.Left:
                        case Keys.Right:
                        case Keys.Up:
                        case Keys.Down:

                        case Keys.Enter:
                        case Keys.Escape:
                        //case Keys.ShiftKey:
                        case Keys.ControlKey:
                        //case Keys.Menu:
                        case Keys.Home:
                        case Keys.End:
                        case Keys.F4:
                            break;

                        default:
                            InEdit = true;
                            break;

                    }
                }

                else
                {

                    switch (ev.KeyData & Keys.KeyCode)
                    {
                        case Keys.Tab:
                        case Keys.Left:
                        case Keys.Right:
                        case Keys.Up:
                        case Keys.Down:
                        case Keys.Enter:
                        case Keys.Escape:
                        //case Keys.ShiftKey:
                        case Keys.ControlKey:
                        //case Keys.Menu:
                        case Keys.Home:
                        case Keys.End:
                        case Keys.F4:
                            break;

                        default:
                            InEdit = true;
                            break;

                    }
                }
            }
               }

        
        /// <summary>
        ///     Event Handler for the ComboBox selected index changed event - if the user changes the selected
        ///     value using the mouse we need to mark the datasource as dirty
        /// </summary>
        private void comboSelectedIndexChanged(object source, EventArgs ev)
        {
            InEdit = true;
            //combo.DroppedDown = true;
        }

        ////private void comboSelectionChangecommitted(object source, EventArgs ev)
        ////{
        ////    InEdit = true;
        ////  //combo.DroppedDown = true;
        ////}

        /// <summary>
        ///     The ComboBox DisplayMember has changed - Cache the DisplayMember PropertyDescriptor so that we don't 
        ///     have to keep looking it up
        /// </summary>
        private void comboDisplayMemberChanged(object source, EventArgs ev) {
            InitializeDisplayMemberPropertyDescriptor();
        }

        
        /// <summary>
        ///     The ComboBox ValueMember has changed - Cache the ValueMember PropertyDescriptor so that we don't 
        ///     have to keep looking it up
        /// </summary>
        private void comboValueMemberChanged(object source, EventArgs ev) {
            InitializeValueMemberPropertyDescriptor();
        }

        /// <summary>
        ///     Called when we stop editing a value 
        /// </summary>
        private void EndEdit() {
            InEdit = false;
            Invalidate();
        }


        /// <summary>
        ///     Hide the ComboBox
        /// </summary>
        private void HideComboBox() {
            if (combo.Focused) {
                this.DataGridTableStyle.DataGrid.Focus();
            }
            combo.Visible = false;
        }


        /// <summary>
        ///     Initialize the DisplayMember property descriptor
        /// </summary>
        private void InitializeDisplayMemberPropertyDescriptor() {

            if (combo.ComboBoxCurrencyManager != null) {
            
                if (DisplayMember == null || DisplayMember == string.Empty) {
                    displayMemberProperty = null;
                } else {
                    PropertyDescriptorCollection props = combo.ComboBoxCurrencyManager.GetItemProperties();
                    BindingMemberInfo displayBindingMember = new BindingMemberInfo(DisplayMember);
                    displayMemberProperty = props.Find(displayBindingMember.BindingField, true);
                } 

                displayMemberPropertyInitialized = true;
            }

        }


        /// <summary>
        ///     Initialize the ValueMember property descriptor
        /// </summary>
        private void InitializeValueMemberPropertyDescriptor() {
            
            if (combo.ComboBoxCurrencyManager != null) {
                
                if (ValueMember == null || ValueMember == string.Empty) {
                    valueMemberProperty = null;
                } else {
                    PropertyDescriptorCollection props = combo.ComboBoxCurrencyManager.GetItemProperties();
                    BindingMemberInfo valueBindingMember = new BindingMemberInfo(ValueMember);
                    valueMemberProperty = props.Find(valueBindingMember.BindingField, true);
                }

                valueMemberPropertyInitialized = true;
            }
        }


        /// <summary>
        ///     Notify the grid that we are going into edit mode
        /// </summary>
        private bool InEdit {
            get {
                return inEdit;
            }
            set {
                //Debug.WriteLine("Start InEdit " + value);
                if (value == inEdit) return ;
                inEdit = value;
                if (inEdit) {
                    try
                    {
                        this.ColumnStartedEditing(combo);
                        F_DIndex = -1;
                    }
                    catch { }
                }

            }
        }

        
        /// <summary>
        ///     Lookup the display text for the given value.
        ///     
        ///     We use the value and ValueMember to look up the row in the 
        ///     ComboBox datasource. we then use DisplayMember to get the 
        ///     text to display
        ///     
        ///     This look up occurs everytime a cell is painted. If the Combobox datasource 
        ///     is not large then this is probably ok as it stands but this could probably 
        ///     benefit from caching 
        ///     
        /// </summary>
        private string LookupDisplayText(object value, int rowNum) {
            
            if (value == null) return null ;
            if (value.Equals(System.DBNull.Value)) return NullText;

            //Make sure we've got the property descriptor for the ValueMember
            //property
            if (!valueMemberPropertyInitialized) {
                InitializeValueMemberPropertyDescriptor();
            }

            if (valueMemberProperty == null) {
                //throw new ArgumentException("ValueMember cannot be null");
				//return ("!!??!!" != null) ? Convert.ToString("!!??!!", CultureInfo.CurrentCulture) : "";
				
				//if (value.Equals(-1)) return "Select";			   
				//if (value.Equals(4)) return "Negatvie Other";			   
                if (ValueMember.ToString().Equals("intChargtype"))
                {
                    if (value.ToString().Equals("1"))
                    {
                        return ("Establisment Fee" != null) ? Convert.ToString("Establisment Fee", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("2"))
                    {
                        return ("Negative Batch" != null) ? Convert.ToString("Negative Batch", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("3"))
                    {
                        return ("Negative Retention" != null) ? Convert.ToString("Negatvie Other", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("4"))
                    {
                        return ("Negatvie Other" != null) ? Convert.ToString("Negatvie Other", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("5"))
                    {
                        return ("Plus Other" != null) ? Convert.ToString("Plus Other", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("6"))
                    {
                        return ("Plus Double" != null) ? Convert.ToString("Plus Double", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("7"))
                    {
                        return ("Factored Credit" != null) ? Convert.ToString("Factored Credit", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("8"))
                    {
                        return ("Advance Repayment" != null) ? Convert.ToString("Advance Repayment", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("9"))
                    {
                        return ("Import Facility" != null) ? Convert.ToString("Import Facility", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("10"))
                    {
                        return ("Cash Cheque" != null) ? Convert.ToString("Cash Cheque", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("11"))
                    {
                        return ("Transaction Fee" != null) ? Convert.ToString("Transaction Fee", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("12"))
                    {
                        return ("Pay Creditor" != null) ? Convert.ToString("Pay Creditor", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("13"))
                    {
                        return ("Payment Direct" != null) ? Convert.ToString("Payment Direct", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("-1"))
                    {
                        return ("Select" != null) ? Convert.ToString("Select", CultureInfo.CurrentCulture) : "";
                    }
                    else //if (value.Equals(0)) 
                    {
                        return ("Select" != null) ? Convert.ToString("Select", CultureInfo.CurrentCulture) : "";
                    }

                }
                else if (ValueMember.ToString().Equals("intInvTerms")) //Inv Terms
                {
                    //
                    if (value.ToString().Equals("1"))
                    {
                        return ("30 days" != null) ? Convert.ToString("30 days", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("2"))
                    {
                        return ("60 days" != null) ? Convert.ToString("60 days", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("3"))
                    {
                        return ("90 days" != null) ? Convert.ToString("90 days", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("4"))
                    {
                        return ("Eom Following" != null) ? Convert.ToString("Eom Following", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("5"))
                    {
                        return ("7 days" != null) ? Convert.ToString("7 days", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("6"))
                    {
                        return ("2 monthly" != null) ? Convert.ToString("2 monthly", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("7"))
                    {
                        return ("3 monthly" != null) ? Convert.ToString("3 monthly", CultureInfo.CurrentCulture) : "";
                    }
                    else //if (value.Equals(0)) 
                    {
                        return ("20th following" != null) ? Convert.ToString("20th following", CultureInfo.CurrentCulture) : "";
                    }

                //Public arrCrTrnTypes() As CrTrnTypes = {New CrTrnTypes("Credit", 2), New CrTrnTypes("Receipt", 3), 
                //New CrTrnTypes("CBT Cr", 4), New CrTrnTypes("Repurchases", 5), New CrTrnTypes("Discount", 6), 
                //New CrTrnTypes("Jnl Cr", 7), New CrTrnTypes("Jnl Dr", -7), New CrTrnTypes("CBT Dr", 9), 
                //New CrTrnTypes("O/Payt", 10)}    
/*
                }
                else if (ValueMember.ToString().Equals("intInvTerms")) //Inv Terms
                {
                    //
                    if (value.ToString().Equals("1"))
                    {
                        return ("30 days" != null) ? Convert.ToString("30 days", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("2"))
                    {
                        return ("60 days" != null) ? Convert.ToString("60 days", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("3"))
                    {
                        return ("90 days" != null) ? Convert.ToString("90 days", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("4"))
                    {
                        return ("Eom Following" != null) ? Convert.ToString("Eom Following", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("5"))
                    {
                        return ("7 days" != null) ? Convert.ToString("7 days", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("6"))
                    {
                        return ("2 monthly" != null) ? Convert.ToString("2 monthly", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("7"))
                    {
                        return ("3 monthly" != null) ? Convert.ToString("3 monthly", CultureInfo.CurrentCulture) : "";

                    }
                    else //if (value.Equals(0)) 
                    {
                        return ("20th following" != null) ? Convert.ToString("20th following", CultureInfo.CurrentCulture) : "";
                    }
*/  
              }
                else if (ValueMember.ToString().Equals("intPaymentType")) //PaymentType
                {
                    //
                    
                    if (value.ToString().Equals("1"))
                    {
                        return ("Direct cr." != null) ? Convert.ToString("Direct cr.", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("2"))
                    {
                        return ("Cheque" != null) ? Convert.ToString("Cheque", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("3"))
                    {
                        return ("AP" != null) ? Convert.ToString("AP", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("4"))
                    {
                        return ("Cash" != null) ? Convert.ToString("Cash", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("5"))
                    {
                        return ("Other" != null) ? Convert.ToString("Other", CultureInfo.CurrentCulture) : "";
                    }
                    else //if (value.Equals(0)) 
                    {
                        return ("Not set" != null) ? Convert.ToString("Not set", CultureInfo.CurrentCulture) : "";
                    }
                    //
                }
                else if (ValueMember.ToString().Equals("intCrTrnTypes")) //CrTrnTypes
                {
                    //
                    if (value.ToString().Equals("1"))
                    {
                        return ("XXX" != null) ? Convert.ToString("XXX", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("2"))
                    {
                        return ("Credit" != null) ? Convert.ToString("Credit", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("3"))
                    {
                        return ("Receipt" != null) ? Convert.ToString("Receipt", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("4"))
                    {
                        return ("CBT Cr" != null) ? Convert.ToString("CBT Cr", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("5"))
                    {
                        return ("Repurchases" != null) ? Convert.ToString("Repurchases", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("6"))
                    {
                        return ("Discount" != null) ? Convert.ToString("Discount", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("7"))
                    {
                        return ("AR Jnl Cr" != null) ? Convert.ToString("AR Jnl Cr", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("8"))
                    {
                        return ("NAR Jnl Cr" != null) ? Convert.ToString("NAR Jnl Cr", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("-7"))
                    {
                        return ("AR Jnl Dr" != null) ? Convert.ToString("AR Jnl Dr", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("-8"))
                    {
                        return ("NAR Jnl Dr" != null) ? Convert.ToString("NAR Jnl Dr", CultureInfo.CurrentCulture) : "";
                    }

                    else if (value.ToString().Equals("9"))
                    {
                        return ("CBT Dr" != null) ? Convert.ToString("CBT Dr", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("10"))
                    {
                        return ("O/Payt" != null) ? Convert.ToString("O/Payt", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("13"))
                    {
                        return ("Apply Unalloc" != null) ? Convert.ToString("Apply Unalloc", CultureInfo.CurrentCulture) : "";
                    }

                    else //if (value.Equals(0)) 
                    {
                        return ("YY" != null) ? Convert.ToString("YY", CultureInfo.CurrentCulture) : "";
                    }
                    //
                }
                else if (ValueMember.ToString().Equals("intCBTCrBalTrnsList")) 
                {
                    //
                    if (value.ToString().Equals("1"))
                    {
                        return ("xxx" != null) ? Convert.ToString("xxx", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("2"))
                    {
                        return ("eee" != null) ? Convert.ToString("eee", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("3"))
                    {
                        return ("yyy" != null) ? Convert.ToString("yyy", CultureInfo.CurrentCulture) : "";
                    }
                    else //if (value.Equals(0)) 
                    {
                        return ("" != null) ? Convert.ToString("", CultureInfo.CurrentCulture) : "";
                    }
                }
                else //Status
                {
                    if (value.ToString().Equals("1"))
                    {
                        return ("NF" != null) ? Convert.ToString("NF", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("2"))
                    {
                        return ("Claimed" != null) ? Convert.ToString("Claimed", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("3"))
                    {
                        return ("Unclaimed" != null) ? Convert.ToString("Unclaimed", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("4"))
                    {
                        return ("Marked 4" != null) ? Convert.ToString("Marked 4", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("5"))
                    {
                        return ("Confirm" != null) ? Convert.ToString("Confirm", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("6"))
                    {
                        return ("Check" != null) ? Convert.ToString("Check", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("7"))
                    {
                        return ("Confirm/Check" != null) ? Convert.ToString("Confirm/Check", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("8"))
                    {
                        return ("Add8" != null) ? Convert.ToString("Add8", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("9"))
                    {
                        return ("Add9" != null) ? Convert.ToString("Add9", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("10"))
                    {
                        return ("Fund" != null) ? Convert.ToString("Fund", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("11"))
                    {
                        return ("Don't Fund" != null) ? Convert.ToString("Don't Fund", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("12"))
                    {
                        return ("Add12" != null) ? Convert.ToString("Add12", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("13"))
                    {
                        return ("Add13" != null) ? Convert.ToString("Add13", CultureInfo.CurrentCulture) : "";
                    }
                    else if (value.ToString().Equals("-1"))
                    {
                        return ("Processing" != null) ? Convert.ToString("Processing", CultureInfo.CurrentCulture) : "";
                    }
                    else //if (value.Equals(0)) 
                    {
                        return ("Funded" != null) ? Convert.ToString("Funded", CultureInfo.CurrentCulture) : "";
                    }
                }
            }

            //Now look up the row in the Combobox datasource - this can be horribly inefficient
            //and it uses reflection which makes it expensive - ripe for optimization
            object valueRow = this.RowObjectFromComboBoxDataSource(valueMemberProperty, value,rowNum);
            if (valueRow == null) return "";

            //Now we've got the row for the value we can get the DisplayText using the DisplayMember

            //Make sure we've got the property descriptor for the DisplayMember property
            if (!displayMemberPropertyInitialized) {
                InitializeDisplayMemberPropertyDescriptor();
            }

            // DisplayMember may be null in which case we will use ToString on the object
            object displayItem = null;
            if (displayMemberProperty != null) {
                displayItem = displayMemberProperty.GetValue(valueRow);
            } else {
                displayItem = valueRow;
            }

            //TODO: Add Formatting of display value here
            return (displayItem != null) ? Convert.ToString(displayItem, CultureInfo.CurrentCulture) : "";
        }


        /// <summary>
        ///     Returns true if the grid, the tablestyle or the column
        ///     or the datasource are readonly
        /// </summary>
        private bool RealReadOnly {
            get {
                bool readOnly = false;
                DataGrid dg = this.DataGridTableStyle.DataGrid;
                CurrencyManager cm = (CurrencyManager) dg.BindingContext[dg.DataSource, dg.DataMember];
                if (cm.List == null) { 
                    readOnly = false;
                } else if (cm.List is IBindingList) {
                    readOnly = !((IBindingList)cm.List).AllowEdit;
                } else 
                    readOnly = cm.List.IsReadOnly;

                return readOnly 
                    || this.ReadOnly 
                    || this.DataGridTableStyle.ReadOnly 
                    || dg.ReadOnly;
            }
        }

        /// <summary>
        ///     Rollback any changes to the cell
        /// </summary>
        private void RollBack() {
            combo.SelectedIndex = oldIndex;
        }

        /// <summary>
        ///     Find the row in the ComboBox currency manager for the current cell
        ///     This can be horribly inefficient and it uses reflection which makes it expensive 
        ///     - ripe for optimization
        /// </summary>
        private object RowObjectFromComboBoxDataSource(PropertyDescriptor property, Object key, int rowNum) {

            if (key == null) throw new ArgumentNullException("Bad Key");

            CurrencyManager cm = combo.ComboBoxCurrencyManager;
            if (cm == null) throw new ArgumentNullException("Bad ComboBox CurrencyManager");

            //If the data source is a bindinglist use that as its probably more efficient
            if (property != null && (cm.List is IBindingList) && ((IBindingList)cm.List).SupportsSearching) {
                int index = ((IBindingList)cm.List).Find(property, key);
                return cm.List[index];
            }
           
            //Otherwise walk across the rows looking for the row we want 
            for (int i = 0; i < cm.List.Count; i++) {
                    object row = cm.List[i];
                    object value = property.GetValue(row);
                    if (key.Equals(value)) {
                        return row;
                    }
            }
                
            for (int i = 0; i < cm.List.Count; i++)
            {
               object row = cm.List[i];
               object value = property.GetValue(row);
               if (key.Equals(value))
                {
                    return row;
                 }
             }
            
            return null;
        }

    }
}
