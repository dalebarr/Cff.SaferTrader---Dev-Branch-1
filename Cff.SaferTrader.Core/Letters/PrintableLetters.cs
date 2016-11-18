using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cff.SaferTrader.Core.Letters
{
    [Serializable]
    public class PrintableLetters:IPrintable
    {
        private readonly string theFileName;
        private readonly string rptHeader;
        private readonly Boolean printThis;
        private Boolean faxEnable;
        private readonly string viewID;

        public PrintableLetters(string letterName, string reportHeader, Boolean printIt, string viewIDValue)
        {
            this.theFileName = letterName;
            this.rptHeader = reportHeader;
            this.printThis = printIt;
            this.faxEnable = false;
            this.viewID = viewIDValue;
        }

        public string PopupPageName
        {
            get { return "LettersPopup.aspx?ViewID=" + this.viewID; }
        }

        public string LetterName
        {
            get { return this.theFileName; }
        }

        public string ReportHeader
        {
            get { return this.rptHeader; }        
        }

        public Boolean PrintThis
        {
            get { return this.printThis; }
        }

        public Boolean FaxEnable
        {
            get { return this.faxEnable; }
            set { this.faxEnable=value; }
        }

    }
}
