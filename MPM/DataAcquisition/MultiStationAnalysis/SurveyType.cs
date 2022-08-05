using System;
using System.ComponentModel;

namespace RoundLAB.eddi.Classes.Enums
{
    [Flags]
    public enum SurveyType
    {
        [Description("Unchanged")]
        Unchanged = -1,

        [Description("Recorded MWD")]
        Recorded = 0,

        [Description("Interpolation")]
        Interpolation = 1,

        [Description("Projection")]
        Projection = 2,

        [Description("Gyro")]
        Gyro = 4,

        [Description("3rd Party/Imported")]
        ThirdParty = 8,

        [Description("CheckShot")]
        CheckShot = 16,

        [Description("CheckShot")]
        Deleted = 32
    }
}