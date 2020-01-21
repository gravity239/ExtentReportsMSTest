using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumCore.ElementWrapper
{
    public class Table : Element
    {
        /// <summary>
        ///     Find a web element of type Table using By locator.
        /// </summary>
        public Table(By locator)
            : base(locator)
        {
        }

        /// <summary>
        ///     Find a web element of type Table using By Xpath locator.
        /// </summary>
        public Table(string locator)
            : base(locator)
        {
        }


        /// <summary>
        /// Get index of table cell value
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        internal void GetTableCellValueIndex(string cellValue, out int rowIndex, out int columnIndex, string rowType = "tr", string colType = "td", bool equalComparison = true)
        {
            rowIndex = columnIndex = -1;
            //ReadOnlyCollection<IWebElement> rowCollection = TableElement.StableFindElements(By.TagName("tr"));
            var rowCollection = GetElement().FindElements(By.XPath($"./{rowType}"));
            foreach (var rowItem in rowCollection)
            {
                bool exitLoop = false;
                bool valueComparison = false;
                ReadOnlyCollection<IWebElement> colCollection = rowItem.FindElements(By.TagName(colType));
                foreach (var colItem in colCollection)
                {
                    if (equalComparison)
                        valueComparison = colItem.GetAttribute("innerText").Trim() == cellValue;
                    else
                        valueComparison = colItem.GetAttribute("innerText").Trim().Contains(cellValue);
                    if (valueComparison)
                    {
                        rowIndex = rowCollection.IndexOf(rowItem) + 1;
                        columnIndex = colCollection.IndexOf(colItem) + 1;
                        exitLoop = true;
                        break;
                    }
                }
                if (exitLoop == true)
                    break;
            }
        }

        /// <summary>
        /// Get value of table cell by index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <param name="colType"></param>
        /// <returns></returns>
        internal string GetTableCellValueByIndex(int rowIndex, int colIndex, string rowType = "tr", string colType = "td")
        {
            return GetElement().FindElement(By.XPath($".//{rowType}[" + rowIndex + "]/" + colType + "[" + colIndex + "]")).Text;
        }

        /// <summary>
        /// Identify TableCell by cell value
        /// </summary>
        /// <param name="TableElement"></param>
        /// <param name="cellValue"></param>
        /// <returns></returns>
        internal Element TableCell(string cellValue, string rowType = "tr", string colType = "td")
        {
            int i_RowNum = -1;
            int i_ColNum = -1;

            GetTableCellValueIndex(cellValue, out i_RowNum, out i_ColNum);
            if (i_RowNum == -1 || i_ColNum == -1)
                return null;
            else
                return new Element(By.XPath($".//{rowType}[" + i_RowNum + "]/" + colType + "[" + i_ColNum + "]"));
        }

        /// <summary>
        /// Identify TableCell by index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        internal Element TableCell(int rowIndex, int colIndex, string rowType = "tr", string colType = "td")
        {
            try
            {
                return new Element(By.XPath(".//tr[" + rowIndex + "]/" + colType + "[" + colIndex + "]"), GetElement());
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get number of rows of a table 
        /// </summary>
        /// <param name="TableElement"></param>
        /// <returns></returns>
        internal int GetTableRowNumber(string rowType = "tr")
        {
            try
            {
                return GetElement().FindElements(By.XPath($"./{rowType}")).Count;
            }

            catch
            {
                return 0;
            }
        }

        internal int GetTableColumnNumber(string colType = "td")
        {
            return GetElement().FindElements(By.XPath($"./{colType}")).Count;
        }
    }
}
