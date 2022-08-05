// author: hoan chau
// purpose: be a client for a signalR server 
//          expected payloads will be trajectory and corrected survey tables
//

using Microsoft.AspNetCore.SignalR.Client;
using MPM.Data;
using Newtonsoft.Json;
using RoundLAB.eddi.Classes.Enums;
using RoundLAB.eddi.Classes.Survey;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPM.DataAcquisition.MultiStationAnalysis
{
    public class CMSAClient
    {
        const int LEG_ID = 0;

        private uint m_iBHA;

        public delegate void SvyDeletedEventHandler(object sender, CEventDeleteSurvey ev);
        public event SvyDeletedEventHandler SvyDeleted;

        public delegate void SvyUpdatedEventHandler(object sender, CEventUpdateSurvey ev);
        public event SvyUpdatedEventHandler SvyUpdated;

        private string m_sAPIKey;
        private string m_sURL;

        private HttpClient m_httpClient;

        public CMSAClient()
        {
            m_iBHA = 0;
            m_sURL = "";
            m_sAPIKey = "";            
        }  
        
        public bool Connect()
        {
            bool bRetVal = false;
            if (m_sAPIKey.Length > 0)
            {
                m_httpClient = new HttpClient();
                m_httpClient.DefaultRequestHeaders.Authorization =
                       new AuthenticationHeaderValue(scheme: "Bearer",
                       parameter: m_sAPIKey);
                m_httpClient.BaseAddress = new Uri(@m_sURL); // @"http://development.roundlabinc.com/SurveyAPI/");  // http://development.roundlabinc.com/SurveyAPI/SurveyHub
                bRetVal = true;
            }
                        
            return bRetVal;
        }

        public string GetJobID()
        {
            string jsonString = "";
            try
            {
                HttpResponseMessage response = m_httpClient.GetAsync(requestUri: "SurveyJob/ApiKeyJobNumber").Result;
                response.EnsureSuccessStatusCode();
                var readTask = response.Content.ReadAsStringAsync();
                readTask.Wait();
                jsonString = readTask.Result;
                Console.WriteLine(jsonString);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in CMSAClient::GetJobID " + ex.Message);
            }
                       
            return jsonString;
        }

        public async void SendVectorSurvey(CSurvey.REC rec_)
        {
            string jsonString = "";
            try
            {
                HttpContent body = new StringContent(
                JsonConvert.SerializeObject(new SurveyMeasurementDetails()
                {
                    BhaNumber = (int)m_iBHA,
                    LegId = LEG_ID,
                    SuppliedInclination = rec_.fInclination,
                    SuppliedAzimuth = rec_.fAzimuth,
                    Ax = rec_.fAY,
                    Ay = rec_.fAZ,
                    Az = rec_.fAX,
                    Mx = rec_.fMY,
                    My = rec_.fMZ,
                    Mz = rec_.fMX,
                    SurveyDepth = rec_.fSurveyDepth,
                }), System.Text.Encoding.UTF8, mediaType: "application/json");

                HttpResponseMessage response = await m_httpClient.PostAsync(requestUri: "SurveyDetail/Auth/AddSurvey", body);

                response.EnsureSuccessStatusCode();

                var readTask = await response.Content.ReadAsStringAsync();               
                
                jsonString = readTask;
                SurveyMeasurementDetails survey = JsonConvert.DeserializeObject<SurveyMeasurementDetails>(jsonString);
                Console.WriteLine(jsonString);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error in CMSAClient::SendVectorSurvey " + ex.Message);
            }
            
        }

        public async void SendQuickSurvey(CSurvey.REC rec_)
        {
            string jsonString = "";
            try
            {
                HttpContent body = new StringContent(
                JsonConvert.SerializeObject(new SurveyMeasurementDetails()
                {
                    AdminState = MSASurveyAdminState.ShortSurvey,
                    SurveyType = SurveyType.Recorded,
                    BhaNumber = (int)m_iBHA,
                    LegId = 0,
                    SuppliedInclination = rec_.fInclination,
                    SuppliedAzimuth = rec_.fAzimuth,
                    SurveyDepth = rec_.fSurveyDepth,
                }), System.Text.Encoding.UTF8, mediaType: "application/json");

                HttpResponseMessage response = await m_httpClient.PostAsync(requestUri: "SurveyDetail/Auth/AddSurvey", body);

                response.EnsureSuccessStatusCode();

                var readTask = await response.Content.ReadAsStringAsync();                

                jsonString = readTask;
                SurveyMeasurementDetails survey = JsonConvert.DeserializeObject<SurveyMeasurementDetails>(jsonString);
                Console.WriteLine(jsonString);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in CMSAClient::SendQuickSurvey " + ex.Message);
            }
            
            //return jsonString;
        }

        public async void UpdateSurvey(int iSurveyID_, int iBHA_, CSurvey.REC rec_)
        {
            string jsonString = "";
            CEventUpdateSurvey ev = new CEventUpdateSurvey();
            ev.iBHA = iBHA_;
            ev.rec = rec_;
            try
            {
                HttpContent body = new StringContent(
                JsonConvert.SerializeObject(new SurveyMeasurementDetails()
                {
                    SurveyId = iSurveyID_,
                    BhaNumber = iBHA_,
                    LegId = 0,
                    
                    SurveyDepth = rec_.fSurveyDepth,
                    DateTimeRecordedUtc = rec_.dtCreated,
                    AdminState = MSASurveyAdminState.Unchanged,
                    SurveyType = SurveyType.Unchanged
                }), System.Text.Encoding.UTF8, mediaType: "application/json");
                

                HttpResponseMessage response = await m_httpClient.PostAsync(requestUri: "SurveyDetail/Auth/Update", body);

                response.EnsureSuccessStatusCode();

                var readTask = await response.Content.ReadAsStringAsync();
                
                jsonString = readTask;
                SurveyMeasurementDetails survey = JsonConvert.DeserializeObject<SurveyMeasurementDetails>(jsonString);
                Console.WriteLine(jsonString);
                ev.ID = iSurveyID_;
                
                MessageBox.Show("Update was successful!", "Survey Update", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                ev.ID = -1;
                
                MessageBox.Show("Update failed! " + ex.Message, "Survey Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (SvyUpdated != null)
                SvyUpdated(this, ev);
        }

        public async void DeleteSurvey(int iSurveyID_)
        {
            string jsonString = "";
            CEventDeleteSurvey ev = new CEventDeleteSurvey();
            try
            {
                var legId = 0;
                var surveyId = iSurveyID_;
                m_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", m_sAPIKey);

                HttpResponseMessage response = m_httpClient.PostAsync($"SurveyDetail/Auth/DeleteSurvey/{legId}/{surveyId}", null).Result;

                response.EnsureSuccessStatusCode();
                var readTask = await response.Content.ReadAsStringAsync();
                jsonString = readTask;

                SurveyMeasurementDetails survey = JsonConvert.DeserializeObject<SurveyMeasurementDetails>(jsonString);
                //Console.WriteLine(jsonString);
                
                ev.ID = iSurveyID_;
                
                MessageBox.Show("Delete was successful!", "Survey Delete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                ev.ID = -1;
                MessageBox.Show("Delete failed! " + ex.Message, "Survey Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (SvyDeleted != null)
                SvyDeleted(this, ev);
        }


        public void SetBHA(string sVal_)
        {
            m_iBHA = System.Convert.ToUInt16(sVal_);
        }

        public void SetInfo(string sURL_, string sAPIKey_)
        {
            if (sURL_[sURL_.Length - 1] != '/')
                sURL_ += "/";
            m_sURL = sURL_;
            m_sAPIKey = sAPIKey_;
        }
    }
}
