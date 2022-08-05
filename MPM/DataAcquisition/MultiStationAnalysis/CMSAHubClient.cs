// author: hoan chau with help from jason hidegh
// purpose: to listen for signalr packets that have corrected surveys

using Microsoft.AspNetCore.SignalR.Client;
using MPM.Utilities;
using RoundLAB.eddi.Classes.Survey;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using MPM.Data;
using MPM.DataAcquisition.Helpers;
using System.Data.Common;
using System.Windows.Forms;

namespace MPM.DataAcquisition.MultiStationAnalysis
{
    public class CMSAHubClient
    {

        private const int MAX_TIME_WITHOUT_HEARTBEAT = 60;  //seconds
        private const string TRAJECTORY_SORT_COLUMN = "Depth[System.Double]";       
        private const string HUB_NAME = "SubscribeAuth";

        enum TRAJECTORY_TBL_INDEX { DEPTH = 5, INC = 6, AZM = 7 }

        HubConnection m_hubCon;

        private string m_sURL = "";
        private string m_sAPIKey = ""; //"OQA=-D5ADB5BF-664E-44A9-813F-EE7DE399A459";
        private string m_sURLSuffix = "SurveyHub";

        private string m_sDataPath;

        private DataTable m_tblSurveySensorDetails;
        private DataTable m_tblTrajectory;

        private DateTime m_dtLastHeartBeat;

        private Thread reconnectThread;
        private bool m_bUnload;        

        public delegate void HubConnectedEventHandler(object sender, bool e);
        public event HubConnectedEventHandler HubConnected;

        CWITSLookupTable m_LookUpWITS;
        public delegate void EventHandler(object sender, CEventSendWITSData e);
        public event EventHandler Transmit;

        
        public delegate void CorrectedEventHandler(object sender, CEventCorrectedDPoint eInc, CEventCorrectedDPoint eAzm);
        public event CorrectedEventHandler CorrectToolface;

        public delegate void CorrectedLastSurveyEventHandler(object sender, CSurvey.REC rec);
        public event CorrectedLastSurveyEventHandler CorrectLastSurvey;

        DbConnection m_dbCnn;
        float m_fLastDepth;
        public CMSAHubClient(ref CWITSLookupTable witsLookUpTbl_, ref DbConnection dbCnn_, string sDataPath_)
        {
            m_LookUpWITS = witsLookUpTbl_;
            m_dbCnn = dbCnn_;
            CSurveyLog log = new CSurveyLog(ref m_dbCnn);
            m_fLastDepth = log.GetLastSurveyDepth();

            m_sDataPath = sDataPath_;

            m_tblSurveySensorDetails = new DataTable();
            m_tblTrajectory = new DataTable();            
            
            try
            {
                DataSet dtset = new DataSet();
                dtset.ReadXml(m_sDataPath + "CorrectedSurveyTable.xml", XmlReadMode.InferSchema);
                m_tblTrajectory = dtset.Tables[0];

                DataSet dtsetSensorDetails = new DataSet();
                dtsetSensorDetails.ReadXml(m_sDataPath + "SurveySensorDetailsTable.xml", XmlReadMode.InferSchema);
                m_tblSurveySensorDetails = dtsetSensorDetails.Tables[0];
            }                                
            catch(Exception ex)
            {
                Debug.WriteLine("Error in CMSAHubClient::CMSAHubClient " + ex.Message);
            }
        }

        ~CMSAHubClient()
        {            
            try
            {
                if (m_tblTrajectory.Rows.Count > 0)
                    m_tblTrajectory.WriteXml(m_sDataPath + "CorrectedSurveyTable.xml");

                if (m_tblSurveySensorDetails.Rows.Count > 0)
                    m_tblSurveySensorDetails.WriteXml(m_sDataPath + "SurveySensorDetailsTable.xml");
            }  
            catch (Exception ex)
            {
                Debug.WriteLine("Error in ~CMSAHubClient: " + ex.Message);
            }
        }

        public void Unload()
        {
            m_bUnload = true;
            m_hubCon.StopAsync();
        }

        public void SetInfo(string sURL_, string sAPIKey_, string sDataPath_)
        {
            m_sURL = sURL_ + "/" + m_sURLSuffix;
            m_sAPIKey = sAPIKey_;
            m_sDataPath = sDataPath_;
        }

        public void Register()
        {
            m_tblTrajectory.Clear();
            m_tblSurveySensorDetails.Clear();
            m_dtLastHeartBeat = DateTime.Now.ToUniversalTime();
            Resuscitate();
            reconnectThread = new Thread(Reconnect);
            reconnectThread.Start();                        
        }        

        private void Reconnect()
        {
            string sAPIKey = m_sAPIKey;
            while (true)
            {
                try
                {
                    if (m_bUnload)
                        break;

                    DateTime dtNow = DateTime.Now.ToUniversalTime();
                    TimeSpan ts = dtNow - m_dtLastHeartBeat;
                    if (ts.TotalSeconds > MAX_TIME_WITHOUT_HEARTBEAT || sAPIKey != m_sAPIKey)
                    {
                        sAPIKey = m_sAPIKey;
                        m_hubCon.StopAsync();
                        Resuscitate();
                        if (HubConnected != null)
                            HubConnected(this, false);
                    } 

                    if (sAPIKey.Trim() == "")
                    {
                        
                        if (HubConnected != null)
                            HubConnected(this, false);
                    }
                        


                    Thread.Sleep(5000);
                }
                catch (TimeoutException) { }
            }
        }        

        private async void Resuscitate()
        {
            m_hubCon = new HubConnectionBuilder()
                .WithUrl(m_sURL)
                .WithAutomaticReconnect(new TimeSpan[] {  TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(25)}).Build();

            m_hubCon.On<DateTime>("Heartbeat", (timestamp =>
            {
                //CSurvey.REC recSvy = new CSurvey.REC();
                //recSvy.fInclination = 90.123f;
                //recSvy.fAzimuth = 270.432f;
                //if (CorrectLastSurvey != null)
                //    CorrectLastSurvey(this, recSvy);
                Debug.WriteLine("MSA Heartbeat: " + timestamp.ToString());
                m_dtLastHeartBeat = timestamp;
                if (HubConnected != null)
                    HubConnected(this, true);
            }));

            try
            {
                await m_hubCon.StartAsync();
                //ListenForSurveyTbl();
                ListenForSurveyMeasurementsTbl();
                ListenForTrajectoryTbl();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error at CMSAHubClient::Resuscitate: " + ex.Message);
            }

            try
            {
                if (m_sAPIKey.Length > 0)
                    await m_hubCon.InvokeAsync(HUB_NAME, m_sAPIKey);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error at CMSAHubClient::Resuscitate::InvokeAsync: " + ex.Message);
            }
            
        }       

        //private bool GetSurveyIndices(DataTable dtTrajectory_, out int iIncIndex_, out int iAzmIndex_, out int iDepthIndex_)
        //{
        //    bool bRetVal = false;
        //    iIncIndex_ = iAzmIndex_ = iDepthIndex_ = 0;
        //    for (int k = 0; k < dtTrajectory_.Columns.Count; k++)
        //    {
        //        if (dtTrajectory_.Columns[k].ColumnName == "Inc[System.Double]")
        //            iIncIndex_ = k;
        //        else if (dtTrajectory_.Columns[k].ColumnName == "Azm[System.Double]")
        //            iAzmIndex_ = k;
        //        else if (dtTrajectory_.Columns[k].ColumnName == "Depth[System.Double]")
        //            iDepthIndex_ = k;
        //    }

        //    if (iIncIndex_ > 0 && iAzmIndex_ > 0 && iDepthIndex_ > 0)
        //        bRetVal = true;

        //    return bRetVal;
        //}

        

        private void ListenForTrajectoryTbl()
        {          
            try
            {
                m_hubCon.On<string, int>("TrajectoryTable", (tableStr, jobId) =>
                {                                                            
                    string[] separatingStrings = { "\r\n" };
                    string [] rowArr = tableStr.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

                    DataTable tblNewTrajectory = new DataTable();

                    // construct columns of table
                    string[] sHeaderArr = rowArr[0].Split('~');
                    int iDepthColumn = -1;
                    for (int i = 0; i < sHeaderArr.Length; i++)
                    {
                        DataColumn column = new DataColumn();
                        //column.DataType = sHeaderArr[i];
                        //string sName = reader.GetName(i);
                        column.ColumnName = sHeaderArr[i];
                        if (column.ColumnName.ToLower().Contains("double"))
                            column.DataType = typeof(double);
                        else if (column.ColumnName.ToLower().Contains("int"))
                            column.DataType = typeof(int);
                        else if (column.ColumnName.ToLower().Contains("datetime"))
                            column.DataType = typeof(DateTime);
                        else
                            column.DataType = typeof(string);
                        tblNewTrajectory.Columns.Add(column);
                        if (column.ColumnName == TRAJECTORY_SORT_COLUMN)
                            iDepthColumn = i;
                    }

                    // populate with data
                    for (int i = 1; i < rowArr.Length; i++)
                    {
                        string[] colArr = rowArr[i].Split('~');
                        DataRow row = tblNewTrajectory.NewRow();

                        for (int j = 0; j < colArr.Length; j++)
                        {
                            if (j == iDepthColumn)
                                row.SetField(j, System.Convert.ToSingle(colArr[j]));
                            else
                            {
                                if (colArr[j] != "")
                                    row.SetField(j, colArr[j]);
                                else if (colArr[j] == "")
                                    row.SetField(j, "0");
                            }
                                
                        }
                        tblNewTrajectory.Rows.Add(row);
                    }

                    //tblNewTrajectory = tblNewTrajectory.DefaultView.Sort = TRAJECTORY_SORT_COLUMN;
                    
                    //surveys = surveys.OrderBy(rec => rec.SurveyDepth);

                    CSurveyLog log = new CSurveyLog(ref m_dbCnn);
                    
                    // find any records that haven't been sent to the EDR
                    for (int i = 0; i < tblNewTrajectory.Rows.Count; i++)
                    {
                        CEventSurvey recSvy = new CEventSurvey();
                                            
                        // get the survey from the trajectory table                        
                        DataRow rowTraj = tblNewTrajectory.Rows[i];                        
                        recSvy.rec.fSurveyDepth = System.Convert.ToSingle(rowTraj[(int)TRAJECTORY_TBL_INDEX.DEPTH]);

                        if (recSvy.rec.fSurveyDepth > m_fLastDepth)
                        {
                            recSvy.rec.fInclination = System.Convert.ToSingle(rowTraj[(int)TRAJECTORY_TBL_INDEX.INC]);
                            recSvy.rec.fAzimuth = System.Convert.ToSingle(rowTraj[(int)TRAJECTORY_TBL_INDEX.AZM]);

                            bool bFoundQualifiers = log.GetQualifiers(recSvy.rec.fSurveyDepth, out recSvy.rec.fMTotal, out recSvy.rec.fGTotal, out recSvy.rec.fDipAngle);
                            if (!bFoundQualifiers)
                                recSvy.rec.fGTotal = recSvy.rec.fMTotal = recSvy.rec.fDipAngle = CCommonTypes.BAD_VALUE;

                            m_fLastDepth = recSvy.rec.fSurveyDepth;

                            // send to edr
                            CEventSendWITSData evtSvyWITS = new CEventSendWITSData();
                            evtSvyWITS.m_sData = GatherWITS(recSvy);                                  
                            Transmit(this, evtSvyWITS);

                            // send only the last record to the toolface                        
                            if (i == tblNewTrajectory.Rows.Count - 1)
                            {
                                CEventCorrectedDPoint evInc = new CEventCorrectedDPoint();
                                evInc.m_ID = (int)Command.COMMAND_RESP_INCLINATION;
                                evInc.m_fValue = recSvy.rec.fInclination;
                                evInc.m_DateTime = DateTime.Now;

                                CEventCorrectedDPoint evAzm = new CEventCorrectedDPoint();
                                evAzm.m_ID = (int)Command.COMMAND_RESP_AZIMUTH;
                                evAzm.m_fValue = recSvy.rec.fAzimuth;
                                evAzm.m_DateTime = evInc.m_DateTime;

                                if (CorrectToolface != null)
                                    CorrectToolface(this, evInc, evAzm);

                                if (CorrectLastSurvey != null)
                                    CorrectLastSurvey(this, recSvy.rec);
                            }
                            
                            Thread.Sleep(100);
                        }                             
                    }
                                                                
                    m_tblTrajectory = tblNewTrajectory.DefaultView.ToTable();
                    m_tblTrajectory.TableName = "Trajectory";
                });
            
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error at CMSAHubClient::ListenForTrajectoryTbl: " + ex.Message);
            }               
        }

        private void ListenForSurveyTbl()
        {
            try
            {
                m_hubCon.On<IEnumerable<SurveyMeasurementDetails>, int>("CompleteSurveyMeasurementsCollection", (surveys, jobid) =>
                {
                    int count = surveys.Count();
                    Console.WriteLine("Caught survey collection update. {0} survey(s) in collection", count);
                    Debug.WriteLine("Caught survey collection update. {0} survey(s) in collection", count);

                    surveys = surveys.OrderBy(rec => rec.SurveyDepth);
                    m_tblSurveySensorDetails = ToDataTable(surveys);                                                            
                });                              
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error at CMSAHubClient::ListenForSurveyTbl: " + ex.Message);
            }
        }  
        
        private void ListenForSurveyMeasurementsTbl()
        {
            try
            {
                m_hubCon.On<string, int>("SurveyMeasurementsTable", (tableStr, jobId) =>
                {
                    string[] separatingStrings = { "\r\n" };
                    string[] rowArr = tableStr.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

                    DataTable tblNewSurveyMeasurementDetails = new DataTable();

                    // construct columns of table
                    string[] sHeaderArr = rowArr[0].Split('~');                    
                    for (int i = 0; i < sHeaderArr.Length; i++)
                    {
                        DataColumn column = new DataColumn();
                        //column.DataType = sHeaderArr[i];
                        //string sName = reader.GetName(i);
                        column.ColumnName = sHeaderArr[i];
                        if (column.ColumnName.ToLower().Contains("double"))
                            column.DataType = typeof(double);
                        else if (column.ColumnName.ToLower().Contains("int"))
                            column.DataType = typeof(int);
                        else if (column.ColumnName.ToLower().Contains("datetime"))
                            column.DataType = typeof(DateTime);
                        else
                            column.DataType = typeof(string);

                        tblNewSurveyMeasurementDetails.Columns.Add(column);                        
                    }

                    // populate with data
                    for (int i = 1; i < rowArr.Length; i++)
                    {
                        string[] colArr = rowArr[i].Split('~');
                        DataRow row = tblNewSurveyMeasurementDetails.NewRow();

                        for (int j = 0; j < colArr.Length; j++)
                        {
                            
                            if (colArr[j] != "")
                                row.SetField(j, colArr[j]);
                            else if (colArr[j] == "")
                                row.SetField(j, "0");                            
                        }
                        tblNewSurveyMeasurementDetails.Rows.Add(row);
                    }
                    
                    m_tblSurveySensorDetails = tblNewSurveyMeasurementDetails.DefaultView.ToTable();
                    m_tblSurveySensorDetails.TableName = "SurveyMeasurementDetails";
                });

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error at CMSAHubClient::ListenForSurveyMeasurementsTbl: " + ex.Message);
            }
        }

        private string GatherWITS(CEventSurvey recSvy_)
        {
            CWITSRecordSurvey rec = new CWITSRecordSurvey();
            string sIncWITSID, sAzmWITSID, sDepthWITSID, sGTotWITSID, sMTotWITSID, sDipWITSID;

            //************************************
            // survey channels
            //************************************
            sDepthWITSID = m_LookUpWITS.Find2(10708);
            sIncWITSID = m_LookUpWITS.Find2((int)Command.COMMAND_RESP_INCLINATION);
            sAzmWITSID = m_LookUpWITS.Find2((int)Command.COMMAND_RESP_AZIMUTH);
            
            rec.AddChannel(sDepthWITSID);
            rec.AddChannel(sIncWITSID);
            rec.AddChannel(sAzmWITSID);
            
            rec.SetValue(sDepthWITSID, (float)recSvy_.rec.fSurveyDepth);
            rec.SetValue(sIncWITSID, (float)recSvy_.rec.fInclination);
            rec.SetValue(sAzmWITSID, (float)recSvy_.rec.fAzimuth);

            //************************************
            // append qualifiers if they exist
            //************************************
            if (recSvy_.rec.fGTotal > CCommonTypes.BAD_VALUE)
            {
                sGTotWITSID = m_LookUpWITS.Find2((int)Command.COMMAND_RESP_GT);
                sMTotWITSID = m_LookUpWITS.Find2((int)Command.COMMAND_RESP_BT);
                sDipWITSID = m_LookUpWITS.Find2((int)Command.COMMAND_RESP_DIPANGLE);

                rec.AddChannel(sMTotWITSID);
                rec.AddChannel(sGTotWITSID);                
                rec.AddChannel(sDipWITSID);

                rec.SetValue(sMTotWITSID, (float)recSvy_.rec.fMTotal);
                rec.SetValue(sGTotWITSID, (float)recSvy_.rec.fGTotal);                
                rec.SetValue(sDipWITSID, (float)recSvy_.rec.fDipAngle);
            }                    
                                       
            string sRetVal = rec.GatherData();
            sRetVal = "&&\r\n" + sRetVal + "!!\r\n";  // prepend and append wits flags
            return sRetVal;
        }

        // borrowed function 
        private DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            // Create the result table, and gather all properties of a T        
            DataTable table = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Add the properties as columns to the datatable
            foreach (var prop in props)
            {
                Type propType = prop.PropertyType;

                // Is it a nullable type? Get the underlying type 
                if (propType.IsGenericType && propType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    propType = new NullableConverter(propType).UnderlyingType;

                table.Columns.Add(prop.Name, propType);
            }

            // Add the property values per T as rows to the datatable
            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                    values[i] = props[i].GetValue(item, null);

                table.Rows.Add(values);
            }

            return table;
        }

        public DataTable GetCorrectedTable()
        {
            return m_tblTrajectory;
        }
        
        public DataTable GetSensorDetailsTable()
        {
            return m_tblSurveySensorDetails;
        }

        public void GetTVDRange(out float fStartDepth_, out float fStopDepth_)
        {
            fStartDepth_ = float.MaxValue;
            fStopDepth_ = float.MinValue;
            bool bFoundStartDepth = false;
            bool bFoundStopDepth = false;
            for (int i = 0; i < m_tblTrajectory.Rows.Count; i++)
            {
                float fTVD = System.Convert.ToSingle(m_tblTrajectory.Rows[i].ItemArray[8]);
                if (fTVD > fStopDepth_)  // keep going bigger
                {
                    fStopDepth_ = fTVD;
                    bFoundStopDepth = true;
                }

                if (fTVD < fStartDepth_)  // keep going smaller
                {
                    fStartDepth_ = fTVD;
                    bFoundStartDepth = true;
                }
            }

            if (!bFoundStartDepth)
                fStartDepth_ = CCommonTypes.BAD_VALUE;

            if (!bFoundStopDepth)
                fStopDepth_ = CCommonTypes.BAD_VALUE;
        }

        public List<CSurveyCalculation.MD_TO_TVD> GetTVDList()
        {
            List<CSurveyCalculation.MD_TO_TVD> lstRet = new List<CSurveyCalculation.MD_TO_TVD>();
            for (int i = 0; i < m_tblTrajectory.Rows.Count; i++)
            {
                CSurveyCalculation.MD_TO_TVD rec = new CSurveyCalculation.MD_TO_TVD();
                rec.md = System.Convert.ToSingle(m_tblTrajectory.Rows[i].ItemArray[5]);
                rec.tvd = System.Convert.ToSingle(m_tblTrajectory.Rows[i].ItemArray[8]);                                
                lstRet.Add(rec);
            }

            return lstRet;
        }
    }
}
