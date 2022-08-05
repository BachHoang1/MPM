using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPM.Data;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Data.Common;
using MPM.DataAcquisition.Helpers;
using MPM.DataAcquisition.MultiStationAnalysis;

namespace UnitTestMPM
{
    [TestClass]
    public class UnitTestLAS
    {
        const string sDataPath = "C:\\APS\\Data\\Jobs\\Default";

        [TestMethod]
        public void TestMethodGammaNoUnitConversion()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();
            CLogDataLayer log = new CLogDataLayer(ref cnn);

            List<CLogASCIIStandard.CURVE_INFO> lst = new List<CLogASCIIStandard.CURVE_INFO>();
            CLogASCIIStandard.CURVE_INFO cv = new CLogASCIIStandard.CURVE_INFO();
            cv.sName = "Gamma";
            cv.sUnit = "CPS";
            cv.iMsgCode = (int)Command.COMMAND_RESP_GAMMA;
            lst.Add(cv);
            DataTable tbl = log.Get(lst[0].iMsgCode.ToString(), 0, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);

            // insert some fake gamma data
            if (tbl.Rows.Count == 0)
            {
                CLogDataRecord rec = new CLogDataRecord();
                rec.iMessageCode = (int)lst[0].iMsgCode;
                rec.sUnit = lst[0].sUnit;
                rec.bParityError = false;
                rec.sName = lst[0].sName; 
                DateTime dt = new DateTime(2017, 11, 11, 12, 0, 0);
                
                for (int i = 0; i < 10; i++)
                {
                    rec.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    rec.iDate = System.Convert.ToInt32(dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00"));
                    rec.iTime = System.Convert.ToInt32(dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Minute.ToString("00"));

                    rec.sValue = (100 + i).ToString();
                    rec.fDepth = 500 + i * 10;
                    dt += TimeSpan.FromMinutes(2);
                    log.Save(rec);
                }
            }

            CWITSLookupTable witsLookUpTbl = new CWITSLookupTable();
            CMSAHubClient msaHubClient = new CMSAHubClient(ref witsLookUpTbl, ref cnn, sDataPath);
            CLogASCIIStandard lasMgr = new CLogASCIIStandard(ref cnn, msaHubClient, 7, 40, CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT);
                        
            string fileName = "testUnitsNoConversion.las";
            float startDepth = 0;
            float stopDepth = 1000;
            DateTime dtDummyStartTime = DateTime.Now;
            DateTime dtDummyStopTime = DateTime.Now;
            bool showUnixTime = true;

            //CCommonTypes.StepLevelEnum stepLevel = CCommonTypes.StepLevelEnum.Step_1_0;
            float fStep = 0.25f;
            CWellJob job = new CWellJob();
            lasMgr.CreateLasFile(job, CLASJobInfo.EXPORT_DATA_TYPE.MD, fileName, startDepth, stopDepth, dtDummyStartTime, dtDummyStopTime, showUnixTime, fStep, CCommonTypes.TELEMETRY_TYPE_FOR_LAS.TT_MP, lst);

            cnn.Close();
        }

        [TestMethod]
        public void TestMethodGammaImperialUnits()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();
            CLogDataLayer log = new CLogDataLayer(ref cnn);

            List<CLogASCIIStandard.CURVE_INFO> lst = new List<CLogASCIIStandard.CURVE_INFO>();
            CLogASCIIStandard.CURVE_INFO cv = new CLogASCIIStandard.CURVE_INFO();
            cv.sName = "Gamma";
            cv.sUnit = "CPS";
            cv.iMsgCode = (int)Command.COMMAND_RESP_GAMMA;
            lst.Add(cv);
            DataTable tbl = log.Get(lst[0].iMsgCode.ToString(), 0, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);

            // insert some fake gamma data
            if (tbl.Rows.Count == 0)
            {
                CLogDataRecord rec = new CLogDataRecord();
                rec.iMessageCode = (int)lst[0].iMsgCode;
                rec.sUnit = lst[0].sUnit;
                rec.bParityError = false;
                rec.sName = lst[0].sName;               
                DateTime dt = new DateTime(2017, 11, 11, 12, 0, 0);

                for (int i = 0; i < 10; i++)
                {
                    rec.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    rec.iDate = System.Convert.ToInt32(dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00"));
                    rec.iTime = System.Convert.ToInt32(dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Minute.ToString("00"));

                    rec.sValue = (100 + i).ToString();
                    rec.fDepth = 500 + i * 10;
                    dt += TimeSpan.FromMinutes(2);
                    log.Save(rec);
                }
            }

            CWITSLookupTable witsLookUpTbl = new CWITSLookupTable();
            CMSAHubClient msaHubClient = new CMSAHubClient(ref witsLookUpTbl, ref cnn, sDataPath);
            CLogASCIIStandard lasMgr = new CLogASCIIStandard(ref cnn, msaHubClient, 7, 40, CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL);
            
            string fileName = "testGammaImperialUnits.las";
            float startDepth = 1640;
            float stopDepth = 1932;
            DateTime dtDummyStartTime = DateTime.Now;
            DateTime dtDummyStopTime = DateTime.Now;
            bool showUnixTime = true;

            //CCommonTypes.StepLevelEnum stepLevel = CCommonTypes.StepLevelEnum.Step_1_0;
            float fStep = 0.25f;
            CWellJob job = new CWellJob();
            lasMgr.CreateLasFile(job, CLASJobInfo.EXPORT_DATA_TYPE.MD, fileName, startDepth, stopDepth, dtDummyStartTime, dtDummyStopTime, showUnixTime, fStep, CCommonTypes.TELEMETRY_TYPE_FOR_LAS.TT_MP, lst);

            cnn.Close();
        }

        [TestMethod]
        public void TestMethodGammaMetricUnits()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();
            CLogDataLayer log = new CLogDataLayer(ref cnn);

            List<CLogASCIIStandard.CURVE_INFO> lst = new List<CLogASCIIStandard.CURVE_INFO>();
            CLogASCIIStandard.CURVE_INFO cv = new CLogASCIIStandard.CURVE_INFO();
            cv.sName = "Gamma";
            cv.sUnit = "CPS";
            cv.iMsgCode = (int)Command.COMMAND_RESP_GAMMA;
            lst.Add(cv);
            DataTable tbl = log.Get(lst[0].iMsgCode.ToString(), 0, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);

            // insert some fake gamma data
            if (tbl.Rows.Count == 0)
            {
                CLogDataRecord rec = new CLogDataRecord();
                rec.iMessageCode = (int)lst[0].iMsgCode;
                rec.sUnit = lst[0].sUnit;
                rec.bParityError = false;
                rec.sName = lst[0].sName;                
                DateTime dt = new DateTime(2017, 11, 11, 12, 0, 0);

                for (int i = 0; i < 10; i++)
                {
                    rec.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    rec.iDate = System.Convert.ToInt32(dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00"));
                    rec.iTime = System.Convert.ToInt32(dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Minute.ToString("00"));

                    rec.sValue = (100 + i).ToString();
                    rec.fDepth = 500 + i * 10;
                    dt += TimeSpan.FromMinutes(2);
                    log.Save(rec);
                }
            }

            CWITSLookupTable witsLookUpTbl = new CWITSLookupTable();
            CMSAHubClient msaHubClient = new CMSAHubClient(ref witsLookUpTbl, ref cnn, sDataPath);
            CLogASCIIStandard lasMgr = new CLogASCIIStandard(ref cnn, msaHubClient, 7, 40, CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC);
            
            string fileName = "testGammaMetricUnits.las";
            float startDepth = 0;
            float stopDepth = 1000;
            DateTime dtDummyStartTime = DateTime.Now;
            DateTime dtDummyStopTime = DateTime.Now;
            bool showUnixTime = true;

            //CCommonTypes.StepLevelEnum stepLevel = CCommonTypes.StepLevelEnum.Step_1_0;
            float fStep = 0.25f;
            CWellJob job = new CWellJob();
            lasMgr.CreateLasFile(job, CLASJobInfo.EXPORT_DATA_TYPE.MD, fileName, startDepth, stopDepth, dtDummyStartTime, dtDummyStopTime, showUnixTime, fStep, CCommonTypes.TELEMETRY_TYPE_FOR_LAS.TT_MP, lst);

            cnn.Close();
        }

        [TestMethod]
        public void TestMethodPressureNoUnitConversion()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();
            CLogDataLayer log = new CLogDataLayer(ref cnn);

            List<CLogASCIIStandard.CURVE_INFO> lst = new List<CLogASCIIStandard.CURVE_INFO>();
            CLogASCIIStandard.CURVE_INFO cv = new CLogASCIIStandard.CURVE_INFO();
            cv.sName = "Annular Pr";
            cv.sUnit = "PSI";
            cv.iMsgCode = (int)Command.COMMAND_RESP_A_PRESSURE;
            lst.Add(cv);
            cv.sName = "Bore Pr";
            cv.sUnit = "PSI";
            cv.iMsgCode = (int)Command.COMMAND_RESP_B_PRESSURE;
            lst.Add(cv);

            DataTable tbl = log.Get(lst[0].iMsgCode.ToString(), 0, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);

            if (tbl.Rows.Count == 0)  // insert some fake annular and bore pressure data
            {
                CLogDataRecord recAnnular = new CLogDataRecord();
                recAnnular.iMessageCode = (int)lst[0].iMsgCode;
                recAnnular.sUnit = lst[0].sUnit;
                recAnnular.bParityError = false;
                recAnnular.sName = lst[0].sName;

                CLogDataRecord recBore = new CLogDataRecord();
                recBore.iMessageCode = (int)lst[1].iMsgCode;
                recBore.sUnit = lst[1].sUnit;
                recBore.bParityError = false;
                recBore.sName = lst[1].sName;                

                DateTime dt = new DateTime(2017, 11, 11, 12, 0, 0);

                for (int i = 0; i < 10; i++)
                {
                    recAnnular.sCreated =  recBore.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    recAnnular.iDate = recBore.iDate = System.Convert.ToInt32(dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00"));
                    recAnnular.iTime = recBore.iTime = System.Convert.ToInt32(dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Minute.ToString("00"));

                    recAnnular.sValue = recBore.sValue = (2000 + i).ToString();
                    recBore.fDepth = 500 + i * 10;
                    recAnnular.fDepth = 501 + i * 10;
                    dt += TimeSpan.FromMinutes(2);
                    log.Save(recBore);
                    log.Save(recAnnular);
                }
            }

            CWITSLookupTable witsLookUpTbl = new CWITSLookupTable();
            CMSAHubClient msaHubClient = new CMSAHubClient(ref witsLookUpTbl, ref cnn, sDataPath);
            CLogASCIIStandard lasMgr = new CLogASCIIStandard(ref cnn, msaHubClient, 7, 40, CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT);
            
            string fileName = "testPressureNoConversion.las";
            float startDepth = 0;
            float stopDepth = 1000;
            DateTime dtDummyStartTime = DateTime.Now;
            DateTime dtDummyStopTime = DateTime.Now;
            bool showUnixTime = true;

            //CCommonTypes.StepLevelEnum stepLevel = CCommonTypes.StepLevelEnum.Step_1_0;
            float fStep = 0.25f;
            CWellJob job = new CWellJob();
            lasMgr.CreateLasFile(job, CLASJobInfo.EXPORT_DATA_TYPE.MD, fileName, startDepth, stopDepth, dtDummyStartTime, dtDummyStopTime, showUnixTime, fStep, CCommonTypes.TELEMETRY_TYPE_FOR_LAS.TT_MP, lst);

            cnn.Close();
        }

        [TestMethod]
        public void TestMethodPressureMetricUnits()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();
            CLogDataLayer log = new CLogDataLayer(ref cnn);

            List<CLogASCIIStandard.CURVE_INFO> lst = new List<CLogASCIIStandard.CURVE_INFO>();
            CLogASCIIStandard.CURVE_INFO cv = new CLogASCIIStandard.CURVE_INFO();
            cv.sName = "Annular Pr";
            cv.sUnit = "PSI";
            cv.iMsgCode = (int)Command.COMMAND_RESP_A_PRESSURE;
            lst.Add(cv);
            cv.sName = "Bore Pr";
            cv.sUnit = "PSI";
            cv.iMsgCode = (int)Command.COMMAND_RESP_B_PRESSURE;
            lst.Add(cv);
            DataTable tbl = log.Get(lst[0].iMsgCode.ToString(), 0, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);

            if (tbl.Rows.Count == 0)  // insert some fake annular pressure data
            {
                CLogDataRecord recAnnular = new CLogDataRecord();
                recAnnular.iMessageCode = (int)lst[0].iMsgCode;
                recAnnular.sUnit = lst[0].sUnit;
                recAnnular.bParityError = false;
                recAnnular.sName = lst[0].sName;

                CLogDataRecord recBore = new CLogDataRecord();
                recBore.iMessageCode = (int)lst[1].iMsgCode;
                recBore.sUnit = lst[1].sUnit;
                recBore.bParityError = false;
                recBore.sName = lst[1].sName;

                DateTime dt = new DateTime(2017, 11, 11, 12, 0, 0);

                for (int i = 0; i < 10; i++)
                {
                    recAnnular.sCreated = recBore.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    recAnnular.iDate = recBore.iDate = System.Convert.ToInt32(dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00"));
                    recAnnular.iTime = recBore.iTime = System.Convert.ToInt32(dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Minute.ToString("00"));

                    recAnnular.sValue = recBore.sValue = (2000 + i).ToString();
                    recBore.fDepth = 500 + i * 10;
                    recAnnular.fDepth = 501 + i * 10;
                    dt += TimeSpan.FromMinutes(2);
                    log.Save(recBore);
                    log.Save(recAnnular);
                }                
            }

            CWITSLookupTable witsLookUpTbl = new CWITSLookupTable();
            CMSAHubClient msaHubClient = new CMSAHubClient(ref witsLookUpTbl, ref cnn, sDataPath);
            CLogASCIIStandard lasMgr = new CLogASCIIStandard(ref cnn, msaHubClient, 7, 40, CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC);
            
            string fileName = "testPressureMetricUnits.las";
            float startDepth = 0;
            float stopDepth = 1000;
            DateTime dtDummyStartTime = DateTime.Now;
            DateTime dtDummyStopTime = DateTime.Now;
            bool showUnixTime = true;

            //CCommonTypes.StepLevelEnum stepLevel = CCommonTypes.StepLevelEnum.Step_1_0;
            float fStep = 0.25f;
            CWellJob job = new CWellJob();
            lasMgr.CreateLasFile(job, CLASJobInfo.EXPORT_DATA_TYPE.MD, fileName, startDepth, stopDepth, dtDummyStartTime, dtDummyStopTime,  showUnixTime, fStep, CCommonTypes.TELEMETRY_TYPE_FOR_LAS.TT_MP, lst);

            cnn.Close();
        }

        [TestMethod]
        public void TestMethodPressureImperialUnits()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();
            CLogDataLayer log = new CLogDataLayer(ref cnn);

            List<CLogASCIIStandard.CURVE_INFO> lst = new List<CLogASCIIStandard.CURVE_INFO>();
            CLogASCIIStandard.CURVE_INFO cv = new CLogASCIIStandard.CURVE_INFO();
            cv.sName = "Annular Pr";
            cv.sUnit = "PSI";
            cv.iMsgCode = (int)Command.COMMAND_RESP_A_PRESSURE;
            lst.Add(cv);
            cv.sName = "Bore Pr";
            cv.sUnit = "PSI";
            cv.iMsgCode = (int)Command.COMMAND_RESP_B_PRESSURE;
            lst.Add(cv);
            DataTable tbl = log.Get(lst[0].iMsgCode.ToString(), 0, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);

            if (tbl.Rows.Count == 0)  // insert some fake annular pressure data
            {
                CLogDataRecord recAnnular = new CLogDataRecord();
                recAnnular.iMessageCode = (int)lst[0].iMsgCode;
                recAnnular.sUnit = lst[0].sUnit;
                recAnnular.bParityError = false;
                recAnnular.sName = lst[0].sName;

                CLogDataRecord recBore = new CLogDataRecord();
                recBore.iMessageCode = (int)lst[1].iMsgCode;
                recBore.sUnit = lst[1].sUnit;
                recBore.bParityError = false;
                recBore.sName = lst[1].sName;

                DateTime dt = new DateTime(2017, 11, 11, 12, 0, 0);

                for (int i = 0; i < 10; i++)
                {
                    recAnnular.sCreated = recBore.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    recAnnular.iDate = recBore.iDate = System.Convert.ToInt32(dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00"));
                    recAnnular.iTime = recBore.iTime = System.Convert.ToInt32(dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Minute.ToString("00"));

                    recAnnular.sValue = recBore.sValue = (2000 + i).ToString();
                    recBore.fDepth = 500 + i * 10;
                    recAnnular.fDepth = 501 + i * 10;
                    dt += TimeSpan.FromMinutes(2);
                    log.Save(recBore);
                    log.Save(recAnnular);
                }
            }

            CWITSLookupTable witsLookUpTbl = new CWITSLookupTable();
            CMSAHubClient msaHubClient = new CMSAHubClient(ref witsLookUpTbl, ref cnn, sDataPath);
            CLogASCIIStandard lasMgr = new CLogASCIIStandard(ref cnn, msaHubClient, 7, 40, CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL);
            
            string fileName = "testPressureImperialUnits.las";
            float startDepth = 1640;
            float stopDepth = 1932;
            DateTime dtDummyStartTime = DateTime.Now;
            DateTime dtDummyStopTime = DateTime.Now;
            bool showUnixTime = true;

            //CCommonTypes.StepLevelEnum stepLevel = CCommonTypes.StepLevelEnum.Step_1_0;
            float fStep = 0.25f;
            CWellJob job = new CWellJob();
            lasMgr.CreateLasFile(job, CLASJobInfo.EXPORT_DATA_TYPE.MD, fileName, startDepth, stopDepth, dtDummyStartTime, dtDummyStopTime, showUnixTime, fStep, CCommonTypes.TELEMETRY_TYPE_FOR_LAS.TT_MP, lst);

            cnn.Close();
        }

        [TestMethod]
        public void TestMethodInsertROP()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();
            CLogDataLayer log = new CLogDataLayer(ref cnn);

            
            List<CLogASCIIStandard.CURVE_INFO> lst = new List<CLogASCIIStandard.CURVE_INFO>();
            CLogASCIIStandard.CURVE_INFO cv = new CLogASCIIStandard.CURVE_INFO();
            cv.sName = "ROP";
            cv.sUnit = "Ft/Hr";
            cv.iMsgCode = (int)Command.COMMAND_ROP;
            lst.Add(cv);
            DataTable tbl = log.Get(lst[0].iMsgCode.ToString(), 500, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);

            if (tbl.Rows.Count == 0)
            {
                // insert some fake rop data
                CLogDataRecord rec = new CLogDataRecord();
                rec.iMessageCode = (int)lst[0].iMsgCode;
                rec.sUnit = lst[0].sUnit;
                rec.bParityError = false;
                rec.sName = lst[0].sName;
                DateTime dt = new DateTime(2017, 11, 11, 12, 0, 0);
            
                for (int i = 0; i < 10; i++)
                {
                    rec.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    rec.iDate = System.Convert.ToInt32(dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00"));
                    rec.iTime = System.Convert.ToInt32(dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Minute.ToString("00"));

                    rec.sValue = (60 + i % 2).ToString();
                    rec.fDepth = 500 + i * 10;
                    rec.sTelemetry = "MP";
                    dt += TimeSpan.FromMinutes(2);
                    log.Save(rec);
                }
            }
                        
            tbl = log.Get(lst[0].iMsgCode.ToString(), 500, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);
            cnn.Close();
            Assert.AreEqual(tbl.Rows.Count, 10);
        }

        [TestMethod]
        public void TestMethodInsertWOB()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();
            CLogDataLayer log = new CLogDataLayer(ref cnn);
            
            List<CLogASCIIStandard.CURVE_INFO> lst = new List<CLogASCIIStandard.CURVE_INFO>();
            CLogASCIIStandard.CURVE_INFO cv = new CLogASCIIStandard.CURVE_INFO();
            cv.sName = "WOB";
            cv.sUnit = "KLB";
            cv.iMsgCode = (int)Command.COMMAND_WOB;
            lst.Add(cv);
            DataTable tbl = log.Get(lst[0].iMsgCode.ToString(), 500, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);

            if (tbl.Rows.Count == 0)
            {
                // insert some fake annular pressure data
                CLogDataRecord rec = new CLogDataRecord();
                rec.iMessageCode = (int)lst[0].iMsgCode;
                rec.sUnit = lst[0].sUnit;
                rec.bParityError = false;
                rec.sName = lst[0].sName;
                DateTime dt = new DateTime(2017, 11, 11, 12, 0, 0);

                for (int i = 0; i < 10; i++)
                {
                    rec.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    rec.iDate = System.Convert.ToInt32(dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00"));
                    rec.iTime = System.Convert.ToInt32(dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Minute.ToString("00"));

                    rec.sValue = (200 + i).ToString();
                    rec.fDepth = 500 + i * 10;
                    rec.sTelemetry = "MP";
                    dt += TimeSpan.FromMinutes(2);
                    log.Save(rec);
                }
            }

            tbl = log.Get(lst[0].iMsgCode.ToString(), 500, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);
            cnn.Close();
            Assert.AreEqual(tbl.Rows.Count, 10);
        }

        [TestMethod]
        public void TestMethodInsertTemperature()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();
            CLogDataLayer log = new CLogDataLayer(ref cnn);           
            
            List<CLogASCIIStandard.CURVE_INFO> lst = new List<CLogASCIIStandard.CURVE_INFO>();
            CLogASCIIStandard.CURVE_INFO cv = new CLogASCIIStandard.CURVE_INFO();
            cv.sName = "Temperature";
            cv.sUnit = "Deg C";
            cv.iMsgCode = (int)Command.COMMAND_RESP_TEMPERATURE;
            lst.Add(cv);
            DataTable tbl = log.Get(lst[0].iMsgCode.ToString(), 500, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);

            if (tbl.Rows.Count == 0)
            {
                // insert some fake annular pressure data
                CLogDataRecord rec = new CLogDataRecord();
                rec.iMessageCode = (int)lst[0].iMsgCode;
                rec.sUnit = lst[0].sUnit;
                rec.bParityError = false;
                rec.sName = lst[0].sName; 
                DateTime dt = new DateTime(2017, 11, 11, 12, 0, 0);

                for (int i = 0; i < 10; i++)
                {
                    rec.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    rec.iDate = System.Convert.ToInt32(dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00"));
                    rec.iTime = System.Convert.ToInt32(dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Minute.ToString("00"));

                    rec.sValue = (29 + i).ToString();
                    rec.fDepth = 500 + i * 10;
                    rec.sTelemetry = "MP";
                    dt += TimeSpan.FromMinutes(2);
                    log.Save(rec);
                }
            }

            tbl = log.Get(lst[0].iMsgCode.ToString(), 500, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);
            cnn.Close();
            Assert.AreEqual(tbl.Rows.Count, 10);
        }

        [TestMethod]
        public void TestMethodInsertBorePressure()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();
            CLogDataLayer log = new CLogDataLayer(ref cnn);
            
            List<CLogASCIIStandard.CURVE_INFO> lst = new List<CLogASCIIStandard.CURVE_INFO>();
            CLogASCIIStandard.CURVE_INFO cv = new CLogASCIIStandard.CURVE_INFO();
            cv.sName = "Bore Pr";
            cv.sUnit = "PSI";
            cv.iMsgCode = (int)Command.COMMAND_RESP_B_PRESSURE;
            lst.Add(cv);
            DataTable tbl = log.Get(lst[0].iMsgCode.ToString(), 500, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);

            if (tbl.Rows.Count == 0)
            {
                // insert some fake bore pressure data
                CLogDataRecord rec = new CLogDataRecord();
                rec.iMessageCode = (int)lst[0].iMsgCode;
                rec.sUnit = lst[0].sUnit;
                rec.bParityError = false;
                rec.sName = lst[0].sName; 
                DateTime dt = new DateTime(2017, 11, 11, 12, 0, 0);
            
                for (int i = 0; i < 10; i++)
                {
                    rec.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    rec.iDate = System.Convert.ToInt32(dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00"));
                    rec.iTime = System.Convert.ToInt32(dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Minute.ToString("00"));

                    rec.sValue = (2000 + i).ToString();
                    rec.fDepth = 500 + i * 10;
                    rec.sTelemetry = "MP";
                    dt += TimeSpan.FromMinutes(2);
                    log.Save(rec);
                }
            }

            tbl = log.Get(lst[0].iMsgCode.ToString(), 500, 1000, CCommonTypes.TELEMETRY_TYPE.TT_MP);
            cnn.Close();
            Assert.AreEqual(tbl.Rows.Count, 10);
            
        }

        [TestMethod]
        public void TestMethodInsertAnnularPressure()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();
            CLogDataLayer log = new CLogDataLayer(ref cnn);            
            
            List<CLogASCIIStandard.CURVE_INFO> lst = new List<CLogASCIIStandard.CURVE_INFO>();
            CLogASCIIStandard.CURVE_INFO cv = new CLogASCIIStandard.CURVE_INFO();
            cv.sName = "Annular Pr";
            cv.sUnit = "PSI";
            cv.iMsgCode = (int)Command.COMMAND_RESP_A_PRESSURE;
            lst.Add(cv);
            DataTable tbl = log.Get(lst[0].iMsgCode.ToString(), 500, 1000, CCommonTypes.TELEMETRY_TYPE.TT_BOTH);

            if (tbl.Rows.Count == 0)
            {
                // insert some fake annular pressure data
                CLogDataRecord rec = new CLogDataRecord();
                rec.iMessageCode = (int)lst[0].iMsgCode;
                rec.sUnit = lst[0].sUnit;
                rec.bParityError = false;
                rec.sName = lst[0].sName;  // 
                DateTime dt = new DateTime(2017, 11, 11, 12, 1, 0);

                for (int i = 0; i < 10; i++)
                {
                    rec.sCreated = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    rec.iDate = System.Convert.ToInt32(dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00"));
                    rec.iTime = System.Convert.ToInt32(dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Minute.ToString("00"));

                    rec.sValue = (2000 + i).ToString();
                    rec.fDepth = 501 + i * 10;
                    rec.sTelemetry = "MP";
                    dt += TimeSpan.FromMinutes(2);
                    log.Save(rec);
                }
            }

            tbl = log.Get(lst[0].iMsgCode.ToString(), 500, 1000, CCommonTypes.TELEMETRY_TYPE.TT_BOTH);
            cnn.Close();
            Assert.AreEqual(tbl.Rows.Count, 10);
        }
    }
}
