using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPM.DataAcquisition.MultiStationAnalysis;

namespace UnitTestMPM
{
    [TestClass]
    public class UnitTestMSA
    {
        [TestMethod]
        public void TestMethodGetJobID()
        {
            CMSAClient client = new CMSAClient();
            //string sJobID = client.GetJobID();
            //Assert.IsTrue(sJobID.Length > 0);
        }

        [TestMethod]
        public void TestMethodGetSendQuickSurvey()
        {
            CMSAClient client = new CMSAClient();
            //string sJobID = client.SendQuickSurvey();
            //Assert.IsTrue(sJobID.Length > 0);
        }

        [TestMethod]
        public void TestMethodGetSendVectorSurvey()
        {
            CMSAClient client = new CMSAClient();
            //string sJobID = client.SendVectorSurvey();
            //Assert.IsTrue(sJobID.Length > 0);
        }
    }
}
