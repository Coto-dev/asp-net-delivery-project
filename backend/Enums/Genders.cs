using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums {
    /// <summary>
    /// 0 - male,
    /// 1- female
    /// </summary>
    public enum Genders  {
        [Description("male")]
        Male = 0,
        [Description("female")]
        Female = 1
    }
}
