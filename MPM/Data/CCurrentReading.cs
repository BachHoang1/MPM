// author: unknown
// purpose: unknown
// note: borrowed from virtual display project

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MPM.DataAcquisition.Helpers;
using MPM.Utilities;
using MPM.DataAcquisition;

namespace MPM.Data
{
    //class CCurrentReading
    //{
    
    //    public static int MaxToolfaceQueueLength = 5;
        
    //    public Queue<double> ToolfaceValues = new Queue<double>(CCurrentReading.MaxToolfaceQueueLength);
    //    public Queue<CCommonTypes.ToolFaceType> ToolfaceTypes = new Queue<CCommonTypes.ToolFaceType>(CCurrentReading.MaxToolfaceQueueLength);
    //    private int shockAxial = -1;
    //    private int shockTransverse = -1;
    //    private int vibAxial = -1;
    //    private int vibTransverse = -1;
    //    public const string MessageNewGamma = "NewGamma";
    //    public const string MessageNewPressure = "NewPressure";
    //    public const string MessageMagneticTotal = "MagneticTotal";
    //    public const string MessageAccelerationTotal = "AccelerationTotal";
    //    public const string MessageSurveyUpdated = "SurveyUpdated";
    //    public const string MessageToolFaceValue = "ToolFaceValue";
    //    public const string MessageDataAzimuthUpdateFlag = "DataAzimuthUpdateFlag";
    //    public const string MessageDataInclinationUpdateFlag = "DataInclinationUpdateFlag";
    //    public const string MessageDepthValuesUpdateFlag = "DepthValuesUpdateFlag";
    //    public const string MessageDepthValuesExtraUpdateFlag = "DepthValuesExtraUpdateFlag";
    //    public const string MessagePowersUpdateFlag = "PowersUpdateFlag";
    //    public const string MessageAnnularsUpdateFlag = "AnnularsUpdateFlag";
    //    public const string MessageBoresUpdateFlag = "BoresUpdateFlag";
    //    public const string MessageVibXsUpdateFlag = "VibXsUpdateFlag";
    //    public const string MessageVibYsUpdateFlag = "VibYsUpdateFlag";
    //    public const string MessageNBGammaUpsUpdateFlag = "NBGammaUpsUpdateFlag";
    //    public const string MessageNBGammaDownsUpdateFlag = "NBGammaDownsUpdateFlag";
    //    public const string MessageNBGammaTotalsUpdateFlag = "NBGammaTotalsUpdateFlag";
    //    public const string MessageTempsUpdateFlag = "TempsUpdateFlag";
    //    public const string MessageGammaUpdateFlag = "GammaUpdateFlag";
    //    public const string MessageMVoltsUpdateFlag = "MVoltsUpdateFlag";
    //    public const string MessageSignalStrength = "SignalStrength";
    //    public const string MessageProcessNoise = "ProcessNoise";
    //    public const string MessageDipAngleUpdateFlag = "DipAngleUpdateFlag";
    //    public const string MessageUpdateDetectLog = "DetectLogFlag";
    //    public const string MessageGammaCorrFactor = "GammaCorrFactor";
    //    public const string MessageGammaToBit = "GammaToBit";
    //    public const string MessageDISensorToBit = "DISensorToBit";
    //    public const string MessageDetectMagDec = "DetectMagDec";
    //    public const string MessageDetectTFO = "DetectTFO";
    //    public const string MessageDataShockColor = "DataShockColor";
    //    public const string MessageDataVibColor = "DataVibColor";
    //    public const string MessageAuxAzimuth = "AuxAzimuth";
    //    public const string MessageAuxInclination = "AuxInclination";
    //    public const string MessageAuxToolface = "AuxToolface";
    //    public const string MessageAuxPumpPressure = "AuxPumpPressure";
    //    public const string MessageAuxPressure = "AuxPressure";
    //    public const string MessageAuxTemps = "AuxTemp";
    //    public const string MessageAuxGamma = "AuxGamma";
    //    public const string MessageAuxShock = "AuxShock";
    //    //public ModelMain ModelMain;
    //    public CBitRun CurrentBitRun;
    //    private Command CurrentCommand;
    //    //private MergeManager _mergeManager;
    //    //private ManagerMagneticValues _managerMagneticValues;
    //    //private ManagerGravityValues _managerGravityValues;
    //    //private ManagerVibrationValues _managerVibrationValues;
    //    //private ManagerPressureValues _managerPressureValues;
    //    //private ManagerDepthValues _managerDepthValues;

    //    //private CBitRun BitRun
    //    //{
    //    //    get
    //    //    {
    //    //        return this.ModelMain.CurrentBitRun;
    //    //    }
    //    //}

    //    //public RealTimeOffsets RealTimeOffsets
    //    //{
    //    //    get
    //    //    {
    //    //        return this.ModelMain.RealTimeOffsets;
    //    //    }
    //    //}

    //    //private CDrillContext Context
    //    //{
    //    //    get
    //    //    {
    //    //        return this.ModelMain.Context;
    //    //    }
    //    //}

    //    public ObservableCollection<string> DetectLogStrings { get; set; }

    //    public ObservableCollection<CGamma> GammaDataMerged { get; set; }

    //    public ObservableCollection<CGamma> GammaData { get; set; }

    //    public ObservableCollection<CPressure> PressureData { get; set; }

    //    public ObservableCollection<CPressure> PressureDataMerged { get; set; }

    //    public ObservableCollection<CDepth> DepthData { get; set; }

    //    // TODO
    //    //public ObservableCollection<VirtualDrill.DataTemperature> TemperatureData { get; set; }

    //    // TODO
    //    //public ObservableCollection<SurveyDataSimple> SurveyData { get; set; }

    //    public CGamma LastGammaMerged
    //    {
    //        get
    //        {
    //            return this.LastItem<CGamma>(this.GammaDataMerged);
    //        }
    //    }

    //    public CGamma LastGamma
    //    {
    //        get
    //        {
    //            return this.LastItem<CGamma>(this.GammaData);
    //        }
    //    }

    //    public CPressure LastPressure
    //    {
    //        get
    //        {
    //            return this.LastItem<CPressure>(this.PressureData);
    //        }
    //    }

    //    public CPressure LastPressureMerged
    //    {
    //        get
    //        {
    //            return this.LastItem<CPressure>(this.PressureDataMerged);
    //        }
    //    }

    //    // TODO
    //    //public VirtualDrill.DataTemperature LastTemperature
    //    //{
    //    //    get
    //    //    {
    //    //        return this.LastItem<VirtualDrill.DataTemperature>(this.TemperatureData);
    //    //    }
    //    //}

    //    public double CbgResA { get; set; }

    //    public double CbgResB { get; set; }

    //    public double CbgResV { get; set; }

    //    public T LastItem<T>(ObservableCollection<T> collection)
    //    {
    //        if (collection.Count == 0)
    //            return default(T);
    //        return collection[collection.Count - 1];
    //    }

    //    internal List<CGamma> LastXGammasMerged(int count)
    //    {
    //        return this.LastXFromList<CGamma>(count, this.GammaDataMerged);
    //    }

    //    public List<CGamma> LastXGammas(int count)
    //    {
    //        return this.LastXFromList<CGamma>(count, this.GammaData);
    //    }

    //    internal List<CPressure> LastXPressuresMerged(int count)
    //    {
    //        return this.LastXFromList<CPressure>(count, this.PressureDataMerged);
    //    }

    //    public List<CPressure> LastXPressures(int count)
    //    {
    //        return this.LastXFromList<CPressure>(count, this.PressureData);
    //    }

    //    public List<T> LastXFromList<T>(int count, List<T> list)
    //    {
    //        List<T> objList = new List<T>();
    //        int count1 = list.Count;
    //        int num1 = Math.Min(count, count1);
    //        int num2 = count1 - num1;
    //        if (num1 > 0)
    //        {
    //            for (int index = 0; index < num1; ++index)
    //                objList.Add(list[num2 + index]);
    //        }
    //        return objList;
    //    }

    //    public List<T> LastXFromList<T>(int count, ObservableCollection<T> list)
    //    {
    //        return this.LastXFromList<T>(count, list.ToList<T>());
    //    }

    //    //public CDepth DepthValues
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerDepthValues.DepthValues;
    //    //    }
    //    //}

    //    public bool DepthValuesUpdateFlag { get; set; }

    //    public bool DepthValuesExtraUpdateFlag { get; set; }

    //    public Queue<ParsedDataPacket> ListRawData { get; private set; }

    //    public bool ListRawDataUpdateFlag { get; set; }

    //    public List<Filtered> ListFilteredData { get; private set; }

    //    public bool ListFilteredDataUpdateFlag { get; set; }

    //    public List<double> Gammas { get; set; }

    //    public bool GammaUpdateFlag { get; set; }

    //    public List<double> Temps { get; set; }

    //    public bool TempsUpdateFlag { get; set; }

    //    public List<double> Powers { get; set; }

    //    public bool PowersUpdateFlag { get; set; }

    //    public List<double> MVolts { get; set; }

    //    public bool MVoltsUpdateFlag { get; set; }

    //    public List<double> Annulars { get; set; }

    //    public bool AnnularsUpdateFlag { get; set; }

    //    public List<double> Bores { get; set; }

    //    public bool BoresUpdateFlag { get; set; }

    //    public List<double> VibXs { get; set; }

    //    public bool VibXsUpdateFlag { get; set; }

    //    public List<double> VibYs { get; set; }

    //    public bool VibYsUpdateFlag { get; set; }

    //    public List<double> NBGammaUps { get; set; }

    //    public bool NBGammaUpsUpdateFlag { get; set; }

    //    public List<double> NBGammaDowns { get; set; }

    //    public bool NBGammaDownsUpdateFlag { get; set; }

    //    public List<double> NBGammaTotals { get; set; }

    //    public bool NBGammaTotalsUpdateFlag { get; set; }

    //    public double DataInclination { get; set; }

    //    public bool DataInclinationUpdateFlag { get; set; }

    //    public double DataAzimuth { get; set; }

    //    public bool DataAzimuthUpdateFlag { get; set; }

    //    public double SignalStrength { get; set; }

    //    public double SignalNoise { get; set; }

    //    public double SignalToNoiseRatio
    //    {
    //        get
    //        {
    //            return this.SignalStrength / this.SignalNoise;
    //        }
    //    }

    //    public CMasterSurveyLog MostRecentSurvey { get; set; }

    //    //public double BitDepth
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerDepthValues.BitDepthCombo;
    //    //    }
    //    //}

    //    public bool BitDepthUpdateFlag { get; set; }

    //    //public double HoleDepth
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerDepthValues.HoleDepthCombo;
    //    //    }
    //    //}

    //    public bool HoleDepthhUpdateFlag { get; set; }

    //    //public double OffBottom
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerDepthValues.OffBottomValue;
    //    //    }
    //    //}

    //    public bool OffBottomUpdateFlag { get; set; }

    //    //public double ROP
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerDepthValues.ROPCombo;
    //    //    }
    //    //}

    //    public bool ROPUpdateFlag { get; set; }

    //    //public double DirDepth
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerDepthValues.DirDepthValue;
    //    //    }
    //    //}

    //    public bool DirDepthUpdateFlag { get; set; }

    //    //public double BlockPos
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerDepthValues.BlockPosValue;
    //    //    }
    //    //}

    //    public bool BlockPosUpdateFlag { get; set; }

    //    //public double GammaMD
    //    //{
    //    //    get
    //    //    {
    //    //        return this.BitDepth - this.DetectGammaToBit;
    //    //    }
    //    //}

    //    public bool GammaMDUpdateFlag { get; set; }

    //    //public double StringSpeed
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerDepthValues.StringSpeedValue;
    //    //    }
    //    //}

    //    public bool StringSpeedUpdateFlag { get; set; }

    //    public double MagDec { get; set; }

    //    public bool MagDecUpdateFlag { get; set; }

    //    public double DetectMagDec { get; set; }

    //    public double DetectTFO { get; set; }

    //    public double DetectGammaCorrFactor { get; set; }

    //    public double DetectGammaToBit { get; set; }

    //    public double DISensorToBit { get; set; }

    //    public double DeltaMTF { get; set; }

    //    public double DataMeasuredDepth { get; set; }

    //    public double DataTrueVerticalDepth { get; set; }

    //    public CCommonTypes.PacketType DataPacketType { get; set; }

    //    //public double DataInsidePressure
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerPressureValues.PressureValues.BorePressure;
    //    //    }
    //    //}

    //    //public double DataOutsidePressure
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerPressureValues.PressureValues.AnnularPressure;
    //    //    }
    //    //}

    //    public long DataResistivity { get; set; }

    //    public int DataBattery { get; set; }

    //    public int DataVib { get; set; }

    //    public bool IsCRCGood { get; set; }

    //    public double DataTemperature { get; set; }

    //    public double DataAxialShock { get; set; }

    //    public double DataTransShock { get; set; }

    //    public double DataPower { get; set; }

    //    public double DataNBGammaUp { get; set; }

    //    public double DataNBGammaDown { get; set; }

    //    public double DataNBGammaTotal { get; set; }

    //    //public double DataAccelerationX
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerGravityValues.X;
    //    //    }
    //    //}

    //    //public double DataAccelerationY
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerGravityValues.Y;
    //    //    }
    //    //}

    //    //public double DataAccelerationZ
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerGravityValues.Z;
    //    //    }
    //    //}

    //    public double DataAccelerationTotal { get; set; }

    //    //public double DataMagneticX
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerMagneticValues.X;
    //    //    }
    //    //}

    //    //public double DataMagneticY
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerMagneticValues.Y;
    //    //    }
    //    //}

    //    //public double DataMagneticZ
    //    //{
    //    //    get
    //    //    {
    //    //        return this._managerMagneticValues.Z;
    //    //    }
    //    //}

    //    public double DataMagneticTotal { get; set; }

    //    public double ToolFaceValue { get; set; }

    //    public double DipAngle { get; set; }

    //    public CCommonTypes.ToolFaceType DataToolfaceType { get; set; }

    //    public int DataVibColor { get; set; }

    //    public int DataShockColor { get; set; }

    //    public double AuxAzimuth { get; set; }

    //    public double AuxInclination { get; set; }

    //    public double AuxPumpPressure { get; set; }

    //    public double AuxPressure { get; set; }

    //    public double AuxTemperature { get; set; }

    //    public double AuxToolface { get; set; }

    //    public double AuxGamma { get; set; }

    //    public double AuxShock { get; set; }

    //    // TODO/FIX
    //    //public CCurrentReading(ModelMain model)
    //    //{
    //    //    this.ModelMain = model;
    //    //    this._mergeManager = new MergeManager(this);
    //    //    this.InitializeListsForDisplay();
    //    //    this._managerMagneticValues = new ManagerMagneticValues(this.ModelMain);
    //    //    this._managerGravityValues = new ManagerGravityValues(this.ModelMain);
    //    //    this._managerVibrationValues = new ManagerVibrationValues();
    //    //    this._managerPressureValues = new ManagerPressureValues();
    //    //    this._managerDepthValues = new ManagerDepthValues(this.ModelMain);
    //    //    this.DetectLogStrings = new ObservableCollection<string>();
    //    //    this.DepthData = new ObservableCollection<CDepth>();
    //    //    this.GammaData = new ObservableCollection<CGamma>();
    //    //    this.GammaDataMerged = new ObservableCollection<CGamma>();
    //    //    this.PressureData = new ObservableCollection<CPressure>();
    //    //    this.PressureDataMerged = new ObservableCollection<CPressure>();
    //    //    this.SurveyData = new ObservableCollection<SurveyDataSimple>();
    //    //    this.TemperatureData = new ObservableCollection<VirtualDrill.DataTemperature>();
    //    //    this.ListFilteredData = new List<Filtered>();
    //    //}

    //    //public void CallOnPropertyChanged(string propertyName)
    //    //{
    //    //    this.ModelMain.CallOnPropertyChanged(propertyName);
    //    //}

    //    private void InitializeListsForDisplay()
    //    {
    //        this.Gammas = new List<double>();
    //        this.MVolts = new List<double>();
    //        this.Temps = new List<double>();
    //        this.Powers = new List<double>();
    //        this.Annulars = new List<double>();
    //        this.Bores = new List<double>();
    //        this.VibXs = new List<double>();
    //        this.VibYs = new List<double>();
    //        this.NBGammaUps = new List<double>();
    //        this.NBGammaDowns = new List<double>();
    //        this.NBGammaTotals = new List<double>();
    //        this.ListRawData = new Queue<ParsedDataPacket>(3);
    //        this.ListFilteredData = new List<Filtered>();
    //    }

    //    // TODO/FIX
    //    //public List<double> LastXValues(ModelMain.DataField field, int x)
    //    //{
    //    //    List<double> doubleList = (List<double>)null;
    //    //    switch (field)
    //    //    {
    //    //        case ModelMain.DataField.Gammas:
    //    //            doubleList = this.LastXDoubles(this.Gammas, x);
    //    //            break;
    //    //        case ModelMain.DataField.Temps:
    //    //            doubleList = this.LastXDoubles(this.Temps, x);
    //    //            break;
    //    //        case ModelMain.DataField.Powers:
    //    //            doubleList = this.LastXDoubles(this.Powers, x);
    //    //            break;
    //    //        case ModelMain.DataField.MVolts:
    //    //            doubleList = this.LastXDoubles(this.MVolts, x);
    //    //            break;
    //    //        case ModelMain.DataField.Annulars:
    //    //            doubleList = this.LastXDoubles(this.Annulars, x);
    //    //            break;
    //    //        case ModelMain.DataField.Bores:
    //    //            doubleList = this.LastXDoubles(this.Bores, x);
    //    //            break;
    //    //        case ModelMain.DataField.VibXs:
    //    //            doubleList = this.LastXDoubles(this.VibXs, x);
    //    //            break;
    //    //        case ModelMain.DataField.VibYs:
    //    //            doubleList = this.LastXDoubles(this.VibYs, x);
    //    //            break;
    //    //        case ModelMain.DataField.NBGammaUps:
    //    //            doubleList = this.LastXDoubles(this.NBGammaUps, x);
    //    //            break;
    //    //        case ModelMain.DataField.NBGammaDowns:
    //    //            doubleList = this.LastXDoubles(this.NBGammaDowns, x);
    //    //            break;
    //    //        case ModelMain.DataField.NBGammaTotals:
    //    //            doubleList = this.LastXDoubles(this.NBGammaTotals, x);
    //    //            break;
    //    //    }
    //    //    return doubleList;
    //    //}

    //    public double? LastFromList(List<double> collection)
    //    {
    //        List<double> doubleList = this.LastXDoubles(collection, 1);
    //        return doubleList.Count == 1 ? new double?(doubleList[0]) : new double?();
    //    }

    //    public List<double> LastXDoubles(List<double> collection, int x)
    //    {
    //        List<double> doubleList = new List<double>();
    //        int count = collection.Count;
    //        int num1 = count - 1;
    //        int num2 = Math.Min(count, x);
    //        for (int index = 0; index < num2; ++index)
    //            doubleList.Add(collection[num1 - index]);
    //        return doubleList;
    //    }

    //    public void RouteDataPackets(ParsedDataPacket pdp, CBitRun bitRun)
    //    {
    //        this.CurrentBitRun = bitRun;
    //        if (pdp.Command != Command.COMMAND_RESP_CRC_BAD || pdp.Command != Command.COMMAND_RESP_CRC_GOOD)
    //            this.CurrentCommand = pdp.Command;
    //        switch (pdp.Command)
    //        {
    //            case Command.COMMAND_RESP_RAW:
    //                this.ProcessRaw(pdp);
    //                break;
    //            case Command.COMMAND_RESP_FILTERED:
    //                this.ProcessFiltered(pdp);
    //                break;
    //            case Command.COMMAND_RESP_TFTYPE:
    //                // TODO/FIX
    //                //this.DataToolfaceType = pdp.ToolFaceType;
    //                break;
    //            case Command.COMMAND_RESP_TF:
    //                this.ProcessToolFace(pdp);
    //                break;
    //            case Command.COMMAND_RESP_INCLINATION:
    //                this.ProcessInclination(pdp);
    //                break;
    //            case Command.COMMAND_RESP_AZIMUTH:
    //                this.ProcessAzimuth(pdp);
    //                break;
    //            // TODO/FIX
    //            //case Command.COMMAND_RESP_GT:
    //            //    this.DataAccelerationTotal = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("AccelerationTotal");
    //            //    break;
    //            //case Command.COMMAND_RESP_BT:
    //            //    this.DataMagneticTotal = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("MagneticTotal");
    //            //    break;
    //            case Command.COMMAND_RESP_TEMPERATURE:
    //                this.ProcessTemperature(pdp);
    //                break;
    //            case Command.COMMAND_RESP_CRC_GOOD:
    //                this.ProcessCRCGood(pdp, true);
    //                break;
    //            case Command.COMMAND_RESP_CRC_BAD:
    //                this.ProcessCRCGood(pdp, false);
    //                break;
    //            case Command.COMMAND_RESP_GX:
    //            case Command.COMMAND_RESP_GY:
    //            case Command.COMMAND_RESP_GZ:
    //                this.ProcessGravityValues(pdp);
    //                break;
    //            case Command.COMMAND_RESP_BX:
    //            case Command.COMMAND_RESP_BY:
    //            case Command.COMMAND_RESP_BZ:
    //                this.ProcessMagneticValues(pdp);
    //                break;
    //            case Command.COMMAND_RESP_PACKET_TYPE:
    //                this.DataPacketType = (CCommonTypes.PacketType)pdp.ValueInt;
    //                break;
    //            case Command.COMMAND_RESP_deltaMTF:
    //                this.ProcessDeltaMTF(pdp);
    //                break;
    //            case Command.COMMAND_RESP_GAMMA:
    //                this.ProcessGamma(pdp);
    //                break;
    //            case Command.COMMAND_RESP_B_PRESSURE:
    //            case Command.COMMAND_RESP_A_PRESSURE:
    //                this.ProcessPressure(pdp);
    //                break;
    //            case Command.COMMAND_RESP_POWER:
    //                this.ProcessPower(pdp);
    //                break;
    //            case Command.COMMAND_RESP_BATTERY:
    //                this.DataBattery = pdp.ValueInt;
    //                break;
    //            case Command.COMMAND_MDecl:
    //                this.ProcessMDecl(pdp);
    //                break;
    //            case Command.COMMAND_SIG_STRENGTH:
    //                this.ProcessSignalStrength(pdp);
    //                break;
    //            case Command.COMMAND_PASON_DEPTH:
    //            case Command.COMMAND_DEPTHTRACKER_EXTRAINFO:
    //                this.ProcessDepthData(pdp);
    //                break;
    //            case Command.COMMAND_RESP_VIBX:
    //                this.ProcessVibX(pdp);
    //                break;
    //            case Command.COMMAND_RESP_VIBY:
    //                this.ProcessVibY(pdp);
    //                break;
    //            case Command.COMMAND_RESP_VIB:
    //                this.DataVib = pdp.ValueInt;
    //                break;
    //            case Command.COMMAND_RESP_NOISE:
    //                this.ProcessNoise(pdp);
    //                break;
    //            case Command.COMMAND_RESP_FORMRES:
    //                this.DataResistivity = pdp.ValueLong;
    //                break;
    //            case Command.COMMAND_RESP_LOG_STRING_UNICODE:
    //                this.ProcessUnicodeString(pdp);
    //                break;

    //            // TODO/FIX
    //            //case Command.COMMAND_RESP_NB_GAMMATOTAL:
    //            //    this.ProcessNBGammaTotal(pdp);
    //            //    break;
    //            case Command.COMMAND_RESP_NB_GAMMAUP:
    //                this.ProcessNBGammaUp(pdp);
    //                break;
    //            case Command.COMMAND_RESP_NB_GAMMADOWN:
    //                this.ProcessNBGammaDown(pdp);
    //                break;
    //            //case Command.COMMAND_RESP_DIPANGLE:
    //            //    this.DipAngle = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("DipAngleUpdateFlag");
    //            //    break;
    //            // TODO/FIX
    //            //case Command.COMMAND_RESP_CBG_RESB:
    //            //    this.CbgResB = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("CbgResB");
    //            //    break;
    //            //case Command.COMMAND_RESP_CBG_RESV:
    //            //    this.CbgResV = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("CbgResV");
    //            //    break;
    //            //case Command.COMMAND_RESP_CBG_RESA:
    //            //    this.CbgResA = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("CbgResA");
    //            //    break;
    //            //case Command.COMMAND_RESP_AUX_AZIMUTH:
    //            //    this.AuxAzimuth = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("AuxAzimuth");
    //            //    break;
    //            //case Command.COMMAND_RESP_AUX_INC:
    //            //    this.AuxInclination = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("AuxInclination");
    //            //    break;
    //            //case Command.COMMAND_RESP_AUX_TOOLFACE:
    //            //    this.AuxToolface = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("AuxToolface");
    //            //    break;
    //            //case Command.COMMAND_RESP_PUMP_PRESSURE:
    //            //    this.AuxPumpPressure = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("AuxPumpPressure");
    //            //    break;
    //            //case Command.COMMAND_RESP_AUX_PRESSURE:
    //            //    this.AuxPressure = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("AuxPressure");
    //            //    break;
    //            //case Command.COMMAND_RESP_AUX_TEMP:
    //            //    this.AuxTemperature = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("AuxTemp");
    //            //    break;
    //            //case Command.COMMAND_RESP_AUX_GAMMA:
    //            //    this.AuxGamma = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("AuxGamma");
    //            //    break;
    //            //case Command.COMMAND_RESP_AUX_SHOCK:
    //            //    this.AuxShock = pdp.ValueDouble;
    //            //    this.CallOnPropertyChanged("AuxShock");
    //            //    break;
    //            case Command.COMMAND_RESP_MAGDEC:
    //            case Command.COMMAND_RESP_TFO:
    //            case Command.COMMAND_RESP_GAMMACOR:
    //            case Command.COMMAND_RESP_GAMMATOBIT:
    //            case Command.COMMAND_RESP_DIRTOBIT:
    //                this.ProcessReturns(pdp);
    //                break;
    //            case Command.COMMAND_RESP_SHOCKLEVEL_AXIAL:
    //            case Command.COMMAND_RESP_SHOCKLEVEL_TRANSVERSE:
    //                if (pdp.Command == Command.COMMAND_RESP_SHOCKLEVEL_AXIAL)
    //                    this.shockAxial = pdp.ValueInt;
    //                else
    //                    this.shockTransverse = pdp.ValueInt;
    //                if (this.shockAxial < 0 || this.shockTransverse < 0)
    //                    break;
    //                this.DataShockColor = this.shockAxial <= this.shockTransverse ? this.shockTransverse : this.shockAxial;
    //                this.shockAxial = this.shockTransverse = -1;
    //                //this.CallOnPropertyChanged("DataShockColor");
    //                break;
    //            case Command.COMMAND_RESP_RMSVIB_AXIAL:
    //            case Command.COMMAND_RESP_RMSVIB_TRANSVERSE:
    //                if (pdp.Command == Command.COMMAND_RESP_RMSVIB_AXIAL)
    //                    this.vibAxial = pdp.ValueInt;
    //                else
    //                    this.vibTransverse = pdp.ValueInt;
    //                if (this.vibAxial < 0 || this.vibTransverse < 0)
    //                    break;
    //                this.DataVibColor = this.vibAxial <= this.vibTransverse ? this.vibTransverse : this.vibAxial;
    //                this.vibAxial = this.vibTransverse = -1;
    //                //this.CallOnPropertyChanged("DataVibColor");
    //                break;
    //            case Command.COMMAND_RESP_SHOCKLEVEL:
    //                this.DataShockColor = pdp.ValueInt;
    //                if (this.DataShockColor > 3)
    //                    this.DataShockColor = 3;
    //                //this.CallOnPropertyChanged("DataShockColor");
    //                break;
    //            case Command.COMMAND_RESP_VIBLEVEL:
    //                this.DataVibColor = pdp.ValueInt;
    //                if (this.DataVibColor > 3)
    //                    this.DataVibColor = 3;
    //                //this.CallOnPropertyChanged("DataVibColor");
    //                break;
    //        }
    //    }

    //    private bool IsValidPacketType(int p)
    //    {
    //        switch (p)
    //        {
    //            case 0:
    //            case 1:
    //            case 2:
    //            case 3:
    //            case 4:
    //            case 5:
    //                return true;
    //            default:
    //                return false;
    //        }
    //    }

    //    private void ProcessCRCGood(ParsedDataPacket pdp, bool isCRCGood)
    //    {
    //        this.IsCRCGood = isCRCGood;
    //        if (!isCRCGood || !this.IsValidPacketType((int)this.DataPacketType))
    //            return;
    //        this.ProcessSurvey();
    //    }

    //    private void ProcessSurvey()
    //    {
    //        CMasterSurveyLog dataMasterSurveyLog = new CMasterSurveyLog(this.CurrentBitRun, this);
    //        dataMasterSurveyLog.FillSurvey();
    //        this.MostRecentSurvey = dataMasterSurveyLog;
    //        // TODO/FIX
    //        //dataMasterSurveyLog.SaveToDatabase(this.Context, true);
    //        //this.CallOnPropertyChanged("SurveyUpdated");
    //    }

    //    private bool IsToolFacePacket
    //    {
    //        get
    //        {
    //            if (this.DataPacketType != CCommonTypes.PacketType.Toolface)
    //                return this.DataPacketType == CCommonTypes.PacketType.ToolfaceGamma;
    //            return true;
    //        }
    //    }

    //    private void ProcessToolFace(ParsedDataPacket pdp)
    //    {
    //        double valueDouble = pdp.ValueDouble;
    //        this.AddValueToToolfaceQueues(valueDouble);
    //        this.ToolFaceValue = valueDouble;
    //        //this.CallOnPropertyChanged("ToolFaceValue");
    //    }

    //    private void AddValueToToolfaceQueues(double toolfaceValue)
    //    {
    //        while (this.ToolfaceValues.Count >= CCurrentReading.MaxToolfaceQueueLength)
    //            this.ToolfaceValues.Dequeue();
    //        while (this.ToolfaceTypes.Count >= CCurrentReading.MaxToolfaceQueueLength)
    //        {
    //            int num = (int)this.ToolfaceTypes.Dequeue();
    //        }
    //        this.ToolfaceValues.Enqueue(toolfaceValue);
    //        this.ToolfaceTypes.Enqueue(this.DataToolfaceType);
    //    }

    //    private void ProcessReturns(ParsedDataPacket pdp)
    //    {
    //        // TODO/FIX
    //        //switch (pdp.Command)
    //        //{
    //        //    case Command.COMMAND_RESP_MAGDEC:
    //        //        this.DetectMagDec = pdp.ValueDouble;
    //        //        this.BitRun.Declination = this.DetectMagDec;
    //        //        this.CallOnPropertyChanged("DetectMagDec");
    //        //        break;
    //        //    case Command.COMMAND_RESP_TFO:
    //        //        this.DetectTFO = pdp.ValueDouble;
    //        //        this.BitRun.ScribeLineOffset = this.DetectTFO;
    //        //        this.CallOnPropertyChanged("DetectTFO");
    //        //        break;
    //        //    case Command.COMMAND_RESP_GAMMACOR:
    //        //        this.DetectGammaCorrFactor = pdp.ValueDouble;
    //        //        this.BitRun.GammaCORFactor = this.DetectGammaCorrFactor;
    //        //        this.CallOnPropertyChanged("GammaCorrFactor");
    //        //        break;
    //        //    case Command.COMMAND_RESP_GAMMATOBIT:
    //        //        this.DetectGammaToBit = pdp.ValueDouble;
    //        //        this.BitRun.GammaToBit = this.DetectGammaToBit;
    //        //        this.CallOnPropertyChanged("GammaToBit");
    //        //        break;
    //        //    case Command.COMMAND_RESP_DIRTOBIT:
    //        //        this.DISensorToBit = pdp.ValueDouble;
    //        //        this.BitRun.DLSensorToBit = this.DISensorToBit;
    //        //        this.CallOnPropertyChanged("DISensorToBit");
    //        //        break;
    //        //}
    //        //this._mergeManager.AddDataPacket(pdp);
    //    }

    //    private void ProcessMDecl(ParsedDataPacket pdp)
    //    {
    //        this.MagDec = pdp.ValueDouble;
    //    }

    //    private void ProcessUnicodeString(ParsedDataPacket pdp)
    //    {
    //        this.AddToUnicodeOutputList(pdp);
    //        // TODO/FIX
    //        //this._mergeManager.AddDataPacket(pdp);
    //    }

    //    private void ProcessDeltaMTF(ParsedDataPacket pdp)
    //    {
    //        this.DeltaMTF = pdp.ValueDouble;
    //        // TODO/FIX
    //        //this._mergeManager.AddDataPacket(pdp);
    //    }

    //    private void ProcessMagneticValues(ParsedDataPacket pdp)
    //    {
    //        // TODO/FIX
    //        //this._managerMagneticValues.ProcessMagneticValues(pdp, this.SignalToNoiseRatio, this.DataMeasuredDepth);
    //        //if (!this._managerMagneticValues.IsZValueProcessed)
    //        //    return;
    //        //this._managerMagneticValues.GetMagObject().SaveToDatabase(this.Context, true);
    //        //this._managerMagneticValues.Reset();
    //        //this.CallOnPropertyChanged("MagneticTotal");
    //    }

    //    private void ProcessGravityValues(ParsedDataPacket pdp)
    //    {
    //        // TODO/FIX
    //        //this._managerGravityValues.ProcessGravityValues(pdp, this.SignalToNoiseRatio, this.DataMeasuredDepth);
    //        //if (!this._managerGravityValues.IsZValueProcessed)
    //        //    return;
    //        //this._managerGravityValues.GetGravityObject().SaveToDatabase(this.Context, true);
    //        //this._managerGravityValues.Reset();
    //        //this.CallOnPropertyChanged("AccelerationTotal");
    //    }

    //    private void ProcessPower(ParsedDataPacket pdp)
    //    {
    //        this.AddDoubleToWatchedList(this.Powers, pdp.ValueDouble, "PowersUpdateFlag");
    //        this.DataPower = pdp.ValueDouble;
    //        // TODO/FIX
    //        //this._mergeManager.AddDataPacket(pdp);
    //    }

    //    private void ProcessPressure(ParsedDataPacket pdp)
    //    {
    //        // TODO/FIX
    //        //this._managerPressureValues.ProcessPressureData(pdp, this.SignalToNoiseRatio, this.ModelMain.RealTimeOffsets);
    //        //if (this._managerPressureValues.AreBothProcessed)
    //        //{
    //        //    this.SavePressureDataToDatabaseAndReset(this._managerPressureValues.ReadAndClear(this.CurrentBitRun.Id));
    //        //    this.PressureData.Add(new CPressure(pdp));
    //        //    this.CallOnPropertyChanged("NewPressure");
    //        //}
    //        //if (pdp.Command == Command.COMMAND_RESP_A_PRESSURE)
    //        //    this.AddDoubleToWatchedList(this.Annulars, this._managerPressureValues.PressureValues.AnnularPressure, "AnnularsUpdateFlag");
    //        //else
    //        //    this.AddDoubleToWatchedList(this.Bores, pdp.PressureValues.BorePressure, "BoresUpdateFlag");
    //        //this.DataTrueVerticalDepth = this._managerPressureValues.PressureValues.TrueVerticalDepth;
    //    }

    //    private void SavePressureDataToDatabaseAndReset(CPressure pressure)
    //    {
    //        // TODO/FIX
    //        //pressure.SaveToDatabase(this.Context, true);
    //        //this._mergeManager.Add(pressure);
    //    }

    //    private void ProcessVibX(ParsedDataPacket pdp)
    //    {
    //        this.AddDoubleToWatchedList(this.VibXs, pdp.ValueDouble, "VibXsUpdateFlag");
    //        this.DataAxialShock = pdp.ValueDouble;
    //        // TODO/FIX
    //        //this._mergeManager.AddDataPacket(pdp);
    //    }

    //    private void ProcessVibY(ParsedDataPacket pdp)
    //    {
    //        this.AddDoubleToWatchedList(this.VibYs, pdp.ValueDouble, "VibYsUpdateFlag");
    //        this.DataTransShock = pdp.ValueDouble;
    //        // TODO/FIX
    //        //this._mergeManager.AddDataPacket(pdp);
    //    }

    //    private void ProcessNBGammaUp(ParsedDataPacket pdp)
    //    {
    //        this.AddDoubleToWatchedList(this.NBGammaUps, pdp.ValueDouble, "NBGammaUpsUpdateFlag");
    //        this.DataNBGammaUp = pdp.ValueDouble;
    //        // TODO/FIX
    //        //this._mergeManager.AddDataPacket(pdp);
    //    }

    //    private void ProcessNBGammaDown(ParsedDataPacket pdp)
    //    {
    //        this.AddDoubleToWatchedList(this.NBGammaDowns, pdp.ValueDouble, "NBGammaDownsUpdateFlag");
    //        this.DataNBGammaDown = pdp.ValueDouble;
    //        // TODO/FIX
    //        //this._mergeManager.AddDataPacket(pdp);
    //    }

    //    private void ProcessNBGammaTotal(ParsedDataPacket pdp)
    //    {
    //        this.AddDoubleToWatchedList(this.NBGammaTotals, pdp.ValueDouble, "NBGammaTotalsUpdateFlag");
    //        this.DataNBGammaTotal = pdp.ValueDouble;
    //        // TODO/FIX
    //        //this._mergeManager.AddDataPacket(pdp);
    //    }

    //    private void ProcessTemperature(ParsedDataPacket pdp)
    //    {
    //        // TODO/FIX
    //        //this.AddDoubleToWatchedList(this.Temps, pdp.ValueDouble, "TempsUpdateFlag");
    //        //this.DataTemperature = pdp.ValueDouble;
    //        //new VirtualDrill.DataTemperature(this.CurrentBitRun, pdp).SaveToDatabase(this.Context, true);
    //    }

    //    private void ProcessGamma(ParsedDataPacket pdp)
    //    {
    //        // TODO/FIX
    //        //GammaValues gammaValues = pdp.GammaValues;
    //        //gammaValues.FOB = this.DepthValues.ForceOnBottom;
    //        //gammaValues.GammaCorrectionFactor = this.CurrentBitRun.GammaCORFactor;
    //        //this.AddDoubleToWatchedList(this.Gammas, gammaValues.GammaWithCorrectionFactor, "GammaUpdateFlag");
    //        //CGamma newDataGamma = gammaValues.GetNewDataGamma(this.CurrentBitRun.Id);
    //        //this.GammaData.Add(newDataGamma);
    //        //newDataGamma.SaveToDatabase(this.Context, true);
    //        //this._mergeManager.Add(newDataGamma);
    //        //this.CallOnPropertyChanged("NewGamma");
    //        //this.DataTrueVerticalDepth = gammaValues.TrueVerticalDepth;
    //        //this.CallOnPropertyChanged("GammaUpdateFlag");
    //    }

    //    private void ProcessSignalStrength(ParsedDataPacket pdp)
    //    {
    //        this.SignalStrength = pdp.ValueDouble;
    //        this.AddDoubleToWatchedList(this.MVolts, pdp.ValueDouble, "MVoltsUpdateFlag");
    //        // TODO/FIX
    //        //this._mergeManager.AddDataPacket(pdp);
    //        //this.CallOnPropertyChanged("SignalStrength");
    //    }

    //    private void ProcessNoise(ParsedDataPacket pdp)
    //    {
    //        this.SignalNoise = pdp.ValueDouble;
    //        // TODO/FIX
    //        //this._mergeManager.AddDataPacket(pdp);
    //        //this.CallOnPropertyChanged(nameof(ProcessNoise));
    //    }

    //    private void ProcessFiltered(ParsedDataPacket pdp)
    //    {
    //    }

    //    private void ProcessRaw(ParsedDataPacket pdp)
    //    {
    //        this.ListRawData.Enqueue(pdp);
    //        // TODO/FIX
    //        //this.CallOnPropertyChanged("ListRawDataUpdateFlag");
    //    }

    //    private void ProcessAzimuth(ParsedDataPacket pdp)
    //    {
    //        this.DataAzimuth = pdp.ValueDouble;
    //        pdp.SignalToNoiseRatio = this.SignalToNoiseRatio;
    //        // TODO/FIX
    //        //this._mergeManager.AddDataPacket(pdp);
    //        //this.CallOnPropertyChanged("DataAzimuthUpdateFlag");
    //    }

    //    private void ProcessInclination(ParsedDataPacket pdp)
    //    {
    //        this.DataInclination = pdp.ValueDouble;
    //        pdp.SignalToNoiseRatio = this.SignalToNoiseRatio;
    //        //this._mergeManager.AddDataPacket(pdp);
    //        //this.CallOnPropertyChanged("DataInclinationUpdateFlag");
    //    }

    //    private void ProcessDepthData(ParsedDataPacket pdp)
    //    {
    //        // TODO/FIX
    //        //this._managerDepthValues.ProcessDepthData(pdp, this.SignalToNoiseRatio, this.ModelMain.RealTimeOffsets);
    //        //if (pdp.Command == Command.COMMAND_PASON_DEPTH)
    //        //{
    //        //    this.DataMeasuredDepth = this._managerDepthValues.DepthValues.BitDepth;
    //        //    this.CallOnPropertyChanged("DepthValuesUpdateFlag");
    //        //}
    //        //else
    //        //{
    //        //    if (this._managerDepthValues.AreBothProcessed)
    //        //    {
    //        //        this._mergeManager.AddDataPacket(pdp);
    //        //        CDepth newDepth = new CDepth(this._managerDepthValues.DepthValues);
    //        //        newDepth.BitDepthOffset = this.CurrentBitRun.DLSensorToBit;
    //        //        this.DepthData.Add(newDepth);
    //        //        this._managerDepthValues.Reset();
    //        //        this._mergeManager.ProcessNewDepth(newDepth);
    //        //    }
    //        //    this.CallOnPropertyChanged("DepthValuesExtraUpdateFlag");
    //        //}
    //    }

    //    private void AddDoubleToWatchedList(List<double> list, double value, string propertyFlag)
    //    {
    //        list.Add(value);
    //        // TODO/FIX
    //        //this.CallOnPropertyChanged(propertyFlag);
    //    }

    //    private void AddToUnicodeOutputList(ParsedDataPacket pdp)
    //    {
    //        this.DetectLogStrings.Add(CCurrentReading.RemoveNewLineChars(pdp.UnicodeString));
    //        // TODO/FIX
    //        //this.CallOnPropertyChanged("DetectLogFlag");
    //    }

    //    private static string RemoveNewLineChars(string s)
    //    {
    //        while (s.StartsWith("\r") || s.StartsWith("\n"))
    //            s = s.Substring(1);
    //        while (s.EndsWith("\r") || s.EndsWith("\n"))
    //            s = s.Substring(0, s.Length - 1);
    //        return s;
    //    }
    //}
}

