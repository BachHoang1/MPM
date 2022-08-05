// author: hoan chau
// purpose: class that implements Survey calculations for a well path

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPM.Data;

namespace MPM.Utilities
{
    public class CSurveyCalculation
    {
        public struct MD_TO_TVD
        {
            public double md;
            public double tvd;
        }

        // tvd is based on minimum curvature calculation
        public double GetTVD(CSurvey.REC rec1, CSurvey.REC rec2 )
        {           
            double dblDL = GetDogLeg(rec1, rec2);

            double dblRF = GetRatioFactor(dblDL);

            //Δ TVD = ½ Δ MD(cos I1 +cos I2 ) RF
            CAngle angle = new CAngle();
            double dblTVD = .5 * (rec2.fBitDepth - rec1.fBitDepth) * (Math.Cos(angle.DegToRad(rec1.fInclination)) + Math.Cos(angle.DegToRad(rec2.fInclination)) ) * dblRF;

            return dblTVD;
        }

        public List<MD_TO_TVD> GetMDToTVDsNoAggregate(CSurvey.REC rec1, CSurvey.REC rec2, double dblStep_ = 10)
        {
            double dblDL = GetDogLeg(rec1, rec2);

            double dblRF = GetRatioFactor(dblDL);

            //Δ TVD = ½ Δ MD(cos I1 +cos I2 ) RF
            List<MD_TO_TVD> lst = new List<MD_TO_TVD>();

            CAngle angle = new CAngle();
            double dblTVD = .5 * (rec2.fBitDepth - rec1.fBitDepth) * (Math.Cos(angle.DegToRad(rec1.fInclination)) + Math.Cos(angle.DegToRad(rec2.fInclination))) * dblRF;

            double dblDepth = rec1.fBitDepth;
            double dblFactor = .5 * (Math.Cos(angle.DegToRad(rec1.fInclination)) + Math.Cos(angle.DegToRad(rec2.fInclination))) * dblRF;
            while (dblDepth < rec2.fBitDepth)
            {                
                double dblNthTVD = (dblStep_) * dblFactor;
                MD_TO_TVD map = new MD_TO_TVD();
                map.md = dblDepth;
                map.tvd = dblNthTVD;
                lst.Add(map);
                dblDepth += dblStep_;
            }

            return lst;
        }

        // tvd formula
        //Δ TVD = ½ Δ MD (cos I1 + cos I2 ) RF
        public List<MD_TO_TVD> GetMDToTVDs(CSurvey.REC rec1, CSurvey.REC rec2, double dblLastTVD_, double dblStep_)
        {
            double dblDL = GetDogLeg(rec1, rec2);

            double dblRF = GetRatioFactor(dblDL);
                                 
            // initialize
            double dblDepth = rec1.fBitDepth;            
            double dblTVDSum = dblLastTVD_;

            CAngle angle = new CAngle();
            double dblFactor = .5 * (Math.Cos(angle.DegToRad(rec1.fInclination)) + Math.Cos(angle.DegToRad(rec2.fInclination))) * dblRF;

            List<MD_TO_TVD> lst = new List<MD_TO_TVD>();

            do
            {
                MD_TO_TVD map = new MD_TO_TVD();
                map.md = dblDepth;
                map.tvd = dblTVDSum;
                lst.Add(map);

                double dblNthTVD = dblStep_ * dblFactor;
                dblTVDSum += dblNthTVD;
                dblDepth += dblStep_;
            } while (dblDepth < rec2.fBitDepth);

            return lst;
        }

        /*
         * DL = acos( cos(I2 – I1) – sinI1 sinI2 ( 1-cos(A2-A1 DL acos( cos(I )))          
         */
        public double GetDogLeg(CSurvey.REC rec1, CSurvey.REC rec2)
        {
            double dblRetVal = CCommonTypes.BAD_VALUE;
            CAngle angle = new CAngle();

            dblRetVal = Math.Acos(Math.Cos(angle.DegToRad(rec2.fInclination) - angle.DegToRad(rec1.fInclination)) - 
                        Math.Sin(angle.DegToRad(rec1.fInclination)) * Math.Sin(angle.DegToRad(rec2.fInclination)) * 
                        (1 - Math.Cos(angle.DegToRad(rec2.fAzimuth) - angle.DegToRad(rec1.fAzimuth))));

            return dblRetVal;
        }

        public double GetDogLegSeverity(double dblDogLeg_, double dblCourseLength_, string sLengthUnit_)
        {
            double dblRetVal = 0;
            CAngle angle = new CAngle();
            if (dblCourseLength_ > 0)
            {
                if (sLengthUnit_.ToLower() == "ft")
                    dblRetVal = dblDogLeg_ * 30 / dblCourseLength_;
                else
                    dblRetVal = dblDogLeg_ * 100 / dblCourseLength_;
            }
            
            return dblRetVal;
        }

        /*
         * RF = 2 tan( DL / 2) / DL          
         */

        private double GetRatioFactor(double dblDogLeg_)
        {
            double dblRetVal = 1;
            if (Math.Abs(dblDogLeg_) > 0)
                dblRetVal =  2 * Math.Tan(dblDogLeg_ / 2) / dblDogLeg_;
            return dblRetVal;
        }

        /*
         * Δ N/S = [(sinI1 × cosA1) + (sinI2 × cosA2)] [R.F. × (ΔMD/2)]
         */
        public double GetNSChange(CSurvey.REC rec1, CSurvey.REC rec2)
        {
            double dblRetVal = 1;

            double dblDogLeg = GetDogLeg(rec1, rec2);
            double dblRF = GetRatioFactor(dblDogLeg);
            double dblDeltaMD = rec2.fBitDepth - rec1.fBitDepth;

            CAngle angle = new CAngle();
            dblRetVal = ((Math.Sin(angle.DegToRad(rec1.fInclination)) * Math.Cos(angle.DegToRad(rec1.fAzimuth))) + 
                         (Math.Sin(angle.DegToRad(rec2.fInclination)) * Math.Cos(angle.DegToRad(rec2.fAzimuth)))) * (dblRF * dblDeltaMD * 0.5) ;
            
            return dblRetVal;
        }

        /*
         * Δ E/W = [(sinI1 × sinA1) + (sinI2 × sinA2)] [R.F. × (ΔMD/2)]
         */
        public double GetEWChange(CSurvey.REC rec1, CSurvey.REC rec2)
        {
            double dblRetVal = 1;

            double dblDogLeg = GetDogLeg(rec1, rec2);
            double dblRF = GetRatioFactor(dblDogLeg);
            double dblDeltaMD = rec2.fBitDepth - rec1.fBitDepth;

            CAngle angle = new CAngle();
            dblRetVal = ((Math.Sin(angle.DegToRad(rec1.fInclination)) * Math.Sin(angle.DegToRad(rec1.fAzimuth))) +
                         (Math.Sin(angle.DegToRad(rec2.fInclination)) * Math.Sin(angle.DegToRad(rec2.fAzimuth)))) * (dblRF * dblDeltaMD * 0.5);

            return dblRetVal;
        }

        /*
         * CD = [(N/S Total * N/S Total) + (E/W Total * E/W Total)]1/2
         */
        public double GetClosureDistance(double dblNSTotal_, double dblEWTotal_)
        {
            double dblRetVal = Math.Sqrt(dblNSTotal_ * dblNSTotal_ + dblEWTotal_ * dblEWTotal_);
            return dblRetVal;
        }

        /*
         * CA = Tan–1[(E/W) Total / (N/S) Total]
         */
        public double GetClosureAzimuth(double dblNSTotal_, double dblEWTotal_)
        {
            CAngle angle = new CAngle();
            double dblRetVal = Math.Atan(dblEWTotal_ / dblNSTotal_);
            dblRetVal = angle.RadToDeg(dblRetVal);
            

            double dblTargetAzm = dblRetVal;
            if (dblTargetAzm < 0)  // make it positive
                dblTargetAzm += 360;
                        
            if (dblTargetAzm >= 90 && dblTargetAzm < 180)
                dblRetVal = 180 - dblRetVal;
            else if (dblTargetAzm >= 180 && dblTargetAzm < 270)
                dblRetVal = 180 + dblRetVal;
            else if (dblTargetAzm >= 270 && dblTargetAzm < 360)
                dblRetVal = 360 - dblRetVal;
            //else if (dblTargetAzm >= 0 && dblTargetAzm < 90)
            //     // do nothing;

            return dblRetVal;
        }
    }
}
