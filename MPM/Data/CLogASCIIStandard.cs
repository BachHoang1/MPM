// author: unknown but subsequently modified by hoan chau
// purpose: to export any d-point in the database with reference to measured/bit depth
//          true vertical depth, or date-time
// note: borrowed from virtual display project

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Linq.Expressions;
using MPM.Utilities;
using System.Data;
using System.Globalization;
using MPM.GUI;
using System.Windows.Forms;
using System.Data.Common;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using MPM.DataAcquisition.Helpers;
using MPM.DataAcquisition.MultiStationAnalysis;

namespace MPM.Data
{
    public class CLogASCIIStandard        
    {
        private int LAS_COLUMN_WIDTH = 16;

        private static float LAS_NULL = -999.25f;
        private static string MessageErrorSavingFile = "Error Saving File";
        private static string FileExtension = ".LAS";
        private List<string> Content = new List<string>();
        private const char Delimiter0 = '.';
        private const char Delimiter1 = ':';
        private CDrillingBitDepth.DepthUnits _depthUnits;
        private int _columnWidth0;
        private int _columnWidth1;
        private CUnitLength m_UnitLength = new CUnitLength();
        private CUnitRateOfPenetration m_UnitROP = new CUnitRateOfPenetration();
        private CUnitTemperature m_UnitTemperature = new CUnitTemperature();
        private CUnitPressure m_UnitPressure = new CUnitPressure();

        private static DbConnection m_dbCnn;
        private CMSAHubClient m_msaHubClient;

        private struct CUSTOM_DPOINT
        {
            public Int64 iMsgCode;
            public float fCurrVal;
            public float fLastVal;
            public float fLinearPt;            
        }

        public struct CURVE_INFO
        {
            public Int64 iMsgCode;
            public string sName;
            public string sUnit;
        }

        public CLogASCIIStandard(ref DbConnection dbCnn_, CMSAHubClient msaHubClient_, int colWidth0, int colWidth1, CCommonTypes.UNIT_SET unitSet_)
        {
            m_dbCnn = dbCnn_;
            m_msaHubClient = msaHubClient_;

            this._columnWidth0 = colWidth0;
            this._columnWidth1 = colWidth1;

            m_UnitLength.SetUnitType(unitSet_);
            m_UnitROP.SetUnitType(unitSet_);
            m_UnitTemperature.SetUnitType(unitSet_);
            m_UnitPressure.SetUnitType(unitSet_);

            if (unitSet_ == CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT ||
                unitSet_ == CCommonTypes.UNIT_SET.UNIT_SET_BY_GROUP)
                _depthUnits = CDrillingBitDepth.DepthUnits.None;
            else if (unitSet_ == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
                _depthUnits = CDrillingBitDepth.DepthUnits.Feet;
            else
                _depthUnits = CDrillingBitDepth.DepthUnits.Meters;
            //this._depthUnits = this._modelMain.CurrentWellJob.CDepth.DepthUnits == CDepth.DepthUnits.Feet ? CDepth.DepthUnits.Feet : CDepth.DepthUnits.Meters;
        }

        public bool CreateLasFile(CWellJob job_, CLASJobInfo.EXPORT_DATA_TYPE ExportType_, string fileName, float startDepth, float stopDepth, DateTime dtStartTime, DateTime dtStopTime, bool useUnixTime, float fStep_, CCommonTypes.TELEMETRY_TYPE_FOR_LAS ttType_, List<CURVE_INFO> lstCurves_)
        {
            bool bRetVal = false;
            if (lstCurves_.Count < 1)
            {
                MessageBox.Show("You haven't selected any curves.", "Custom Curve Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            
            List<string> lstContents = GenerateLasFileContent(job_, ExportType_, startDepth, stopDepth, dtStartTime, dtStopTime, useUnixTime, fStep_, ttType_, lstCurves_);
            fileName = this.EnsureFileExtension(fileName);
            this.EnsureDirectoryExists(fileName);
            string messageErrorSavingFile = CLogASCIIStandard.MessageErrorSavingFile;
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(fileName))
                {
                    foreach (string str in lstContents)
                        streamWriter.WriteLine(str);

                    bRetVal = true;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return bRetVal;
        }

        private void EnsureDirectoryExists(string fileName)
        {
            string directoryName = new FileInfo(fileName).DirectoryName;
            if (Directory.Exists(directoryName))
                return;
            Directory.CreateDirectory(directoryName);
        }

        private string EnsureFileExtension(string lasFileName)
        {
            string str = lasFileName.Trim();
            if (!str.EndsWith(CLogASCIIStandard.FileExtension, StringComparison.OrdinalIgnoreCase))
                str += CLogASCIIStandard.FileExtension;
            return str;
        }

        private static string DeleteExtension(string fileName)
        {
            fileName = fileName.Substring(0, fileName.Length - 4);
            return fileName;
        }

        private List<CSurveyCalculation.MD_TO_TVD> GenerateTVDFromMD(float startDepth, float stopDepth, CCommonTypes.TELEMETRY_TYPE_FOR_LAS ttType_)
        {
            CSurveyLog log = new CSurveyLog(ref m_dbCnn);
            DataTable tblSurvey = log.Get(ttType_.ToString(), "ACCEPT");

            List<CSurveyCalculation.MD_TO_TVD> lstAggregate = new List<CSurveyCalculation.MD_TO_TVD>();

            double dblTVD = 0;
            CSurveyCalculation calc = new CSurveyCalculation();
            int iMDStartIndex = 0;
            int iMDStopIndex = 0;
            List<double> lstTVDRef = new List<double>();

            // first survey depth should also be the first tvd depth
            CSurvey.REC rec0 = new CSurvey.REC();
            if (tblSurvey.Rows.Count > 0)
            {
                rec0.fBitDepth = (float)tblSurvey.Rows[0].Field<decimal>("bitDepth");
                lstTVDRef.Add(rec0.fBitDepth);
            }

            for (int i = 0; i < tblSurvey.Rows.Count - 1; i++)
            {
                if ((int)(startDepth * 1000) >= (int)(dblTVD * 1000))
                {
                    iMDStartIndex = i - 1;
                    if (iMDStartIndex < 0)
                        iMDStartIndex = 0;
                }

                CSurvey.REC rec1 = new CSurvey.REC();
                CSurvey.REC rec2 = new CSurvey.REC();

                rec1.fBitDepth = (float)tblSurvey.Rows[i].Field<decimal>("bitDepth");
                rec1.fSurveyDepth = (float)tblSurvey.Rows[i].Field<decimal>("surveyDepth");
                rec1.fInclination = (float)tblSurvey.Rows[i].Field<decimal>("inc");
                rec1.fAzimuth = (float)tblSurvey.Rows[i].Field<decimal>("azm");                                

                rec2.fBitDepth = (float)tblSurvey.Rows[i + 1].Field<decimal>("bitDepth");
                rec2.fSurveyDepth = (float)tblSurvey.Rows[i + 1].Field<decimal>("surveyDepth");
                rec2.fInclination = (float)tblSurvey.Rows[i + 1].Field<decimal>("inc");
                rec2.fAzimuth = (float)tblSurvey.Rows[i + 1].Field<decimal>("azm");                                

                double dblTVDTemp = calc.GetTVD(rec1, rec2);
                dblTVD += dblTVDTemp;
                lstTVDRef.Add(dblTVD);

                if ((int)(stopDepth * 1000) >= (int)(dblTVD * 1000))
                {
                    iMDStopIndex = i + 1;
                    if (iMDStopIndex > tblSurvey.Rows.Count - 1)
                        iMDStopIndex = tblSurvey.Rows.Count - 1;
                }
            }

            // determine where the starting md is given the starting tvd 
            // determine where the ending md is given the ending tvd                
            for (int i = iMDStartIndex; i < iMDStopIndex; i++)
            {
                CSurvey.REC rec1 = new CSurvey.REC();
                CSurvey.REC rec2 = new CSurvey.REC();

                rec1.fAzimuth = (float)tblSurvey.Rows[i].Field<decimal>("azm");
                rec1.fInclination = (float)tblSurvey.Rows[i].Field<decimal>("inc");
                rec1.fBitDepth = (float)tblSurvey.Rows[i].Field<decimal>("bitDepth");

                rec2.fAzimuth = (float)tblSurvey.Rows[i + 1].Field<decimal>("azm");
                rec2.fInclination = (float)tblSurvey.Rows[i + 1].Field<decimal>("inc");
                rec2.fBitDepth = (float)tblSurvey.Rows[i + 1].Field<decimal>("bitDepth");

                List<CSurveyCalculation.MD_TO_TVD> lst = calc.GetMDToTVDs(rec1, rec2, lstTVDRef[i], .001);
                lstAggregate.AddRange(lst);
            }
            return lstAggregate;
        }

        private List<CSurveyCalculation.MD_TO_TVD> GenerateTVDFromMD2(float startMD, float stopMD, CCommonTypes.TELEMETRY_TYPE_FOR_LAS ttType_)
        {
            CSurveyLog log = new CSurveyLog(ref m_dbCnn);
            DataTable tblSurvey = log.Get(ttType_.ToString(), "ACCEPT");

            List<CSurveyCalculation.MD_TO_TVD> lstAggregate = new List<CSurveyCalculation.MD_TO_TVD>();
            
            CSurveyCalculation calc = new CSurveyCalculation();
            int iMDStartIndex = 0;
            int iMDStopIndex = 0;
            List<double> lstTVDRef = new List<double>();

            // first survey depth should also be the first tvd depth
            CSurvey.REC rec0 = new CSurvey.REC();
            if (tblSurvey.Rows.Count > 0)
            {
                rec0.fBitDepth = (float)tblSurvey.Rows[0].Field<decimal>("bitDepth");
                lstTVDRef.Add(rec0.fBitDepth);
            }

            // compute the TVD for all survey stations
            double dblTVD = 0;
            for (int i = 0; i < tblSurvey.Rows.Count - 1; i++)
            {
                if ((int)(startMD * 1000) >= (int)((float)tblSurvey.Rows[i].Field<decimal>("bitDepth") * 1000))
                {
                    iMDStartIndex = i - 1;
                    if (iMDStartIndex < 0)
                        iMDStartIndex = 0;
                }

                CSurvey.REC rec1 = new CSurvey.REC();
                CSurvey.REC rec2 = new CSurvey.REC();

                rec1.fAzimuth = (float)tblSurvey.Rows[i].Field<decimal>("azm");
                rec1.fInclination = (float)tblSurvey.Rows[i].Field<decimal>("inc");
                rec1.fBitDepth = (float)tblSurvey.Rows[i].Field<decimal>("bitDepth");
                rec1.fSurveyDepth = (float)tblSurvey.Rows[i].Field<decimal>("surveyDepth");

                rec2.fAzimuth = (float)tblSurvey.Rows[i + 1].Field<decimal>("azm");
                rec2.fInclination = (float)tblSurvey.Rows[i + 1].Field<decimal>("inc");
                rec2.fBitDepth = (float)tblSurvey.Rows[i + 1].Field<decimal>("bitDepth");
                rec2.fSurveyDepth = (float)tblSurvey.Rows[i + 1].Field<decimal>("surveyDepth");

                double dblTVDTemp = calc.GetTVD(rec1, rec2);
                dblTVD += dblTVDTemp;
                lstTVDRef.Add(dblTVD);

                if ((int)(stopMD * 1000) >= (int)((float)tblSurvey.Rows[i + 1].Field<decimal>("bitDepth") * 1000))
                {
                    iMDStopIndex = i + 1;
                    if (iMDStopIndex > tblSurvey.Rows.Count - 1)
                        iMDStopIndex = tblSurvey.Rows.Count - 1;
                }
            }

            // determine where the starting md is given the starting tvd 
            // determine where the ending md is given the ending tvd                
            for (int i = iMDStartIndex; i < iMDStopIndex; i++)
            {
                CSurvey.REC rec1 = new CSurvey.REC();
                CSurvey.REC rec2 = new CSurvey.REC();

                rec1.fAzimuth = (float)tblSurvey.Rows[i].Field<decimal>("azm");
                rec1.fInclination = (float)tblSurvey.Rows[i].Field<decimal>("inc");
                rec1.fBitDepth = (float)tblSurvey.Rows[i].Field<decimal>("bitDepth");

                rec2.fAzimuth = (float)tblSurvey.Rows[i + 1].Field<decimal>("azm");
                rec2.fInclination = (float)tblSurvey.Rows[i + 1].Field<decimal>("inc");
                rec2.fBitDepth = (float)tblSurvey.Rows[i + 1].Field<decimal>("bitDepth");

                List<CSurveyCalculation.MD_TO_TVD> lst = calc.GetMDToTVDs(rec1, rec2, lstTVDRef[i], .001);
                lstAggregate.AddRange(lst);
            }
            return lstAggregate;
        }
        private List<string> GenerateLasFileContent(CWellJob job_, CLASJobInfo.EXPORT_DATA_TYPE ExportType_, float startDepth, float stopDepth, DateTime dtStartTime, DateTime dtStopTime, bool useUnixTime, float fStep_, CCommonTypes.TELEMETRY_TYPE_FOR_LAS ttType_, List<CURVE_INFO> lstCurves_)
        {
            //CWellJob currentWellJob = this._modelMain.CurrentWellJob;
            CWellJob currentWellJob = job_;
            //CDrillContext context = this._modelMain.Context;
            long startTimeTicks = 0;
            long endTimeTicks = 0;
            
            int pressureCount = 0;
            DataTable tbl = new DataTable();

            List<CSurveyCalculation.MD_TO_TVD> lstAggregate = new List<CSurveyCalculation.MD_TO_TVD>();

            DateTime start;
            DateTime end;

            // go back to the units that the values came in as            
            startDepth = m_UnitLength.ReverseConvert(startDepth);
            stopDepth = m_UnitLength.ReverseConvert(stopDepth);
            if (ExportType_ == CLASJobInfo.EXPORT_DATA_TYPE.MD)
            {
                lstAggregate = GenerateTVDFromMD2(startDepth, stopDepth, ttType_);
                GetCustom(startDepth, stopDepth, ttType_, ref startTimeTicks, ref endTimeTicks, ref pressureCount, ref tbl, out start, out end, lstCurves_[0].iMsgCode.ToString());
            }
            else if (ExportType_ == CLASJobInfo.EXPORT_DATA_TYPE.TIME)
            {
                GetCustom(dtStartTime, dtStopTime, ttType_, ref startTimeTicks, ref endTimeTicks, ref pressureCount, ref tbl, out start, out end, lstCurves_[0].iMsgCode.ToString());
            }
            else if (ExportType_ == CLASJobInfo.EXPORT_DATA_TYPE.TVD)
            {                  
                lstAggregate = GenerateTVDFromMD(startDepth, stopDepth, ttType_);
                GetCustom(startDepth, stopDepth, ttType_, ref startTimeTicks, ref endTimeTicks, ref pressureCount, ref tbl, out start, out end, lstCurves_[0].iMsgCode.ToString());
            }
            else  // tvd from corrected survey
            {
                // get the lstTVDRef
                List<CSurveyCalculation.MD_TO_TVD> lstTVDRef = lstAggregate = m_msaHubClient.GetTVDList();
                // determine starting md given the starting tvd
                float fMDStart = 0, fMDStop = 0;
                if (lstTVDRef.Count > 0)
                {
                    fMDStart = (float)lstTVDRef[0].md;
                    // determine ending md of the md given the ending tvd
                    fMDStop = (float)lstTVDRef[lstTVDRef.Count - 1].md;
                }
                
                GetCustom(fMDStart, fMDStop, ttType_, ref startTimeTicks, ref endTimeTicks, ref pressureCount, ref tbl, out start, out end, lstCurves_[0].iMsgCode.ToString());
            }

            CreateHeader();
            AddBlankLine();

            CreateVersionSection();
            AddBlankLine();

            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0);
            CreateWellSection((long)dtStartTime.Subtract(dt1970).TotalSeconds, (long)dtStopTime.Subtract(dt1970).TotalSeconds, start, end, startDepth, stopDepth, fStep_, currentWellJob, useUnixTime, ExportType_);
            AddBlankLine();

            CreateCurveSection(ExportType_, useUnixTime, lstCurves_);
            AddBlankLine();
            
            if (ExportType_ == CLASJobInfo.EXPORT_DATA_TYPE.TIME)
            {                
                Add(String.Empty);
                string sColumnHeader = "~Ascii  Time";
                for (int i = 0; i < lstCurves_.Count; i++)
                {
                    string s = lstCurves_[i].sName;
                    string t = s.PadLeft(LAS_COLUMN_WIDTH, ' ');
                    sColumnHeader += t;
                }

                // *********************************
                // create 2-dimensional table 
                // *********************************
                int xDim = 0;
                DateTime dtTime = dtStartTime;
                int iSeconds = (int)(fStep_ * 60);
                TimeSpan tmStep = new TimeSpan(0, 0, iSeconds);
                while (dtTime <= dtStopTime)
                {
                    dtTime += tmStep;
                    xDim++;
                }

                // initialize table with null values except for the time column 
                // which will use the time steps
                double[,] fArrTimeVersusValues = new double[xDim, lstCurves_.Count + 1];  // + 1 for time
                dtTime = dtStartTime;
                
                for (int i = 0; i < xDim; i++)
                {
                    TimeSpan span = dtTime.Subtract(dt1970);
                    fArrTimeVersusValues[i, 0] = (double)span.TotalSeconds;

                    for (int j = 0; j < lstCurves_.Count; j++)
                    {
                        fArrTimeVersusValues[i, j + 1] = LAS_NULL;
                    }
                    dtTime += tmStep;
                }

                Add(sColumnHeader);
                List<List<double>> lstAll = new List<List<double>>();
                for (int i = 0; i < lstCurves_.Count; i++)
                {
                    GetCustom(dtStartTime, dtStopTime, ttType_,  /*context, */ref startTimeTicks, ref endTimeTicks, ref pressureCount, ref tbl, out start, out end, lstCurves_[i].iMsgCode.ToString());
                    List<double> lst = InterpolateTime(tbl, dtStartTime, dtStopTime, tmStep);
                    lstAll.Add(lst);
                    for (int j = 0; j < lst.Count; j++)
                    {
                        fArrTimeVersusValues[j, i + 1] = lst[j];
                    }
                }
                
                for (int i = 0; i < xDim; i++)
                {
                    string sRow = "";
                    for (int j = 0; j < lstCurves_.Count + 1; j++)
                    {
                        string s = "";
                        if (j > 0)
                            s = ((float)fArrTimeVersusValues[i, j]).ToString("0.00");
                        else  // don't format time
                        {
                            if (useUnixTime)
                                s = (fArrTimeVersusValues[i, j]).ToString();
                            else
                            {                                
                                DateTime dt = dt1970.AddSeconds(fArrTimeVersusValues[i, j]);
                                s = dt.ToString("yyyy-MM-dd_HH:mm:ss");  // use underscore to because LAS delimiter is a space
                            }
                        }
                            
                        string t = s.PadLeft(LAS_COLUMN_WIDTH, ' ');
                        sRow += t;
                    }
                    Add(sRow);
                }
            }         
            // ************************************************************************************************
            else if (ExportType_ == CLASJobInfo.EXPORT_DATA_TYPE.TVD ||
                     ExportType_ == CLASJobInfo.EXPORT_DATA_TYPE.TVD_FROM_CORRECTED_SVY)
            // ************************************************************************************************
            {
                Add(String.Empty);
                string sColumnHeader = "~Ascii  TVD";
                for (int i = 0; i < lstCurves_.Count; i++)
                {
                    string s = lstCurves_[i].sName;
                    string t = s.PadLeft(LAS_COLUMN_WIDTH, ' ');
                    sColumnHeader += t;
                }

                // *********************************
                // create 2-dimensional table 
                // *********************************
                int xDim = 0;
                float fDepth = startDepth;
                while (fDepth <= stopDepth)
                {
                    fDepth += fStep_;
                    xDim++;
                }

                // initialize table with null values except for the depth column 
                // which will use the depth steps
                float[,] fArrDepthVersusValues = new float[xDim, lstCurves_.Count + 1];  // + 1 for depth
                fDepth = startDepth;
                for (int i = 0; i < xDim; i++)
                {
                    fArrDepthVersusValues[i, 0] = fDepth;
                    for (int j = 0; j < lstCurves_.Count; j++)
                    {
                        fArrDepthVersusValues[i, j + 1] = LAS_NULL;
                    }
                    fDepth += fStep_;
                }

                Add(sColumnHeader);
                List<List<float>> lstAll = new List<List<float>>();

                //*****************************************************
                // map the tvd depth to the measured depth
                //******************************************************
                float fStartMD = 0, fStopMD = 0;
                int iClosestStartIndex = -1;
                int iStartMDIndex = -1;
                int iClosestStopIndex = -1;
                int iStopMDIndex = -1;

                if (ExportType_ == CLASJobInfo.EXPORT_DATA_TYPE.TVD)
                {
                    iStartMDIndex = BinarySearchTVD(lstAggregate, (int)((startDepth - .5) * 1000), ref iClosestStartIndex);
                    if (iStartMDIndex > -1)
                        fStartMD = (float)lstAggregate[iStartMDIndex].md;
                    else
                    {
                        if (iClosestStartIndex > -1)
                            fStartMD = (float)lstAggregate[iClosestStartIndex].md;
                    }
                        

                    iStopMDIndex = BinarySearchTVD(lstAggregate, (int)((stopDepth + .5) * 1000), ref iClosestStopIndex);
                    if (iStopMDIndex > -1)
                        fStopMD = (float)lstAggregate[iStopMDIndex].md;
                    else
                    {
                        if (iClosestStopIndex > lstAggregate.Count - 1)
                            iClosestStopIndex = lstAggregate.Count - 1;
                        fStopMD = (float)lstAggregate[iClosestStopIndex].md;
                    }
                }
                else  // for tvd from corrected surveys, there is only the start and stop
                {
                    fStartMD = (float)lstAggregate[0].md;
                    fStopMD = (float)lstAggregate[lstAggregate.Count - 1].md;
                }
                                                                   
                for (int i = 0; i < lstCurves_.Count; i++)
                {
                    List<float> lst;
                    if (lstCurves_[i].iMsgCode == (long)Command.COMMAND_BIT_DEPTH)  // handle measured depth separately
                    {
                        lst = MapTVDToMD(lstAggregate, startDepth, stopDepth, fStep_);
                    }
                    else  // all other curves get interpolated
                    {
                        GetCustom(fStartMD, fStopMD, ttType_, ref startTimeTicks, ref endTimeTicks, ref pressureCount, ref tbl, out start, out end, lstCurves_[i].iMsgCode.ToString());
                        
                        if (ExportType_ == CLASJobInfo.EXPORT_DATA_TYPE.TVD)
                            lst = InterpolateTVD(tbl, lstAggregate, startDepth, stopDepth, fStep_);
                        else
                            lst = InterpolateTVDFromCorrectedSurveys(tbl, lstAggregate, startDepth, stopDepth, fStep_);
                    }
                                          
                    lstAll.Add(lst);
                    for (int j = 0; j < lst.Count; j++)
                    {
                        fArrDepthVersusValues[j, i + 1] = lst[j];
                    }
                }

                for (int i = 0; i < xDim; i++)
                {
                    string sRow = "";
                    for (int j = 0; j < lstCurves_.Count + 1; j++)
                    {
                        string s = m_UnitLength.Convert(fArrDepthVersusValues[i, j]).ToString("0.00");
                        string t = s.PadLeft(LAS_COLUMN_WIDTH, ' ');
                        sRow += t;
                    }
                    Add(sRow);
                }

            }            
            //*******************************************************************************************
            else // measured depth
            //*******************************************************************************************
            {
                Add(String.Empty);
                string sColumnHeader = "~Ascii  Depth";                
                for (int i = 0; i < lstCurves_.Count; i++)
                {
                    string s = lstCurves_[i].sName;
                    string t = s.PadLeft(LAS_COLUMN_WIDTH, ' ');
                    sColumnHeader += t;
                }
                    
                // *********************************
                // create 2-dimensional table 
                // *********************************
                int xDim = 0;
                float fDepth = startDepth;
                while (fDepth <= stopDepth)
                {
                    fDepth += fStep_;
                    xDim++;
                }

                // initialize table with null values except for the depth column 
                // which will use the depth steps
                float[,] fArrDepthVersusValues = new float[xDim, lstCurves_.Count + 1];  // + 1 for depth
                fDepth = startDepth;
                for (int i = 0; i < xDim; i++)
                {
                    fArrDepthVersusValues[i, 0] = fDepth;
                    for (int j = 0; j < lstCurves_.Count; j++)
                    {
                        fArrDepthVersusValues[i, j + 1] = LAS_NULL;
                    }
                    fDepth += fStep_;
                }


                Add(sColumnHeader);
                List<List<float>> lstAll = new List<List<float>>();
                for (int i = 0; i < lstCurves_.Count; i++)
                {
                    List<float> lst = new List<float>();
                    if (lstCurves_[i].iMsgCode == (long)Command.COMMAND_TVD)  // handle tvd separately
                    {
                        lst = MapMDToTVD(lstAggregate, startDepth, stopDepth, fStep_);
                    }
                    else
                    {
                        GetCustom(startDepth, stopDepth, ttType_, ref startTimeTicks, ref endTimeTicks, ref pressureCount, ref tbl, out start, out end, lstCurves_[i].iMsgCode.ToString());
                        lst = Interpolate(tbl, startDepth, stopDepth, fStep_);
                    }                       
                    
                    lstAll.Add(lst);
                    for (int j = 0; j < lst.Count; j++)
                    {
                        fArrDepthVersusValues[j, i + 1] = lst[j];
                    }
                }
                                           
                for (int i = 0; i < xDim; i++)
                {
                    string sRow = "";
                    for (int j = 0; j < lstCurves_.Count + 1; j++)
                    {
                        string s = m_UnitLength.Convert(fArrDepthVersusValues[i, j]).ToString("0.00");
                        string t = s.PadLeft(LAS_COLUMN_WIDTH, ' ');
                        sRow += t;
                    }
                    Add(sRow);
                }
                
            }
            
            return Content;
        }

        private int BinarySearchTVD(List<CSurveyCalculation.MD_TO_TVD> lst, int key, ref int iRetVal_)
        {
            int min = 0;
            int max = lst.Count - 1;
            while (min <= max)
            {
                int mid = (min + max) / 2;
                if (key == (int)(lst[mid].tvd * 1000))
                {
                    iRetVal_ = ++mid;
                    return iRetVal_;
                }
                else if (key < (int)(lst[mid].tvd * 1000))
                {
                    max = mid - 1;
                    iRetVal_ = max;
                }
                else
                {
                    min = mid + 1;
                    iRetVal_ = min;
                }
            }
            return -1;
        }

        private int BinarySearchMD(List<CSurveyCalculation.MD_TO_TVD> lst, int key, ref int iRetVal_)
        {
            int min = 0;
            int max = lst.Count - 1;
            while (min <= max)
            {
                int mid = (min + max) / 2;
                if (key == (int)(lst[mid].md * 1000))
                {
                    iRetVal_ = ++mid;
                    return iRetVal_;
                }
                else if (key < (int)(lst[mid].md * 1000))
                {
                    max = mid - 1;
                    iRetVal_ = max;
                }
                else
                {
                    min = mid + 1;
                    iRetVal_ = min;
                }
            }
            return -1;
        }

        private double GetLinearInterpolationPoint(double y0, double y1, double x0, double x1, double x)
        {
            double y = y0 + (x - x0) * (y1 - y0) / (x1 - x0);
            return y;
        }

        private float GetLinearPoint(float startx, float starty, float endx, float endy, float x)
        {
            return starty + (float)(((double)x - (double)startx) * ((double)starty - (double)endy) / ((double)startx - (double)endx));
        }
        private DateTime GetLinearTime(float startx, DateTime starttime, float endx, DateTime endtime, float x)
        {
            return DateTime.FromOADate(starttime.ToOADate() + ((double)x - (double)startx) * (starttime.ToOADate() - endtime.ToOADate()) / ((double)startx - (double)endx));
        }

        public void InterpolateGammaRelatedData(DataTable tbl_, float fStep_, float fStartDepth_, float fStopDepth_)
        {
            float fStep = fStep_;
            float fStartDepth = fStartDepth_;
            float fStopDepth = fStopDepth_;
            float startx = 0.0f;
            float starty1 = 0.0f;
            float starty2 = 0.0f;
            float starty3 = 0.0f;
            DateTime starttime = new DateTime(0L);
            float fCurrentDepth = 0.0f;
            float fROP = LAS_NULL;
            float fGamma = LAS_NULL;
            float fWOB = LAS_NULL;
            float fLastWOB = 0.0f;
            float fTemperature = LAS_NULL;
            float fLastTemperature = 0.0f;

            DateTime dtLastTime = new DateTime(1970, 1, 1, 0, 0, 0);
            if (tbl_.Rows.Count > 0)
                dtLastTime = DateTime.ParseExact(tbl_.Rows[0].Field<string>("created"), "MM/dd/yyyy hh:mm:ss tt", (IFormatProvider)CultureInfo.InvariantCulture).AddSeconds((double)(0 * new Decimal(-1))); 
            for (int i = 0; i < tbl_.Rows.Count; i++)
            {                
                float fGammaSensorDepth = System.Convert.ToSingle(tbl_.Rows[i].Field<double>("depth"));                
                if ((double)fGammaSensorDepth >= 0.0)
                {                                        
                    float fBitDepth = System.Convert.ToSingle(tbl_.Rows[i].Field<double>("depth"));  // bit depth                    
                    if ((double)fGammaSensorDepth >= (double)fStartDepth &&
                        (double)fGammaSensorDepth <= (double)fStopDepth)
                    {
                        //string s = strArray[5];
                        //string str3 = strArray[0] + " " + strArray[1];
                        string sTime = tbl_.Rows[i].Field<string>("created");
                        DateTime dateTime = DateTime.ParseExact(sTime, "MM/dd/yyyy hh:mm:ss tt", (IFormatProvider)CultureInfo.InvariantCulture).AddSeconds((double)(0 * new Decimal(-1)));
                        switch (tbl_.Rows[i].Field<Int64>("messageCode"))
                        {
                            case 10113: // ROP
                                fROP = System.Convert.ToSingle(tbl_.Rows[i].Field<string>("value"));
                                break;
                            case 23:  // GAMMA
                                fGamma = System.Convert.ToSingle(tbl_.Rows[i].Field<string>("value"));
                                break;
                            case 10211:  // WOB
                                fWOB = System.Convert.ToSingle(tbl_.Rows[i].Field<string>("value"));
                                break;
                            case 12:  // temperature
                                fTemperature = System.Convert.ToSingle(tbl_.Rows[i].Field<string>("value"));
                                break;
                            default:
                                continue;
                        }
                        
                        
                        if (i < tbl_.Rows.Count - 1)  // peek at the next one
                        {
                            bool bNewTime = false;                                                        
                            DateTime datePeek = DateTime.ParseExact(tbl_.Rows[i + 1].Field<string>("created"), "MM/dd/yyyy hh:mm:ss tt", (IFormatProvider)CultureInfo.InvariantCulture);
                            if (datePeek > dateTime)
                                bNewTime = true;
                            
                            // if it's different then allow the rest of the algorithm to proceed
                            if (!bNewTime)
                                continue;
                        }
                                                                                                    
                        dtLastTime = dateTime;

                            
                        //if (this.filet == FileType.Timeline)
                        //    this.WriteLine(sw, 0.0f, fGammaValue, fROP, fBitDepth, dateTime);
                        //else 
                        if ((double)startx == 0.0)
                        {                                
                            System.Diagnostics.Debug.Print("Start: " + fGamma.ToString() + ", " + fCurrentDepth.ToString());
                            string sRow = sTime + " " + m_UnitLength.Convert(fGammaSensorDepth).ToString("0.00") + " -9999.000 " + fGamma.ToString("0.0") + " " + fWOB.ToString("0.00") + " " + m_UnitTemperature.Convert(fTemperature).ToString("0.00")  + " " + m_UnitROP.Convert(fROP).ToString("0.00");
                            Add(sRow);
                            startx = fGammaSensorDepth;
                            starty1 = fGamma;
                            starttime = dateTime;
                            starty2 = fROP;
                            starty3 = fBitDepth;
                            fLastWOB = fWOB;
                            fLastTemperature = fTemperature;
                            fCurrentDepth = fGammaSensorDepth + fStep;
                        }
                        else if ((double)fGammaSensorDepth > (double)fCurrentDepth)
                        {
                            while ((double)fGammaSensorDepth > (double)fCurrentDepth)
                            {
                                float fGammaLinearPoint = fGamma;
                                DateTime linearTime = dateTime;
                                float fROPLinearPoint = fROP;
                                float fDepthLinearPoint = fBitDepth;
                                float fWOBLinearPoint = fWOB;
                                float fTemperatureLinearPoint = fTemperature;
                                //this.WriteLine(sw, fCurrentDepth, linearPoint1, linearPoint2, linearPoint3, linearTime);
                                //System.Diagnostics.Debug.Print("Cont: " + fGamma.ToString() + ", " + fCurrentDepth.ToString());
                                if (fStep > 0)
                                {
                                    fGammaLinearPoint = this.GetLinearPoint(startx, starty1, fGammaSensorDepth, fGamma, fCurrentDepth);
                                    linearTime = this.GetLinearTime(startx, starttime, fGammaSensorDepth, dateTime, fCurrentDepth);
                                    sTime = linearTime.ToString("yyyy/MM/dd hh:mm:ss");
                                    fROPLinearPoint = this.GetLinearPoint(startx, starty2, fGammaSensorDepth, fROP, fCurrentDepth);
                                    fDepthLinearPoint = this.GetLinearPoint(startx, starty3, fGammaSensorDepth, fBitDepth, fCurrentDepth);
                                    fWOBLinearPoint = this.GetLinearPoint(startx, fLastWOB, fGammaSensorDepth, fWOB, fCurrentDepth);
                                    fTemperatureLinearPoint = this.GetLinearPoint(startx, fLastTemperature, fGammaSensorDepth, fTemperature, fCurrentDepth);
                                }

                                string sRow = sTime + " " + m_UnitLength.Convert(fDepthLinearPoint).ToString("0.00") + " -9999.000 " +  fGammaLinearPoint.ToString("0.0") + " " + fWOBLinearPoint.ToString("0.00") + " " + m_UnitTemperature.Convert(fTemperatureLinearPoint).ToString("0.00") + " " + m_UnitROP.Convert(fROPLinearPoint).ToString("0.00");
                                Add(sRow);
                                if (fStep > 0)
                                    fCurrentDepth += fStep;
                                else  // no steps.  just output what's in the database
                                    fCurrentDepth = fGammaSensorDepth;
                            }

                            startx = fGammaSensorDepth;
                            starty1 = fGamma;
                            starttime = dateTime;
                            starty2 = fROP;
                            starty3 = fBitDepth;
                            fLastWOB = fWOB;
                            fLastTemperature = fTemperature;
                        }
                    }                    
                }

            }
        }

        public void InterpolatePressureRelatedData(DataTable tbl_, float fStep_, float fStartDepth_, float fStopDepth_)
        {
            float fStep = fStep_;
            float fStartDepth = fStartDepth_;
            float fStopDepth = fStopDepth_;
            float startx = 0.0f;
            float starty1 = 0.0f;
            float starty2 = 0.0f;
            float starty3 = 0.0f;
            DateTime starttime = new DateTime(0L);
            float fCurrentDepth = 0.0f;
            float fAnnularPressure = LAS_NULL;
            float fBorePressure = LAS_NULL;

            float fVibX = LAS_NULL;
            float fLastVibX = 0.0f;

            float fVibY = LAS_NULL;
            float fLastVibY = 0.0f;

            DateTime dtLastTime = new DateTime(1970, 1, 1, 0, 0, 0);
            if (tbl_.Rows.Count > 0)
                dtLastTime = DateTime.Parse(tbl_.Rows[0].Field<string>("created"), (IFormatProvider)CultureInfo.InvariantCulture).AddSeconds((double)(0 * new Decimal(-1)));
            for (int i = 0; i < tbl_.Rows.Count; i++)
            {                
                float fDepth = System.Convert.ToSingle(tbl_.Rows[i].Field<double>("depth"));
                if ((double)fDepth >= 0.0)
                {
                    float fBitDepth = System.Convert.ToSingle(tbl_.Rows[i].Field<double>("depth"));  // bit depth
                    if ((double)fDepth >= (double)fStartDepth &&
                        (double)fDepth <= (double)fStopDepth)
                    {                        
                        string sTime = tbl_.Rows[i].Field<string>("created");
                        DateTime dateTime = DateTime.Parse(sTime, (IFormatProvider)CultureInfo.InvariantCulture).AddSeconds((double)(0 * new Decimal(-1)));
                        switch (tbl_.Rows[i].Field<Int64>("messageCode"))
                        {
                            case 24:
                                fBorePressure = System.Convert.ToSingle(tbl_.Rows[i].Field<string>("value"));
                                break;
                            case 25:
                                fAnnularPressure = System.Convert.ToSingle(tbl_.Rows[i].Field<string>("value"));
                                break;                           
                            case 37:
                                fVibX = System.Convert.ToSingle(tbl_.Rows[i].Field<string>("value"));
                                break;
                            case 38:
                                fVibY = System.Convert.ToSingle(tbl_.Rows[i].Field<string>("value"));
                                break;
                            default:
                                continue;
                        }


                        if (i < tbl_.Rows.Count - 1)  // peek at the next one
                        {
                            bool bNewTime = false;
                            DateTime datePeek = DateTime.ParseExact(tbl_.Rows[i + 1].Field<string>("created"), "MM/dd/yyyy hh:mm:ss tt", (IFormatProvider)CultureInfo.InvariantCulture);
                            if (datePeek > dateTime)
                                bNewTime = true;

                            // if it's different then allow the rest of the algorithm to proceed
                            if (!bNewTime)
                                continue;
                        }

                        dtLastTime = dateTime;


                        //if (this.filet == FileType.Timeline)
                        //    this.WriteLine(sw, 0.0f, fGammaValue, fAnnularPressure, fBitDepth, dateTime);
                        //else 
                        if ((double)startx == 0.0)
                        {
                            System.Diagnostics.Debug.Print("Start: " + fBorePressure.ToString() + ", " + fCurrentDepth.ToString());
                            string sRow = sTime + " " + m_UnitLength.Convert(fDepth).ToString("0.00") + " -9999.000 " + m_UnitPressure.Convert(fBorePressure).ToString("0.0") + " " + m_UnitPressure.Convert(fAnnularPressure).ToString("0.00") + " " + fVibX.ToString("0.00") + " " + fVibY.ToString("0.00");
                            Add(sRow);
                            startx = fDepth;
                            starty1 = fBorePressure;
                            starttime = dateTime;
                            starty2 = fAnnularPressure;
                            starty3 = fBitDepth;
                            fLastVibX = fVibX;
                            fLastVibY = fVibY;
                            fCurrentDepth = fDepth + fStep;
                        }
                        else if ((double)fDepth > (double)fCurrentDepth)
                        {
                            while ((double)fDepth > (double)fCurrentDepth)
                            {
                                float fBorePressureLinearPoint = fBorePressure;
                                DateTime linearTime = dateTime;
                                float fAnnularPressureLinearPoint = fAnnularPressure;
                                float fDepthLinearPoint = fBitDepth;
                                float fVibXLinearPoint = fVibX;
                                float fVibYLinearPoint = fVibY;
                                //this.WriteLine(sw, fCurrentDepth, linearPoint1, linearPoint2, linearPoint3, linearTime);
                                //System.Diagnostics.Debug.Print("Cont: " + fBorePressure.ToString() + ", " + fCurrentDepth.ToString());
                                if (fStep > 0)
                                {
                                    fBorePressureLinearPoint = this.GetLinearPoint(startx, starty1, fDepth, fBorePressure, fCurrentDepth);
                                    linearTime = this.GetLinearTime(startx, starttime, fDepth, dateTime, fCurrentDepth);
                                    sTime = linearTime.ToString("yyyy/MM/dd hh:mm:ss");
                                    fAnnularPressureLinearPoint = this.GetLinearPoint(startx, starty2, fDepth, fAnnularPressure, fCurrentDepth);
                                    fDepthLinearPoint = this.GetLinearPoint(startx, starty3, fDepth, fBitDepth, fCurrentDepth);
                                    fVibXLinearPoint = this.GetLinearPoint(startx, fLastVibX, fDepth, fVibX, fCurrentDepth);
                                    fVibYLinearPoint = this.GetLinearPoint(startx, fLastVibY, fDepth, fVibY, fCurrentDepth);
                                }

                                string sRow = sTime + " " + m_UnitLength.Convert(fDepthLinearPoint).ToString("0.00") + " -9999.000 " + m_UnitPressure.Convert(fBorePressureLinearPoint).ToString("0.0") + " " + m_UnitPressure.Convert(fAnnularPressureLinearPoint).ToString("0.00") + " " + fVibXLinearPoint.ToString("0.00") + " " + fVibYLinearPoint.ToString("0.00");
                                Add(sRow);
                                if (fStep > 0)
                                    fCurrentDepth += fStep;
                                else  // no steps.  just output what's in the database
                                    fCurrentDepth = fDepth;
                            }

                            startx = fDepth;
                            starty1 = fBorePressure;
                            starttime = dateTime;
                            starty2 = fAnnularPressure;
                            starty3 = fBitDepth;
                            fLastVibX = fVibX;
                            fLastVibY = fVibY;
                        }
                    }
                }

            }
        }

        public bool BinarySearch(int[] inputArray, int key, ref int index, ref int min_, ref int max_)
        {
            int min = 0;
            int max = inputArray.Length - 1;
            while (min <= max)
            {
                int mid = (min + max) / 2;
                if (key == inputArray[mid])
                {
                    index = ++mid;
                    return true;
                }
                else if (key < inputArray[mid])
                {
                    max_ = max = mid - 1;
                    //index = max;
                }
                else
                {
                    min_ = min = mid + 1;
                    //index = min;
                }
            }
            return false;
        }

        public List<double> InterpolateTime(DataTable tbl_, DateTime dtStartTime_, DateTime dtStopTime_, TimeSpan tsStep_)
        {
            List<double> lstRet = new List<double>();
            float fVal0 = 0.0f;
            float fVal1 = 0.0f;            

            DateTime dtCurrentTime = dtStartTime_;

            DateTime dtNullValuesBeforeTime = dtStartTime_;
            if (tbl_.Rows.Count > 0)
            {
                dtNullValuesBeforeTime = System.Convert.ToDateTime(tbl_.Rows[0].Field<string>("created"));
            }

            DateTime dtTime0;
            DateTime dtTime1;
            dtCurrentTime = dtStartTime_;
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0);
            DateTime dt1971 = new DateTime(1971, 1, 1, 0, 0, 0);
            while (dtCurrentTime <= dtStopTime_)
            {
                double fInterpolatedVal = LAS_NULL;
                if (dtCurrentTime < dtNullValuesBeforeTime)
                {
                    // do nothing. default to null value               
                }
                else
                {
                    // find the lower boundary
                    dtTime0 = dt1970;
                    for (int i = 0; i < tbl_.Rows.Count; i++)  // find lower bound
                    {
                        if (dtCurrentTime >= System.Convert.ToDateTime(tbl_.Rows[i].Field<string>("created")))
                        {
                            //iIndex = i + 1;  // gotta start from at least one more next time
                            dtTime0 = System.Convert.ToDateTime(tbl_.Rows[i].Field<string>("created"));
                            string sVal0 = tbl_.Rows[i].Field<string>("value");
                            sVal0.Replace('f', ' ');
                            sVal0.Trim();
                            fVal0 = System.Convert.ToSingle(sVal0);
                            break;
                        }
                    }

                    // find the upper boundary
                    dtTime1 = dt1970;
                    for (int j = 0; j < tbl_.Rows.Count; j++)  // find upper bound
                    {
                        if (dtCurrentTime <= System.Convert.ToDateTime(tbl_.Rows[j].Field<string>("created")))
                        {
                            dtTime1 = System.Convert.ToDateTime(tbl_.Rows[j].Field<string>("created"));
                            string sVal1 = tbl_.Rows[j].Field<string>("value");
                            sVal1.Replace('f', ' ');
                            sVal1.Trim();
                            fVal1 = System.Convert.ToSingle(sVal1);

                            break;
                        }
                    }

                    if (dtTime1 < dt1971 ||
                        (dtTime0 <= dt1971 && dtTime1 <= dt1971))
                        fInterpolatedVal = LAS_NULL;
                    else
                    {
                        double dblTime0 = dtTime0.Subtract(dt1970).TotalSeconds;
                        double dblTime1 = dtTime1.Subtract(dt1970).TotalSeconds;
                        double dblCurrentVal = dtCurrentTime.Subtract(dt1970).TotalSeconds;
                        fInterpolatedVal = GetLinearInterpolationPoint(fVal0, fVal1, dblTime0, dblTime1, dblCurrentVal);
                    }                        
                }

                lstRet.Add(fInterpolatedVal);
                dtCurrentTime += tsStep_;
            }

            return lstRet;
        }

        public List<float> Interpolate(DataTable tbl_, float fStartDepth_, float fStopDepth_, float fStep_)
        {
            List<float> lstRet = new List<float>();
            double fVal0 = 0.0f;
            double fVal1 = 0.0f;
            
            //float fDepth1 = 0.0f;
            
            float fCurrentDepth = fStartDepth_;

            float fNullValuesBeforeDepth = 0.0f;
            if (tbl_.Rows.Count > 0)
            {
                fNullValuesBeforeDepth = System.Convert.ToSingle(tbl_.Rows[0].Field<double>("depth"));
            }

            double fDepth0 = 0.0f;
            double fDepth1 = 0.0f;            
            fCurrentDepth = fStartDepth_;            
            while (fCurrentDepth <= fStopDepth_)           
            {
                double fInterpolatedVal = LAS_NULL;
                if (fCurrentDepth < fNullValuesBeforeDepth)
                {
                    // do nothing. default to null value               
                }
                else
                {
                    // find the lower boundary
                    for (int i = 0; i < tbl_.Rows.Count; i++)  // find lower bound
                    {
                        if (fCurrentDepth >= System.Convert.ToSingle(tbl_.Rows[i].Field<double>("depth")))
                        {
                            //iIndex = i + 1;  // gotta start from at least one more next time
                            fDepth0 = System.Convert.ToSingle(tbl_.Rows[i].Field<double>("depth"));
                            string sVal0 = tbl_.Rows[i].Field<string>("value");
                            sVal0.Replace('f', ' ');
                            sVal0.Trim();
                            
                            bool bParse = double.TryParse(sVal0, out fVal0);
                            if (!bParse)
                            {
                                if (sVal0 == "G")
                                    fVal0 = 0;
                                else if (sVal0 == "M")
                                    fVal0 = 1;
                                else
                                    fVal0 = LAS_NULL;
                            }
                                
                            break;
                        }
                    }

                    // find the upper boundary
                    fDepth1 = CCommonTypes.BAD_VALUE;
                    for (int j = 0; j < tbl_.Rows.Count; j++)  // find upper bound
                    {
                        if (fCurrentDepth <= System.Convert.ToSingle(tbl_.Rows[j].Field<double>("depth")))
                        {
                            fDepth1 = System.Convert.ToSingle(tbl_.Rows[j].Field<double>("depth"));
                            string sVal1 = tbl_.Rows[j].Field<string>("value");
                            sVal1.Replace('f', ' ');
                            sVal1.Trim();

                            bool bParse = double.TryParse(sVal1, out fVal1);
                            if (!bParse)
                            {
                                if (sVal1 == "G")
                                    fVal0 = 0;
                                else if (sVal1 == "M")
                                    fVal0 = 1;
                                else
                                    fVal1 = LAS_NULL;
                            }
                                

                            break;
                        }
                    }
                    
                    if (fDepth1 < CCommonTypes.BAD_VALUE + 1 ||
                        (fDepth0 <= 0 && fDepth1 <= 0))
                        fInterpolatedVal = LAS_NULL;
                    else
                        fInterpolatedVal = GetLinearInterpolationPoint(fVal0, fVal1, fDepth0, fDepth1, fCurrentDepth);
                }
                    
                lstRet.Add((float)fInterpolatedVal);
                fCurrentDepth += fStep_;
            }            

            return lstRet;
        }

        public List<float> MapTVDToMD(List<CSurveyCalculation.MD_TO_TVD> lst_, float fStartDepth_, float fStopDepth_, float fStep_)
        {
            List<float> lstRet = new List<float>();

            float fCurrentTVD = fStartDepth_;
            while (fCurrentTVD <= fStopDepth_)
            {
                float fMD = 0;
                int iClosestIndex = -1;
                int iMDIndex = BinarySearchTVD(lst_, (int)(fCurrentTVD * 1000), ref iClosestIndex);
                if (iMDIndex > -1)
                    fMD = (float)lst_[iMDIndex].md;
                else
                {
                    if (iClosestIndex < lst_.Count)
                        fMD = (float)lst_[iClosestIndex].md;
                    else
                        fMD = LAS_NULL;
                }

                lstRet.Add(fMD);
                fCurrentTVD += fStep_;
            }

            return lstRet;
        }

        public List<float> MapMDToTVD(List<CSurveyCalculation.MD_TO_TVD> lst_, float fStartDepth_, float fStopDepth_, float fStep_)
        {
            List<float> lstRet = new List<float>();

            float fCurrentMD = fStartDepth_;
            while (fCurrentMD <= fStopDepth_)
            {
                float fTVD = 0;
                int iClosestIndex = -1;
                int iMDIndex = BinarySearchMD(lst_, (int)(fCurrentMD * 1000), ref iClosestIndex);
                if (iMDIndex > -1)
                    fTVD = (float)lst_[iMDIndex].tvd;
                else
                {
                    if (iClosestIndex < lst_.Count)
                        fTVD = (float)lst_[iClosestIndex].tvd;
                    else
                        fTVD = LAS_NULL;
                }

                lstRet.Add(fTVD);
                fCurrentMD += fStep_;
            }

            return lstRet;
        }

        public List<float> InterpolateTVD(DataTable tbl_, List<CSurveyCalculation.MD_TO_TVD> lst_, float fStartDepth_, float fStopDepth_, float fStep_)
        {
            List<float> lstRet = new List<float>();
            double fVal0 = 0.0f;
            double fVal1 = 0.0f;
                    
            float fNullValuesBeforeDepth = 0.0f;
            if (tbl_.Rows.Count > 0)
            {
                fNullValuesBeforeDepth = System.Convert.ToSingle(tbl_.Rows[0].Field<double>("depth"));
            }

            double fDepth0 = 0.0f;
            double fDepth1 = 0.0f;
            float fCurrentTVD = fStartDepth_;
            while (fCurrentTVD <= fStopDepth_)
            {
                double fInterpolatedVal = LAS_NULL;

                float fMD = 0;
                int iClosestIndex = -1;
                int iMDIndex = BinarySearchTVD(lst_, (int)(fCurrentTVD * 1000), ref iClosestIndex);
                if (iMDIndex > -1)
                    fMD = (float)lst_[iMDIndex].md;
                else
                {
                    if (iClosestIndex < 0)
                        fMD = (float)lst_[0].md;
                    else if (iClosestIndex > -1 && iClosestIndex < lst_.Count)
                        fMD = (float)lst_[iClosestIndex].md;
                    else
                        fMD = (float)lst_[lst_.Count - 1].md;
                }
                    

                if (fMD < fNullValuesBeforeDepth)
                {
                    // do nothing. default to null value               
                }
                else
                {
                    // find the lower boundary
                    for (int i = 0; i < tbl_.Rows.Count; i++)  // find lower bound
                    {
                        if (fMD >= System.Convert.ToSingle(tbl_.Rows[i].Field<double>("depth")))
                        {
                            //iIndex = i + 1;  // gotta start from at least one more next time
                            fDepth0 = System.Convert.ToSingle(tbl_.Rows[i].Field<double>("depth"));
                            string sVal0 = tbl_.Rows[i].Field<string>("value");
                            sVal0.Replace('f', ' ');
                            sVal0.Trim();
                            fVal0 = System.Convert.ToSingle(sVal0);
                            break;
                        }
                    }

                    // find the upper boundary
                    fDepth1 = CCommonTypes.BAD_VALUE;
                    for (int j = 0; j < tbl_.Rows.Count; j++)  // find upper bound
                    {
                        if (fMD <= System.Convert.ToSingle(tbl_.Rows[j].Field<double>("depth")))
                        {
                            fDepth1 = System.Convert.ToSingle(tbl_.Rows[j].Field<double>("depth"));
                            string sVal1 = tbl_.Rows[j].Field<string>("value");
                            sVal1.Replace('f', ' ');
                            sVal1.Trim();
                            fVal1 = System.Convert.ToSingle(sVal1);

                            break;
                        }
                    }

                    if (fDepth1 < CCommonTypes.BAD_VALUE + 1 ||
                        (fDepth0 <= 0 && fDepth1 <= 0))
                        fInterpolatedVal = LAS_NULL;
                    else
                        fInterpolatedVal = GetLinearInterpolationPoint(fVal0, fVal1, fDepth0, fDepth1, fMD);
                }

                lstRet.Add((float)fInterpolatedVal);
                fCurrentTVD += fStep_;
            }

            return lstRet;
        }

        public List<float> InterpolateTVDFromCorrectedSurveys(DataTable tbl_, List<CSurveyCalculation.MD_TO_TVD> lst_, float fTVDStartDepth_, float fTVDStopDepth_, float fStep_)
        {
            List<float> lstRet = new List<float>();
                     
            float fNullValuesBeforeDepth = 0.0f;
            if (tbl_.Rows.Count > 0)
            {
                fNullValuesBeforeDepth = System.Convert.ToSingle(tbl_.Rows[0].Field<double>("depth"));
            }

            double fTVDDepth0 = 0.0f;
            double fTVDDepth1 = 0.0f;
            float fCurrentTVD = fTVDStartDepth_;
            while (fCurrentTVD <= fTVDStopDepth_)
            {
               
                fTVDDepth0 = fCurrentTVD;
                fTVDDepth1 = fCurrentTVD + fStep_;

                double fMDDepth0 = 0.0f;
                double fMDDepth1 = 0.0f;
                int iTVDStartIndex = -1;
                int iTVDStopIndex = -1;

                // find the tvd range
                // find the lower boundary
                for (int i = 0; i < lst_.Count; i++)
                {                                                            
                    if (fCurrentTVD >= lst_[i].tvd)
                    {
                        iTVDStartIndex = i;
                    }
                }

                // find the upper boundary
                for (int i = lst_.Count - 1; i >= 0; i--)
                {                                        
                    if (fCurrentTVD < lst_[i].tvd)
                    {
                        iTVDStopIndex = i;
                    }
                }

                double fInterpolatedVal = LAS_NULL;

                //double fValueInterpolated = LAS_NULL;
                if (iTVDStartIndex < 0 || iTVDStopIndex < 0)  // no interval was found
                {
                    // assign LAS NULL value                    
                }
                else  // get the measured depth start and stop
                {
                    fMDDepth0 = lst_[iTVDStartIndex].md;
                    fMDDepth1 = lst_[iTVDStopIndex].md;

                    // do linear interpolation
                    fTVDDepth0 = lst_[iTVDStartIndex].tvd;
                    fTVDDepth1 = lst_[iTVDStopIndex].tvd;
                    double fMDInterpolated = GetLinearInterpolationPoint(fMDDepth0, fMDDepth1, fTVDDepth0, fTVDDepth1, fCurrentTVD);

                    int iValueStartIndex = -1;
                    int iValueStopIndex = -1;
                    // now map the measured depth to the curve list
                    for (int j = 0; j < tbl_.Rows.Count; j++)
                    {
                        if (fMDInterpolated >= System.Convert.ToSingle(tbl_.Rows[j].Field<double>("depth")))
                        {
                            iValueStartIndex = j;
                        }                        
                    }

                    for (int j = 0; j < tbl_.Rows.Count; j++)
                    {
                        if (fMDInterpolated < System.Convert.ToSingle(tbl_.Rows[j].Field<double>("depth")))
                        {
                            iValueStopIndex = j;
                        }                        
                    }

                    if (iValueStopIndex > -1 && iValueStartIndex > -1)  // do the linear interpolation
                    {
                        string sVal0 = tbl_.Rows[iValueStartIndex].Field<string>("value");
                        sVal0.Replace('f', ' ');
                        sVal0.Trim();
                        double fVal0 = System.Convert.ToSingle(sVal0);

                        string sVal1 = tbl_.Rows[iValueStopIndex].Field<string>("value");
                        sVal1.Replace('f', ' ');
                        sVal1.Trim();
                        double fVal1 = System.Convert.ToSingle(sVal1);

                        fInterpolatedVal = GetLinearInterpolationPoint(fVal0, fVal1, fMDDepth0, fMDDepth1, fMDInterpolated);
                    }
                }
                
                lstRet.Add((float)fInterpolatedVal);
                fCurrentTVD += fStep_;
            }

            return lstRet;
        }

        public void InterpolateData(List<CURVE_INFO> lstCurves_, DataTable tbl_, float fStep_, float fStartDepth_, float fStopDepth_)
        {
            float fStep = fStep_;
            float fStartDepth = fStartDepth_;
            float fStopDepth = fStopDepth_;
            float startx = 0.0f;
            
            
            float starty3 = 0.0f;
            DateTime starttime = new DateTime(0L);
            float fCurrentDepth = 0.0f;

            float starty2 = 0.0f;
            float fAnnularPressure = LAS_NULL;

            float starty1 = 0.0f;
            float fBorePressure = LAS_NULL;

            float fVibX = LAS_NULL;
            float fLastVibX = 0.0f;

            float fVibY = LAS_NULL;
            float fLastVibY = 0.0f;

            CUSTOM_DPOINT[] fArr = new CUSTOM_DPOINT[lstCurves_.Count];
            for (int i = 0; i < lstCurves_.Count; i++)
            {
                fArr[i].fCurrVal = LAS_NULL;
                fArr[i].fLastVal = LAS_NULL;
                fArr[i].iMsgCode = lstCurves_[i].iMsgCode;
            }


            DateTime dtLastTime = new DateTime(1970, 1, 1, 0, 0, 0);
            if (tbl_.Rows.Count > 0)
                dtLastTime = DateTime.Parse(tbl_.Rows[0].Field<string>("created"), (IFormatProvider)CultureInfo.InvariantCulture).AddSeconds((double)(0 * new Decimal(-1)));

            for (int i = 0; i < tbl_.Rows.Count; i++)
            {

                float fDepth = System.Convert.ToSingle(tbl_.Rows[i].Field<double>("depth"));
                if ((double)fDepth >= 0.0)
                {
                    float fBitDepth = System.Convert.ToSingle(tbl_.Rows[i].Field<double>("depth"));  // bit depth
                    if ((double)fDepth >= (double)fStartDepth &&
                        (double)fDepth <= (double)fStopDepth)
                    {
                        string sTime = tbl_.Rows[i].Field<string>("created");
                        DateTime dateTime = DateTime.Parse(sTime, (IFormatProvider)CultureInfo.InvariantCulture).AddSeconds((double)(0 * new Decimal(-1)));
                        for (int j = 0; j < fArr.Length; j++)
                        {
                            if (tbl_.Rows[i].Field<Int64>("messageCode") == fArr[j].iMsgCode)
                            {
                                fArr[j].fCurrVal = System.Convert.ToSingle(tbl_.Rows[i].Field<string>("value"));
                                if (fArr[j].fLastVal < LAS_NULL + 1.0f)  // initialize the last value to the current value since there was no previous last value
                                    fArr[j].fLastVal = fArr[j].fCurrVal;
                                break;
                            }
                        }
                        

                        if (i < tbl_.Rows.Count - 1)  // peek at the next one
                        {
                            bool bNewTime = false;
                            DateTime datePeek = DateTime.Parse(tbl_.Rows[i + 1].Field<string>("created"), (IFormatProvider)CultureInfo.InvariantCulture);
                            if (datePeek > dateTime)
                                bNewTime = true;

                            // if it's different then allow the rest of the algorithm to proceed
                            if (!bNewTime)
                                continue;
                        }

                        dtLastTime = dateTime;
                         
                        if ((double)startx == 0.0)
                        {
                            //stem.Diagnostics.Debug.Print("Start: " + fBorePressure.ToString() + ", " + fCurrentDepth.ToString());
                            //string sRow = sTime + " " + m_UnitLength.Convert(fDepth).ToString("0.00") + " -9999.000 " + m_UnitPressure.Convert(fBorePressure).ToString("0.0") + " " + m_UnitPressure.Convert(fAnnularPressure).ToString("0.00") + " " + fVibX.ToString("0.00") + " " + fVibY.ToString("0.00");
                            string sRow = sTime + " " + m_UnitLength.Convert(fDepth).ToString("0.00") + " ";
                            sRow += m_UnitLength.Convert(fArr[i].fCurrVal).ToString("0.00");
                            Add(sRow);
                            startx = fDepth;                            
                            starttime = dateTime;
                            starty1 = fBorePressure;
                            starty2 = fAnnularPressure;
                            starty3 = fBitDepth;
                            fLastVibX = fVibX;
                            fLastVibY = fVibY;

                            for (int j = 0; j < fArr.Length; j++)
                            {
                                if (tbl_.Rows[i].Field<Int64>("messageCode") == fArr[j].iMsgCode)
                                {
                                    string s = tbl_.Rows[i].Field<string>("value").ToString();
                                    s = s.Replace('f', ' ');
                                    s = s.Trim();
                                    fArr[j].fCurrVal = System.Convert.ToSingle(s);
                                    break;
                                }
                            }

                            fCurrentDepth = fDepth + fStep;
                        }
                        else if ((double)fDepth > (double)fCurrentDepth)
                        {
                            while ((double)fDepth > (double)fCurrentDepth)
                            {
                                float fBorePressureLinearPoint = fBorePressure;
                                DateTime linearTime = dateTime;
                                float fAnnularPressureLinearPoint = fAnnularPressure;
                                float fDepthLinearPoint = fBitDepth;
                                float fVibXLinearPoint = fVibX;
                                float fVibYLinearPoint = fVibY;
                                //this.WriteLine(sw, fCurrentDepth, linearPoint1, linearPoint2, linearPoint3, linearTime);
                                //System.Diagnostics.Debug.Print("Cont: " + fBorePressure.ToString() + ", " + fCurrentDepth.ToString());
                                if (fStep > 0)
                                {
                                    fBorePressureLinearPoint = this.GetLinearPoint(startx, starty1, fDepth, fBorePressure, fCurrentDepth);
                                    linearTime = this.GetLinearTime(startx, starttime, fDepth, dateTime, fCurrentDepth);
                                    sTime = linearTime.ToString("yyyy/MM/dd hh:mm:ss");
                                    fAnnularPressureLinearPoint = this.GetLinearPoint(startx, starty2, fDepth, fAnnularPressure, fCurrentDepth);
                                    fDepthLinearPoint = this.GetLinearPoint(startx, starty3, fDepth, fBitDepth, fCurrentDepth);
                                    fVibXLinearPoint = this.GetLinearPoint(startx, fLastVibX, fDepth, fVibX, fCurrentDepth);
                                    fVibYLinearPoint = this.GetLinearPoint(startx, fLastVibY, fDepth, fVibY, fCurrentDepth);

                                    for (int j = 0; j < fArr.Length; j++)
                                    {
                                        //if (tbl_.Rows[i].Field<Int64>("messageCode") == fArr[j].iMsgCode)
                                        //{
                                            fArr[j].fLinearPt = this.GetLinearPoint(startx, fArr[j].fLastVal, fDepth, fArr[j].fCurrVal, fCurrentDepth);
                                            //break;
                                        //}
                                    }                                    
                                }

                                //string sRow = sTime + " " + m_UnitLength.Convert(fDepthLinearPoint).ToString("0.00") + " -9999.000 " + m_UnitPressure.Convert(fBorePressureLinearPoint).ToString("0.0") + " " + m_UnitPressure.Convert(fAnnularPressureLinearPoint).ToString("0.00") + " " + fVibXLinearPoint.ToString("0.00") + " " + fVibYLinearPoint.ToString("0.00");
                                string sRow = sTime + " " + m_UnitLength.Convert(fDepthLinearPoint).ToString("0.00") + " "; 
                                for (int k = 0; k < lstCurves_.Count; k++)
                                    sRow += m_UnitLength.Convert(fArr[k].fLinearPt).ToString("0.00") + " ";
                                
                                Add(sRow);
                                if (fStep > 0)
                                    fCurrentDepth += fStep;
                                else  // no steps.  just output what's in the database
                                    fCurrentDepth = fDepth;
                            }

                            startx = fDepth;
                            starty1 = fBorePressure;
                            starttime = dateTime;
                            starty2 = fAnnularPressure;
                            starty3 = fBitDepth;
                            fLastVibX = fVibX;
                            fLastVibY = fVibY;
                        }
                    }
                }

            }
        }

        //public void Extrapolate(StreamReader sr, float fStep_, float fStartDepth_, float fStopDepth_)
        //{
        //    float fStep = fStep_;
        //    float fStartDepth = fStartDepth_;
        //    float fStopDepth = fStopDepth_;
        //    float startx = 0.0f;
        //    float starty1 = 0.0f;
        //    float starty2 = 0.0f;
        //    float starty3 = 0.0f;
        //    DateTime starttime = new DateTime(0L);
        //    float fCurrentDepth = 0.0f;
        //    while (!sr.EndOfStream)
        //    {
        //        string[] strArray = sr.ReadLine().Split(' ');
        //        if (strArray.Length < 19)
        //            continue;
        //        float fGammaSensorDepth = float.Parse(strArray[9]);  // gamma sensor depth
        //        if ((double)fGammaSensorDepth >= 0.0)
        //        {
        //            float fROP = float.Parse(strArray[14]);  // rop
        //            if ((double)fROP >= 0.0)
        //            {
        //                float fBitDepth = float.Parse(strArray[19]);  // bit depth
        //                if ((double)fGammaSensorDepth >= (double)fStartDepth &&
        //                    (double)fGammaSensorDepth <= (double)fStopDepth)
        //                {
        //                    string s = strArray[5];
        //                    string str3 = strArray[0] + " " + strArray[1];
        //                    DateTime dateTime = DateTime.ParseExact(str3.Substring(1, str3.Length - 2), "yy-MMM-dd HH:mm:ss", (IFormatProvider)CultureInfo.InvariantCulture).AddSeconds((double)(0 * new Decimal(-1)));
        //                    float fGammaValue = float.Parse(s);
        //                    //if (this.filet == FileType.Timeline)
        //                    //    this.WriteLine(sw, 0.0f, fGammaValue, fROP, fBitDepth, dateTime);
        //                    //else 
        //                    if ((double)startx == 0.0)
        //                    {
        //                        //this.WriteLine(sw, fGammaSensorDepth, fGammaValue, fROP, fBitDepth, dateTime);
        //                        System.Diagnostics.Debug.Print("Start: " + fGammaValue.ToString() + ", " + fCurrentDepth.ToString());
        //                        startx = fGammaSensorDepth;
        //                        starty1 = fGammaValue;
        //                        starttime = dateTime;
        //                        starty2 = fROP;
        //                        starty3 = fBitDepth;
        //                        fCurrentDepth = fGammaSensorDepth + fStep;
        //                    }
        //                    else if ((double)fGammaSensorDepth > (double)fCurrentDepth)
        //                    {
        //                        while ((double)fGammaSensorDepth > (double)fCurrentDepth)
        //                        {
        //                            float linearPoint1 = this.GetLinearPoint(startx, starty1, fGammaSensorDepth, fGammaValue, fCurrentDepth);
        //                            DateTime linearTime = this.GetLinearTime(startx, starttime, fGammaSensorDepth, dateTime, fCurrentDepth);
        //                            float linearPoint2 = this.GetLinearPoint(startx, starty2, fGammaSensorDepth, fROP, fCurrentDepth);
        //                            float linearPoint3 = this.GetLinearPoint(startx, starty3, fGammaSensorDepth, fBitDepth, fCurrentDepth);
        //                            //this.WriteLine(sw, fCurrentDepth, linearPoint1, linearPoint2, linearPoint3, linearTime);
        //                            System.Diagnostics.Debug.Print("Start: " + fGammaValue.ToString() + ", " + fCurrentDepth.ToString());
        //                            fCurrentDepth += fStep;
        //                        }
        //                        startx = fGammaSensorDepth;
        //                        starty1 = fGammaValue;
        //                        starttime = dateTime;
        //                        starty2 = fROP;
        //                        starty3 = fBitDepth;
        //                    }
        //                }
        //            }
        //        }

        //    }
        //}

        private static void GetPressures(float fStartDepth_, float fStopDepth_, CCommonTypes.TELEMETRY_TYPE ttType_, /* CDrillContext context,*/ ref long startTimeTicks, ref long endTimeTicks, ref int pressureCount, ref DataTable tblPressure_, out DateTime start, out DateTime end)
        {
            List<CURVE_INFO> lst = new List<CURVE_INFO>() { new CURVE_INFO {sName = "BPress", iMsgCode = 24}, 
                                                            new CURVE_INFO {sName = "APress", iMsgCode = 25}, 
                                                            new CURVE_INFO {sName = "Vib X", iMsgCode = 37 }, 
                                                            new CURVE_INFO {sName = "Vib Y", iMsgCode = 38 } };  // bore pressure, annular pressure, vib x, vib y            
            CLogDataLayer log = new CLogDataLayer(ref m_dbCnn);
            //tblPressure_ = log.Get(lst, fStartDepth_, fStopDepth_, ttType_);
            if (tblPressure_.Rows.Count > 0)
            {
                start = System.Convert.ToDateTime(tblPressure_.Rows[0].Field<string>("created"));
                end = System.Convert.ToDateTime(tblPressure_.Rows[tblPressure_.Rows.Count - 1].Field<string>("created"));
                startTimeTicks = CDateTime.UnixTicksFromDate(start);
                endTimeTicks = CDateTime.UnixTicksFromDate(end);
            }
            else
                CLogASCIIStandard.SetUnusedParametersToNull(ref startTimeTicks, ref endTimeTicks, out start, out end);           
        }

        private static void GetCustom(float fStartDepth_, float fStopDepth_, CCommonTypes.TELEMETRY_TYPE_FOR_LAS ttType_, /* CDrillContext context,*/ ref long startTimeTicks, ref long endTimeTicks, ref int pressureCount, ref DataTable tblCustom_, out DateTime start, out DateTime end, string sMessageCode_)
        {                      
            CLogDataLayer log = new CLogDataLayer(ref m_dbCnn);
            tblCustom_ = log.Get(sMessageCode_, fStartDepth_, fStopDepth_, (CCommonTypes.TELEMETRY_TYPE)ttType_);
            if (tblCustom_.Rows.Count > 0)
            {
                start = System.Convert.ToDateTime(tblCustom_.Rows[0].Field<string>("created"));
                end = System.Convert.ToDateTime(tblCustom_.Rows[tblCustom_.Rows.Count - 1].Field<string>("created"));
                startTimeTicks = CDateTime.UnixTicksFromDate(start);
                endTimeTicks = CDateTime.UnixTicksFromDate(end);
            }
            else
                CLogASCIIStandard.SetUnusedParametersToNull(ref startTimeTicks, ref endTimeTicks, out start, out end);
        }

        private static void GetCustom(DateTime dtStartTime_, DateTime dtStopTime_, CCommonTypes.TELEMETRY_TYPE_FOR_LAS ttType_, /* CDrillContext context,*/ ref long startTimeTicks, ref long endTimeTicks, ref int pressureCount, ref DataTable tblCustom_, out DateTime start, out DateTime end, string sMessageCode_)
        {
            CLogDataLayer log = new CLogDataLayer(ref m_dbCnn);
            tblCustom_ = log.Get(sMessageCode_, dtStartTime_, dtStopTime_, (CCommonTypes.TELEMETRY_TYPE)ttType_);
            if (tblCustom_.Rows.Count > 0)
            {
                start = System.Convert.ToDateTime(tblCustom_.Rows[0].Field<string>("created"));
                end = System.Convert.ToDateTime(tblCustom_.Rows[tblCustom_.Rows.Count - 1].Field<string>("created"));
                startTimeTicks = CDateTime.UnixTicksFromDate(start);
                endTimeTicks = CDateTime.UnixTicksFromDate(end);
            }
            else
                CLogASCIIStandard.SetUnusedParametersToNull(ref startTimeTicks, ref endTimeTicks, out start, out end);
        }

        private static void SetUnusedParametersToNull(ref long startTimeTicks, ref long endTimeTicks, out DateTime start, out DateTime end)
        {
            start = new DateTime();
            end = new DateTime();
            startTimeTicks = 0L;
            endTimeTicks = 0L;
        }

        // TODO/FIX
        private void GetGammas(float fStartDepth_, float fStopDepth_, CCommonTypes.TELEMETRY_TYPE ttType_, /*CDrillContext context,*/ ref long startTimeTicks, ref long endTimeTicks, ref int gammaCount, ref DataTable tblGamma_, ref List<CDrillingBitDepth> depthList, out DateTime start, out DateTime end)
        {                        
            //List<int> lst = new List<int>() { 23, 10113, 10211, 12 };  // gamma, rop, wob, temperature  
            List<CURVE_INFO> lst = new List<CURVE_INFO>() { new CURVE_INFO {sName = "Gamma", iMsgCode = 23},
                                                            new CURVE_INFO {sName = "ROP", iMsgCode = 10113},
                                                            new CURVE_INFO {sName = "WOB", iMsgCode = 10211},
                                                            new CURVE_INFO {sName = "TEMP", iMsgCode = 12} };
            CLogDataLayer log = new CLogDataLayer(ref m_dbCnn);           
            //tblGamma_ = log.Get(lst, fStartDepth_, fStopDepth_, ttType_);                     
            if (tblGamma_.Rows.Count > 0)
            {                
                start = System.Convert.ToDateTime(tblGamma_.Rows[0].Field<string>("created"));                
                end = System.Convert.ToDateTime(tblGamma_.Rows[tblGamma_.Rows.Count - 1].Field<string>("created"));
                startTimeTicks = CDateTime.UnixTicksFromDate(start);
                endTimeTicks = CDateTime.UnixTicksFromDate(end);
            }
            else
                CLogASCIIStandard.SetUnusedParametersToNull(ref startTimeTicks, ref endTimeTicks, out start, out end);
        }

        private void AddBlankLine()
        {
            this.Add(string.Empty);
        }

        private void CreatePressureDataSection(List<CPressure> pressureList, /*CDrillContext context,*/ bool useUnixTime/*, CCommonTypes.StepLevelEnum stepLevel*/)
        {
            // TODO/FIX
            //DateTime surveyTime1 = pressureList.First<CPressure>().SurveyTime;
            //DateTime surveyTime2 = pressureList.Last<CPressure>().SurveyTime;
            //CTrueVerticalDepthManager lasTvdManager = new CTrueVerticalDepthManager(context, this._modelMain.CurrentBitRun.Id, pressureList.First<CPressure>().MeasuredDepth, pressureList.Last<CPressure>().MeasuredDepth);
            this.Add(string.Empty);
            if (useUnixTime)
                this.Add("~Ascii   TIME         DEPTH           TVD        BoreP        AnnuP        VibX        VibY");
            else
                this.Add("~Ascii            TIME         DEPTH           TVD        BoreP        AnnuP        VibX        VibY");
            //foreach (CPressure interpolatePressure in this.InterpolatePressures(pressureList, stepLevel))
            //    this.Add(string.Format("   {0}   {1, 11:F3}   {2, 11:F3}   {3, 10:F3}   {4, 10:F3}  {5, 10:F3}  {6, 10:F3}", (object)CLogASCIIStandard.TimeString(interpolatePressure.SurveyTime, useUnixTime), (object)interpolatePressure.MeasuredDepth, (object)lasTvdManager.GetTVD(interpolatePressure.MeasuredDepth), (object)interpolatePressure.BorePressure, (object)interpolatePressure.AnnularPressure, (object)interpolatePressure.VibrationX, (object)interpolatePressure.VibrationY));
        }

        // TODO/FIX
        //private List<CPressure> InterpolatePressures(List<CPressure> pressureList, ViewModelExport.StepLevelEnum stepLevel)
        //{
            
        //    return new PressureInterpolator(pressureList, stepLevel).Results;
        //}

        public static string TimeString(DateTime time, bool useUnixTime)
        {
            if (useUnixTime)
                return CDateTime.UnixTicksFromDate(time).ToString();
            return string.Format("{0}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}", (object)time.Year, (object)time.Month, (object)time.Day, (object)time.Hour, (object)time.Minute, (object)time.Second);
        }

        
        private void CreateGammaDataSection(DataTable gammaList, List<CDrillingBitDepth> depthList, DateTime dtStart_, DateTime dtEnd_, /*CDrillContext context,*/ bool useUnixTime/*, CCommonTypes.StepLevelEnum stepLevel*/)
        {
            //DateTime startTime = gammaList.First<CGamma>().SurveyTime;
            //DateTime endTime = gammaList.Last<CGamma>().SurveyTime;
            //Interpolator interpolator1 = new Interpolator(context.DataTemperature.Where<DataTemperature>((Expression<Func<DataTemperature, bool>>)(c => startTime <= c.SurveyTime && c.SurveyTime <= endTime)).ToList<DataTemperature>());
            //Interpolator interpolator2 = new Interpolator(depthList);
            Add(string.Empty);
            AddColumnHeaders(useUnixTime);
            //foreach (CGamma interpolateGamma in this.InterpolateGammas(gammaList, stepLevel, this._modelMain.CurrentBitRun.GammaToBit))
            //{
            //    double inputValue1 = interpolator1.GetValue(interpolateGamma.SurveyTime);
            //    double inputValue2 = interpolator2.GetValue(interpolateGamma.SurveyTime);
            //    string str1 = CLogASCIIStandard.CheckForUnset(inputValue1);
            //    string str2 = CLogASCIIStandard.CheckForUnset(inputValue2);
            //    this.Add(string.Format("   {0}   {1, 11:F3}   {2, 11:F3}   {3, 10:F3}   {4, 10:F2}   {5}  {6, 10:F3}", (object)CLogASCIIStandard.TimeString(interpolateGamma.SurveyTime, useUnixTime), (object)interpolateGamma.MeasuredDepth, (object)interpolateGamma.TrueVerticalDepth, (object)interpolateGamma.DataValue, (object)str2, (object)str1, (object)interpolateGamma.ROP));
            //}
        }

        // TODO/FIX
        //private List<CGamma> InterpolateGammas(List<CGamma> gammaList, ViewModelExport.StepLevelEnum stepLevel, double gammaToBit)
        //{
        //    return new GammaInterpolator(gammaList, stepLevel, gammaToBit).Results;
        //}

        private void AddColumnHeaders(bool useUnixTime)
        {
            if (useUnixTime)
                this.Add("~Ascii   TIME         DEPTH           TVD        GAMMA          WOB         TEMP         ROP");
            else
                this.Add("~Ascii            TIME         DEPTH           TVD        GAMMA          WOB         TEMP         ROP");
        }

        private static string CheckForUnset(double inputValue)
        {
            return inputValue == CSystemConstants.NotSetDouble ? " -9999.000" : string.Format("{0, 10:F3}", (object)inputValue);
        }

        private void CreateCurveSection(CLASJobInfo.EXPORT_DATA_TYPE ExportType_, bool useUnixTime, List<CURVE_INFO> lstCurves_)
        {
            string s2_1 = "";
            string s2_2 = "";
            string sTemperatureUnit = "";
            string sBorePressureUnit = "";
            string sAnnularPressureUnit = "";

            CDPointLookupTable DPointTable = new CDPointLookupTable();
            DPointTable.Load();

            if (this._depthUnits == CDrillingBitDepth.DepthUnits.Feet)
            {
                s2_1 = m_UnitLength.GetImperialUnitDesc();
                s2_2 = m_UnitROP.GetImperialUnitDesc();
                sTemperatureUnit = m_UnitTemperature.GetImperialUnitDesc();
                sAnnularPressureUnit = sBorePressureUnit = m_UnitPressure.GetImperialUnitDesc();
            }
            else if (this._depthUnits == CDrillingBitDepth.DepthUnits.Meters)
            {
                s2_1 = m_UnitLength.GetMetricUnitDesc();
                s2_2 = m_UnitROP.GetMetricUnitDesc();
                sTemperatureUnit = m_UnitTemperature.GetMetricUnitDesc();
                sAnnularPressureUnit = sBorePressureUnit = m_UnitPressure.GetMetricUnitDesc();
            }
            else  // get it from the table
            {
                
                CDPointLookupTable.DPointInfo dpi = DPointTable.Get((int)Command.COMMAND_BIT_DEPTH);  // bit depth
                s2_1 = dpi.sUnits;
                // hoan - use commands
                dpi = DPointTable.Get(10113);  // rate of penetration
                s2_2 = dpi.sUnits;
                dpi = DPointTable.Get(12);  // temperature
                sTemperatureUnit = dpi.sUnits;
                dpi = DPointTable.Get(24);  // bore pressure
                sBorePressureUnit = dpi.sUnits;
                dpi = DPointTable.Get(25);  // annular pressure
                sAnnularPressureUnit = dpi.sUnits;
            }
            sTemperatureUnit = sTemperatureUnit.Replace("°", "Deg ");

            this.Add("~Curve Information Section");
            this.AddParametersLine("#MNEM", "Unit", "Value/Name", "Curve Description");
            this.AddParametersLine("#----", "-----", "----------", "-----------------");            
            
            if (ExportType_ == CLASJobInfo.EXPORT_DATA_TYPE.TIME)
            {
                if (useUnixTime)
                    this.AddParametersLine("TIME", "s", "", "0 Seconds Since January 1, 1970 - Unix time");
                else
                    this.AddParametersLine("TIME", "s", "", "0 Date/Time");

                for (int i = 0; i < lstCurves_.Count; i++)
                {
                    // look up the description using the message code
                    CDPointLookupTable.DPointInfo dpi = DPointTable.Get((int)lstCurves_[i].iMsgCode);
                    this.AddParametersLine(lstCurves_[i].sName, lstCurves_[i].sUnit, "", (i + 1).ToString() + " " + dpi.sPurpose);
                }
            }
            else             
            {
                if (ExportType_ == CLASJobInfo.EXPORT_DATA_TYPE.TVD)
                    this.AddParametersLine("TVD", s2_1, "", "0 True Vertical Depth");
                else
                    this.AddParametersLine("DEPTH", s2_1, "", "0 Measured Depth");

                for (int i = 0; i < lstCurves_.Count; i++)
                {
                    // look up the description using the message code
                    CDPointLookupTable.DPointInfo dpi = DPointTable.Get((int)lstCurves_[i].iMsgCode);
                    this.AddParametersLine(lstCurves_[i].sName, lstCurves_[i].sUnit, "", (i + 1).ToString() + " " + dpi.sPurpose);
                }               
            }
        }

        private void CreateWellSection(long startTime, long stopTime, DateTime start, DateTime end, float fStartDepth, float fStopDepth, float fStep, CWellJob job, bool useUnixTime, CLASJobInfo.EXPORT_DATA_TYPE exportType_)
        {
            this.Add("~Well Information Section");
            this.AddParametersLine("#MNEM", "Unit", "Value/Name", "Description");
            this.AddParametersLine("#----", "----", "----------", "-----------");
            if (exportType_ == CLASJobInfo.EXPORT_DATA_TYPE.TIME)
            {
                this.SetTimeFormat(startTime, stopTime, start, end, useUnixTime);
                float fSeconds = (fStep * 60.0f);
                this.AddParametersLine("STEP", "s", fSeconds.ToString(), "Seconds");
            }                
            else
            {
                CDPointLookupTable DPointTable = new CDPointLookupTable();
                DPointTable.Load();
                CDPointLookupTable.DPointInfo dpi = DPointTable.Get((int)Command.COMMAND_BIT_DEPTH);
                string sUnitOfLength = dpi.sUnits;

                if (_depthUnits == CDrillingBitDepth.DepthUnits.Feet)
                    sUnitOfLength = m_UnitLength.GetImperialUnitDesc();
                else if (_depthUnits == CDrillingBitDepth.DepthUnits.Meters)
                    sUnitOfLength = m_UnitLength.GetMetricUnitDesc();

                this.SetDepthFormat(fStartDepth, fStopDepth, sUnitOfLength);
                this.AddParametersLine("STEP", sUnitOfLength, fStep.ToString(), "Depth");
            }
                
            
            this.AddParametersLine("NULL", "", LAS_NULL.ToString(), "Null Value");
            this.AddParametersLine("COMP", "", job.Client, "Company Name");
            this.AddParametersLine("WELL", "", job.WellId, "Well Name");
            this.AddParametersLine("FLD", "", job.Field, "Field Name");
            this.AddParametersLine("LOC", "", job.Facility, "Facility");
            this.SetCountryRegionValues(job);
            string s3_1 = string.Format("{0}:{1}:{2}", (object)job.LongitudeDegrees, (object)job.LongitudeMinutes, (object)job.LongitudeSeconds);
            string s3_2 = string.Format("{0}:{1}:{2}", (object)job.LatitudeDegrees, (object)job.LatitudeMinutes, (object)job.LatitudeSeconds);
            this.AddParametersLine("LONG", "", s3_1, "");
            this.AddParametersLine("LATI", "", s3_2, "");
            this.AddParametersLine("GDAT", "", "", "");
            this.AddParametersLine("SRVC", "", job.ServiceCompany, "Service Company Name");
            this.AddParametersLine("DATE", "Month/Day/Year", job.EndDate.ToShortDateString(), "Log Date");
        }

        private void SetTimeFormat(long startTime, long stopTime, DateTime start, DateTime end, bool useUnixTime)
        {
            if (useUnixTime)
            {
                this.AddParametersLine("STRT", "s", startTime.ToString(), "Seconds since January 1, 1970");
                this.AddParametersLine("STOP", "s", stopTime.ToString(), "Seconds since January 1, 1970");
            }
            else
            {
                DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0);

                this.AddParametersLine("STRT", "s", dt1970.AddSeconds(startTime).ToString("yyyy-MM-dd HH:mm:ss"), "Date/Time yyyy-MM-dd HH:mm:ss");
                this.AddParametersLine("STOP", "s", dt1970.AddSeconds(stopTime).ToString("yyyy-MM-dd HH:mm:ss"), "Date/Time yyyy-MM-dd HH:mm:ss");
            }
        }

        private void SetDepthFormat(float fStartDepth, float fStopDepth, string sUnit_)
        {
            this.AddParametersLine("STRT", sUnit_, fStartDepth.ToString("0.00"), "Depth");
            this.AddParametersLine("STOP", sUnit_, fStopDepth.ToString("0.00"), "Depth");
        }

        private void SetCountryRegionValues(CWellJob job)
        {
            if (job.Country == "CANADA")
            {
                this.AddParametersLine("CTRY", "", "CA", "Country");
                this.AddParametersLine("PROV", "", job.Area, "Province");
            }
            else if (job.Country == "USA")
            {
                this.AddParametersLine("CTRY", "", "USA", "Country");
                this.AddParametersLine("STAT", "", job.Area, "State");
            }
            else
            {
                this.AddParametersLine("CTRY", "", job.Country, "Country");
                this.AddParametersLine("CNTY", "", job.Area, "Region");
            }
        }

        private void CreateVersionSection()
        {
            this.Add("~Version Information Section");
            this.AddParametersLine("VERS", "", "3.0", "LAS Version Number");
            this.AddParametersLine("WRAP", "", "NO", "Wrap record to more than one line");
            this.AddParametersLine("DLM", "", "SPACE", "DELIMITING CHARACTER BETWEEN DATA COLUMNS");
        }

        private void CreateHeader()
        {
            this.Add("#################################################################");
            this.Add("#                                                               #");
            this.Add("# LAS file generated by Applied Physics Systems Display 2.0     #");
            this.Add("#                                                               #");
            this.Add("#################################################################");
        }

        private void Add(string s)
        {
            this.Content.Add(s);
        }

        private void AddParametersLine(string s1, string s2, string s3, string s4)
        {
            this.Add(this.MakeColumn0(s1) + this.MakeColumn1(s2, s3) + " " + s4);
        }

        private string MakeColumn0(string s)
        {
            if (s == null)
                s = string.Empty;
            StringBuilder stringBuilder = new StringBuilder(s.Length <= this._columnWidth0 - 1 ? s : s.Substring(0, this._columnWidth0 - 1), this._columnWidth0);
            for (int length = s.Length; length < this._columnWidth0 - 1; ++length)
                stringBuilder.Append(' ');
            stringBuilder.Append('.');
            return stringBuilder.ToString();
        }

        private string MakeColumn1(string left, string right)
        {
            if (left == null)
                left = string.Empty;
            if (right == null)
                right = string.Empty;
            int num = this._columnWidth1 - right.Length - 2;
            StringBuilder stringBuilder = new StringBuilder(left, this._columnWidth1);
            for (int length = left.Length; length < num - 1; ++length)
                stringBuilder.Append(' ');
            foreach (char ch in right)
                stringBuilder.Append(ch);
            for (int index = 0; index < 2; ++index)
                stringBuilder.Append(' ');
            stringBuilder.Append(':');
            return stringBuilder.ToString();
        }
    }
}

