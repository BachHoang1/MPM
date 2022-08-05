using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPM.Data;

namespace UnitTestMPM
{
    [TestClass]
    public class UnitTestFormSettings
    {
        [TestMethod]
        public void TestMethodLoad()
        {
            CWidgetInfoLookupTable formSettingsTable = new CWidgetInfoLookupTable();
            string sTbl = formSettingsTable.Load();
            Assert.AreEqual(sTbl, "Form");
        }

        [TestMethod]
        public void TestMethodGet()
        {
            CWidgetInfoLookupTable formSettingsTable = new CWidgetInfoLookupTable();
            string sTbl = formSettingsTable.Load();
            int iMessageCode = formSettingsTable.GetMessageCode("FormQualifiers", "userControlDPoint1");
            Assert.AreEqual(iMessageCode, 18);
        }

        [TestMethod]
        public void TestMethodSet()
        {
            CWidgetInfoLookupTable formSettingsTable = new CWidgetInfoLookupTable();
            string sTbl = formSettingsTable.Load();
            bool bSuccess = formSettingsTable.Set("FormQualifiers", "userControlDPoint1", 9);
            int iMessageCode = formSettingsTable.GetMessageCode("FormQualifiers", "userControlDPoint1");
            Assert.AreEqual(iMessageCode, 9);
        }

        [TestMethod]
        public void TestMethodSave()
        {
            CWidgetInfoLookupTable formSettingsTable = new CWidgetInfoLookupTable();
            string sTbl = formSettingsTable.Load();
            formSettingsTable.Set("FormQualifiers", "userControlDPoint1", 9);
            formSettingsTable.Save();

            // reload 2nd time
            formSettingsTable.Load();
            int iMessageCode = formSettingsTable.GetMessageCode("FormQualifiers", "userControlDPoint1");
            Assert.AreEqual(iMessageCode, 9);
            formSettingsTable.Set("FormQualifiers", "userControlDPoint1", 10);
            formSettingsTable.Save();

            // reload 3rd time
            formSettingsTable.Load();
            iMessageCode = formSettingsTable.GetMessageCode("FormQualifiers", "userControlDPoint1");
            Assert.AreEqual(iMessageCode, 10);
        }

    }
}
