using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Common.ExcelInterop
{
    public static class ExcelDriver
    {
        public static ExcelHelper GetExcelHelper(string filePath)
        {
            string fileType = GetFileType(filePath);

            if (fileType == ".xlsx")
                return new New_ExcelHelper(filePath);
            return new Old_ExcelHelper(fileType, filePath);
        }

        private static string GetFileType(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Extension.ToLower();
        }
    }

}
