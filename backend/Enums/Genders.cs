using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common.Enums {
    /// <summary>
    /// 0 - male,
    /// 1- female
    /// </summary> 
    [Serializable]
    public enum Genders  {
        [Description("male")]
        Male,
        [Description("female")]
        Female 
    }
}
