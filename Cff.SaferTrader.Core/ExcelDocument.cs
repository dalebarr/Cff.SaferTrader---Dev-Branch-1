using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using System.IO;
using NPOI.SS.Util;

namespace Cff.SaferTrader.Core
{
    [CLSCompliant(false)]
    public class ExcelDocument : IDisposable
    {
        public HSSFFont fontNormal, fontTitle, fontHeader, fontFooter;
        public HSSFDataFormat df;

        private HSSFCellStyle cellStyleTitle, cellStyle, cellHeaderStyle, cellFooterStyle;
        private HSSFSheet sheet;
        private readonly HSSFWorkbook workbook;
        private int columnIndex;
        private int rowIndex;
        private int offset;
        private readonly bool removeBr;
        private int columnMax;

        public ExcelDocument(): this(false)
        {
        }

        public ExcelDocument(bool parRemoveBr)
        {
            offset = 0;
            workbook = new HSSFWorkbook();
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Cashflow Funding Limited";
            workbook.DocumentSummaryInformation = dsi;
            sheet = (HSSFSheet)workbook.CreateSheet("CFF Export Data");
            
            cellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            cellStyle.WrapText = true;
            cellStyleTitle = (HSSFCellStyle)workbook.CreateCellStyle();
            cellHeaderStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            cellFooterStyle = (HSSFCellStyle)workbook.CreateCellStyle();

            this.removeBr = parRemoveBr;

            if (this.removeBr == true)
            {
                cellStyle.BorderLeft = CellBorderType.NONE;
                cellStyle.BorderRight = CellBorderType.NONE;
                cellStyle.BorderBottom = CellBorderType.NONE;
                cellStyle.BorderTop = CellBorderType.NONE;
            }

            fontNormal = (HSSFFont)workbook.CreateFont();
            fontTitle = (HSSFFont)workbook.CreateFont();
            fontHeader = (HSSFFont)workbook.CreateFont();
            fontFooter = (HSSFFont)workbook.CreateFont();

            df = (HSSFDataFormat)workbook.CreateDataFormat();
        }

        private HSSFCellStyle GetCurrencyStyleFormat()
        {
            cellStyle = GetDefaultFontStyle();
            cellStyle.DataFormat = df.GetFormat("$#,##0.00");
            return cellStyle;
        }

        public void AddCurrencyCell(decimal amount, NPOI.SS.UserModel.HorizontalAlignment xAlign = NPOI.SS.UserModel.HorizontalAlignment.GENERAL)
        {
            HSSFCell cell = (HSSFCell)_HSSFCurrentRow.CreateCell(columnIndex);
            cell.SetCellValue(Convert.ToDouble(amount));
            cell.CellStyle = GetCurrencyStyleFormat();
            cell.CellStyle.BorderLeft = CellBorderType.NONE;
            cell.CellStyle.BorderRight = CellBorderType.NONE;
            ManualAdjustColumnWidth(Convert.ToString(amount));
            if (xAlign != HorizontalAlignment.GENERAL)
                cell.CellStyle.Alignment = xAlign;
            else
                cell.CellStyle.Alignment = HorizontalAlignment.RIGHT;
            columnIndex++;
        }

        public void AddCurrencyCell(decimal amount, int index, NPOI.SS.UserModel.HorizontalAlignment xAlign = NPOI.SS.UserModel.HorizontalAlignment.GENERAL)
        {
            columnIndex = index + offset;
            HSSFCell cell = (HSSFCell)_HSSFCurrentRow.CreateCell(columnIndex);
            cell.SetCellValue(Convert.ToDouble(amount));
            cell.CellStyle = GetCurrencyStyleFormat();
            cell.CellStyle.BorderLeft = CellBorderType.NONE;
            cell.CellStyle.BorderRight = CellBorderType.NONE;
            ManualAdjustColumnWidth(Convert.ToString(amount));
            if (xAlign != HorizontalAlignment.GENERAL)
                cell.CellStyle.Alignment = xAlign;
            else
                cell.CellStyle.Alignment = HorizontalAlignment.RIGHT;
            columnIndex++;
        }

        public void AddCurrencyCellLedger(decimal amount, int index, bool isDoubleLine)
        {
            columnIndex = index + offset;
            HSSFCellStyle style = GetCurrencyStyleFormat();
            if (isDoubleLine)
            {
                style.BorderLeft = CellBorderType.DOUBLE;
                style.BorderRight = CellBorderType.DOUBLE;
            }
            HSSFCell cell = (HSSFCell)_HSSFCurrentRow.CreateCell(columnIndex);
            cell.SetCellValue(Convert.ToDouble(amount));
            cell.CellStyle = style;
            ManualAdjustColumnWidth(Convert.ToString(amount));
            columnIndex++;
        }

        public void AddCurrencyHeaderCellLedger(decimal amount, int index, bool isDoubleLine, HorizontalAlignment xAlign = HorizontalAlignment.RIGHT)
        {
            columnIndex = index + offset;
            HSSFCellStyle style = HeaderCellStyle(xAlign); //GetCurrencyStyleFormat();
            style.DataFormat = df.GetFormat("$#,##0.00");
            if (isDoubleLine)
            {
                style.BorderBottom = CellBorderType.DOUBLE;
                style.BottomBorderColor = HSSFColor.WHITE.index;
            }

            HSSFCell cell = (HSSFCell)_HSSFCurrentRow.CreateCell(columnIndex);
            cell.SetCellValue(Convert.ToDouble(amount));
            cell.CellStyle = style;
            ManualAdjustColumnWidth(Convert.ToString(amount));
            columnIndex++;
        }

        public void AddCell(string text, int index=-1, HorizontalAlignment xAlign = HorizontalAlignment.GENERAL) 
        {
            if (index>=0)
                columnIndex = index + offset;
            
            HSSFCell cell = (HSSFCell)_HSSFCurrentRow.CreateCell(columnIndex, CellType.STRING);
            cell.CellStyle = GetDefaultFontStyle();
            cell.CellStyle.BorderLeft = CellBorderType.NONE;
            cell.CellStyle.BorderRight = CellBorderType.NONE;

            cell.SetCellValue(text);
            if (text.Length > 255)
            {
                cell.CellStyle.WrapText = true;
            }

            if (text.Length > 255)
            {  //todo: edit this part so we don't hardcode
                _HSFFGetSheet.SetColumnWidth(columnIndex, 100 * 256);
                cell.CellStyle.WrapText = true;
                cell.CellStyle.Alignment = HorizontalAlignment.JUSTIFY;
            }
            else
            {
                ManualAdjustColumnWidth(text);
                cell.CellStyle.Alignment = xAlign;
            }

           
            columnIndex++;
        }

        public void AddHeaderCell(string caption, NPOI.SS.UserModel.HorizontalAlignment xAlign = NPOI.SS.UserModel.HorizontalAlignment.CENTER)
        {
            HSSFCell cell = (HSSFCell)_HSSFCurrentRow.CreateCell(columnIndex, CellType.STRING);
            cell.CellStyle = GetDefaultHeaderStyle();
            cell.CellStyle.BorderLeft = CellBorderType.NONE;
            cell.CellStyle.BorderRight = CellBorderType.NONE;
            cell.CellStyle.BorderBottom = CellBorderType.MEDIUM;
            cell.CellStyle.BottomBorderColor = HSSFColor.GREY_50_PERCENT.index;
            cell.SetCellValue(caption);
            ManualAdjustColumnWidth(caption);
            cell.CellStyle.Alignment = xAlign;
            columnIndex++;
        }

        public void InsertEmptyRow()
        {
            MoveToNextRow();
            MoveToNextRow();
        }

        public void FormatAsHeaderRow(int columnCount, NPOI.SS.UserModel.HorizontalAlignment xAlign = NPOI.SS.UserModel.HorizontalAlignment.CENTER)
        {
            for (int index = 0; index < columnCount; index++)
            {
                _HSSFCurrentRow.CreateCell(index + offset).CellStyle = HeaderCellStyle(xAlign);
            }
        }

        public void FormatAsSubheaderRow(int columnCount, NPOI.SS.UserModel.HorizontalAlignment xAlign = NPOI.SS.UserModel.HorizontalAlignment.CENTER)
        {
            for (int index = 0; index < columnCount; index++)
            {
                _HSSFCurrentRow.CreateCell(index + offset).CellStyle = HeaderCellStyle(xAlign);
            }
        }

        public void StartFooterRow(int columnCount)
        {
            MoveToNextRow();
            for (int index = 0; index < columnCount; index++)
            {
                _HSSFCurrentRow.CreateCell((index + offset), CellType.STRING).CellStyle = FooterCellStyle();
            }
        }

        public void AddFooterCellToCurrentRow(string text, int index, NPOI.SS.UserModel.HorizontalAlignment xAlign = NPOI.SS.UserModel.HorizontalAlignment.LEFT)
        {
            //HSSFCellStyle style = FooterCellStyle(xAlign);
            int idx = (index < 0) ? columnIndex:index;
            ManualAdjustColumnWidth(text, idx);

            //HSSFCell cellFtr = (HSSFCell)_HSSFCurrentRow.GetCell(idx);
            HSSFFont fFooter = (HSSFFont)workbook.CreateFont();
            fFooter.FontName = "Calibri";
            fFooter.Boldweight = (short)FontBoldWeight.BOLD;
            fFooter.Color = HSSFColor.BLACK.index;

            HSSFCellStyle cFooterStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            cFooterStyle.SetFont(fFooter);
            cFooterStyle.BorderTop = CellBorderType.THIN;
            cFooterStyle.TopBorderColor = HSSFColor.GREY_50_PERCENT.index;
           
            HSSFCell cellFtr = (HSSFCell)_HSSFCurrentRow.CreateCell(idx, CellType.STRING);
            cellFtr.CellStyle = cFooterStyle;
            cellFtr.CellStyle.BorderLeft = CellBorderType.NONE;
            cellFtr.CellStyle.BorderRight = CellBorderType.NONE;
            cellFtr.SetCellValue(text);
            ManualAdjustColumnWidth(text, idx);
            cellFtr.CellStyle.Alignment = xAlign;
            columnIndex++;
        }

        public void AddNumericCellToCurrentRow(double value, NPOI.SS.UserModel.HorizontalAlignment xAlign = NPOI.SS.UserModel.HorizontalAlignment.RIGHT)
        {
            try {
                _HSSFCurrentRow.GetCell(columnIndex).SetCellValue(value);
                ManualAdjustColumnWidth(Convert.ToString(value));
                _HSSFCurrentRow.GetCell(columnIndex).CellStyle.Alignment = xAlign;
            } 
            catch { }
            columnIndex++;
        }

        public void AddCurrencyFooterCellToCurrentRow(decimal amount, int index,  NPOI.SS.UserModel.HorizontalAlignment xAlign = NPOI.SS.UserModel.HorizontalAlignment.RIGHT)
        {
            HSSFCell cell = (HSSFCell)_HSSFCurrentRow.CreateCell(index, CellType.STRING);
            ManualAdjustColumnWidth(Convert.ToString(amount), index);
            HSSFCellStyle style = FooterCellStyle(xAlign, df.GetFormat("$#,##0.00"));
            cell.SetCellValue(Convert.ToDouble(amount));
            cell.CellStyle = style;
            columnIndex++;
        }

        private void AddCopyrightNotice()
        {
            MoveToNextRow();
            MoveToNextRow();
            _HSSFCurrentRow.CreateCell(0).SetCellValue(string.Format("© 1998-{0} Cashflow Funding Limited", DateTime.Today.Year));
        }

        public void MoveToNextRow()
        {
            rowIndex++;
            sheet.CreateRow(rowIndex);
            columnMax = columnMax < columnIndex ? columnIndex : columnMax;
            columnIndex = offset;
        }

        public void MoveToNextCell()
        {
            columnIndex++;
        }

        public HSSFSheet _HSFFGetSheet
        {
            get { return this.sheet; }
        }

        public HSSFRow _HSSFCurrentRow
        {
            get { return (HSSFRow)(this.sheet.GetRow(rowIndex)); }
        }


        public void Indent()
        {
            offset++;
            columnIndex += offset;
        }

        public void Deindent()
        {
            offset--;
            columnIndex += offset;
        }

        public void WriteTitle(string title)
        {
            fontTitle.FontName = "Calibri";
            fontTitle.Color = HSSFColor.COLOR_NORMAL;
            fontTitle.FontHeightInPoints = 12;
            cellStyleTitle.SetFont(fontTitle);

            sheet.CreateRow(rowIndex).CreateCell(0).SetCellValue(title);
            sheet.GetRow(rowIndex).GetCell(0).CellStyle = cellStyleTitle;
            sheet.GetRow(rowIndex).HeightInPoints = 18;
            InsertEmptyRow();
        }

        private HSSFCellStyle GetDefaultFontStyle()
        { 
            fontNormal.FontName = "Calibri";
            fontNormal.Color = HSSFColor.COLOR_NORMAL;
            fontNormal.FontHeightInPoints = 10;
            cellStyle.SetFont(fontNormal);
            return cellStyle;
        }

        private HSSFCellStyle GetDefaultHeaderStyle()
        {
            fontHeader.FontName = "Calibri";
            fontHeader.Color = HSSFColor.BLACK.index;
            fontHeader.FontHeightInPoints = 10;
            fontHeader.Boldweight = 800;
            HSSFCellStyle xCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            xCellStyle.SetFont(fontHeader);
            return xCellStyle;
        }
        
        private HSSFCellStyle HeaderCellStyle(NPOI.SS.UserModel.HorizontalAlignment xHAlignment)
        {   //start as per marty's suggestions
            //cellHeaderStyle.FillForegroundColor = HSSFColor.GREY_25_PERCENT.index; 
            //cellHeaderStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            fontHeader.Color = HSSFColor.BLACK.index;
            //end as per marty's suggestions

            fontHeader.FontName = "Calibri";
            fontHeader.Boldweight = (short)FontBoldWeight.BOLD;
            cellHeaderStyle.SetFont(fontHeader);
            cellHeaderStyle.Alignment = xHAlignment;
            cellHeaderStyle.BorderBottom = CellBorderType.THIN;
            cellHeaderStyle.BottomBorderColor = HSSFColor.BLACK.index;
            return cellHeaderStyle;
        }

        private HSSFCellStyle FooterCellStyle(NPOI.SS.UserModel.HorizontalAlignment xAlign = HorizontalAlignment.GENERAL, short dataFormat=-1)
        {
            //cellFooterStyle.FillForegroundColor = HSSFColor.GREY_25_PERCENT.index;
            //cellFooterStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            fontFooter.FontName = "Calibri";
            fontFooter.Boldweight = (short)FontBoldWeight.BOLD;
            fontFooter.Color = HSSFColor.BLACK.index;
            cellFooterStyle.SetFont(fontFooter);
            if (dataFormat > 0)
                cellFooterStyle.DataFormat = dataFormat;
            cellFooterStyle.BorderTop = CellBorderType.THIN;
            cellFooterStyle.TopBorderColor = HSSFColor.GREY_50_PERCENT.index;
            cellFooterStyle.Alignment = xAlign;
            return cellFooterStyle;
        }

        public int RowIndex
        {
            get { return rowIndex; }
        }

        public int ColumnIndex
        {
            get { return columnIndex; }
        }

        public bool RemoveBr
        {
            get { return removeBr; }
        }

        public MemoryStream WriteToStream()
        {
            AddCopyrightNotice();
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            return stream;
        }

        private void ManualAdjustColumnWidth(string rowText)
        {
            int curWidth = _HSFFGetSheet.GetColumnWidth(ColumnIndex);
            int calWidth = (rowText.Length + 2) * 256; // add extra 1 char
            int maxWidth = curWidth > calWidth ? curWidth : calWidth;
            try
            {
                _HSFFGetSheet.SetColumnWidth(ColumnIndex, maxWidth);
            }
            catch (Exception)
            { //for memo field, note/comments exceeds 255
                ICellStyle cStyle = _HSFFGetSheet.GetColumnStyle(ColumnIndex);
                if (cStyle != null) {
                    cStyle.WrapText = true;
                    cStyle.Alignment = HorizontalAlignment.JUSTIFY;
                }
            }
        }

        private void ManualAdjustColumnWidth(string rowText, int index)
        {
            int curWidth = _HSFFGetSheet.GetColumnWidth(index);
            int calWidth = (rowText.Length + 2) * 256; // add extra 1 char
            int maxWidth = curWidth > calWidth ? curWidth : calWidth;
            _HSFFGetSheet.SetColumnWidth(index, maxWidth);
        }

        private void AutoAdjustColumnWidth(int columnMax)
        {
            for (int n = 0; n < columnMax; n++)
            {
                sheet.AutoSizeColumn(n, true);
            }
        }

        public  void Dispose() { }

    }
}
