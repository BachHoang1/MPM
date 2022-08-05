using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPM.Data;
using System.Data;
using System.Data.Common;

namespace UnitTestMPM
{
    [TestClass]
    public class UnitTestTransducerTableTransaction
    {
        [TestMethod]
        public void TestMethodInsert()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();
            CLogTransducerSettingRecord rec = new CLogTransducerSettingRecord();
            rec.iDate = 20190902;
            rec.iTime = 120101;
            rec.sType = "3,000";
            rec.sUnit = "Psi";
            rec.fOffset = 34;
            rec.fGain = 2;

            CLogTransducerSetting log = new CLogTransducerSetting(ref cnn);
            bool b = log.Save(rec);
            cnn.Close();
            Assert.AreEqual(b, true);            
        }

        [TestMethod]
        public void TestMethodRead()
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            DbConnection cnn = fact.CreateConnection();

            cnn.ConnectionString = CCommonTypes.TEST_DB;
            cnn.Open();

            CLogTransducerSettingRecord rec = new CLogTransducerSettingRecord();
            rec.iDate = 20190902;
            rec.iTime = 120101;
            rec.sType = "3,000";
            rec.sUnit = "Psi";
            rec.fOffset = 34;
            rec.fGain = 2;

            CLogTransducerSetting log = new CLogTransducerSetting(ref cnn);
            bool b = log.Save(rec);
            
            DataTable dt = log.Get();
            cnn.Close();
            Assert.AreNotEqual(dt.Rows, 0);
        }
    }
}
