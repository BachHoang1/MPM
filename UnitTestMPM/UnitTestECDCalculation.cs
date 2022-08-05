using System;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPM.Data;
using MPM.DataAcquisition.Helpers;
using MPM.GUI;
using MPM.Utilities;
using static MPM.Data.CCommonTypes;

namespace UnitTestMPM
{
    [TestClass]
    public class UnitTestECDCalculation
    {
        public struct REC
        {
            public REC(double md_, double inc_, double azm_)
            {
                md = md_;
                inc = inc_;
                azm = azm_;
            }
            public double md;
            public double inc;
            public double azm;

        }

        [TestMethod]
        public void TestMethodECDWithSurvey()
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
            CLogDataLayer logDetect = new CLogDataLayer(ref cnn);

            CDPointLookupTable lookupTbl = new CDPointLookupTable();
            lookupTbl.Load();

           
            CUnitSelection units = new CUnitSelection();
            units.LoadDefaults();

            DateTime dtTheTime = DateTime.Now;
            TimeSpan tsIntervalTenMinutes = new TimeSpan(0, 10, 0);
            DateTime dtAnnularPressure = DateTime.Now;
            TimeSpan tsIntervalOneMinute = new TimeSpan(0, 10, 0);
            CSurveyCalculation calc = new CSurveyCalculation();

            CSurvey.REC_CALC rekCalc = new CSurvey.REC_CALC();
            double dblTVDTotal = 0.0;

            for (int i = 0; i < arrRec.Length; i++)
            {
                CSurvey.REC recSvy = new CSurvey.REC();

                recSvy.fAzimuth = (float)arrRec[i].azm;
                recSvy.fInclination = (float)arrRec[i].inc;
                recSvy.fBitDepth = (float)arrRec[i].md;
                recSvy.TelemetryType = CCommonTypes.TELEMETRY_TYPE.TT_EM;
                recSvy.Status = CSurvey.STATUS.ACCEPT;
                recSvy.fDirToBit = 0.0f;
                recSvy.dtCreated = dtTheTime;

                // insert data into tblSurvey                
                Assert.AreEqual(logSvy.Save(recSvy), true);

                rekCalc = logSvy.Calculate(recSvy);
                rekCalc.dtCreatedSvy = recSvy.dtCreated;
                dblTVDTotal = rekCalc.fTVD;

                // insert into tblSurveyCalc
                Assert.AreEqual(logSvy.SaveCalc(rekCalc), true);

                // generate fake annular pressure               
                CLogDataRecord recAnnularPressure = new CLogDataRecord();
                recAnnularPressure.iMessageCode = (int)Command.COMMAND_RESP_A_PRESSURE;
                recAnnularPressure.sCreated = dtAnnularPressure.ToString("yyyy-MM-dd HH:mm:ss.fff");
                recAnnularPressure.iDate = System.Convert.ToInt32(dtAnnularPressure.ToString("yyyyMMdd"));
                recAnnularPressure.iTime = System.Convert.ToInt32(dtAnnularPressure.ToString("HHmmss"));
                recAnnularPressure.sName = "A Pressure";
                recAnnularPressure.sValue = (i * 2).ToString();
                recAnnularPressure.sUnit = "Psi";
                recAnnularPressure.bParityError = false;
                recAnnularPressure.fDepth = recSvy.fBitDepth;
                recAnnularPressure.sTelemetry = "EM";

                Assert.AreEqual(logDetect.Save(recAnnularPressure), true);
                CWITSLookupTable witsLookupTbl = new CWITSLookupTable();
                witsLookupTbl.Load();
                // calculate ecd
                CEquivalentCirculatingDensity ecd = new CEquivalentCirculatingDensity(ref cnn, ref witsLookupTbl, ref lookupTbl, units);                
                float fECD = ecd.CalculateECD(i * 2, i * 2, ecd.GetTVD(recSvy), 10.0f, true);

                // insert into 
                CLogDataRecord recECD = new CLogDataRecord();
                recECD.iMessageCode = (int)Command.COMMAND_RESP_A_PRESSURE;
                recECD.sCreated = dtAnnularPressure.ToString("yyyy-MM-dd HH:mm:ss.fff");
                recECD.iDate = System.Convert.ToInt32(dtAnnularPressure.ToString("yyyyMMdd"));
                recECD.iTime = System.Convert.ToInt32(dtAnnularPressure.ToString("HHmmss"));
                recECD.sName = "ECD";
                recECD.sValue = fECD.ToString();
                recECD.sUnit = "ppg";
                recECD.bParityError = false;
                recECD.fDepth = recSvy.fBitDepth;
                recECD.sTelemetry = "EM";

                Assert.AreEqual(logDetect.Save(recECD), true);

                dtAnnularPressure += tsIntervalOneMinute;
                
                dtTheTime += tsIntervalTenMinutes;
            }
            cnn.Close();

            string sTVD = dblTVDTotal.ToString("0.0");

            Assert.AreEqual("4370.7", sTVD);
        }
    }
}
