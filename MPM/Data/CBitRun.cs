using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace MPM.Data
{
    public class CBitRun
    {
    
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Comments { get; set; }

        public string MudType { get; set; }

        public double HoleSize { get; set; }

        public double MudDensity { get; set; }

        public double MudResistivity { get; set; }

        public double StartDepth { get; set; }

        public double DLSensorToBit { get; set; }

        public double GammaToBit { get; set; }

        public double ResistivityToBit { get; set; }

        public string UpperBatterySerialNumber { get; set; }

        public string LowerBatterySerialNumber { get; set; }

        public string DISensorSerialNumber { get; set; }

        public string TXRFirmware { get; set; }

        public string GAPSUBSerialNumber { get; set; }

        public string TransmitterSerialNumber { get; set; }

        public string GammaSerialNumber { get; set; }

        public string ResistivitySerialNumber { get; set; }

        public string MonelSerialNumber { get; set; }

        public double UpperStartJoules { get; set; }

        public double LowerStartJoules { get; set; }

        public double CollarOD { get; set; }

        public double CollarID { get; set; }

        public double GammaCORFactor { get; set; }

        public double RemainingJoulesUpper { get; set; }

        public double RemainingJoulesLower { get; set; }

        public double CirculatingHours { get; set; }

        public double RunEndDepth { get; set; }

        public double PlugInHours { get; set; }

        public double Downtime { get; set; }

        public double GravityField { get; set; }

        public double TotalMagField { get; set; }

        public double DipAngle { get; set; }

        public double Declination { get; set; }

        public double GridDeclination { get; set; }

        public double ScribeLineOffset { get; set; }

        public bool UpperBatteryNumberFailed { get; set; }

        public bool LowerBatteryNumberFailed { get; set; }

        public bool DISensorNumberFailed { get; set; }

        public bool TXRFirmwareFailed { get; set; }

        public bool GAPSUBNumberFailed { get; set; }

        public bool TransmitterNumberFailed { get; set; }

        public bool GammaNumberFailed { get; set; }

        public bool ResistivityNumberFailed { get; set; }

        public bool MonelNumberFailed { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime PlugInTime { get; set; }

        public DateTime RunEndDate { get; set; }

        public DateTime UnplugTime { get; set; }

        public CBitRun()
        {
            this.Clear();
        }

        public CBitRun(CBitRun source)
        {
            this.Id = source.Id;
            this.Copy(source);
        }

        public void Clear()
        {
            this.Name = string.Empty;
            this.Comments = string.Empty;
            this.MudType = string.Empty;
            this.TXRFirmware = string.Empty;
            this.HoleSize = CSystemConstants.NotSetDouble;
            this.MudDensity = CSystemConstants.NotSetDouble;
            this.MudResistivity = CSystemConstants.NotSetDouble;
            this.StartDepth = CSystemConstants.NotSetDouble;
            this.DLSensorToBit = CSystemConstants.NotSetDouble;
            this.GammaToBit = CSystemConstants.NotSetDouble;
            this.ResistivityToBit = CSystemConstants.NotSetDouble;
            this.UpperBatterySerialNumber = string.Empty;
            this.LowerBatterySerialNumber = string.Empty;
            this.DISensorSerialNumber = string.Empty;
            this.GAPSUBSerialNumber = string.Empty;
            this.TransmitterSerialNumber = string.Empty;
            this.GammaSerialNumber = string.Empty;
            this.ResistivitySerialNumber = string.Empty;
            this.MonelSerialNumber = string.Empty;
            this.GammaCORFactor = 1.0;
            this.UpperStartJoules = CSystemConstants.NotSetDouble;
            this.LowerStartJoules = CSystemConstants.NotSetDouble;
            this.CollarOD = CSystemConstants.NotSetDouble;
            this.CollarID = CSystemConstants.NotSetDouble;
            this.RemainingJoulesUpper = CSystemConstants.NotSetDouble;
            this.RemainingJoulesLower = CSystemConstants.NotSetDouble;
            this.CirculatingHours = CSystemConstants.NotSetDouble;
            this.RunEndDepth = CSystemConstants.NotSetDouble;
            this.PlugInHours = CSystemConstants.NotSetDouble;
            this.Downtime = CSystemConstants.NotSetDouble;
            this.GravityField = CSystemConstants.NotSetDouble;
            this.TotalMagField = CSystemConstants.NotSetDouble;
            this.DipAngle = CSystemConstants.NotSetDouble;
            this.Declination = CSystemConstants.NotSetDouble;
            this.GridDeclination = CSystemConstants.NotSetDouble;
            this.ScribeLineOffset = CSystemConstants.NotSetDouble;
            this.UpperBatteryNumberFailed = false;
            this.LowerBatteryNumberFailed = false;
            this.DISensorNumberFailed = false;
            this.TXRFirmwareFailed = false;
            this.GAPSUBNumberFailed = false;
            this.TransmitterNumberFailed = false;
            this.GammaNumberFailed = false;
            this.ResistivityNumberFailed = false;
            this.MonelNumberFailed = false;
            this.StartDate = DateTime.Now;
            this.PlugInTime = DateTime.Now;
            this.RunEndDate = DateTime.Now;
            this.UnplugTime = DateTime.Now;
        }

        public void Copy(CBitRun source)
        {
            this.Name = source.Name;
            this.Comments = source.Comments;
            this.MudType = source.MudType;
            this.TXRFirmware = source.TXRFirmware;
            this.HoleSize = source.HoleSize;
            this.MudDensity = source.MudDensity;
            this.MudResistivity = source.MudResistivity;
            this.StartDepth = source.StartDepth;
            this.DLSensorToBit = source.DLSensorToBit;
            this.GammaToBit = source.GammaToBit;
            this.ResistivityToBit = source.ResistivityToBit;
            this.UpperBatterySerialNumber = source.UpperBatterySerialNumber;
            this.UpperStartJoules = source.UpperStartJoules;
            this.LowerBatterySerialNumber = source.LowerBatterySerialNumber;
            this.LowerStartJoules = source.LowerStartJoules;
            this.DISensorSerialNumber = source.DISensorSerialNumber;
            this.GAPSUBSerialNumber = source.GAPSUBSerialNumber;
            this.TransmitterSerialNumber = source.TransmitterSerialNumber;
            this.GammaSerialNumber = source.GammaSerialNumber;
            this.ResistivitySerialNumber = source.ResistivitySerialNumber;
            this.MonelSerialNumber = source.MonelSerialNumber;
            this.CollarOD = source.CollarOD;
            this.CollarID = source.CollarID;
            this.GammaCORFactor = source.GammaCORFactor;
            this.RemainingJoulesUpper = source.RemainingJoulesUpper;
            this.RemainingJoulesLower = source.RemainingJoulesLower;
            this.CirculatingHours = source.CirculatingHours;
            this.RunEndDepth = source.RunEndDepth;
            this.PlugInHours = source.PlugInHours;
            this.Downtime = source.Downtime;
            this.GravityField = source.GravityField;
            this.TotalMagField = source.TotalMagField;
            this.DipAngle = source.DipAngle;
            this.Declination = source.Declination;
            this.GridDeclination = source.GridDeclination;
            this.ScribeLineOffset = source.ScribeLineOffset;
            this.UpperBatteryNumberFailed = source.UpperBatteryNumberFailed;
            this.LowerBatteryNumberFailed = source.LowerBatteryNumberFailed;
            this.DISensorNumberFailed = source.DISensorNumberFailed;
            this.TXRFirmwareFailed = source.TXRFirmwareFailed;
            this.GAPSUBNumberFailed = source.GAPSUBNumberFailed;
            this.TransmitterNumberFailed = source.TransmitterNumberFailed;
            this.GammaNumberFailed = source.GammaNumberFailed;
            this.ResistivityNumberFailed = source.ResistivityNumberFailed;
            this.MonelNumberFailed = source.MonelNumberFailed;
            this.StartDate = source.StartDate;
            this.PlugInTime = source.PlugInTime;
            this.RunEndDate = source.RunEndDate;
            this.UnplugTime = source.UnplugTime;
        }
    }
}

