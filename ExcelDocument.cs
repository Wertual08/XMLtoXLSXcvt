using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace XMLtoXLSXcvt
{
    class ExcelDocument : IDisposable
    {
        private Excel.Application ExcelApplication;
        private Excel.Workbook ExcelWorkbook;
        private Excel.Worksheet ExcelWorksheet;
        private int CurrentRowCount = 0;

        public float ColumnRow { get; set; } = 6.0f;
        public float ImageColumnWidth { get; set; } = 5.8f;

        public ExcelDocument()
        {
            ExcelApplication = new Excel.Application();
            if (ExcelApplication == null) throw new Exception("Excel API error: Excel is not installed.");
            ExcelApplication.DisplayAlerts = false;
            ExcelWorkbook = ExcelApplication.Workbooks.Add();
            ExcelWorksheet = ExcelWorkbook.Worksheets[1];
        }
        public ExcelDocument(string path)
        {
            ExcelApplication = new Excel.Application();
            if (ExcelApplication == null) throw new Exception("Excel API error: Excel is not installed.");
            ExcelWorkbook = ExcelApplication.Workbooks.Open(path);
            if (ExcelWorkbook.ReadOnly) throw new FileLoadException("Document [" + path + "] is read only.");
            ExcelWorksheet = ExcelWorkbook.Worksheets[1]; 
        }
        ~ExcelDocument()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
        }
        protected bool IsDisposed { get; private set; } = false;
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            ExcelWorkbook?.Close(true);
            ExcelApplication?.Quit();
            if (ExcelWorksheet != null) Marshal.ReleaseComObject(ExcelWorksheet);
            if (ExcelWorkbook != null) Marshal.ReleaseComObject(ExcelWorkbook);
            if (ExcelApplication != null) Marshal.ReleaseComObject(ExcelApplication);
            ExcelWorksheet = null;
            ExcelWorkbook = null;
            ExcelApplication = null;
            IsDisposed = true;
        }

        public void AddRow()
        {
            CurrentRowCount++;
        }
        public void AddRow(string[] values)
        {
            for (int i = 0; i < values.Length; i++)
                this[i, CurrentRowCount] = values[i];
            CurrentRowCount++;
        }
        public void AddRow(List<string> values)
        {
            for (int i = 0; i < values.Count; i++)
                this[i, CurrentRowCount] = values[i];
            CurrentRowCount++;
        }
        public int RowCount
        {
            get => ExcelWorksheet.Cells.Find("*", System.Reflection.Missing.Value,
                               System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                               Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious,
                               false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;
        }
        public int ColumnCount
        {
            get => ExcelWorksheet.Cells.Find("*", System.Reflection.Missing.Value,
                               System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                               Excel.XlSearchOrder.xlByColumns, Excel.XlSearchDirection.xlPrevious,
                               false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;
        }
        public string this[int x, int y]
        {
            get => ExcelWorksheet.Cells[y + 1, x + 1].Value?.ToString() ?? "";
            set
            {
                Excel.Range range = ExcelWorksheet.Cells[y + 1, x + 1];
                range.NumberFormat = "@";
                ExcelWorksheet.Cells[y + 1, x + 1] = value;
            }
        }
        public void AddImage(int x, int y, string path)
        {
            Excel.Range oRange = ExcelWorksheet.Cells[y + 1, x + 1];
            float Left = (float)oRange.Left;
            float Top = (float)oRange.Top;
            var shape = ExcelWorksheet.Shapes.AddPicture(
                path,
                Microsoft.Office.Core.MsoTriState.msoFalse,
                Microsoft.Office.Core.MsoTriState.msoCTrue,
                Left, Top, -1, -1);
            oRange.ColumnWidth = Math.Max(shape.Width / ImageColumnWidth, (float)oRange.ColumnWidth);
            oRange.RowHeight = Math.Max(shape.Height * ColumnRow / ImageColumnWidth, (float)oRange.RowHeight);
        }

        public bool Save(string path)
        {
            try
            {
                ExcelWorkbook.SaveAs(path, AccessMode: Excel.XlSaveAsAccessMode.xlExclusive);
                return true;
            }
            catch { return false; }
        }
        public bool Save()
        {
            try
            {
                ExcelWorkbook.Save();
                return true;
            }
            catch { return false; }
        }
    }
}
