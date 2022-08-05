using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License;

namespace UnitTestLicense
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethodCheckTblVersion0()
        {
            CLicenseDatabase dbObject = new CLicenseDatabase();
            int iVersion = dbObject.CheckTblVersionExists();
            Assert.AreNotEqual(iVersion, -1);
        }

        [TestMethod]
        public void TestMethodUpgradeTblVersion0to1()
        {
            CLicenseDatabase dbObject = new CLicenseDatabase();
            int iVersion = dbObject.CheckTblVersionExists();
            if (iVersion == -1)
            {
                // run upgrade script 
                bool bRetVal = dbObject.UpgradeVersion0to1();
                Assert.AreNotEqual(bRetVal, true);
            }
        }
    }
}
