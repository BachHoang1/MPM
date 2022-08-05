// author: hoan chau
// purpose: to calculate pump pressure based on incoming samples
//          as a result, pulse height can be calculated as well

using MPM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Utilities
{
    public class CPumpPressure
    {        
        private const float LPF_BETA_DEFAULT = 0.02f;  // 0<ß<1
        private const int MAX_SAMPLE_SETS = 5;  // each set represents 1 second 

        List<float> m_RunningAvgedSampleList = new List<float>();  //4/22/22
        List<int> m_iSampleList = new List<int>();    
      
       // List<float> m_MaxPressureList = new List<float>();
        //List<float> m_MinPressureList = new List<float>();

        struct SAMPLE_AGGREGATE
        {
            public float fMax;
            public float fMin;
            //public float fAvg;
        }
        private List<SAMPLE_AGGREGATE> m_lstSampleAgg = new List<SAMPLE_AGGREGATE>();

        private CDetectDataLayer m_DataLayer;

        CCommonTypes.UNIT_SET m_iUnitSet = CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL;
        
        private float m_fMPSmoothData = 0.0f;
        
        public event ChangedEventHandler Changed;

        private CPressureTransducer m_PressureTransducer;
             
        public void Init()
        {
            m_lstSampleAgg.Clear();

            //m_MaxPressureList.Clear(); //4/22/22
            //m_MinPressureList.Clear(); //4/22/22
        }

        public void SetUnitSet(CCommonTypes.UNIT_SET iVal_)
        {
            m_iUnitSet = iVal_;
        }

        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.AcquiredMPSamples += new MPSampleAcquiredEventHandler(GetSamples);
        }

        public void SetPressureTransducer(ref CPressureTransducer pressureTransducer_)
        {
            m_PressureTransducer = pressureTransducer_;
        }

        //4/29/22
         /* //5/3/22
        void SetSamples560C(CEventRawMPSamples evRaw_)
        {
            bool bIsMP = false;

            if (evRaw_ == null)
            {
                return;
            }
          
            if (evRaw_.m_shArrSample.Count() <= 0) //4/19/22
            {  
                return;
            }
          
            if (evRaw_.m_iTelemetryType == 0)
                bIsMP = true;

            try
            {
                if (bIsMP)
                {
                    float fMPSmoothDataMin = float.MaxValue;
                    float fMPSmoothDataMax = float.MinValue;

                    for (int i = 0; i < evRaw_.m_shArrSample.Count(); i++)
                    {
                        //float fCurrentVal = m_iUnitSet != CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC ? evRaw_.m_shArrSample[i] * 0.145038f : evRaw_.m_shArrSample[i]; // convert kpa to psi          
                        float fCurrentVal = evRaw_.m_shArrSample[i] * m_PressureTransducer.GetTransducerScale() * m_PressureTransducer.GetTransducerGain() + m_PressureTransducer.GetPressureOffset();

                        // inline low pass filter to speed up code
                        float fLowPassVal = m_fMPSmoothData = m_fMPSmoothData - (LPF_BETA_DEFAULT * (m_fMPSmoothData - fCurrentVal));
                        if (fLowPassVal > fMPSmoothDataMax)
                            fMPSmoothDataMax = fLowPassVal;
                        else if (fLowPassVal < fMPSmoothDataMin)
                            fMPSmoothDataMin = fLowPassVal;
                    }

                    SAMPLE_AGGREGATE saNew = new SAMPLE_AGGREGATE();
                    saNew.fMax = fMPSmoothDataMax;
                    saNew.fMin = fMPSmoothDataMin;
                    m_lstSampleAgg.Add(saNew);
                    if (m_lstSampleAgg.Count() == MAX_SAMPLE_SETS)
                    {
                        float fMinVal = float.MaxValue;
                        float fMaxVal = float.MinValue;
                        for (int k = 0; k < m_lstSampleAgg.Count; k++)
                        {
                            if (m_lstSampleAgg[k].fMin < fMinVal)
                                fMinVal = m_lstSampleAgg[k].fMin;

                            if (m_lstSampleAgg[k].fMax > fMaxVal)
                                fMaxVal = m_lstSampleAgg[k].fMax;
                        }

                        CEventDPoint ev = new CEventDPoint();
                        ev.m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_MP;
                        ev.m_sValue = fMinVal.ToString("0.0");
                        ev.m_DateTime = DateTime.Now;  // this should come from detect
                        ev.m_ID = 88;

                        if (Changed != null)
                            Changed(this, ev);

                        // pulse height
                        ev.m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_MP;
                        ev.m_sValue = (fMaxVal - fMinVal).ToString("0.0");
                        ev.m_DateTime = DateTime.Now;  // this should come from detect
                        ev.m_ID = 20000;

                        if (Changed != null)
                            Changed(this, ev);

                        m_lstSampleAgg.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("GetSamples Error: " + ex.Message);
            }
        }
        */ //5/3/22

        void SetSamples560R(CEventRawMPSamples evRaw_)
        {
            //???  //4/29/22
            //float temp = (float)1.99;
            //CEventDPoint ev1 = new CEventDPoint();
            //ev1.m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_MP;
            //ev1.m_sValue = temp.ToString("0.00");
            //ev1.m_DateTime = DateTime.Now;  // this should come from detect
            //ev1.m_ID = 88;
            //
            //if (Changed != null)
            //    Changed(this, ev1);
            //
            //// pulse height
            //
            //temp = (float)0.45;
            //ev1.m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_MP;
            //ev1.m_sValue = temp.ToString("0.00");
            //ev1.m_DateTime = DateTime.Now;  // this should come from detect
            //ev1.m_ID = 20000;
            //
            //if (Changed != null)
            //    Changed(this, ev1);
            //
            //return;
            //4/29/22

            bool bIsMP = false;

            int uShortMax = (int)ushort.MaxValue;
            int uShortMin = (int)ushort.MinValue;

            float maxRunAvg = 0;
            float minRunAvg = 0;

            int sampleRunAvgWindowSize = 6;

            float pressureHigh = 0;
            float pressureLow = 0;

            int movingSum = 0;
            float runAvg = 0;
            int sampleCounts = 0;

            int iRawSampleData = 0;
            int isampleData = 0;
            int itemp = 0;

            if (evRaw_ == null)
            {
                return;
            }

            if (evRaw_.m_shArrSample.Count() <= 0)
            {
                return;
            }

            m_RunningAvgedSampleList.Clear();
            m_iSampleList.Clear();

            if (evRaw_.m_shArrSample.Count() < 6)
            {
                sampleRunAvgWindowSize = evRaw_.m_shArrSample.Count();
            }

            if (evRaw_.m_iTelemetryType == 0)
                bIsMP = true;

            // there are three type of tranduser used: 0 = 3000, 1 = 5000, and  2 = 10000 PSI
            int transducerType = m_PressureTransducer.GetTransducerType();
            float maxPressure = 0;

            if (transducerType == 0)
            {
                maxPressure = 3000;
            }
            else if (transducerType == 1)
            {
                maxPressure = 5000;
            }
            else if (transducerType == 2)
            {
                maxPressure = 10000;
            }

            float scale = (float)maxPressure / (float)uShortMax;

            try
            {
                if (bIsMP)
                {
                    //Convert the sample data to the proper form
                    //and get the sample points running average
                    //Note: the sample points running average
                    //window size is 6

                    for (int i = 0; i < evRaw_.m_shArrSample.Count(); i++)
                    {
                        iRawSampleData = (int)evRaw_.m_shArrSample[i];

                        if (iRawSampleData >= 0)
                        {
                            isampleData = 32767 + iRawSampleData;
                        }
                        else
                        {
                            itemp = iRawSampleData & (0xFFFF);
                            isampleData = itemp - 32768;
                        }

                        if (sampleCounts >= sampleRunAvgWindowSize)
                        {
                            movingSum = movingSum - m_iSampleList[0];

                            m_iSampleList.RemoveAt(0);

                            m_iSampleList.Add(isampleData);

                            movingSum = movingSum + isampleData;

                            runAvg = (float)movingSum / (float)sampleRunAvgWindowSize;

                            m_RunningAvgedSampleList.Add(runAvg);
                        }
                        else
                        {
                            m_iSampleList.Add(isampleData);

                            sampleCounts++;

                            movingSum = movingSum + isampleData;

                            if (sampleCounts >= sampleRunAvgWindowSize)
                            {
                                runAvg = (float)movingSum / (float)sampleRunAvgWindowSize;

                                m_RunningAvgedSampleList.Add(runAvg);
                            }
                        }
                    }

                    maxRunAvg = m_RunningAvgedSampleList.Max();
                    minRunAvg = m_RunningAvgedSampleList.Min();

                    if (maxRunAvg > uShortMax)
                        maxRunAvg = uShortMax;

                    if (minRunAvg < uShortMin)
                        minRunAvg = uShortMin;

                    pressureHigh = scale * maxRunAvg + m_PressureTransducer.GetPressureOffset();
                    pressureLow = scale * minRunAvg + m_PressureTransducer.GetPressureOffset();

                    CEventDPoint ev = new CEventDPoint();
                    ev.m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_MP;
                    ev.m_sValue = pressureHigh.ToString("0.00");
                    ev.m_DateTime = DateTime.Now;  // this should come from detect
                    ev.m_ID = 88;

                    if (Changed != null)
                        Changed(this, ev);

                    // pulse height
                    ev.m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_MP;
                    ev.m_sValue = (pressureHigh - pressureLow).ToString("0.00");
                    ev.m_DateTime = DateTime.Now;  // this should come from detect
                    ev.m_ID = 20000;

                    if (Changed != null)
                        Changed(this, ev);

                } //if (bIsMP)
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("GetSamples Error: " + ex.Message);
            }
        }

        public CPressureTransducer GetPressureTransducer()
        {
            return m_PressureTransducer;
        }
        //4/29/22

        //public void SetSamples(CEventRawMPSamples evRaw_)
        //{
        //    bool bIsMP = false;
        //
        //    if (evRaw_.m_shArrSample.Count() <= 0) //4/19/22
        //    {  
        //        return;
        //    }
        //
        //    if (evRaw_.m_iTelemetryType == 0)
        //        bIsMP = true;
        //
        //    try
        //    {
        //        if (bIsMP)
        //        {
        //            //4/19/22float fMPSmoothDataMin = float.MaxValue;
        //            //4/19/22float fMPSmoothDataMax = float.MinValue;
        //
        //            float fMPSmoothDataMin = (float)ushort.MaxValue;
        //            float fMPSmoothDataMax = (float)ushort.MinValue;
        //
        //            for (int i = 0; i < evRaw_.m_shArrSample.Count(); i++)
        //            {
        //                //float fCurrentVal = m_iUnitSet != CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC ? evRaw_.m_shArrSample[i] * 0.145038f : evRaw_.m_shArrSample[i]; // convert kpa to psi
        //                //
        //                ushort usSample = (ushort) evRaw_.m_shArrSample[i];  //4/19/22
        //                //4/19/22float fCurrentVal = evRaw_.m_shArrSample[i] * m_PressureTransducer.GetTransducerScale() * m_PressureTransducer.GetTransducerGain() + m_PressureTransducer.GetPressureOffset();
        //                float fCurrentVal = (float)usSample * m_PressureTransducer.GetTransducerScale() * m_PressureTransducer.GetTransducerGain() + m_PressureTransducer.GetPressureOffset(); //4/19/22
        //
        //                // inline low pass filter to speed up code
        //                float fLowPassVal = m_fMPSmoothData = m_fMPSmoothData - (LPF_BETA_DEFAULT * (m_fMPSmoothData - fCurrentVal));
        //                if (fLowPassVal > fMPSmoothDataMax)
        //                    fMPSmoothDataMax = fLowPassVal;
        //                else if (fLowPassVal < fMPSmoothDataMin)
        //                    fMPSmoothDataMin = fLowPassVal;
        //            }
        //
        //            SAMPLE_AGGREGATE saNew = new SAMPLE_AGGREGATE();
        //            saNew.fMax = fMPSmoothDataMax;
        //            saNew.fMin = fMPSmoothDataMin;
        //            m_lstSampleAgg.Add(saNew);
        //            if (m_lstSampleAgg.Count() == MAX_SAMPLE_SETS)
        //            {
        //                //4/19/22float fMinVal = float.MaxValue;
        //                //4/19/22float fMaxVal = float.MinValue;
        //
        //                float fMinVal = (float)ushort.MaxValue;
        //                float fMaxVal = (float)ushort.MinValue;
        //
        //                for (int k = 0; k < m_lstSampleAgg.Count; k++)
        //                {
        //                    if (m_lstSampleAgg[k].fMin < fMinVal)
        //                        fMinVal = m_lstSampleAgg[k].fMin;
        //
        //                    if (m_lstSampleAgg[k].fMax > fMaxVal)
        //                        fMaxVal = m_lstSampleAgg[k].fMax;
        //                }
        //
        //                CEventDPoint ev = new CEventDPoint();
        //                ev.m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_MP;
        //                ev.m_sValue = fMinVal.ToString("0.0");
        //                ev.m_DateTime = DateTime.Now;  // this should come from detect
        //                ev.m_ID = 88;
        //
        //                if (Changed != null)
        //                    Changed(this, ev);
        //
        //                // pulse height
        //                ev.m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_MP;
        //                ev.m_sValue = (fMaxVal - fMinVal).ToString("0.0");
        //                ev.m_DateTime = DateTime.Now;  // this should come from detect
        //                ev.m_ID = 20000;
        //
        //                if (Changed != null)
        //                    Changed(this, ev);
        //
        //                m_lstSampleAgg.Clear();
        //            }
        //        } //if (bIsMP)
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.Write("GetSamples Error: " + ex.Message);
        //    }
        //}

        //5/3/22
        void SetSamples560C(CEventRawMPSamples evRaw_)
        {
            bool bIsMP = false;

            //int uShortMax = (int)ushort.MaxValue;
            //int uShortMin = (int)ushort.MinValue;
            //Note: the 560C ADC Count Max and Min
            //      are imperical determined
            int ACDCountMax = 28800;
            int ADCCountMin = 60;

            float maxRunAvg = 0;
            float minRunAvg = 0;

            int sampleRunAvgWindowSize = 6;

            float pressureHigh = 0;
            float pressureLow = 0;

            int movingSum = 0;
            float runAvg = 0;
            int sampleCounts = 0;

            //int iRawSampleData = 0;
            int isampleData = 0;    

            if (evRaw_ == null)
            {
                return;
            }

            if (evRaw_.m_shArrSample.Count() <= 0)
            {
                return;
            }

            m_RunningAvgedSampleList.Clear();
            m_iSampleList.Clear();

            if (evRaw_.m_shArrSample.Count() < 6)
            {
                sampleRunAvgWindowSize = evRaw_.m_shArrSample.Count();
            }

            if (evRaw_.m_iTelemetryType == 0)
                bIsMP = true;

            // there are three type of tranduser used: 0 = 3000, 1 = 5000, and  2 = 10000 PSI
            int transducerType = m_PressureTransducer.GetTransducerType();
            float maxPressure = 0;

            if (transducerType == 0)
            {
                maxPressure = 3000;
            }
            else if (transducerType == 1)
            {
                maxPressure = 5000;
            }
            else if (transducerType == 2)
            {
                maxPressure = 10000;
            }

            float scale = (float)maxPressure / (float)(ACDCountMax - ADCCountMin);

            try
            {
                if (bIsMP)
                {
                    //Convert the sample data to the proper form
                    //and get the sample points running average
                    //Note: the sample points running average
                    //window size is 6

                    for (int i = 0; i < evRaw_.m_shArrSample.Count(); i++)
                    {
                        isampleData = (int)evRaw_.m_shArrSample[i];

                        if (isampleData > ACDCountMax)
                        {
                            isampleData = ACDCountMax;
                        }
                        else if (isampleData < ADCCountMin)
                        {
                            isampleData = ADCCountMin;
                        }

                        if (sampleCounts >= sampleRunAvgWindowSize)
                        {
                            movingSum = movingSum - m_iSampleList[0];

                            m_iSampleList.RemoveAt(0);

                            m_iSampleList.Add(isampleData);

                            movingSum = movingSum + isampleData;

                            runAvg = (float)movingSum / (float)sampleRunAvgWindowSize;

                            m_RunningAvgedSampleList.Add(runAvg);
                        }
                        else
                        {
                            m_iSampleList.Add(isampleData);

                            sampleCounts++;

                            movingSum = movingSum + isampleData;

                            if (sampleCounts >= sampleRunAvgWindowSize)
                            {
                                runAvg = (float)movingSum / (float)sampleRunAvgWindowSize;

                                m_RunningAvgedSampleList.Add(runAvg);
                            }
                        }
                    }

                    maxRunAvg = m_RunningAvgedSampleList.Max();
                    minRunAvg = m_RunningAvgedSampleList.Min();

                    //if (maxRunAvg > uShortMax)
                    //    maxRunAvg = uShortMax;

                    //if (minRunAvg < uShortMin)
                    //    minRunAvg = uShortMin;

                    //pressureHigh = scale * maxRunAvg + m_PressureTransducer.GetPressureOffset();
                    //pressureLow = scale * minRunAvg + m_PressureTransducer.GetPressureOffset();

                    pressureHigh = scale * maxRunAvg;
                    pressureLow = scale * minRunAvg;

                    CEventDPoint ev = new CEventDPoint();
                    ev.m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_MP;
                    ev.m_sValue = pressureHigh.ToString("0.00");
                    ev.m_DateTime = DateTime.Now;  // this should come from detect
                    ev.m_ID = 88;

                    if (Changed != null)
                        Changed(this, ev);

                    // pulse height
                    ev.m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_MP;
                    ev.m_sValue = (pressureHigh - pressureLow).ToString("0.00");
                    ev.m_DateTime = DateTime.Now;  // this should come from detect
                    ev.m_ID = 20000;

                    if (Changed != null)
                        Changed(this, ev);

                } //if (bIsMP)
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("GetSamples Error: " + ex.Message);
            }
        }
        //5/3/22

        //4/22/22
        public void SetSamples(CEventRawMPSamples evRaw_)
        { 
            if (m_PressureTransducer.GetReceiverType() == 0) // Receiver Type: 0 = 560R, 1 = 560C //4/29/22
            {
                SetSamples560R(evRaw_);
            }
            else
            {
                SetSamples560C(evRaw_);
            }
        }
        public void GetSamples(object sender, CEventRawMPSamples evRaw_)
        {
            bool bIsMP = false;

            if (evRaw_.m_shArrSample.Count() <= 0) //4/19/22
            {
                return;
            }

            if (evRaw_.m_iTelemetryType == 0)
                bIsMP = true;
            
            try
            {
                if (bIsMP)
                {                                        
                    //4/19/22float fMPSmoothDataMin = float.MaxValue;
                    //4/19/22float fMPSmoothDataMax = float.MinValue;

                    float fMPSmoothDataMin = (float)ushort.MaxValue;
                    float fMPSmoothDataMax = (float)ushort.MinValue;

                    for (int i = 0; i < evRaw_.m_shArrSample.Count(); i++)
                    {
                        //float fCurrentVal = m_iUnitSet != CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC ? evRaw_.m_shArrSample[i] * 0.145038f : evRaw_.m_shArrSample[i]; // convert kpa to psi          
                        ushort usSample = (ushort)evRaw_.m_shArrSample[i];  //4/19/22
                        //4/19/22float fCurrentVal = evRaw_.m_shArrSample[i] * m_PressureTransducer.GetTransducerScale() * m_PressureTransducer.GetTransducerGain() + m_PressureTransducer.GetPressureOffset();
                        float fCurrentVal = (float)usSample * m_PressureTransducer.GetTransducerScale() * m_PressureTransducer.GetTransducerGain() + m_PressureTransducer.GetPressureOffset();

                        // inline low pass filter to speed up code
                        float fLowPassVal = m_fMPSmoothData = m_fMPSmoothData - (LPF_BETA_DEFAULT * (m_fMPSmoothData - fCurrentVal));
                        if (fLowPassVal > fMPSmoothDataMax)
                            fMPSmoothDataMax = fLowPassVal;
                        else if (fLowPassVal < fMPSmoothDataMin)
                            fMPSmoothDataMin = fLowPassVal;                       
                    }

                    SAMPLE_AGGREGATE saNew = new SAMPLE_AGGREGATE();
                    saNew.fMax = fMPSmoothDataMax;
                    saNew.fMin = fMPSmoothDataMin;
                    m_lstSampleAgg.Add(saNew);
                    if (m_lstSampleAgg.Count() == MAX_SAMPLE_SETS)
                    {
                        //4/19/22float fMinVal = float.MaxValue;
                        //4/19/22float fMaxVal = float.MinValue;

                        float fMinVal = (float)ushort.MaxValue;
                        float fMaxVal = (float)ushort.MinValue;

                        for (int k = 0; k < m_lstSampleAgg.Count; k++)
                        {
                            if (m_lstSampleAgg[k].fMin < fMinVal)
                                fMinVal = m_lstSampleAgg[k].fMin;

                            if (m_lstSampleAgg[k].fMax > fMaxVal)
                                fMaxVal = m_lstSampleAgg[k].fMax;
                        }
                        
                        CEventDPoint ev = new CEventDPoint();
                        ev.m_iTechnology =  CCommonTypes.TELEMETRY_TYPE.TT_MP;
                        ev.m_sValue = fMinVal.ToString("0.0");
                        ev.m_DateTime = DateTime.Now;  // this should come from detect
                        ev.m_ID = 88;
                        
                        if (Changed != null)
                            Changed(this, ev);

                        // pulse height
                        ev.m_iTechnology = CCommonTypes.TELEMETRY_TYPE.TT_MP;
                        ev.m_sValue = (fMaxVal - fMinVal).ToString("0.0");
                        ev.m_DateTime = DateTime.Now;  // this should come from detect
                        ev.m_ID = 20000;

                        if (Changed != null)
                            Changed(this, ev);

                        m_lstSampleAgg.Clear();
                    }
                } //if (bIsMP)
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("GetSamples Error: " + ex.Message);
            }
        }
    }
}
