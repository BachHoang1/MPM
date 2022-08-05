using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPM.Data;
using System.Data;
using System.Data.Common;

namespace UnitTestMPM
{
    [TestClass]
    public class UnitTestSurveyLog
    {
        [TestMethod]
        public void TestMethodInsert()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();
            
            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();

            CSurvey.REC rec = new CSurvey.REC();
            rec.fInclination = 128.83f;
            rec.fAzimuth = 75.5418f;
            rec.fGTotal = 1.4521f;
            rec.fMTotal = 0.712214f;
            rec.fDipAngle = 36.2609f;
            rec.fBitDepth = 5225.0f;
            rec.fDirToBit = 225.0f;
            rec.fMX = 1;
            rec.fMY = 2;
            rec.fMZ = 3;
            rec.fAX = 1;
            rec.fAY = 2;
            rec.fAZ = 3;
            rec.dtCreated = DateTime.Now;
            rec.Status = CSurvey.STATUS.NONE;
            CSurveyLog log = new CSurveyLog(ref cnn);
            Assert.AreEqual(log.Save(rec), true);

            Assert.AreEqual(log.Update(rec.dtCreated, CSurvey.STATUS.ACCEPT.ToString(), rec.fBitDepth), true);

            cnn.Close();
        }

        [TestMethod]
        public void TestMethodGetCount()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();

            CSurveyLog log = new CSurveyLog(ref cnn);
            int i = log.GetCount();

            cnn.Close();            
        }
    }
}
