using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Common.ExcelInterop
{
    public interface ExcelHelper
    {
        void Open(string filePath, string sheetName);
        string GetAllValue();
        int GetExcelTotalRows();
        string GetAllExcelRowsValue(int rowIndex);
        void WriteDataToExcelFile(string filePath, string sheetName, int rowIndex, int colIndex, string cellValue);
        void OpenExcelfileToView(string filePath, string sheetName, int timeout);
        string GetCellValue(int intRow, int intColumn);
        int[] Search(string strKeyword, Boolean blnCaseSensitive = true);
        void Close();
    }
}
