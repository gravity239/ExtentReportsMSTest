using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Common.Options
{
    public enum SampleOptions
    {
        [Description("")]
        Green = 0,
        [Description("Green Yellow")]
        GreenYellow = 1,
        Red = 2
    }
}
