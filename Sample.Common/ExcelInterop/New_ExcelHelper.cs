using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Sample.Common.ExcelInterop
{
    class New_ExcelHelper : ExcelHelper
    {
        /*
        This class support for working with Excel 2007 and newer (.XLSX)
        */

        public ExcelWorksheet workSheet;
        public ExcelPackage package;
        //private Application excel = new Application();

        public New_ExcelHelper(string filePath) { package = new ExcelPackage(new FileInfo(filePath)); }

        public void Open(string sheetName)
        {
            try
            {
                if (sheetName.Trim() == "")
                    workSheet = package.Workbook.Worksheets[1];
                else
                    workSheet = package.Workbook.Worksheets[sheetName];
            }
            catch (Exception)
            {
                //excel.Application.Quit();
                //excel.Quit();
            }
        }

        public string GetAllValue()
        {
            string strCellValue = "";

            ExcelAddressBase oRange = workSheet.Dimension;
            int rowCount = oRange.End.Row;
            int colCount = oRange.End.Column;

            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= colCount; j++)
                {
                    try
                    {
                        strCellValue += workSheet.Cells[i, j].Value.ToString() + ",";
                    }
                    catch (NullReferenceException)
                    {
                        strCellValue += "" + ",";
                    }

                }
            }
            strCellValue = strCellValue.Remove(strCellValue.Length - 1);
            return strCellValue;
        }

        public string GetAllExcelRowsValue(int rowIndex)
        {
            string strCellValue = "";
            try
            {
                ExcelAddressBase oRange = workSheet.Dimension;
                int rowCount = oRange.End.Row;
                int colCount = oRange.End.Column;
                if (rowIndex > rowCount)
                    return strCellValue;
                else
                {
                    for (int colIndex = 1; colIndex <= colCount; colIndex++)
                    {
                        try
                        {
                            strCellValue += workSheet.Cells[rowIndex, colIndex].Value.ToString() + ",";
                        }
                        catch (NullReferenceException)
                        {
                            strCellValue += "" + ",";
                        }

                    }
                }

                strCellValue = strCellValue.Remove(strCellValue.Length - 1);
                return strCellValue;
            }

            catch (Exception)
            {
                //excel.Application.Quit();
                //excel.Quit();
                return strCellValue;
            }
        }

        public void WriteDataToExcelFile(string filePath, string sheetName, int rowIndex, int colIndex, string cellValue)
        {
            //var workBooks = excel.Workbooks;
            //Workbook workBook = workBooks.Open(filePath, ReadOnly: false, Editable: true);
            //Worksheet workSheet;
            //if (sheetName.Trim() == "")
            //    workSheet = workBook.Worksheets[1];
            //else
            //    workSheet = workBook.Sheets[sheetName];
            //try
            //{
            //    if (workSheet == null)
            //        return;

            //    Range cell = workSheet.Rows.Cells[rowIndex, colIndex];
            //    cell.Value = cellValue;

            //    excel.Application.ActiveWorkbook.Save();
            //}
            //catch (Exception)
            //{
            //    excel.Application.Quit();
            //    excel.Quit();
            //}
            //finally
            //{
            //    if (workSheet != null) Marshal.ReleaseComObject(workSheet);
            //    if (workBook != null) Marshal.ReleaseComObject(workBook);
            //    if (workBooks != null) Marshal.ReleaseComObject(workBooks);
            //}

        }

        public void OpenExcelfileToView(string filePath, string sheetName, int timeout)
        {
            //var workBooks = excel.Workbooks;
            //Workbook workBook = workBooks.Open(filePath);
            //Worksheet workSheet;
            //if (sheetName.Trim() == "")
            //    workSheet = workBook.Worksheets[1];
            //else
            //    workSheet = workBook.Sheets[sheetName];

            //try
            //{
            //    workSheet.Activate();
            //    excel.Visible = true;
            //    Thread.Sleep(timeout * 1000);
            //    excel.Visible = false;
            //    //excel.Application.Quit();
            //    //excel.Quit();
            //}
            //catch (Exception)
            //{
            //    excel.Application.Quit();
            //    excel.Quit();
            //}
            //finally
            //{
            //    if (workSheet != null) Marshal.ReleaseComObject(workSheet);
            //    if (workBook != null) Marshal.ReleaseComObject(workBook);
            //    if (workBooks != null) Marshal.ReleaseComObject(workBooks);
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //}
        }

        public int GetExcelTotalRows()
        {
            try
            {
                ExcelAddressBase oRange = workSheet.Dimension;
                return oRange.End.Row;
            }
            catch (Exception)
            {
                //excel.Application.Quit();
                //excel.Quit();
                return 0;
            }
        }

        public int GetExcelTotalColumns()
        {
            try
            {
                ExcelAddressBase oRange = workSheet.Dimension;
                return oRange.End.Column;
            }
            catch (Exception)
            {
                //excel.Application.Quit();
                //excel.Quit();
                return 0;
            }
        }

        public string GetCellValue(int intRow, int intColumn)
        {
            try
            {
                return workSheet.Cells[intRow, intColumn].Value.ToString();
            }
            catch (Exception)
            {
                //excel.Application.Quit();
                //excel.Quit();
                return null;
            }
        }

        public bool IsCellAutoFiltered(int intColumn)
        {
            try
            {
                return workSheet.Cells[1, intColumn].AutoFilter;
            }
            catch (Exception)
            {
                //excel.Application.Quit();
                //excel.Quit();
                return false;
            }
        }

        public int[] Search(string strKeyword, bool blnCaseSensitive = true)
        {
            try
            {
                if (!blnCaseSensitive) strKeyword = strKeyword.ToLower();

                ExcelAddressBase oRange = workSheet.Dimension;
                int rowCount = oRange.End.Row;
                int colCount = oRange.End.Column;

                for (int i = 1; i <= rowCount; i++)
                    for (int j = 1; j <= colCount; j++)
                    {
                        if (workSheet.Cells[i, j].Value == null) continue;

                        string strCell = workSheet.Cells[i, j].Value.ToString();
                        if ((blnCaseSensitive && strCell.Equals(strKeyword)) ||
                                (!blnCaseSensitive && strCell.ToLower().Equals(strKeyword)))
                            return new int[2] { i, j };
                    }
                return new int[2] { -1, -1 };
            }
            catch (Exception)
            {

                //excel.Application.Quit();
                //excel.Quit();
                return new int[2] { -1, -1 };
            }

        }

        public void Close()
        {
            //excel.Application.Quit();
            //excel.Quit();
            //Process[] excelProcs = Process.GetProcessesByName("EXCEL");
            //foreach (Process proc in excelProcs)
            //{
            //    proc.Kill();
            //}
            //if (excel != null) Marshal.ReleaseComObject(excel);
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }
    }
}
