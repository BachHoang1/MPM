// author: hoan chua
// purpose: to allow access to the data for multi-station analysis settings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    public class CMSASettings
    {
        private string m_sURL;
        private int m_iPortNumber;
        private string m_sAPIKey;

        public CMSASettings()
        {
            // set defaults
        }

        public void SetURL(string sVal_)
        {
            m_sURL = sVal_;
        }

        public string GetURL()
        {
            return m_sURL;
        }

        public void SetPortNumber(int iVal_)
        {
            m_iPortNumber = iVal_;
        }

        public int GetPortNumber()
        {
            return m_iPortNumber;
        }

        public void SetAPIKey(string sVal_)
        {
            m_sAPIKey = sVal_;
        }

        public string GetAPIKey()
        {
            return m_sAPIKey;
        }

        public void Load()
        {

        }

        public void Save()
        {

        }


    }
}
