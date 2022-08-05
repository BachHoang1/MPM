// author: unknown
// purpose: utility class to calculate angles
// note: borrowed from virtual display project

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Utilities
{
    public class CAngle
    {    
        private static double MaxError = 1E-11;
        private static double PI = Math.PI;
        private static double PI2 = Math.PI / 2.0;

        public static double GetDipFromAngles(double Inclination, double Roll, double MagRoll, double Azimuth)
        {
            double num1 = MagRoll - Roll;
            double num2 = Math.Sin(num1);
            double num3 = Math.Sin(Inclination);
            double num4 = Math.Sin(Azimuth);
            double num5 = num4 * num3;
            return CAngle.RadiansToDegrees(Math.Atan2((Math.Cos(Inclination) * Math.Cos(Azimuth) * num2 - Math.Cos(num1) * num4) * num5, num2 * num3 * num5));
        }

        public static double RadiansToDegrees(double radians)
        {
            double num = radians * 180.0 / Math.PI;
            while (num < 0.0)
                num += 360.0;
            while (num >= 360.0)
                num -= 360.0;
            return num;
        }

        public double DegToRad(double dblDegrees_)
        {
            double dblRetVal = dblDegrees_ / 180 * Math.PI;
            return dblRetVal;
        }

        public double RadToDeg(double dblRadians_)
        {
            double dblRetVal = dblRadians_ * 180 / Math.PI;
            return dblRetVal;
        }

        public static double GetPitch(double AX, double AY, double AZ)
        {
            return CAngle.RadiansToDegrees(CAngle.FindAngle(Math.Sqrt(AY * AY + AZ * AZ), AX));
        }

        public static double GetRoll(double AY, double AZ)
        {
            return CAngle.RadiansToDegrees(CAngle.FindAngle(AY, AZ));
        }

        public static double GetMagRoll(double MY, double MZ)
        {
            return CAngle.RadiansToDegrees(CAngle.FindAngle(-MY, -MZ));
        }

        public static double GetAzimuth(double MX, double MY)
        {
            return CAngle.RadiansToDegrees(CAngle.FindAngle(MX, MY) - CAngle.PI / 2.0);
        }

        private static double FindAngle(double o, double a)
        {
            if (Math.Abs(a) < CAngle.MaxError)
            {
                if (o < 0.0)
                    return CAngle.PI + CAngle.PI2;
                return CAngle.PI2;
            }
            if (o > 0.0)
            {
                if (a <= 0.0)
                    return CAngle.PI - Math.Atan(o / -a);
                return Math.Atan(o / a);
            }
            if (a >= 0.0)
                return CAngle.PI * 2.0 - Math.Atan(-o / a);
            return CAngle.PI + Math.Atan(o / a);
        }

        public static double GetDip(double MX, double MY, double MZ, double AX, double AY, double AZ)
        {
            double num1 = AX * AX + AY * AY + AZ * AZ;
            double num2 = MX * MX + MY * MY + MZ * MZ;
            double num3 = MX * AX + MY * AY + MZ * AZ;
            double num4 = Math.Sqrt(num2 * num1 - num3 * num3);
            if (Math.Abs(num4) >= CAngle.MaxError)
                return CAngle.RadiansToDegrees(Math.Atan(num3 / num4));
            return num4 * num3 >= 0.0 ? 90.0 : 270.0;
        }
    }
}

