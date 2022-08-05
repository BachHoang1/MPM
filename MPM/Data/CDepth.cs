// author: hoan chau
// purpose: to track depth throughout the various states of drilling

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPM.DataAcquisition.Helpers;

namespace MPM.Data
{
    public class CDrillingBitDepth: CDrillingBase
    {
        public enum DepthUnits
        {
            None = -1,
            Feet = 0,
            Meters = 1,
        }

        public CDrillingBitDepth(ref DbConnection dbCnn_, int iMessageCode_)
        {
            m_dbCnn = dbCnn_;
            m_fValue = CCommonTypes.BAD_VALUE;
            m_iMsgCode = iMessageCode_;
        }

        public float GetLastDepth()
        {
            float fRetVal = CCommonTypes.BAD_VALUE;

            string sQuery = "SELECT MAX(depth) FROM tblDetect WHERE created = (SELECT MAX(created) FROM tblDetect WHERE depth > 0)";
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    fRetVal = System.Convert.ToSingle(reader[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return fRetVal;
        }  
        
        public bool GetDepthRange(out float fMinDepth_, out float fMaxDepth_)
        {
            bool bRetVal = false;
            fMinDepth_ = 0;
            fMaxDepth_ = 0;
            string sQuery = "SELECT MIN(depth), MAX(depth) FROM tblDetect WHERE depth >= 0";
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    fMinDepth_ = System.Convert.ToSingle(reader[0]);
                    fMaxDepth_ = System.Convert.ToSingle(reader[1]);
                    bRetVal = true;
                }                                    
            }
            catch (Exception ex)
            {                
                Console.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return bRetVal;
        }

        public bool GetTimeRange(out string sStartTime_, out string sStopTime_)
        {
            bool bRetVal = false;
            sStartTime_ = sStopTime_ = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            string sQuery = "SELECT MIN(created), MAX(created) FROM tblDetect WHERE depth >= 0";
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    sStartTime_ = reader[0].ToString();
                    sStopTime_ = reader[1].ToString();
                    if (sStartTime_.Trim().Length > 0 &&
                        sStopTime_.Trim().Length > 0)
                        bRetVal = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return bRetVal;
        }

    }
}
