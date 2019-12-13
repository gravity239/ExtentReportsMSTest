using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumCore.Utilities
{
    /// <summary>
    ///     File utility
    /// </summary>
    public class FileUtils
    {
        /// <summary>
        ///     Get current path
        /// </summary>
        /// <returns></returns>
        public static string GetBasePath()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (!path.EndsWith("\\")) path += "\\";

            return path;
        }

        /// <summary>
        ///     Create new folder name
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static DirectoryInfo CreateFolder(string parent, string folderName)
        {
            return Directory.CreateDirectory(parent + folderName);
        }

        /// <summary>
        ///     Get parent path
        /// </summary>
        /// <returns></returns>
        public static string GetParentPath()
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return appDirectory.Substring(0, appDirectory.IndexOf("\\bin"));
        }

        /// <summary>
        ///     Delete all file in folder
        /// </summary>
        /// <param name="days"></param>
        /// <param name="directoryPath"></param>
        /// <param name="fileExtension"></param>
        public static void DeleteFilesOlderThan(int days, string directoryPath, string fileExtension = "")
        {
            foreach (var file in new DirectoryInfo(directoryPath).GetFiles("*" + fileExtension))
                if (file.CreationTime.AddDays(days).CompareTo(DateTime.Now) < 0)
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
        }
    }
}
