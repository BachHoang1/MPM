// author: hoan chau
// purpose: to access data saved in a database for reporting

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using MPM.DataAcquisition.Helpers;
using MPM.Utilities;

namespace MPM.Data
{
    public class CLogDataLayer
    {        
        private const int DETECT_LOG_COLUMNS = 7;
        private const int CUSTOM_COLUMNS = 4;  // for use with LAS exports

        public struct PLOT_DATA
        {
            public string sDateTime;
            public float fValue;
            public int iDate;
            public int iTime;
        }
        

        private DbConnection m_dbCnn;

        public CLogDataLayer(ref DbConnection dbCnn_)
        {
            m_dbCnn = dbCnn_;
        }

        public bool Save(CLogDataRecord rec_)
        {
            bool bRetVal = false;
           
            string sQuery = string.Format("INSERT INTO tblDetect (messageCode, created, date, time, name, value, unit, parityError, depth, telemetry) " +
                                          " VALUES ({0}, '{1}', {2}, {3}, '{4}', '{5}', '{6}', {7}, {8}, '{9}')", 
                                          rec_.iMessageCode, rec_.sCreated, rec_.iDate, rec_.iTime, rec_.sName, rec_.sValue, rec_.sUnit, rec_.bParityError? 1: 0, rec_.fDepth, rec_.sTelemetry);
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                    
                // Retrieve the data.
                command.ExecuteNonQueryAsync();

                //Console.WriteLine("Inserted {0} rows.", iRowsAffected);
                bRetVal = true; // iRowsAffected == 1 ? true : false;                       
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception.Message: {0}", ex.Message);
            }
                
            return bRetVal;
        }

        public DataTable GetMessageCodes(CLASJobInfo.EXPORT_DATA_TYPE exportType, CCommonTypes.UNIT_SET m_iUnitSet)
        {
            DataTable table = new DataTable();

            string sQuery = "SELECT DISTINCT (1==0) AS checkbox, name, unit, messagecode " +
                            "FROM tblDetect " +
                            "ORDER BY NAME";
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;

                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();
                
                for (int i = 0; i < CUSTOM_COLUMNS; i++)
                {
                    DataColumn column = new DataColumn();
                    if (i == 0)
                        column.DataType = typeof(bool);
                    else
                        column.DataType = reader.GetFieldType(i);
                    string sName = reader.GetName(i);
                    column.ColumnName = sName;
                    table.Columns.Add(column);
                }

                // Create new DataTable and DataSource objects.                    
                // Declare DataColumn and DataRow variables.                                        
                // Create new DataRow objects and add to DataTable.  
                int iCount = 1;   // tracks number of records                   
                while (reader.Read())
                {                    
                    DataRow row = table.NewRow();
                    row[0] = iCount;
                    for (int i = 0; i < CUSTOM_COLUMNS; i++)
                        row[i] = reader[i];

                    table.Rows.Add(row);
                    iCount++;                    
                }

                // hoan should use global dpoint table
                CDPointLookupTable DPointTable = new CDPointLookupTable();
                DPointTable.Load();
                CDPointLookupTable.DPointInfo dpi = DPointTable.Get((int)Command.COMMAND_BIT_DEPTH);
                string sUnitOfLength = dpi.sUnits;

                CLogASCIIStandard.CURVE_INFO ciBitDepth = new CLogASCIIStandard.CURVE_INFO();

                // if md is the y-axis
                if (exportType == CLASJobInfo.EXPORT_DATA_TYPE.MD)
                {
                    // add tvd unchecked
                    
                    ciBitDepth.iMsgCode = (long)Command.COMMAND_TVD;
                    ciBitDepth.sName = "TVD";
                    ciBitDepth.sUnit = sUnitOfLength;

                    CUnitLength m_UnitLength = new CUnitLength();
                    m_UnitLength.SetUnitType(m_iUnitSet);
                    if (m_iUnitSet == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
                        ciBitDepth.sUnit = m_UnitLength.GetImperialUnitDesc();
                    else if (m_iUnitSet == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
                        ciBitDepth.sUnit = m_UnitLength.GetMetricUnitDesc();
                }
                else if (exportType == CLASJobInfo.EXPORT_DATA_TYPE.TVD)
                {
                    // add md unchecked
                    ciBitDepth.iMsgCode = (long)Command.COMMAND_BIT_DEPTH;
                    ciBitDepth.sName = "MD";
                    ciBitDepth.sUnit = sUnitOfLength;

                    CUnitLength m_UnitLength = new CUnitLength();
                    m_UnitLength.SetUnitType(m_iUnitSet);
                    if (m_iUnitSet == CCommonTypes.UNIT_SET.UNIT_SET_TO_IMPERIAL)
                        ciBitDepth.sUnit = m_UnitLength.GetImperialUnitDesc();
                    else if (m_iUnitSet == CCommonTypes.UNIT_SET.UNIT_SET_TO_METRIC)
                        ciBitDepth.sUnit = m_UnitLength.GetMetricUnitDesc();
                }

                DataRow rowDepth = table.NewRow();
                rowDepth[0] = 0;
                rowDepth[1] = ciBitDepth.sName;
                rowDepth[2] = ciBitDepth.sUnit;
                rowDepth[3] = ciBitDepth.iMsgCode;
                
                table.Rows.Add(rowDepth);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return table;
        }
               
        public DataTable Get(string sDepthUnit_)
        {            
            DataTable table = new DataTable();
            
            string sQuery = "SELECT created, depth, messageCode, name, value, unit, telemetry " + 
                            "FROM tblDetect";
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;                   
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                DataColumn columnFirst = new DataColumn();
                columnFirst.ColumnName = "#";
                table.Columns.Add(columnFirst);
                for (int i = 0; i < DETECT_LOG_COLUMNS; i++)
                {
                    DataColumn column = new DataColumn();
                    column.DataType = reader.GetFieldType(i);
                    string sName = reader.GetName(i);
                    column.ColumnName = sName == "depth" ? sName + "(" + sDepthUnit_ + ")": sName;
                    table.Columns.Add(column);
                }

                // Create new DataTable and DataSource objects.                    
                // Declare DataColumn and DataRow variables.                                        
                // Create new DataRow objects and add to DataTable.  
                int iCount = 1;   // tracks number of records                   
                while (reader.Read())
                {
                    //Console.WriteLine("{0}, {1}, {2}, {3}, {4}", reader[0], reader[1], reader[2], reader[3], reader[4]);
                    DataRow row = table.NewRow();
                    row[0] = iCount;
                    for (int i = 1; i < DETECT_LOG_COLUMNS + 1; i++)                                                    
                        row[i] = reader[i - 1];
                                                                        
                    table.Rows.Add(row);
                    iCount++;                                             
                    //break;  // don't break, if you're going to do an insert afterwards because it will say that the datbase is locked
                                // let the while loop read through everything
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception.Message: {0}", ex.Message);
            }
                
            return table;
        }

        

        public DataTable Get(string sMessageCode_, float fStartDepth_, float fEndDepth_, CCommonTypes.TELEMETRY_TYPE ttType_)
        {
            DataTable table = new DataTable();
            
            string sMessageCodes = sMessageCode_;
            //for (int i = 0; i < lstMessageCode_.Count; i++)
            //{
            //    if (i > 0)
            //        sMessageCodes += ",";
            //    sMessageCodes += lstMessageCode_[i].iMsgCode.ToString();           
            //}

            string sTelemetryType = "AND telemetry = 'MP' OR telemetry = 'EM'";
            if (ttType_ == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                sTelemetryType = "AND telemetry = 'EM' "; 
            else if (ttType_ == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                sTelemetryType = "AND telemetry = 'MP' ";
            
            string sQuery = String.Format("SELECT created, depth, messageCode, name, value, unit, telemetry " +
                                        "FROM tblDetect " +
                                        "WHERE messageCode IN ({0}) " +
                                        "AND depth >= {1} " +
                                        "AND depth <= {2} " +
                                        sTelemetryType +
                                        "ORDER BY depth", sMessageCodes, fStartDepth_, fEndDepth_);
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                DataColumn columnFirst = new DataColumn();
                columnFirst.ColumnName = "#";
                table.Columns.Add(columnFirst);
                for (int i = 0; i < DETECT_LOG_COLUMNS; i++)
                {
                    DataColumn column = new DataColumn();
                    column.DataType = reader.GetFieldType(i);                        
                    column.ColumnName = reader.GetName(i);                    
                    table.Columns.Add(column);
                }

                // Create new DataTable and DataSource objects.                    
                // Declare DataColumn and DataRow variables.                                        
                // Create new DataRow objects and add to DataTable.  
                int iCount = 1;   // tracks number of records                   
                while (reader.Read())
                {
                    //Console.WriteLine("{0}, {1}, {2}, {3}, {4}", reader[0], reader[1], reader[2], reader[3], reader[4]);
                    DataRow row = table.NewRow();
                    row[0] = iCount;
                    for (int i = 1; i < DETECT_LOG_COLUMNS + 1; i++)
                        row[i] = reader[i - 1];

                    table.Rows.Add(row);
                    iCount++;
                    //break;  // don't break, if you're going to do an insert afterwards because it will say that the datbase is locked
                    // let the while loop read through everything
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception.Message: {0}", ex.Message);
            }
                

            return table;
        }

        public List<PLOT_DATA> Get(int iMessageCode_)
        {
            List<PLOT_DATA> lst = new List<PLOT_DATA>();
            string sQuery = "SELECT created, value, date, time " +
                            "FROM tblDetect " +
                            "WHERE messageCode = " + iMessageCode_.ToString();

            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;

                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();               
                
                while (reader.Read())
                {
                    PLOT_DATA pdRec = new PLOT_DATA();
                    pdRec.sDateTime = reader[0].ToString();
                    pdRec.fValue = System.Convert.ToSingle(reader[1]);
                    pdRec.iDate = System.Convert.ToInt32(reader[2]);
                    pdRec.iTime = System.Convert.ToInt32(reader[3]);
                    lst.Add(pdRec);                
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return lst;
        }

        public List<PLOT_DATA> GetLast(int iCount_, int iMessageCode_)
        {
            List<PLOT_DATA> lst = new List<PLOT_DATA>();
            string sQuery = "SELECT created, value, date, time " +
                            "FROM tblDetect " +
                            "WHERE messageCode = " + iMessageCode_.ToString() + " " +
                            "ORDER BY created DESC " +
                            "LIMIT " + iCount_.ToString();

            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;

                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    PLOT_DATA pdRec = new PLOT_DATA();
                    pdRec.sDateTime = reader[0].ToString();
                    pdRec.fValue = System.Convert.ToSingle(reader[1]);
                    pdRec.iDate = System.Convert.ToInt32(reader[2]);
                    pdRec.iTime = System.Convert.ToInt32(reader[3]);
                    lst.Add(pdRec);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return lst;
        }

        public DataTable Get(string sMessageCode_, DateTime dtStartTime_, DateTime dtEndTime_, CCommonTypes.TELEMETRY_TYPE ttType_)
        {
            DataTable table = new DataTable();

            string sMessageCodes = sMessageCode_;            

            string sTelemetryType = "AND telemetry = 'MP' OR telemetry = 'EM'";
            if (ttType_ == CCommonTypes.TELEMETRY_TYPE.TT_EM)
                sTelemetryType = "AND telemetry = 'EM' ";
            else if (ttType_ == CCommonTypes.TELEMETRY_TYPE.TT_MP)
                sTelemetryType = "AND telemetry = 'MP' ";

            string sQuery = String.Format("SELECT created, depth, messageCode, name, value, unit, telemetry " +
                                        "FROM tblDetect " +
                                        "WHERE messageCode IN ({0}) " +
                                        "AND created >= '{1}' " +
                                        "AND created <= '{2}' " +
                                        sTelemetryType +
                                        "ORDER BY datetime(created)", sMessageCodes, dtStartTime_.ToString("yyyy-MM-dd HH:mm:ss"), dtEndTime_.ToString("yyyy-MM-dd HH:mm:ss"));
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                DataColumn columnFirst = new DataColumn();
                columnFirst.ColumnName = "#";
                table.Columns.Add(columnFirst);
                for (int i = 0; i < DETECT_LOG_COLUMNS; i++)
                {
                    DataColumn column = new DataColumn();
                    column.DataType = reader.GetFieldType(i);
                    column.ColumnName = reader.GetName(i);
                    table.Columns.Add(column);
                }

                // Create new DataTable and DataSource objects.                    
                // Declare DataColumn and DataRow variables.                                        
                // Create new DataRow objects and add to DataTable.  
                int iCount = 1;   // tracks number of records                   
                while (reader.Read())
                {
                    //Console.WriteLine("{0}, {1}, {2}, {3}, {4}", reader[0], reader[1], reader[2], reader[3], reader[4]);
                    DataRow row = table.NewRow();
                    row[0] = iCount;
                    for (int i = 1; i < DETECT_LOG_COLUMNS + 1; i++)
                        row[i] = reader[i - 1];

                    table.Rows.Add(row);
                    iCount++;
                    //break;  // don't break, if you're going to do an insert afterwards because it will say that the datbase is locked
                    // let the while loop read through everything
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception.Message: {0}", ex.Message);
            }


            return table;
        }
    }

    
}
