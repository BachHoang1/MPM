// author: unknown
// purpose: wrapper class for various bits of information about the drilling job at hand
// note: borrowed from virtual display project

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.ComponentModel.DataAnnotations;

namespace MPM.Data
{
    public class CWellJob
    {    
        public CWellJob()
        {
            this.WellPaths = (ICollection<CWellPath>)new List<CWellPath>();
        }

        public CWellJob(CWellJob source)
        {
            this.WellPaths = (ICollection<CWellPath>)new List<CWellPath>();
            this.Copy(source, false);
            foreach (CWellPath wellPath in (IEnumerable<CWellPath>)source.WellPaths)
                this.WellPaths.Add(new CWellPath(wellPath));
        }

        private void SetDefaultValues()
        {
            this.MagTotalTolerance = CSystemConstants.DefaultToleranceTotalMag;
            this.GravTotalTolerance = CSystemConstants.DefaultToleranceTotalGrav;
            this.DipAngleTolerance = CSystemConstants.DefaultToleranceDipAngle;
            this.SignalNoiseTolerance = CSystemConstants.DefaultToleranceSignalNoise;
        }

        //[Key]
        public string WellId { get; set; }

        public virtual ICollection<CWellPath> WellPaths { get; set; }

        public string Client { get; set; }

        public string RigName { get; set; }

        public string WellInfo { get; set; }

        public string Country { get; set; }

        public string Area { get; set; }

        public string Facility { get; set; }

        public string Field { get; set; }

        public string ServiceCompany { get; set; }

        public string Slot { get; set; }

        public string Pad { get; set; }

        public string Path { get; set; }

        public string WellBore { get; set; }

        public int DepthUnitsNumeric { get; set; }

        public CDrillingBitDepth.DepthUnits DepthUnits
        {
            get
            {
                return (CDrillingBitDepth.DepthUnits)this.DepthUnitsNumeric;
            }
            set
            {
                this.DepthUnitsNumeric = (int)value;
            }
        }

        public int LatitudeNE { get; set; }

        public double LatitudeDegrees { get; set; }

        public double LatitudeMinutes { get; set; }

        public double LatitudeSeconds { get; set; }

        public double GroundElevation { get; set; }

        public int LongitudeNE { get; set; }

        public double LongitudeDegrees { get; set; }

        public double LongitudeMinutes { get; set; }

        public double LongitudeSeconds { get; set; }

        public double WaterDepth { get; set; }

        public double MagTotalTarget { get; set; }

        public double GravTotalTarget { get; set; }

        public double DipAngleTarget { get; set; }

        public double MagTotalTolerance { get; set; }

        public double GravTotalTolerance { get; set; }

        public double DipAngleTolerance { get; set; }

        public double SignalNoiseTolerance { get; set; }

        public double CorrectedDipAngle { get; set; }

        public double MeasuredGValue { get; set; }

        public double CorrectedTotalMagField { get; set; }

        public double DoglegSeverity { get; set; }

        public double TargetDirection { get; set; }

        public double CharacteristicDoglegSeverity { get; set; }

        public string SurveyNotificationSound { get; set; }

        public string Comments { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string CountyOrUwi { get; set; }

        public string ApiOrLicense { get; set; }

        public void Copy(CWellJob source, bool includeLists)
        {
            if (includeLists)
                this.WellPaths = source.WellPaths;
            this.Client = source.Client;
            this.RigName = source.RigName;
            this.WellInfo = source.WellInfo;
            this.Country = source.Country;
            this.Area = source.Area;
            this.Facility = source.Facility;
            this.Field = source.Field;
            this.ServiceCompany = source.ServiceCompany;
            this.Slot = source.Slot;
            this.Pad = source.Pad;
            this.Path = source.Path;
            this.WellBore = source.WellBore;
            this.LatitudeNE = source.LatitudeNE;
            this.LatitudeDegrees = source.LatitudeDegrees;
            this.LatitudeMinutes = source.LatitudeMinutes;
            this.LatitudeSeconds = source.LatitudeSeconds;
            this.GroundElevation = source.GroundElevation;
            this.LongitudeNE = source.LongitudeNE;
            this.LongitudeDegrees = source.LongitudeDegrees;
            this.LongitudeMinutes = source.LongitudeMinutes;
            this.LongitudeSeconds = source.LongitudeSeconds;
            this.WaterDepth = source.WaterDepth;
            this.DepthUnits = source.DepthUnits;
            this.DepthUnitsNumeric = source.DepthUnitsNumeric;
            this.CorrectedDipAngle = source.CorrectedDipAngle;
            this.MeasuredGValue = source.MeasuredGValue;
            this.CorrectedTotalMagField = source.CorrectedTotalMagField;
            this.DoglegSeverity = source.DoglegSeverity;
            this.TargetDirection = source.TargetDirection;
            this.CharacteristicDoglegSeverity = source.CharacteristicDoglegSeverity;
            this.SurveyNotificationSound = source.SurveyNotificationSound;
            this.Comments = source.Comments;
            this.StartDate = source.StartDate;
            this.EndDate = source.EndDate;
            this.MagTotalTarget = source.MagTotalTarget;
            this.GravTotalTarget = source.GravTotalTarget;
            this.DipAngleTarget = source.DipAngleTarget;
            this.MagTotalTolerance = source.MagTotalTolerance;
            this.GravTotalTolerance = source.GravTotalTolerance;
            this.DipAngleTolerance = source.DipAngleTolerance;
            this.SignalNoiseTolerance = source.SignalNoiseTolerance;
            this.CountyOrUwi = source.CountyOrUwi;
            this.ApiOrLicense = source.ApiOrLicense;
        }
    }
}

