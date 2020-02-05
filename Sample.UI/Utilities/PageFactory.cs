using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.UI.Utilities
{
    public class PageFactory
    {
        public static T Get<T>()
        {
            var type = typeof(T);
            string fullName = null;
            if (Config.MachineType == null || Config.MachineType == "Desk")
                 fullName = type.FullName;
            else
                fullName = type.FullName + Config.MachineType;
            type = Type.GetType(fullName);
            return (T)Activator.CreateInstance(type);
        }

        public static T Get<T>(string machineType)
        {
            var type = typeof(T);
            string fullName = null;
            if (machineType == null || machineType == "Desk")
                fullName = type.FullName;
            else
                fullName = type.FullName + machineType;
            type = Type.GetType(fullName);
            return (T)Activator.CreateInstance(type);
        }
    }
}
