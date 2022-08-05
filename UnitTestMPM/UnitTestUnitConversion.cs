using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPM.Utilities;

namespace UnitTestMPM
{
    [TestClass]
    public class UnitTestUnitConversion
    {
        [TestMethod]
        public void TestMethodTemperatureCelsius()
        {
            CUnitTemperature temperature = new CUnitTemperature();
            temperature.SetUnitType(MPM.Data.CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC);
            float fFromVal = 5.0f;
            float fNewVal = temperature.Convert(fFromVal);
            Assert.AreEqual(fNewVal, -15.0f);
        }

        [TestMethod]
        public void TestMethodTemperatureFahrenheit()
        {
            CUnitTemperature temperature = new CUnitTemperature();
            temperature.SetUnitType(MPM.Data.CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL);
            float fFromVal = 5.0f;
            float fNewVal = temperature.Convert(fFromVal);
            Assert.AreEqual(fNewVal, 41.0f);
        }

        [TestMethod]
        public void TestMethodTemperatureNoConversion()
        {
            CUnitTemperature temperature = new CUnitTemperature();
            temperature.SetUnitType(MPM.Data.CCommonTypes.UNIT_SET.UNIT_SET_BY_EACH_DPOINT);
            float fFromVal = 5.0f;
            float fNewVal = temperature.Convert(fFromVal);
            Assert.AreEqual(fNewVal, 5.0f);
        }

        [TestMethod]
        public void TestMethodLengthMeters()
        {
            CUnitLength length = new CUnitLength();
            length.SetUnitType(MPM.Data.CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC);
            float fFromVal = 5.0f;
            float fNewVal = length.Convert(fFromVal);
            Assert.AreEqual(fNewVal, 1.524f);
        }

        [TestMethod]
        public void TestMethodROPMeters()
        {
            CUnitRateOfPenetration length = new CUnitRateOfPenetration();
            length.SetUnitType(MPM.Data.CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC);
            float fFromVal = 5.0f;
            float fNewVal = length.Convert(fFromVal);
            Assert.AreEqual(fNewVal, 1.524f);
        }

        [TestMethod]
        public void TestMethodLengthFeet()
        {
            CUnitLength length = new CUnitLength();
            length.SetUnitType(MPM.Data.CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL);
            float fFromVal = 5.0f;
            float fNewVal = length.Convert(fFromVal);
            Assert.AreEqual(fNewVal, 16.4040012f);
        }

        [TestMethod]
        public void TestMethodROPFeet()
        {
            CUnitRateOfPenetration length = new CUnitRateOfPenetration();
            length.SetUnitType(MPM.Data.CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL);
            float fFromVal = 5.0f;
            float fNewVal = length.Convert(fFromVal);
            Assert.AreEqual(fNewVal, 16.4040012f);
        }

        [TestMethod]
        public void TestMethodPressureKPa()
        {
            CUnitPressure pressure = new CUnitPressure();
            pressure.SetUnitType(MPM.Data.CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC);
            float fFromVal = 5.0f;
            float fNewVal = pressure.Convert(fFromVal);
            Assert.AreEqual(fNewVal, 34.4735f);
        }

        [TestMethod]
        public void TestMethodPressurePSI()
        {
            CUnitPressure pressure = new CUnitPressure();
            pressure.SetUnitType(MPM.Data.CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL);
            float fFromVal = 5.0f;
            float fNewVal = pressure.Convert(fFromVal);
            Assert.AreEqual(fNewVal, 0.72499994f);
        }
    }
}
