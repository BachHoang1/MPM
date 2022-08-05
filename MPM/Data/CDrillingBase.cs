// author: hoan chau
// purpose: base class for drilling parameters or states

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    public class CDrillingBase
    {
        public DbConnection m_dbCnn;
        protected CDetectDataLayer m_DataLayer;

        protected float m_fValue;
        protected string m_sUnit;
        protected int m_iMsgCode;
        protected CDPointLookupTable.DPointInfo m_dpointInfo;


        public CDrillingBase()
        {
            
        }

        public CDrillingBase(ref DbConnection dbCnn_, int iMessageCode_)
        {
            m_dbCnn = dbCnn_;
            m_fValue = CCommonTypes.BAD_VALUE;
            m_iMsgCode = iMessageCode_;
        }

        public virtual float GetLast()
        {
            float fRetVal = 0.0f;
            return fRetVal;
        }

        public void SetListener(CDetectDataLayer dataLayer_)
        {
            m_DataLayer = dataLayer_;
            m_DataLayer.Changed += new ChangedEventHandler(Handle);
        }

        protected void Handle(object sender, CEventDPoint e)
        {
            if (e.m_ID == m_iMsgCode)
            {
                float fResult = 0.0f;
                if (float.TryParse(e.m_sValue, out fResult))
                    m_fValue = fResult;
            }
        }

        public float Get()
        {
            return m_fValue;
        }

        public void Set(float fVal_)
        {
            m_fValue = fVal_;
        }
        

        public void SetDPointInfo(CDPointLookupTable.DPointInfo val_)
        {
            m_dpointInfo = val_;
        }

        public CDPointLookupTable.DPointInfo GetDPointInfo()
        {
            return m_dpointInfo;
        }

    }
}
