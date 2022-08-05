using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPM.DataAcquisition.Helpers
{
    public enum Command
    {
        COMMAND_RESP_NULL,      //  Do Nothing
        COMMAND_REQ_STATUS,           // Display computer requests status
        COMMAND_ACK_STATUS,           // Measurement computer acknowledges status
        COMMAND_RESP_RAW,             // Raw data from the measurement computer -- data in d.uwData[]
        COMMAND_RESP_FILTERED,        // Filtered data from the measurement computer -- data in d.uwData[]

        // From here down the information is in d.udInfo unless otherwise specified
        COMMAND_RESP_ERROR,           // Error message from the measurement computer (32 bits)
        COMMAND_RESP_TFTYPE,          // TF Type (1 bit)
        COMMAND_RESP_TF,              // TF (6 bits)
        COMMAND_RESP_INCLINATION,     // Inclination (12 bits)
        COMMAND_RESP_AZIMUTH,         // Azimuth (12 bits) 
        COMMAND_RESP_GT,              // Gt (8 bits) - 10
        COMMAND_RESP_BT,              // Bt (10 bits)
        COMMAND_RESP_TEMPERATURE,     // Temperature (7 bits)
        COMMAND_RESP_CRC_GOOD,        // CRC was good -- no info
        COMMAND_RESP_CRC_BAD,         // CRC was bad -- no info
        COMMAND_RESP_GX,              // Gx (12 bits) 
        COMMAND_RESP_GY,              // Gy (12 bits)
        COMMAND_RESP_GZ,              // Gz (12 bits)
        COMMAND_RESP_BX,              // Bx (12 bits)
        COMMAND_RESP_BY,              // By (12 bits)
        COMMAND_RESP_BZ,              // Bz (12 bits) - 20 
        COMMAND_RESP_PACKET_TYPE,     // int 0 through 15
        COMMAND_RESP_deltaMTF,        // 5 bits LSB = 0.5
        COMMAND_RESP_GAMMA,           // 9 bit int
        COMMAND_RESP_B_PRESSURE,      // 10 bit pressure, scale not yet known
        COMMAND_RESP_A_PRESSURE,
        COMMAND_RESP_A_VOLT,          // 6 bit LSB approx 0.01953125
        COMMAND_RESP_POWER,           // 3 bit int
        COMMAND_RESP_BATTERY,         // 1 bit
        COMMAND_PING,
        COMMAND_ACK_PING,//30
        COMMAND_MDecl,
        COMMAND_TFO,
        COMMAND_SIG_STRENGTH,
        COMMAND_RESP_LOG_STRING,
        COMMAND_PASON_DEPTH,//35
        COMMAND_PASON_HOLE_DEPTH,
        COMMAND_RESP_VIBX,
        COMMAND_RESP_VIBY,
        COMMAND_RESP_VIB,
        COMMAND_RESP_NOISE, //40
        COMMAND_RESP_RETURN,
        COMMAND_RESP_QUIET,
        COMMAND_RESP_RIGCODE,
        COMMAND_RESP_RETURN2,
        COMMAND_RESP_BATCAPA,
        COMMAND_RESP_BATCAPB,
        COMMAND_RESP_FORMRES,
        COMMAND_RESP_XMITPOWER,
        COMMAND_RESP_RETURN3,
        COMMAND_RESP_RETURN4,//50
        COMMAND_RESP_BX16,
        COMMAND_RESP_BY16,
        COMMAND_RESP_BZ16,
        COMMAND_RESP_LOG_STRING_UNICODE,
        COMMAND_RESP_GAMMAVB,                     // 55
        COMMAND_RESP_VIBXY,
        COMMAND_DEPTHTRACKER_EXTRAINFO,
        //TV
        COMMAND_RESP_TV_DIAG,
        COMMAND_RESP_TV_DATA1,
        //Near Bit
        COMMAND_RESP_NBINC,				//  60
        COMMAND_RESP_NB_AZIMUTH,
        COMMAND_RESP_NB_GAMMAUP,
        COMMAND_RESP_NB_GAMMADOWN,
        COMMAND_RESP_NB_SHOCK,
        COMMAND_RESP_NB_ROTATION,
        //Bent Sub
        COMMAND_RESP_BENTSUB,				//66
        COMMAND_RESP_DIPANGLE,
        COMMAND_RESP_BENTSUB_1,
        COMMAND_RESP_BENTSUB_2,
        COMMAND_RESP_BENTSUB_3,
        COMMAND_RESP_BENTSUB_4, // 71
        COMMAND_RESP_GAMMASD,
        COMMAND_RESP_ROTATION,
        COMMAND_RESP_LOCALVIB5,
        COMMAND_RESP_LOCALVIB8,
        COMMAND_RESP_GAMMA12,
        COMMAND_RESP_RETURN5,
        COMMAND_RESP_TV_DATA2,
        COMMAND_RESP_TV_DATA3,
        COMMAND_RESP_RESB,  //80
        COMMAND_RESP_RESV,
        COMMAND_RESP_RESA,
        COMMAND_RESP_RESC,
        COMMAND_RESP_NGT,
        COMMAND_RESP_AUX_AZIMUTH,
        COMMAND_RESP_AUX_INCL_12,
        COMMAND_RESP_AUX_TOOLFACE,
        COMMAND_RESP_PUMP_PRESSURE,
        COMMAND_RESP_AUX_PRESSURE,
        COMMAND_RESP_AUX_TEMP,//90
        COMMAND_RESP_AUX_GAMMA,
        COMMAND_RESP_AUX_SHOCK,
        COMMAND_RESP_MAGDEC,
        COMMAND_RESP_TFO,
        COMMAND_RESP_GAMMACOR,
        COMMAND_RESP_GAMMATOBIT,
        COMMAND_RESP_DIRTOBIT,   //97
        COMMAND_RESP_SHOCKLEVEL_AXIAL,
        COMMAND_RESP_SHOCKLEVEL_TRANSVERSE,
        COMMAND_RESP_SHOCKAVG_AXIAL,  //100
        COMMAND_RESP_SHOCKAVG_TRANSVERSE,
        COMMAND_RESP_SHOCKMAX_AXIAL,
        COMMAND_RESP_SHOCKMAX_TRANSVERSE,
        COMMAND_RESP_RMSVIB_AXIAL,
        COMMAND_RESP_RMSVIB_TRANSVERSE,
        COMMAND_RESP_SHOCKLEVEL,
        COMMAND_RESP_VIBLEVEL,
        COMMAND_RESP_RES_PHASE,
        COMMAND_RESP_RES_RATIO,
        COMMAND_RESP_SHOCK_CNT,  //110
        COMMAND_RESP_AUX_ACCX,
        COMMAND_RESP_AUX_ACCY,
        COMMAND_RESP_AUX_ACCZ,
        COMMAND_RESP_AUX_MAGX,
        COMMAND_RESP_AUX_MAGY,
        COMMAND_RESP_AUX_MAGZ,
        COMMAND_RESP_AUX_GTOTAL,
        COMMAND_RESP_AUX_MTOTAL,
        COMMAND_RESP_AUX_DIPANGLE,
        COMMAND_RESP_AUX_GAMMASHOCK, //120
        COMMAND_RESP_SHOCKMIN_AXIAL,
        COMMAND_RESP_SHOCKMIN_TRANSVERSE,
        COMMAND_RESP_RMSVIBMAX_AXIAL,
        COMMAND_RESP_RMSVIBMAX_TRANSVERSE,
        COMMAND_RESP_RMSVIBMIN_AXIAL,
        COMMAND_RESP_RMSVIBMIN_TRANSVERSE,
        COMMAND_RESP_AUX_AZI,
        COMMAND_RESP_AUX_INC,
        COMMAND_RESP_AUX_TF,
        COMMAND_RESP_AUX_ROTATION, //130
        COMMAND_RESP_PULSER_BAUD,

        // LOCATOR COMMANDS
        COMMAND_RESP_ACX,
        COMMAND_RESP_ACY,
        COMMAND_RESP_ACZ,
        COMMAND_RESP_ACQUAL,

        //9/26/19COMMAND_RESP_ACMOT_ERR,
        COMMAND_RESP_ACMOT_QUAL,      //AC Motion Quality  //9/26/19
        COMMAND_RESP_ACMAG_ERROR,     //AC Mag Errpr       //9/26/19
        COMMAND_RESP_LOC_N,           //int new solutions found for location, number of sol found
        COMMAND_RESP_LOC,             //Location as 3 ints as meters*10^4 i.e. 1= 0.1mm  each sol found sent separately lowest sol sent first
        COMMAND_RESP_LOC2D,           //Location as 3 ints as meters*10^4, and axial mismatch as away field mismatch times (4 ints total)
        COMMAND_RESP_VERSION,         //140      int                                                                    
        COMMAND_RESP_POSIT,           //     command only --- put up 0-current dialog                               
        COMMAND_RESP_NEG,             //  command only --- put up negative current dialog
        COMMAND_RESP_POS,             //  command only --- put up positive current dialog

        //     COMMANDS to HDD from User Interface
        COMMAND_RESP_GET_VERSION,     //  command only
        COMMAND_RESP_ROTATE_MOVE,     //  6 ints, rotations in 10^-4 degrees and offsetts in 0.1mm (10^-4 m)
        COMMAND_RESP_AC_CURRENT,      //  int peak to peak current in 0.1mA
        COMMAND_RESP_NEG_CURRENT,     //  int negative current in 0.1mA
        COMMAND_RESP_POS_CURRENT,     //  int positive current in 0.1mA
        COMMAND_RESP_FLAGS,           //  int various flags bit 0 - Use AC, bit 1 - Units (0=meters 1=feet) bit 2 - use 2D solver and "plug" away
        COMMAND_RESP_OK,              //150      command only response to dialog//150
        COMMAND_RESP_CANCEL,          //  command only response to dialog 
        COMMAND_RESP_SHOT,            //  command only start shot
        COMMAND_RESP_DECL,            //  int declination in 10^-4 degrees
        COMMAND_RESP_AWAY,            //  int peg away in 10^-4 m
        COMMAND_RESP_PORT,            //  port for wireline
        COMMAND_RESP_BAUD,            //  baud for wireline
        COMMAND_RESP_VERTEX,          //  name of vertex list filename (full path)
        COMMAND_RESP_AC_AVE,          //  (int) no. of averages for AC Localization
        COMMAND_RESP_INVALID,         //  int invalid packet count
        COMMAND_RESP_BUTTON,          //160      command only for debugging only

        //     new commands---placed at end for compatability with old software
        COMMAND_RESP_JSS,             //    int, current consumed by DI in wireline system                     
        COMMAND_SKIPPED_PACKETS,      //No of packet to skip when current is switch (Zero, Neg, Pos etc) //8/9/19
        COMMAND_RESP_ROLL,            //Roll = uncorrected toolface
        COMMAND_RESP_SOLN_INDEX,      //Selected index into the possble solns (the correction solution) //9/11/19
        COMMAND_RESP_AC_EXPONENT,      //Get the AC field value along with the mantissa.

        COMMAND_BIT_DEPTH = 10108,        
        COMMAND_TVD = 10111,
        COMMAND_ROP = 10113,
        COMMAND_WOB = 10117,

        COMMAND_ECD_TVD = 10430,
        COMMAND_HYDRO_STATIC_PRESSURE = 10431,
        COMMAND_MUD_DENSITY = 10432,

        COMMAND_CS_INC = 10722,
        COMMAND_CS_AZM = 10723,

        COMMAND_HOLE_DEPTH = 11108,
        COMMAND_RAW_MP_CNT = 20024,  // troubleshoot when raw packets exceed expected 2 per second
        COMMAND_RAW_EM_CNT = 20027   // troubleshoot when raw packets exceed expected 2 per second
    }
}
