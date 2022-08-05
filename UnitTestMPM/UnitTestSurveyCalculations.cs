// author: hoan chau
// purpose: a set of unit tests for survey calculations, especially TVD

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPM.Data;
using MPM.Utilities;
using System.Data.Common;
using MPM.DataAcquisition.Helpers;
using System.Collections.Generic;
using System.Data;
using MPM.DataAcquisition.MultiStationAnalysis;


namespace UnitTestMPM
{
    [TestClass]
    public class UnitTestSurveyCalculations
    {
        const string sDataPath = "C:\\APS\\Data\\Jobs\\Default";
        public struct REC
        {
            public REC(double md_, double inc_, double azm_) {
                md = md_;
                inc = inc_;
                azm = azm_;
            }
            public double md;
            public double inc;
            public double azm;

        }

        [TestMethod]
        public void TestMethodTVD()
        {
            // example data taken from 
            // https://www.netwasgroup.us/services-2/minimum-curvature.html
            REC[] arrRec = new REC[]
            {
                new REC(0, 0, 0 ),
                new REC(1000, 0, 0),
                new REC(1100,3.00,21.70),
                new REC(1200,6.00,26.50),
                new REC(1300,9.00,23.30),
                new REC(1400,12.00,20.30),
                new REC(1500,15.00, 23.30),
                new REC(1600,18.00,23.90),
                new REC(1700,21.00,24.40),
                new REC(1800,24.00,23.40),
                new REC(1900,27.00,23.70),
                new REC(2000,30.00,23.30),
                new REC(2100,30.20,22.80),
                new REC(2200,30.40,22.50),
                new REC(2300,30.30,22.10),
                new REC(2400,30.60,22.40),
                new REC(2500,31.00,22.50),
                new REC(2600,31.20,21.60),
                new REC(2700,30.70,20.80),
                new REC(2800,31.40,20.90),
                new REC(2900,30.60,22.00),
                new REC(3000,30.50,22.50),
                new REC(3100,30.40,23.90),
                new REC(3200,30.00,24.50),
                new REC(3300,30.20,24.90),
                new REC(3400,31.00,25.70),
                new REC(3500,31.10,25.50),
                new REC(3600,32.00,24.40),
                new REC(3700,30.80,24.00),
                new REC(3800,30.60,22.30),
                new REC(3900,31.20,21.70),
                new REC(4000,30.80,20.80),
                new REC(4100,30.00,20.80),
                new REC(4200,29.70,19.80),
                new REC(4300,29.80,20.80),
                new REC(4400,29.50,21.10),
                new REC(4500,29.20,20.80),
                new REC(4600,29.00,20.60),
                new REC(4700,28.70,21.40),
                new REC(4800,28.50,21.20)
            };

            double dblTVD = 0;
            CSurveyCalculation calc = new CSurveyCalculation();
            for (int i = 0; i < arrRec.Length - 1; i++)
            {
                CSurvey.REC rec1 = new CSurvey.REC();
                CSurvey.REC rec2 = new CSurvey.REC();

                rec1.fAzimuth = (float)arrRec[i].azm;
                rec1.fInclination = (float)arrRec[i].inc;
                rec1.fBitDepth = (float)arrRec[i].md;

                rec2.fAzimuth = (float)arrRec[i + 1].azm;
                rec2.fInclination = (float)arrRec[i + 1].inc;
                rec2.fBitDepth = (float)arrRec[i + 1].md;

                double dblTVDTemp = calc.GetTVD(rec1, rec2);
                dblTVD += dblTVDTemp;
            }

            Assert.IsTrue(dblTVD >= 4370.0 && dblTVD <= 4371.0);
            Console.WriteLine("Total TVD is: " + dblTVD.ToString());
        }

        [TestMethod]
        public void TestMethodTVDSegments()
        {
            REC[] arrRec = new REC[]
            {
                new REC(0, 0, 0 ),
                new REC(1000, 0, 0),
                new REC(1100,3.00,21.70),
                new REC(1200,6.00,26.50),
                new REC(1300,9.00,23.30),
                new REC(1400,12.00,20.30),
                new REC(1500,15.00, 23.30),
                new REC(1600,18.00,23.90),
                new REC(1700,21.00,24.40),
                new REC(1800,24.00,23.40),
                new REC(1900,27.00,23.70),
                new REC(2000,30.00,23.30),
                new REC(2100,30.20,22.80),
                new REC(2200,30.40,22.50),
                new REC(2300,30.30,22.10),
                new REC(2400,30.60,22.40),
                new REC(2500,31.00,22.50),
                new REC(2600,31.20,21.60),
                new REC(2700,30.70,20.80),
                new REC(2800,31.40,20.90),
                new REC(2900,30.60,22.00),
                new REC(3000,30.50,22.50),
                new REC(3100,30.40,23.90),
                new REC(3200,30.00,24.50),
                new REC(3300,30.20,24.90),
                new REC(3400,31.00,25.70),
                new REC(3500,31.10,25.50),
                new REC(3600,32.00,24.40),
                new REC(3700,30.80,24.00),
                new REC(3800,30.60,22.30),
                new REC(3900,31.20,21.70),
                new REC(4000,30.80,20.80),
                new REC(4100,30.00,20.80),
                new REC(4200,29.70,19.80),
                new REC(4300,29.80,20.80),
                new REC(4400,29.50,21.10),
                new REC(4500,29.20,20.80),
                new REC(4600,29.00,20.60),
                new REC(4700,28.70,21.40),
                new REC(4800,28.50,21.20)
            };

            double dblTVD = 0;
            CSurveyCalculation calc = new CSurveyCalculation();
            for (int i = 0; i < arrRec.Length - 1; i++)
            {
                CSurvey.REC rec1 = new CSurvey.REC();
                CSurvey.REC rec2 = new CSurvey.REC();

                rec1.fAzimuth = (float)arrRec[i].azm;
                rec1.fInclination = (float)arrRec[i].inc;
                rec1.fBitDepth = (float)arrRec[i].md;

                rec2.fAzimuth = (float)arrRec[i + 1].azm;
                rec2.fInclination = (float)arrRec[i + 1].inc;
                rec2.fBitDepth = (float)arrRec[i + 1].md;

                double dblTVDTemp = calc.GetTVD(rec1, rec2);

                double dblTotalTVD = 0;
                if (i + 1 == arrRec.Length - 1)
                {
                    List<CSurveyCalculation.MD_TO_TVD> lstAggregate = calc.GetMDToTVDs(rec1, rec2, dblTVD, .001);
                    List<CSurveyCalculation.MD_TO_TVD> lst = calc.GetMDToTVDsNoAggregate(rec1, rec2, .001);

                    for (int j = 0; j < lst.Count; j++)
                        dblTotalTVD += lst[j].tvd;

                    // prove that the sum of the steps is about equal to the whole arc
                    Assert.AreEqual((int)(dblTotalTVD * 1000), (int)(dblTVDTemp * 1000));

                    List<double> lstTVD = new List<double>();
                    double dbltempTVD = dblTVD;  // initialize
                    lstTVD.Add(dbltempTVD);
                    for (int j = 0; j < lst.Count; j++)
                    {
                        lstTVD.Add(dbltempTVD + lst[i].tvd);
                        dbltempTVD += lst[i].tvd;
                    }
                }

                dblTVD += dblTVDTemp;
            }
        }

        [TestMethod]
        public void TestMethodCreateSurveyAndGammaData()
        {
            // generate fake survey data
            // example data taken from 
            // https://www.netwasgroup.us/services-2/minimum-curvature.html
            REC[] arrRec = new REC[]
            {
                new REC(0, 0, 0 ),
                new REC(1000, 0, 0),
                new REC(1100,3.00,21.70),
                new REC(1200,6.00,26.50),
                new REC(1300,9.00,23.30),
                new REC(1400,12.00,20.30),
                new REC(1500,15.00,23.30),
                new REC(1600,18.00,23.90),
                new REC(1700,21.00,24.40),
                new REC(1800,24.00,23.40),
                new REC(1900,27.00,23.70),
                new REC(2000,30.00,23.30),
                new REC(2100,30.20,22.80),
                new REC(2200,30.40,22.50),
                new REC(2300,30.30,22.10),
                new REC(2400,30.60,22.40),
                new REC(2500,31.00,22.50),
                new REC(2600,31.20,21.60),
                new REC(2700,30.70,20.80),
                new REC(2800,31.40,20.90),
                new REC(2900,30.60,22.00),
                new REC(3000,30.50,22.50),
                new REC(3100,30.40,23.90),
                new REC(3200,30.00,24.50),
                new REC(3300,30.20,24.90),
                new REC(3400,31.00,25.70),
                new REC(3500,31.10,25.50),
                new REC(3600,32.00,24.40),
                new REC(3700,30.80,24.00),
                new REC(3800,30.60,22.30),
                new REC(3900,31.20,21.70),
                new REC(4000,30.80,20.80),
                new REC(4100,30.00,20.80),
                new REC(4200,29.70,19.80),
                new REC(4300,29.80,20.80),
                new REC(4400,29.50,21.10),
                new REC(4500,29.20,20.80),
                new REC(4600,29.00,20.60),
                new REC(4700,28.70,21.40),
                new REC(4800,28.50,21.20)
            };
            
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();

            CSurveyLog logSvy = new CSurveyLog(ref cnn);
            CLogDataLayer logGamma = new CLogDataLayer(ref cnn);

            DateTime dtTheTime = DateTime.Now;
            TimeSpan tsIntervalTenMinutes = new TimeSpan(0, 10, 0);
            DateTime dtGammaTime = DateTime.Now;
            TimeSpan tsIntervalOneMinute = new TimeSpan(0, 10, 0);

            for (int i = 0; i < arrRec.Length; i++)
            {
                CSurvey.REC recSvy = new CSurvey.REC();

                recSvy.fAzimuth = (float)arrRec[i].azm;
                recSvy.fInclination = (float)arrRec[i].inc;
                recSvy.fBitDepth = (float)arrRec[i].md;
                recSvy.TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_EM;
                recSvy.Status = CSurvey.STATUS.ACCEPT;
                recSvy.fDirToBit = 20.0f;
                recSvy.dtCreated = dtTheTime;

                // insert data into tblSurvey                
                Assert.AreEqual(logSvy.Save(recSvy), true);

                // generate fake gamma data
                for (int j = 0; j < 10; j++)
                {
                    CLogDataRecord recGamma = new CLogDataRecord();
                    recGamma.iMessageCode = (int)Command.COMMAND_RESP_GAMMA;
                    recGamma.sCreated = dtGammaTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    recGamma.iDate = System.Convert.ToInt32(dtGammaTime.ToString("yyyyMMdd"));
                    recGamma.iTime = System.Convert.ToInt32(dtGammaTime.ToString("HHmmss"));
                    recGamma.sName = "Gamma";
                    recGamma.sValue = (j * 1).ToString();
                    recGamma.sUnit = "CPS";
                    recGamma.bParityError = false;
                    recGamma.fDepth = (float)(arrRec[i].md + (j * 10.0f));
                    recGamma.sTelemetry = "EM";

                    Assert.AreEqual(logGamma.Save(recGamma), true);

                    // insert gamma into tblLog
                    dtGammaTime += tsIntervalOneMinute;
                }

                dtTheTime += tsIntervalTenMinutes;
            }
            cnn.Close();

        }

        [TestMethod]
        public void TestMethodTVDWithGammaLAS()
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
            //DataTable tbl = log.Get(lst[0].iMsgCode.ToString(), 4700, 4800, CCommonTypes.TELEMETRY_TYPE.TT_MP);
            CWITSLookupTable witsLookUpTbl = new CWITSLookupTable();
            CMSAHubClient msaHubClient = new CMSAHubClient(ref witsLookUpTbl, ref cnn, sDataPath);
            CLogASCIIStandard lasMgr = new CLogASCIIStandard(ref cnn, msaHubClient, 7, 40, CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT);

            string fileName = "testTVDWithGamma.las";
            float startDepth = 4282.0f;
            float stopDepth = 4370.0f;
            DateTime dtDummyStartTime = DateTime.Now;
            DateTime dtDummyStopTime = DateTime.Now;
            bool showUnixTime = true;

            float fStep = 0.1f;
            CWellJob job = new CWellJob();
            lasMgr.CreateLasFile(job, CLASJobInfo.EXPORT_DATA_TYPE.TVD, fileName, startDepth, stopDepth, dtDummyStartTime, dtDummyStopTime, showUnixTime, fStep, CCommonTypes.TELEMETRY_TYPE_FOR_LAS.TT_EM, lst);

            cnn.Close();
        }

        [TestMethod]
        public void TestMethodNSChange()
        {
            // test data from https://directionaldrillingart.blogspot.com/2015/09/
            CSurveyCalculation sc = new CSurveyCalculation();
            CSurvey.REC recSvy1 = new CSurvey.REC();
            recSvy1.fInclination = 13.6f;
            recSvy1.fAzimuth = 315.20f;
            recSvy1.fBitDepth = 1914.75f;

            CSurvey.REC recSvy2 = new CSurvey.REC();
            recSvy2.fInclination = 10.7f;
            recSvy2.fAzimuth = 314.0f;
            recSvy2.fBitDepth = 1940.3f;
            
            double dblRes = sc.GetNSChange(recSvy1, recSvy2);
            Assert.AreEqual(dblRes.ToString("0.00"), "3.78");
        }

        [TestMethod]
        public void TestMethodEWChange()
        {
            // test data from https://directionaldrillingart.blogspot.com/2015/09/
            CSurveyCalculation sc = new CSurveyCalculation();
            CSurvey.REC recSvy1 = new CSurvey.REC();
            recSvy1.fInclination = 13.6f;
            recSvy1.fAzimuth = 315.20f;
            recSvy1.fBitDepth = 1914.75f;

            CSurvey.REC recSvy2 = new CSurvey.REC();
            recSvy2.fInclination = 10.7f;
            recSvy2.fAzimuth = 314.0f;
            recSvy2.fBitDepth = 1940.3f;

            double dblRes = sc.GetEWChange(recSvy1, recSvy2);
            string sRes = dblRes.ToString("0.00");
            Assert.AreEqual(sRes, "-3.82");
        }

        [TestMethod]
        public void TestClosureDistance()
        {
            // test data from https://directionaldrillingart.blogspot.com/2015/09/
            CSurveyCalculation sc = new CSurveyCalculation();
            CSurvey.REC recSvy1 = new CSurvey.REC();
            recSvy1.fInclination = 13.6f;
            recSvy1.fAzimuth = 315.20f;
            recSvy1.fBitDepth = 1914.75f;

            CSurvey.REC recSvy2 = new CSurvey.REC();
            recSvy2.fInclination = 10.7f;
            recSvy2.fAzimuth = 314.0f;
            recSvy2.fBitDepth = 1940.3f;

            double dblNSChange = sc.GetNSChange(recSvy1, recSvy2);
            double dblNS = 311.7 + dblNSChange;

            double dblEWChange = sc.GetEWChange(recSvy1, recSvy2);
            double dblEW;
            dblEW = dblEWChange - (double)(299.27);

            double dblRes = sc.GetClosureDistance(dblNS, dblEW);
            string sRes = dblRes.ToString("0.00");
            Assert.AreEqual(sRes, "437.49");
        }

        [TestMethod]
        public void TestClosureAzimuth()
        {
            // test data from https://directionaldrillingart.blogspot.com/2015/09/
            CSurveyCalculation sc = new CSurveyCalculation();
            CSurvey.REC recSvy1 = new CSurvey.REC();
            recSvy1.fInclination = 13.6f;
            recSvy1.fAzimuth = 315.20f;
            recSvy1.fBitDepth = 1914.75f;

            CSurvey.REC recSvy2 = new CSurvey.REC();
            recSvy2.fInclination = 10.7f;
            recSvy2.fAzimuth = 314.0f;
            recSvy2.fBitDepth = 1940.3f;

            double dblNSChange = sc.GetNSChange(recSvy1, recSvy2);
            double dblNS = 311.7 + dblNSChange;

            double dblEWChange = sc.GetEWChange(recSvy1, recSvy2);
            double dblEW;
            dblEW = dblEWChange - (double)(299.27);

            double dblRes = sc.GetClosureAzimuth(dblNS, dblEW);
            string sRes = dblRes.ToString("0.00");
            Assert.AreEqual(sRes, "316.16");
        }

        [TestMethod]
        public void TestDogLegSeverity()
        {
            // test data from https://directionaldrillingart.blogspot.com/2015/09/
            CSurveyCalculation sc = new CSurveyCalculation();
            CSurvey.REC recSvy1 = new CSurvey.REC();
            recSvy1.fInclination = 13.6f;
            recSvy1.fAzimuth = 315.20f;
            recSvy1.fBitDepth = 1914.75f;

            CSurvey.REC recSvy2 = new CSurvey.REC();
            recSvy2.fInclination = 10.7f;
            recSvy2.fAzimuth = 314.0f;
            recSvy2.fBitDepth = 1940.3f;

            double dblDogLeg = sc.GetDogLeg(recSvy1, recSvy2);
            CAngle angle = new CAngle();
            double dblDogLegDeg = angle.RadToDeg(dblDogLeg);
            double dblCourseLength = recSvy2.fBitDepth - recSvy1.fBitDepth;

            double dblRes = sc.GetDogLegSeverity(dblDogLegDeg, dblCourseLength, "ft");
            
            string sRes = dblRes.ToString("0.00");
            Assert.AreEqual(sRes, "3.42");
        }

        // should be run after fake survey data has been created from website
        // https://www.netwasgroup.us/services-2/minimum-curvature.html
        [TestMethod]
        public void TestSurveyCount()
        {            
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();

            CSurveyLog logSvy = new CSurveyLog(ref cnn);
            List<CSurvey.REC> lst = logSvy.GetSurveysForTVD();
            Assert.AreEqual(lst.Count, 6);
        }

        [TestMethod]
        public void TestTVDAfterSurveys()
        {
            // generate fake survey data
            // example data taken from 
            // https://www.netwasgroup.us/services-2/minimum-curvature.html
            REC[] arrRec = new REC[]
            {
                new REC(0, 0, 0 ),
                new REC(1000, 0, 0),
                new REC(1100,3.00,21.70),
                new REC(1200,6.00,26.50),
                new REC(1300,9.00,23.30),
                new REC(1400,12.00,20.30),
                new REC(1500,15.00,23.30),
                new REC(1600,18.00,23.90),
                new REC(1700,21.00,24.40),
                new REC(1800,24.00,23.40),
                new REC(1900,27.00,23.70),
                new REC(2000,30.00,23.30),
                new REC(2100,30.20,22.80),
                new REC(2200,30.40,22.50),
                new REC(2300,30.30,22.10),
                new REC(2400,30.60,22.40),
                new REC(2500,31.00,22.50),
                new REC(2600,31.20,21.60),
                new REC(2700,30.70,20.80),
                new REC(2800,31.40,20.90),
                new REC(2900,30.60,22.00),
                new REC(3000,30.50,22.50),
                new REC(3100,30.40,23.90),
                new REC(3200,30.00,24.50),
                new REC(3300,30.20,24.90),
                new REC(3400,31.00,25.70),
                new REC(3500,31.10,25.50),
                new REC(3600,32.00,24.40),
                new REC(3700,30.80,24.00),
                new REC(3800,30.60,22.30),
                new REC(3900,31.20,21.70),
                new REC(4000,30.80,20.80),
                new REC(4100,30.00,20.80),
                new REC(4200,29.70,19.80),
                new REC(4300,29.80,20.80),
                new REC(4400,29.50,21.10),
                new REC(4500,29.20,20.80),
                new REC(4600,29.00,20.60),
                new REC(4700,28.70,21.40),
                new REC(4800,28.50,21.20)
            };

            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();

            CSurveyLog logSvy = new CSurveyLog(ref cnn);
            CSurveyCalculation calc = new CSurveyCalculation();
            DataTable tblSurvey = logSvy.Get(CCommonTypes.TELEMETRY_TYPE_FOR_LAS.TT_BOTH.ToString(), "ACCEPT");            

            CSurvey.REC_CALC rekCalc = new CSurvey.REC_CALC();           
            double dblTVDTotal = 0.0;
            DataTable tblSvy = new DataTable();

            //***************************************************
            // add each survey and calculate the TVD for each survey station            
            //***************************************************
            CSurvey.REC recSvy = new CSurvey.REC();
            for (int i = 1; i < arrRec.Length; i++)
            {
                recSvy.fAzimuth = (float)arrRec[i].azm;
                recSvy.fInclination = (float)arrRec[i].inc;
                recSvy.fBitDepth = (float)arrRec[i].md;
                recSvy.TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_EM;
                recSvy.Status = CSurvey.STATUS.ACCEPT;
                recSvy.fDirToBit = 0.0f;
                recSvy.dtCreated = DateTime.Now;

                // insert data into tblSurvey                
                Assert.AreEqual(logSvy.Save(recSvy), true);

                rekCalc = logSvy.Calculate(recSvy);
                
                dblTVDTotal = rekCalc.fTVD;                
                             
                // insert into tblSurveyCalc
                Assert.AreEqual(logSvy.SaveCalc(rekCalc), true);
            }

            string sTVD = dblTVDTotal.ToString("0.0");
            
            Assert.AreEqual("4370.7", sTVD);           
        }
    }
}
