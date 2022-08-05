using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPM.Data;

namespace UnitTestMPM
{
    [TestClass]
    public class UnitTestXMLFileData
    {
        [TestMethod]
        public void TestMethodLoad()
        {
            CDPointLookupTable DPointTable = new CDPointLookupTable();
            string sTbl = DPointTable.Load();
            Assert.AreEqual(sTbl, "Command");            
        }

        [TestMethod]
        public void TestMethodGet()
        {
            CDPointLookupTable DPointTable = new CDPointLookupTable();
            string sTbl = DPointTable.Load();
            CDPointLookupTable.DPointInfo dpi = DPointTable.Get(8);
            Assert.AreEqual(dpi.sDescriptiveName, "Inclination");
        }

        [TestMethod]
        public void TestMethodSetDisplayName()
        {
            CDPointLookupTable DPointTable = new CDPointLookupTable();
            string sTbl = DPointTable.Load();

            DPointTable.SetDisplayName(8, "Inclination", "INC_Test");
            CDPointLookupTable.DPointInfo dpi = DPointTable.Get(8);
            Assert.AreEqual(dpi.sDisplayName, "INC_Test");

            DPointTable.SetDisplayName(8, "Inclination", "INC");  // set it back
            dpi = DPointTable.Get(8);
            Assert.AreEqual(dpi.sDisplayName, "INC");
        }

        [TestMethod]
        public void TestMethodSave()
        {
            CDPointLookupTable DPointTable = new CDPointLookupTable();
            string sTbl = DPointTable.Load();
            DPointTable.SetDisplayName(9, "Azimuth", "AZM_Test");
            DPointTable.SetUnitName(10211, "WOB", "FT/KG");           
            DPointTable.Save();

            // reload 2nd time
            DPointTable.Load();
            CDPointLookupTable.DPointInfo dpi = DPointTable.Get(9);
            Assert.AreEqual(dpi.sDisplayName, "AZM_Test");
            DPointTable.SetDisplayName(9, "Azimuth", "AZM");

            dpi = DPointTable.Get(10211);
            Assert.AreEqual(dpi.sUnits, "FT/KG");
            DPointTable.SetUnitName(10211, "WOB", "KLB");            
            DPointTable.Save();

            // reload 3rd time
            DPointTable.Load();
            dpi = DPointTable.Get(9);
            Assert.AreEqual(dpi.sDisplayName, "AZM");

            dpi = DPointTable.Get(10211);
            Assert.AreEqual(dpi.sUnits, "KLB");
        }
    }
}
