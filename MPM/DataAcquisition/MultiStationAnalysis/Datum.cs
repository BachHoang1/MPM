using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RoundLAB.eddi.Classes.Enums
{
    public enum Datum
    {
        [Description("WGS 84")]
        WGS_84 = 1,

        [Description("WGS 72")]
        WGS_72 = 2,

        [Description("NAD 83")]
        NAD_83 = 3,

        [Description("NAD 27")]
        NAD_27 = 4,

        [Description("Clarke 1880")]
        Clarke_1880 = 5,

        [Description("Clarke 1866")]
        Clarke_1866 = 6
    }
}
