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
            if (Config.Env == null || Config.Env == "Desk")
                 fullName = type.FullName;
            else
                fullName = type.FullName + Config.Env;
            type = Type.GetType(fullName);
            return (T)Activator.CreateInstance(type);
        }
    }
}
