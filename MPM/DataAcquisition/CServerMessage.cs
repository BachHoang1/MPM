// author: hoan chau
// purpose: queue of messages received from the server that the client needs to process

using MPM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPM.DataAcquisition
{
    public class CServerMessage
    {
        public struct MESSAGE_REC
        {
            public CCommonTypes.SERVER_PACKET_ID ID;
            public string sID;  // string description instead of numeric version
            public bool bAccept;
            public bool bReject;
            public string sData;            
        }
        private List<MESSAGE_REC> m_lstMsg;
        CWITSLookupTable m_LookUpWITS;
        public CServerMessage(ref CWITSLookupTable witsLookUpTbl_)
        {
            m_lstMsg = new List<MESSAGE_REC>();
            m_LookUpWITS = witsLookUpTbl_;
        }

        ~CServerMessage()
        {

        }

        public bool PromptToSend(string sTitle_)
        {
            bool bRetVal = false;

            DialogResult dlgres = MessageBox.Show("Send these changes to connected client(s)?", sTitle_, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgres == DialogResult.Yes)
                bRetVal = true;

            return bRetVal;
        }

        public List<MESSAGE_REC> Get()
        {
            return m_lstMsg;
        }

        public void Add(string sPacket_)
        {
            sPacket_.Trim();
            if (sPacket_.Length > 0)  // determine the packet ID
            {
                string[] sArrCols = sPacket_.Split(';');
                if (sArrCols.Length > 0)
                {
                    string[] sArrData = sArrCols[0].Split('=');
                    if (sArrData[0] == CCommonTypes.PACKET_ID)
                    {
                        MESSAGE_REC rec = new MESSAGE_REC();
                        rec.ID = (CCommonTypes.SERVER_PACKET_ID)System.Convert.ToInt32(sArrData[1]);
                        rec.sID = ((CCommonTypes.SERVER_PACKET_ID)System.Convert.ToInt32(sArrData[1])).ToString();
                        rec.bAccept = false;
                        rec.bReject = false;                        
                        rec.sData = sPacket_;

                        m_lstMsg.Add(rec);
                    }                        
                }
            }                            
        }
        public bool Process(string sCommand_, ref CDPointLookupTable dpointTable_)
        {
            bool bRetVal = false;

            string[] sArrCols = sCommand_.Split(';');
            if (sArrCols.Length > 0)
            {
                string[] sArrData = sArrCols[0].Split('=');
                if (sArrData.Length > 0)
                {
                    if (sArrData[0] == CCommonTypes.PACKET_ID)
                    {
                        int iID = System.Convert.ToInt32(sArrData[1]);
                        switch ((CCommonTypes.SERVER_PACKET_ID)iID)
                        {
                            case CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_WITS_RECORD:  // WITS

                                m_LookUpWITS.Update(sArrCols);
                                bRetVal = true;
                                break;
                            case CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_JOB_OR_RIG_DESCRIPTION:  // rig and job info
                                // handled in Main on arrival
                                break;
                            case CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_PRESSURE_TRANSDUCER_SETTINGS:
                                // handled in Main on arrival
                                break;                            
                            case CCommonTypes.SERVER_PACKET_ID.UPDATE_TO_A_UNIT_OF_MEASUREMENT:
                            case CCommonTypes.SERVER_PACKET_ID.UPDATE_A_GROUP_OF_MEASUREMENTS:
                                sArrData = sArrCols[1].Split('=');
                                short iMessageCode = System.Convert.ToInt16(sArrData[1]);
                                sArrData = sArrCols[2].Split('=');
                                string sAPSName = sArrData[1];
                                sArrData = sArrCols[3].Split('=');
                                string sDisplayName = sArrData[1];
                                dpointTable_.SetDisplayName(iMessageCode, sAPSName, sDisplayName);

                                sArrData = sArrCols[4].Split('=');
                                string sUnit = sArrData[1];
                                dpointTable_.SetUnitName(iMessageCode, sAPSName, sUnit);

                                dpointTable_.Save();

                                bRetVal = true;
                                break;
                            case CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_METRIC:
                            case CCommonTypes.SERVER_PACKET_ID.UNIT_OF_MEASUREMENT_SET_TO_IMPERIAL:
                                // handled in Main on arrival
                                break;
                            case CCommonTypes.SERVER_PACKET_ID.UPDATE_A_GROUP_OF_MEASUREMENT_TYPE:
                                // handled in Main on arrival
                                break;
                            default:
                                break;
                        }
                    }
                    else  // bogus data
                    {
                        // ignore
                    }
                }
                else  // no data
                {
                    // ignore
                }                
            }

            return bRetVal;
        }

        public void Remove(string sCommand_)
        {
            // remove the record that was executed
            for (int i = 0; i < m_lstMsg.Count; i++)
            {
                if (m_lstMsg[i].sData == sCommand_)
                {
                    m_lstMsg.RemoveAt(i);
                    break;
                }
            }
        }


    }
}
