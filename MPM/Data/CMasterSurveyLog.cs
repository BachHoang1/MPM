// author: unknown
// purpose: to hold all relevant information about Surveys
// note: borrowed from virtual display project

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MPM.Utilities;

namespace MPM.Data
{
    //class CMasterSurveyLog : CSurvey
    //{
       
    //    public static double NotSetDouble;
    //    public static int NotSetInt;
    //    private CCurrentReading _readings;

    //    public CMasterSurveyLog.SurveySource SurveyType { get; set; }

    //    public int Mode { get; set; }

    //    public string Type { get; set; }

    //    public int PacketType { get; set; }

    //    public double MeasuredDepth { get; set; }

    //    public double Inclination { get; set; }

    //    public double Azimuth { get; set; }

    //    public double DMTF { get; set; }

    //    public double ToolFace { get; set; }

    //    public double AccelerationTotal { get; set; }

    //    public double MagneticTotal { get; set; }

    //    public double DipAngle { get; set; }

    //    public double AccelerationX { get; set; }

    //    public double AccelerationY { get; set; }

    //    public double AccelerationZ { get; set; }

    //    public double MagneticX { get; set; }

    //    public double MagneticY { get; set; }

    //    public double MagneticZ { get; set; }

    //    public double Temperature { get; set; }

    //    public double TrueVerticalDepth { get; set; }

    //    public double North { get; set; }

    //    public double East { get; set; }

    //    public double VerticalSection { get; set; }

    //    public double DogLeg { get; set; }

    //    public double TrueVerticalDepthSubSea { get; set; }

    //    public double Gamma { get; set; }

    //    public double InsidePressure { get; set; }

    //    public double OutsidePressure { get; set; }

    //    public double Resistivity { get; set; }

    //    public double AxialShock { get; set; }

    //    public double TransShock { get; set; }

    //    public double NBGammaUp { get; set; }

    //    public double NBGammaDown { get; set; }

    //    public double NBGammaTotal { get; set; }

    //    public int Battery { get; set; }

    //    public int Vib { get; set; }

    //    public double Power { get; set; }

    //    public double SignalStrength { get; set; }

    //    public bool Parity { get; set; }

    //    public double Noise { get; set; }

    //    public double NorthOffset { get; set; }

    //    public double EastOffset { get; set; }

    //    public override double SignalToNoiseRatio
    //    {
    //        get
    //        {
    //            return this.SignalStrength / this.Noise;
    //        }
    //        set
    //        {
    //        }
    //    }

    //    public CMasterSurveyLog(CBitRun bitRun, CCurrentReading current)
    //    {
    //        this.SetBitRunId(bitRun);
    //        this._readings = current;
    //        this.Clear();
    //    }

    //    public CMasterSurveyLog()
    //    {
    //        this.Clear();
    //    }

    //    public CMasterSurveyLog(CMasterSurveyLog source)
    //    {
    //        this.CopyFrom(source);
    //    }

    //    public void Clear()
    //    {
    //        this.SurveyType = CMasterSurveyLog.SurveySource.Original;
    //        this.Type = string.Empty;
    //        this.Mode = CMasterSurveyLog.NotSetInt;
    //        this.PacketType = CMasterSurveyLog.NotSetInt;
    //        this.MeasuredDepth = CMasterSurveyLog.NotSetDouble;
    //        this.Inclination = CMasterSurveyLog.NotSetDouble;
    //        this.Azimuth = CMasterSurveyLog.NotSetDouble;
    //        this.DMTF = CMasterSurveyLog.NotSetDouble;
    //        this.ToolFace = CMasterSurveyLog.NotSetDouble;
    //        this.AccelerationTotal = CMasterSurveyLog.NotSetDouble;
    //        this.MagneticTotal = CMasterSurveyLog.NotSetDouble;
    //        this.DipAngle = CMasterSurveyLog.NotSetDouble;
    //        this.AccelerationX = CMasterSurveyLog.NotSetDouble;
    //        this.AccelerationY = CMasterSurveyLog.NotSetDouble;
    //        this.AccelerationZ = CMasterSurveyLog.NotSetDouble;
    //        this.MagneticX = CMasterSurveyLog.NotSetDouble;
    //        this.MagneticY = CMasterSurveyLog.NotSetDouble;
    //        this.MagneticZ = CMasterSurveyLog.NotSetDouble;
    //        this.Temperature = CMasterSurveyLog.NotSetDouble;
    //        this.TrueVerticalDepth = CMasterSurveyLog.NotSetDouble;
    //        this.North = CMasterSurveyLog.NotSetDouble;
    //        this.East = CMasterSurveyLog.NotSetDouble;
    //        this.VerticalSection = CMasterSurveyLog.NotSetDouble;
    //        this.DogLeg = CMasterSurveyLog.NotSetDouble;
    //        this.TrueVerticalDepthSubSea = CMasterSurveyLog.NotSetDouble;
    //        this.Gamma = CMasterSurveyLog.NotSetDouble;
    //        this.InsidePressure = CMasterSurveyLog.NotSetDouble;
    //        this.OutsidePressure = CMasterSurveyLog.NotSetDouble;
    //        this.Resistivity = CMasterSurveyLog.NotSetDouble;
    //        this.AxialShock = CMasterSurveyLog.NotSetDouble;
    //        this.TransShock = CMasterSurveyLog.NotSetDouble;
    //        this.NBGammaUp = CMasterSurveyLog.NotSetDouble;
    //        this.NBGammaDown = CMasterSurveyLog.NotSetDouble;
    //        this.NBGammaTotal = CMasterSurveyLog.NotSetDouble;
    //        this.Battery = CMasterSurveyLog.NotSetInt;
    //        this.Vib = CMasterSurveyLog.NotSetInt;
    //        this.Power = CMasterSurveyLog.NotSetDouble;
    //        this.SignalStrength = CMasterSurveyLog.NotSetDouble;
    //        this.Parity = false;
    //        this.Noise = CMasterSurveyLog.NotSetDouble;
    //        this.NorthOffset = CMasterSurveyLog.NotSetDouble;
    //        this.EastOffset = CMasterSurveyLog.NotSetDouble;
    //        this.IsValidData = false;
    //    }

    //    public CMasterSurveyLog CreateCorrectionLog(CMasterSurveyLog original)
    //    {
    //        return new CMasterSurveyLog(original)
    //        {
    //            SurveyType = CMasterSurveyLog.SurveySource.Correction
    //        };
    //    }

    //    public void CopyFrom(CMasterSurveyLog source)
    //    {
    //        this.SetBitRunId(source.BitRunId);
    //        this.CopyBase((CSurvey)source);
    //        this.SurveyType = source.SurveyType;
    //        this.Type = source.Type;
    //        this.Mode = source.Mode;
    //        this.PacketType = source.PacketType;
    //        this.MeasuredDepth = source.MeasuredDepth;
    //        this.Inclination = source.Inclination;
    //        this.Azimuth = source.Azimuth;
    //        this.DMTF = source.DMTF;
    //        this.ToolFace = source.ToolFace;
    //        this.AccelerationTotal = source.AccelerationTotal;
    //        this.MagneticTotal = source.MagneticTotal;
    //        this.DipAngle = source.DipAngle;
    //        this.AccelerationX = source.AccelerationX;
    //        this.AccelerationY = source.AccelerationY;
    //        this.AccelerationZ = source.AccelerationZ;
    //        this.MagneticX = source.MagneticX;
    //        this.MagneticY = source.MagneticY;
    //        this.MagneticZ = source.MagneticZ;
    //        this.Temperature = source.Temperature;
    //        this.TrueVerticalDepth = source.TrueVerticalDepth;
    //        this.North = source.North;
    //        this.East = source.East;
    //        this.VerticalSection = source.VerticalSection;
    //        this.DogLeg = source.DogLeg;
    //        this.TrueVerticalDepthSubSea = source.TrueVerticalDepthSubSea;
    //        this.Gamma = source.Gamma;
    //        this.InsidePressure = source.InsidePressure;
    //        this.OutsidePressure = source.OutsidePressure;
    //        this.Resistivity = source.Resistivity;
    //        this.AxialShock = source.AxialShock;
    //        this.TransShock = source.TransShock;
    //        this.NBGammaUp = source.NBGammaUp;
    //        this.NBGammaDown = source.NBGammaDown;
    //        this.NBGammaTotal = source.NBGammaTotal;
    //        this.Battery = source.Battery;
    //        this.Vib = source.Vib;
    //        this.Power = source.Power;
    //        this.SignalStrength = source.SignalStrength;
    //        this.Parity = source.Parity;
    //        this.Noise = source.Noise;
    //        this.NorthOffset = source.NorthOffset;
    //        this.EastOffset = source.EastOffset;
    //    }

    //    public void SaveToDatabase(CDrillContext context, bool doSave)
    //    {
    //        // TO DO/FIX
    //        //context.DataMasterSurveyLog.Add(this);
    //        //if (!doSave)
    //        //    return;
    //        //context.SaveChanges();
    //    }

    //    public void UpdateToDatabase(CDrillContext context)
    //    {
    //        // TO DO/FIX
    //        //context.SaveChanges();
    //    }

    //    public void FillSurvey()
    //    {
    //        CCommonTypes.Mode mode;
    //        switch (this._readings.DataPacketType)
    //        {
    //            case CCommonTypes.PacketType.Angle:
    //            case CCommonTypes.PacketType.AngleGamma:
    //            case CCommonTypes.PacketType.Toolface:
    //            case CCommonTypes.PacketType.ToolfaceGamma:
    //                mode = CCommonTypes.Mode.Angles;
    //                break;
    //            case CCommonTypes.PacketType.Vector:
    //            case CCommonTypes.PacketType.VectorGamma:
    //                mode = CCommonTypes.Mode.Vectors;
    //                break;
    //            default:
    //                mode = CCommonTypes.Mode.Manual;
    //                break;
    //        }
    //        this.SurveyTime = DateTime.Now;
    //        this.SurveyType = CMasterSurveyLog.SurveySource.Original;
    //        this.Type = string.Empty;
    //        this.Mode = (int)mode;
    //        this.MeasuredDepth = this.GetMeasuredDepth();
    //        this.Inclination = this._readings.DataInclination;
    //        this.Azimuth = this._readings.DataAzimuth;
    //        this.DMTF = this._readings.DeltaMTF;
    //        this.ToolFace = this._readings.ToolFaceValue;
    //        this.AccelerationTotal = 0.0;
    //        this.MagneticTotal = 0.0;
    //        this.DipAngle = 0.0;
    //        this.AccelerationX = 0.0;
    //        this.AccelerationY = 0.0;
    //        this.AccelerationZ = 0.0;
    //        this.MagneticX = 0.0;
    //        this.MagneticY = 0.0;
    //        this.MagneticZ = 0.0;
    //        this.Temperature = this._readings.DataTemperature;
    //        this.North = 0.0;
    //        this.East = 0.0;
    //        this.TrueVerticalDepth = 0.0;
    //        this.VerticalSection = 0.0;
    //        this.DogLeg = 0.0;
    //        this.TrueVerticalDepthSubSea = 0.0;
    //        // TODO/FIX
    //        //this.InsidePressure = this._readings.DataInsidePressure;
    //        //this.OutsidePressure = this._readings.DataOutsidePressure;
    //        this.Resistivity = (double)this._readings.DataResistivity;
    //        this.AxialShock = this._readings.DataAxialShock;
    //        this.TransShock = this._readings.DataTransShock;
    //        this.Battery = this._readings.DataBattery;
    //        this.Vib = this._readings.DataVib;
    //        this.Power = this._readings.DataPower;
    //        this.SignalStrength = this._readings.SignalStrength;
    //        this.Parity = this._readings.IsCRCGood;
    //        this.Noise = this._readings.SignalNoise;
    //        this.NBGammaUp = this._readings.DataNBGammaUp;
    //        this.NBGammaDown = this._readings.DataNBGammaDown;
    //        this.NBGammaTotal = this._readings.DataNBGammaTotal;
    //        switch (mode)
    //        {
    //            case CCommonTypes.Mode.Vectors:
    //                this.SetVectorValues();
    //                break;
    //            case CCommonTypes.Mode.Angles:
    //                this.SetAngleValues();
    //                break;
    //        }
    //    }

    //    private void SetVectorValues()
    //    {
    //        // TODO/FIX
    //        //this.AccelerationTotal = Math.Sqrt(this._readings.DataAccelerationX * this._readings.DataAccelerationX + this._readings.DataAccelerationY * this._readings.DataAccelerationY + this._readings.DataAccelerationZ * this._readings.DataAccelerationZ);
    //        //this.MagneticTotal = 100.0 * Math.Sqrt(this._readings.DataMagneticX * this._readings.DataMagneticX + this._readings.DataMagneticY * this._readings.DataMagneticY + this._readings.DataMagneticZ * this._readings.DataMagneticZ);
    //        //this.DipAngle = this.GetDip(this._readings.DataMagneticX, this._readings.DataMagneticY, this._readings.DataMagneticZ, this._readings.DataAccelerationX, this._readings.DataAccelerationY, this._readings.DataAccelerationZ);
    //        //this.AccelerationX = this._readings.DataAccelerationX;
    //        //this.AccelerationY = this._readings.DataAccelerationY;
    //        //this.AccelerationZ = this._readings.DataAccelerationZ;
    //        //this.MagneticX = this._readings.DataMagneticX;
    //        //this.MagneticY = this._readings.DataMagneticY;
    //        //this.MagneticZ = this._readings.DataMagneticZ;
    //    }

    //    private double GetMeasuredDepth()
    //    {
    //        return this._readings.DataMeasuredDepth;
    //    }

    //    private double GetDip(double mx, double my, double mz, double ax, double ay, double az)
    //    {
    //        double num1 = ax * ax + ay * ay + az * az;
    //        double num2 = mx * mx + my * my + mz * mz;
    //        double num3 = mx * ax + my * ay + mz * az;
    //        double num4 = Math.Sqrt(num2 * num1 - num3 * num3);
    //        if (Math.Abs(num4) >= 1E-11)
    //            return CAngle.RadiansToDegrees(Math.Atan(num3 / num4));
    //        return num4 * num3 >= 0.0 ? 90.0 : 270.0;
    //    }

    //    private void SetAngleValues()
    //    {

    //        //CurrentReadings readings = this._readings;
    //        //double num1 = Math.Atan2(-(-Math.Cos(readings.DipAngle) * Math.Sin(readings.DataAzimuth)), -(Math.Sin(readings.DipAngle) * Math.Sin(readings.DataInclination) - Math.Cos(readings.DipAngle) * Math.Cos(readings.DataInclination) * Math.Cos(readings.DataAzimuth)));
    //        //double num2 = Math.Sin(readings.DataInclination);
    //        //double num3 = Math.Cos(readings.DataInclination);
    //        //double num4 = Math.Sin(readings.DataAzimuth);
    //        //double num5 = Math.Cos(readings.DataAzimuth);
    //        //double dataMagneticTotal = readings.DataMagneticTotal;
    //        //double accelerationTotal = readings.DataAccelerationTotal;
    //        //double num6 = dataMagneticTotal * Math.Cos(readings.DipAngle);
    //        //double num7 = dataMagneticTotal * Math.Sin(readings.DipAngle);
    //        //double num8;
    //        //double MagRoll;
    //        //if (readings.DataToolfaceType == CCommonTypes.ToolFaceType.Gravity)
    //        //{
    //        //    num8 = readings.ToolFaceValue;
    //        //    MagRoll = num8 + num1;
    //        //}
    //        //else
    //        //{
    //        //    MagRoll = readings.ToolFaceValue;
    //        //    num8 = MagRoll + num1;
    //        //}
    //        //this.AccelerationX = accelerationTotal * num3;
    //        //this.AccelerationY = accelerationTotal * Math.Sin(num8) * num2;
    //        //this.AccelerationZ = accelerationTotal * Math.Cos(num8) * num2;
    //        //this.MagneticX = num7 * num3 + num6 * num2 * num4;
    //        //double num9 = -num6 * num4;
    //        //double num10 = num7 * num2 - num6 * num3 * num5;
    //        //this.MagneticY = num9 * Math.Cos(num8) + num10 * Math.Sin(num8);
    //        //this.MagneticZ = num10 * Math.Cos(num8) - num9 * Math.Sin(num8);
    //        //this.DipAngle = CAngle.GetDipFromAngles(readings.DataInclination, num8, MagRoll, readings.DataAzimuth);
    //        //this.AccelerationTotal = readings.DataAccelerationTotal;
    //        //this.MagneticTotal = readings.DataMagneticTotal;
    //    }

    //    public enum SurveySource
    //    {
    //        Original,
    //        Correction,
    //    }
    //}
}

