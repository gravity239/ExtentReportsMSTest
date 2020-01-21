using SeleniumCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.UI.Utilities
{
    public static class Config
    {
        [ThreadStatic]
        public static string ConfigFilePath;

        [ThreadStatic]
        public static string Driver; // can be chrome,ie,firefox

        [ThreadStatic]
        public static string Env; // can be Desk or Mobile

        [ThreadStatic]
        public static string LogPath;
    }
}
