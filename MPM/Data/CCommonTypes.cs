// author: hoan chau
// purpose: reference constants and enums that can be used by more than one module

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    public class CCommonTypes
    {
        public enum Mode
        {
            Vectors,
            Angles,
            Manual,
        }

        public enum PacketType
        {
            Angle,
            AngleGamma,
            Vector,
            VectorGamma,
            Toolface,
            ToolfaceGamma,
        }

        public enum ToolFaceType
        {
            Magnetic,
            Gravity,
            NoValue,
        }

        public enum StepLevelEnum
        {
            StepNone,
            Step_0_25,
            Step_0_5,
            Step_1_0,
        }

        public enum UNIT_SET
        {
            UNIT_SET_TO_METRIC,
            UNIT_SET_TO_IMPERIAL,
            UNIT_SET_BY_EACH_DPOINT,
            UNIT_SET_BY_GROUP  // e.g., all length would be feet or meters, all temperature would be f or c, etc.
        }

        public enum SERVER_PACKET_ID
        {
            UPDATE_TO_WITS_RECORD = 1,
            UPDATE_TO_JOB_OR_RIG_DESCRIPTION = 2,
            UPDATE_TO_PRESSURE_TRANSDUCER_SETTINGS = 3,
            UPDATE_TO_A_UNIT_OF_MEASUREMENT = 4,
            UNIT_OF_MEASUREMENT_SET_TO_METRIC = 5,
            UNIT_OF_MEASUREMENT_SET_TO_IMPERIAL = 6,
            UNIT_OF_MEASUREMENT_SET_TO_DPOINT = 7,
            WITS_RECORDS_SET_TO_DEFAULT = 8,
            UNIT_OF_MEASUREMENT_SET_TO_GROUP = 9,  // identifies that the option changed for the type of unit set
            UPDATE_A_GROUP_OF_MEASUREMENTS = 10,
            UPDATE_A_GROUP_OF_MEASUREMENT_TYPE = 11  // identifies if a group changed from ft to meters, for example
        }

        public enum SURVEY_TYPE
        {
            ANGLE = 0,
            VECTOR_WITH_GAMMA = 1,
            VECTOR = 4,
            VECTOR_WITH_GAMMA2 = 5,
            VECTOR_WITH_PRESSURE = 6,
            SHORT_ANGLE_WITH_GAMMA = 7,
            SHORT_ANGLE = 14,
            UNKNOWN = 999
        };

        public enum RECEIVER_TYPE
        {
            UNKNOWN = 0,
            B = 1,  // 560B
            C = 2,  // 560C
            R = 3   // 560R
        };

        public enum TELEMETRY_TYPE { TT_MP = 0, TT_EM, TT_BOTH, TT_RL, TT_NONE };
        public enum TELEMETRY_TYPE_FOR_LAS { TT_MP = TELEMETRY_TYPE.TT_MP, TT_EM = TELEMETRY_TYPE.TT_EM, TT_BOTH = TELEMETRY_TYPE.TT_BOTH};

        public const string EM_SUPER_SCRIPT = "ᴱᴹ";
        public const string MP_SUPER_SCRIPT = "ᴹᴾ";
        public const string ROUND_LAB_DESCRIPTOR = "rL ☑";
        public const string ROUND_LAB_DESCRIPTOR2 = "☑";

        // thresholds for window resizing
        public const int MAX_WIDTH_LEFT_SIDE_WIDGETS = 255;  // ideal width size in pixels of windows on the left hand side
        public const int MAX_WIDTH_RIGHT_SIDE_WIDGETS = 202;  // width size of the widget containing the pump pressure, battery, and temperature
        public const int MIN_WIDTH_BEFORE_SHRINKING_SIDES = 1175;  // width of the main window before the two sides of the TF begin
                                                                   // to shrink to preserve the TF a little bit

        public const int MAX_HEIGHT_LAST_SURVEY_SIDE_WIDGETS = 250;
        public const int MAX_HEIGHT_LEFT_SIDE_WIDGETS = 200;
        public const int MAX_HEIGHT_RIGHT_SIDE_WIDGETS = 541;
        public const int MIN_HEIGHT_BEFORE_SHRINKING_SIDES = 725;  // based on a screenshot with Team Viewer showing the height at about 725

        public const int IDEAL_TF_DIMENSION = 512;
        public const int MIN_WIDTH_OF_TF_BEFORE_INC_AZM_SHRINK = 720;

        public const float INC_WIDGET_OFFSET = -30.0f;  // the amount to draw of the widget offscreen such that the wedge part can still be seen        
                

        public const string GTF = "GTF";
        public const string MTF = "MTF";

        public const string NO_IP_ADDRESS = "0.0.0.0";

        public const string DATA_FOLDER = "C:\\APS\\Data\\";
        public const string BACKUP_FOLDER = "C:\\APS\\backup\\";
        public const string TEST_DB = "Data Source=C:\\APS\\Data\\Jobs\\Default\\Log.db";

        public const string PACKET_ID = "PacketID";

        public const float BAD_VALUE = -9999.0f;        
    }
}
