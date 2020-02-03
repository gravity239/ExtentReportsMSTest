using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ExcelDataReader;
using Microsoft.Office.Interop.Excel;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Sample.Common.ExcelInterop
{
    class Old_ExcelHelper : ExcelHelper
    {
        /*
         This class support for working with Excel 97-2003 (.XLS) and CSV format 
         */
        public string fileType;
        private Application excel;
        public System.Data.DataTable table;
        public string excelFilePath;

        public Old_ExcelHelper() { }

        public Old_ExcelHelper(string fileType, string filePath)
        {
            this.fileType = fileType;
            if (fileType == ".csv")
                table = ConvertCSVtoDataTable(filePath);
            excelFilePath = filePath;
        }

        public void Open(string sheetName)
        {
            if (fileType != ".csv")
            {
                using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        DataSet result = reader.AsDataSet();
                        if (sheetName.Trim() == "")
                            table = result.Tables[0];
                        else
                            table = result.Tables[sheetName.Trim()];
                    }
                }
            }

        }

        public string GetAllValue()
        {
            string strCellValue = "";


            int rowCount = table.Rows.Count;
            int colCount = table.Columns.Count;

            for (int i = 0; i < rowCount; i++)
                for (int j = 0; j < colCount; j++)
                    strCellValue += table.Rows[i][j].ToString() + ",";

            strCellValue = strCellValue.Remove(strCellValue.Length - 1);

            return strCellValue;
        }

        public string GetAllExcelRowsValue(int rowIndex)
        {
            string strCellValue = "";

            int rowCount = table.Rows.Count;
            int colCount = table.Columns.Count;

            if (rowIndex > rowCount)
                return strCellValue;
            else
            {
                for (int colIndex = 0; colIndex < colCount; colIndex++)
                    strCellValue += table.Rows[rowIndex][colIndex].ToString() + ",";

                strCellValue = strCellValue.Remove(strCellValue.Length - 1);

                return strCellValue;
            }
        }

        public void WriteDataToExcelFile(string filePath, string sheetName, int rowIndex, int colIndex, string cellValue)
        {
            //Not implement for .csv file
        }


        public void OpenExcelfileToView(string filePath, string sheetName, int timeout)
        {
            excel = new Application();
            var workBooks = excel.Workbooks;
            Workbook workBook = workBooks.Open(filePath, ReadOnly: false, Editable: true);
            Worksheet workSheet;
            if (sheetName.Trim() == "")
                workSheet = workBook.Worksheets[1];
            else
                workSheet = workBook.Sheets[sheetName];

            try
            {
                excel.Visible = true;
                workSheet.Activate();

                Thread.Sleep(timeout * 1000);
                excel.Visible = false;
                // workBook.Close();
                //excel.Application.Quit();
                // excel.Quit();
            }
            catch (Exception)
            {
                excel.Application.Quit();
                excel.Quit();
            }
            finally
            {
                if (workSheet != null) Marshal.ReleaseComObject(workSheet);
                if (workBook != null) Marshal.ReleaseComObject(workBook);
                if (workBooks != null) Marshal.ReleaseComObject(workBooks);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void Close()
        {
            excel.Application.Quit();
            excel.Quit();
            Process[] excelProcs = Process.GetProcessesByName("EXCEL");
            foreach (Process proc in excelProcs)
            {
                proc.Kill();
            }
            if (excel != null) Marshal.ReleaseComObject(excel);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        public int GetExcelTotalRows()
        {
            return table.Rows.Count;
        }

        public string GetCellValue(int intRow, int intColumn)
        {
            string cellValue = null;

            cellValue = table.Rows[intRow - 1][intColumn - 1].ToString();

            return cellValue;
        }

        public int[] Search(string strKeyword, bool blnCaseSensitive = true)
        {
            if (!blnCaseSensitive) strKeyword = strKeyword.ToLower();

            int rowCount = table.Rows.Count;
            int colCount = table.Columns.Count;

            for (int i = 0; i < rowCount; i++)
                for (int j = 0; j < colCount; j++)
                {
                    string strCell = table.Rows[i][j].ToString();
                    if ((blnCaseSensitive && strCell.Equals(strKeyword)) ||
                            (!blnCaseSensitive && strCell.ToLower().Equals(strKeyword)))
                        return new int[2] { i + 1, j + 1 };

                }
            return new int[2] { -1, -1 };
        }

        private System.Data.DataTable ConvertCSVtoDataTable(string filePath)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            using (StreamReader sr = new StreamReader(filePath))
            {
                // Get first row - header
                string[] headers = sr.ReadLine().Split(',');
                DataRow firstRow = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dt.Columns.Add("Header " + i.ToString());
                    firstRow[i] = headers[i].Trim();
                }
                dt.Rows.Add(firstRow);

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                            dr[i] = rows[i].Trim();
                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }
    }
}
