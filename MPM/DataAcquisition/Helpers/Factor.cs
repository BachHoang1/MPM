using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPM.DataAcquisition.Helpers
{
    public static class Factor
    {
        public static int Error        = 1;
        public static int TFType       = 1;
        public static int Good         = 1;
        public static int Bad          = 1;
        public static int PacketType   = 1;
        public static int DeltaMTF     = 1;
        public static int Gamma        = 1;
        public static int Power        = 1;
        public static int Battery      = 1;
        public static int SigStrength  = 1;
        public static int Vib          = 1;
        public static int Noise        = 1;
        public static int Return       = 1;
        public static int RigCode      = 1;
        public static int Return2      = 1;
        public static int BatCapA      = 1;
        public static int BatCapB      = 1;
        public static int FormRes      = 1;
        public static int XmitPower    = 1;
        public static int Return3      = 1;
        public static int Return4      = 1;
        public static int GammaVB      = 1;
        public static int VibXY        = 1;
        public static int TvDiag       = 1;
        public static int NbGammaUp    = 1;
        public static int NbGammaDown  = 1;
        public static int NbRotation   = 1;
        public static int BentSub1     = 1;
        public static int BentSub2     = 1;
        public static int BentSub3     = 1;
        public static int BentSub4     = 1;
        public static int Rotation     = 1;
        public static int LocalVib5    = 1;
        public static int Return5      = 1;
        public static int CbgResc      = 1;
        public static int CbgNgt       = 1;
        public static int PumpPressure = 1;
        public static int AuxPressure  = 1;
        public static int AuxGamma     = 1;
        //public static double MDecl        = 1.0;  // This is the old way of sending the MDecl
        //public static double TFO          = 1.0;  // This is the old way of sending the TFO



        public static double APressure = 10.0;
        public static double BPressure = 10.0;



        public static double MagDec     = 100.0;
        public static double TFO        = 100.0;
        public static double GammaCor   = 100.0;
        public static double GammaToBit = 100.0;
        public static double DirToBit   = 100.0;



        public static double Std = 10000.0;

        public static double TF          = Std;
        public static double Inclination = Std;
        public static double Azimuth     = Std;
        public static double GT          = Std;
        public static double BT          = Std;
        public static double Temperature = Std;
        public static double GX          = Std;
        public static double GY          = Std;
        public static double GZ          = Std;
        public static double BX          = Std;
        public static double BY          = Std;
        public static double BZ          = Std;
        public static double AVolt       = Std;
        public static double VibX        = Std;
        public static double VibY        = Std;
        public static double BX16        = Std;
        public static double BY16        = Std;
        public static double BZ16        = Std;
        public static double NbInc       = Std;
        public static double NbAzimuth   = Std;
        public static double DipAngle    = Std;
        public static double Gamma12     = Std;
        public static double CbgResB     = Std;
        public static double CbgResV     = Std;
        public static double CbgResA     = Std;
        public static double AuxAzimuth  = Std;
        public static double AuxInc      = Std;
        public static double AuxToolface = Std;
        public static double AuxTemp     = Std;
        public static double AuxShock    = Std;
    }
}
