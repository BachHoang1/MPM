// author: hoan chau
// purpose: database access layer

using System;
using System.Data.Common;
using System.Data;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using MPM.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MPM.Data
{
    public class CSurveyLog
    {        
        private int COLUMNS = 17;
        private DbConnection m_dbCnn;

        private string m_sLengthUnit;

        public CSurveyLog(ref DbConnection dbCnn_)
        {
            m_dbCnn = dbCnn_;
            m_sLengthUnit = "ft";
        }

        public bool Save(CSurvey.REC rec_)
        {
            bool bRetVal = false;
                            
            string sQuery = string.Format("INSERT INTO tblSurvey (created, status, type, telemetry, bitDepth, dirToBit, inc, azm, mTotal, dipAngle, gTotal, ax, ay, az, mx, my, mz) " +
                                            " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16})", 
                                            rec_.dtCreated.ToString("yyyy-MM-dd HH:mm:ss.fff"), rec_.Status, rec_.Type, rec_.TelemetryType,
                                            rec_.fBitDepth, rec_.fDirToBit, rec_.fInclination, rec_.fAzimuth, 
                                            rec_.fMTotal, rec_.fDipAngle, rec_.fGTotal, 
                                            rec_.fAX, rec_.fAY, rec_.fAZ, 
                                            rec_.fMX, rec_.fMY, rec_.fMZ);

            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;

                // Retrieve the data.
                int iRowsAffected = command.ExecuteNonQuery();

                //Console.WriteLine("Inserted {0} rows.", iRowsAffected);
                bRetVal = iRowsAffected == 1 ? true : false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }               

            return bRetVal;
        }

        public bool SaveCalc(CSurvey.REC_CALC rec_)
        {
            bool bRetVal = false;

            string sQuery = string.Format("INSERT INTO tblSurveyCalc (created, createdSvy, courseLength, tvd, dls, ns, ew) " +
                                          " VALUES ('{0}', '{1}', {2}, {3}, {4}, {5}, {6})",
                                          rec_.dtCreated.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                          rec_.dtCreatedSvy.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                          rec_.fCourseLength, 
                                          rec_.fTVD, 
                                          rec_.fDLS,
                                          rec_.fNS, 
                                          rec_.fEW);

            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;

                // Retrieve the data.
                int iRowsAffected = command.ExecuteNonQuery();

                //Console.WriteLine("Inserted {0} rows.", iRowsAffected);
                bRetVal = iRowsAffected == 1 ? true : false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return bRetVal;
        }

        public bool UpdateCalc(CSurvey.REC_CALC rec_, out string sErrorMessage)
        {
            bool bRetVal = false;
            sErrorMessage = "";

            string sQuery = string.Format("UPDATE tblSurveyCalc SET " + 
                                          "courseLength = {0}, " +
                                          "tvd = {1}, " +
                                          "dls = {2}, " +
                                          "ns = {3}, " +
                                          "ew = {4} " +
                                          "WHERE createdSvy = '{5}'",                                          
                                          rec_.fCourseLength,
                                          rec_.fTVD,
                                          rec_.fDLS,
                                          rec_.fNS,
                                          rec_.fEW,
                                          rec_.dtCreatedSvy.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;

                // Retrieve the data.
                int iRowsAffected = command.ExecuteNonQuery();

                bRetVal = iRowsAffected == 1 ? true : false;
            }
            catch (Exception ex)
            {
                sErrorMessage = ex.Message;
            }

            return bRetVal;
        }

        public CSurvey.REC_CALC Calculate(CSurvey.REC recSvy)
        {
            CSurvey.REC_CALC rekCalc = new CSurvey.REC_CALC();
            DataTable tblSvy = new DataTable();
            tblSvy = Get(true, 0, recSvy.fBitDepth - recSvy.fDirToBit - 1.0f, false);

            CSurvey.REC rec1 = new CSurvey.REC();
            CSurvey.REC rec2 = new CSurvey.REC();

            if (tblSvy.Rows.Count < 1)
            {
                rec1.fAzimuth = 0;
                rec1.fInclination = 0;
                rec1.fBitDepth = 0;
            }
            else
            {
                DataRow dr = tblSvy.Rows[tblSvy.Rows.Count - 1];
                rec1.fBitDepth = System.Convert.ToSingle(dr.ItemArray[0]);
                rec1.fSurveyDepth = System.Convert.ToSingle(dr.ItemArray[1]);
                rec1.dtCreated = System.Convert.ToDateTime(dr.ItemArray[2]);
                rec1.fInclination = System.Convert.ToSingle(dr.ItemArray[3]);
                rec1.fAzimuth = System.Convert.ToSingle(dr.ItemArray[4]);                                
            }

            rec2 = recSvy;

            rekCalc.dtCreated = DateTime.Now;
            rekCalc.dtCreatedSvy = rec2.dtCreated;
            rekCalc.fCourseLength = rec2.fBitDepth - rec1.fBitDepth;

            // calculate the tvd  
            CSurveyCalculation calc = new CSurveyCalculation();
            rekCalc.fTVD = calc.GetTVD(rec1, rec2);            
            double dblTVD = GetTVD(rec1.dtCreated);
            if (tblSvy.Rows.Count > 0)
                rekCalc.fTVD += dblTVD;

            // coordinates
            double dblNS = calc.GetNSChange(rec1, rec2);
            rekCalc.fNS = GetNS(rec1.dtCreated);
            double dblEW = calc.GetEWChange(rec1, rec2);
            rekCalc.fEW = GetEW(rec1.dtCreated);
            if (tblSvy.Rows.Count > 0)
            {
                rekCalc.fNS += dblNS;
                rekCalc.fEW += dblEW;
            }

            // dog leg
            double dl = calc.GetDogLeg(rec1, rec2);
            rekCalc.fDLS = calc.GetDogLegSeverity(dl, rekCalc.fCourseLength, m_sLengthUnit);

            return rekCalc;            
        }
        
        public double GetTVD(DateTime dtCreated_)
        {
            double fRetVal = 0;

            string sQuery = String.Format("SELECT tvd FROM tblSurveyCalc WHERE createdSvy = '{0}'", dtCreated_.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                reader.Read();
                fRetVal = System.Convert.ToSingle(reader[0]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return fRetVal;
        }

        public double GetNS(DateTime dtCreated_)
        {
            double fRetVal = 0;

            string sQuery = String.Format("SELECT ns FROM tblSurveyCalc WHERE createdSvy = '{0}'", dtCreated_.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                reader.Read();
                fRetVal = System.Convert.ToSingle(reader[0]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return fRetVal;
        }

        public double GetEW(DateTime dtCreated_)
        {
            double fRetVal = 0;

            string sQuery = String.Format("SELECT ew FROM tblSurveyCalc WHERE createdSvy = '{0}'", dtCreated_.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                reader.Read();
                fRetVal = System.Convert.ToSingle(reader[0]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return fRetVal;
        }

        public bool Update(DateTime dtCreated_, String status_, float fBitDepth_)
        {
            bool bRetVal = false;
            
            string sQuery = string.Format("UPDATE tblSurvey SET status = '{1}', bitDepth = {2} WHERE created = '{0}'",
                                            dtCreated_.ToString("yyyy-MM-dd HH:mm:ss.fff"), status_, fBitDepth_);

            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;

                // Retrieve the data.
                int iRowsAffected = command.ExecuteNonQuery();

                //Console.WriteLine("Updated {0} rows.", iRowsAffected);
                bRetVal = iRowsAffected == 1 ? true : false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }
               
            return bRetVal;
        }

        public int GetCount()
        {
            int iRetVal = 0;
            
            string sQuery = String.Format("SELECT COUNT(*) " +
                                            "FROM tblSurvey ");
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                reader.Read();
                iRetVal = System.Convert.ToInt32(reader[0]);                   
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }
               
            return iRetVal;
        }

        public float GetLastSurveyDepth()
        {
            float fRetVal = CCommonTypes.BAD_VALUE;

            string sQuery = String.Format("SELECT (bitDepth - dirToBit) as surveyDepth " +
                                          "FROM tblSurvey WHERE status = 'ACCEPT' ORDER BY bitDepth DESC LIMIT 1 ");
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                reader.Read();
                fRetVal = System.Convert.ToSingle(reader[0]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return fRetVal;
        }

        private int GetTableVersion()
        {
            int iRetVal = -1;  // doesn't exist

            string sQuery = String.Format("SELECT version " +
                                          "FROM tblVersions " +
                                          "WHERE tblName = 'tblSurveyCalc' "); // depth can fall in without worrying about float precision

            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                reader.Read();
                iRetVal = System.Convert.ToInt32(reader[0]);                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return iRetVal;
        }

        public bool GetQualifiers(float fSurveyDepth_, out float fMTotal_, out float fGTotal_, out float fDipAngle_)
        {
            bool bRetVal = false;
            fMTotal_ = fGTotal_ = fDipAngle_ = CCommonTypes.BAD_VALUE;

            string sQuery = String.Format("SELECT mtotal, gtotal, dipangle " + 
                                          "FROM tblsurvey " +
                                          "WHERE bitdepth - dirtobit >= {0} " + 
                                          "AND bitdepth - dirtobit <= {1} " + 
                                          "AND status = 'ACCEPT' ORDER BY created DESC LIMIT 1 ", 
                                          fSurveyDepth_ - 1.0f, // provide one unit intervals so the survey
                                          fSurveyDepth_ + 1.0f); // depth can fall in without worrying about float precision
            
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                reader.Read();
                fMTotal_ = System.Convert.ToSingle(reader[0]);
                fGTotal_ = System.Convert.ToSingle(reader[1]);
                fDipAngle_ = System.Convert.ToSingle(reader[2]);
                bRetVal = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return bRetVal;
        }

        public bool RecalculateTVD()
        {
            bool bRetVal = false;
            bool bErrorFlag = false;

            // get all the surveys with time greater than or equal to start time
            List<CSurvey.REC> lst = GetSurveysForTVD();
            string sErrorMessage = "";

            for (int i = 0; i < lst.Count; i++)
            {
                // recalculate the tvd and all other survey calculations
                CSurvey.REC_CALC rekCalc = Calculate(lst[i]);                
                if (!UpdateCalc(rekCalc, out sErrorMessage))
                {
                    sErrorMessage = "Survey #: " + i.ToString() + "'" + sErrorMessage + "'.";
                    bErrorFlag = true;
                    break;
                }                    
            }

            if (bErrorFlag)            
                MessageBox.Show("An error occurred when updating survey calculations: " + sErrorMessage + " Please try again.", "Survey Recalculations", MessageBoxButtons.OK, MessageBoxIcon.Error);            

            return bRetVal;
        }

        public List<CSurvey.REC> GetSurveysForTVD()
        {
            List<CSurvey.REC> lstSurvey = new List<CSurvey.REC>();
            int iSurveyCalcTableVersion = GetTableVersion();
            string sQuery;

            if (iSurveyCalcTableVersion > 0)
            {
                sQuery = String.Format("SELECT created, bitDepth, bitDepth - dirToBit AS surveyDepth, inc, azm " +                                          
                                        "FROM tblSurvey " +
                                        "WHERE status = '" + CSurvey.STATUS.ACCEPT.ToString() + "' " +
                                        "ORDER BY bitDepth");

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
                        CSurvey.REC rec = new CSurvey.REC();
                        rec.dtCreated = System.Convert.ToDateTime(reader.GetFieldValue<string>(0));
                        rec.fBitDepth = System.Convert.ToSingle(reader[1]);
                        rec.fSurveyDepth = System.Convert.ToSingle(reader[2]);
                        rec.fInclination = System.Convert.ToSingle(reader[3]);
                        rec.fAzimuth = System.Convert.ToSingle(reader[4]);

                        lstSurvey.Add(rec);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception.Message: {0}", ex.Message);
                }

            }            

            return lstSurvey;
        }

        public DataTable Get(string sStatus_ = "")
        {
            DataTable table = new DataTable();

            string sQuery;
            int iSurveyCalcTableVersion = GetTableVersion();
            int iAdditionalColumns = 0;

            if (iSurveyCalcTableVersion > 0)
            {
                sQuery = String.Format("SELECT s.created, status, type, telemetry, bitDepth, bitDepth - dirToBit AS surveyDepth," +
                                          "inc, azm, ax, ay, az, mx, my, mz, mTotal, gTotal, dipAngle, " +
                                          "printf('%.2f', c.courseLength) AS courseLength, printf('%.2f', c.tvd) AS TVD, " +
                                          "printf('%.2f', c.ns) AS NS, printf('%.2f', c.ew) AS EW, printf('%.2f', c.dls) AS DLS " +
                                          "FROM tblSurvey s, tblSurveyCalc c " +
                                          "WHERE s.created = c.createdSvy " +
                                          (sStatus_ == "" ? "" : "AND status = '" + sStatus_ + "'") +                                          
                                          "ORDER BY datetime('s.created')");

                iAdditionalColumns = 5;  // fields from tblSurveyCalc
            }                
            else
                sQuery = String.Format("SELECT created, status, type, telemetry, bitDepth, bitDepth - dirToBit AS surveyDepth," +
                                          "inc, azm, ax, ay, az, mx, my, mz, mTotal, gTotal, dipAngle " +
                                          "FROM tblSurvey " + 
                                          (sStatus_ == "" ? "": "WHERE status = '" + sStatus_  + "'") +
                                          "ORDER BY datetime('created')");
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
                for (int i = 0; i < COLUMNS + iAdditionalColumns; i++)
                {
                    DataColumn column = new DataColumn();
                    //column.DataType = reader.GetFieldType(i);
                    column.ColumnName = reader.GetName(i);
                    if (column.ColumnName == "created")
                        column.DataType = System.Type.GetType("System.DateTime");
                    else if (column.ColumnName == "status" || column.ColumnName == "type" || column.ColumnName == "telemetry")
                        column.DataType = System.Type.GetType("System.String");
                    else
                        column.DataType = System.Type.GetType("System.Decimal");
                    table.Columns.Add(column);
                }

                    
                int iCount = 1;   // tracks number of records                   
                while (reader.Read())
                {
                    //Console.WriteLine("{0}, {1}, {2}, {3}, {4}", reader[0], reader[1], reader[2], reader[3], reader[4]);
                    DataRow row = table.NewRow();
                    row[0] = iCount;
                    for (int i = 1; i < COLUMNS + iAdditionalColumns + 1; i++)
                        row[i] = reader[i - 1];

                    table.Rows.Add(row);
                    iCount++;
                    //break;  // don't break, if you're going to do an insert afterwards because it will say that the datbase is locked
                    // let the while loop read through everything
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }
                
            return table;
        }


        public DataTable Get(string sTelemetry_, string sStatus_ = "")
        {
            DataTable table = new DataTable();

            string sQuery = String.Format("SELECT created, status, type, telemetry, bitDepth, bitDepth - dirToBit AS surveyDepth," +
                                          "inc, azm, ax, ay, az, mx, my, mz, mTotal, gTotal, dipAngle " +
                                          "FROM tblSurvey " +
                                          "WHERE telemetry = '{0}' " + 
                                          (sStatus_ == "" ? "" : "AND status = '" + sStatus_ + "' ") +
                                          "ORDER BY bitDepth", sTelemetry_);
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
                for (int i = 0; i < COLUMNS; i++)
                {
                    DataColumn column = new DataColumn();
                    //column.DataType = reader.GetFieldType(i);
                    column.ColumnName = reader.GetName(i);
                    if (column.ColumnName == "created")
                        column.DataType = System.Type.GetType("System.DateTime");
                    else if (column.ColumnName == "status" || column.ColumnName == "type" || column.ColumnName == "telemetry")
                        column.DataType = System.Type.GetType("System.String");
                    else
                        column.DataType = System.Type.GetType("System.Decimal");
                    table.Columns.Add(column);
                }


                int iCount = 1;   // tracks number of records                   
                while (reader.Read())
                {
                    //Console.WriteLine("{0}, {1}, {2}, {3}, {4}", reader[0], reader[1], reader[2], reader[3], reader[4]);
                    DataRow row = table.NewRow();
                    row[0] = iCount;
                    for (int i = 1; i < COLUMNS + 1; i++)
                        row[i] = reader[i - 1];

                    table.Rows.Add(row);
                    iCount++;
                    //break;  // don't break, if you're going to do an insert afterwards because it will say that the datbase is locked
                    // let the while loop read through everything
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return table;
        }

        public DataTable Get(bool bIsAll_, float fStartDepth_, float fEndDepth_, bool bUseNT_)
        {
            DataTable table = new DataTable();
            double dblMagneticFactor = 1.0;
            if (bUseNT_)
                dblMagneticFactor = 100000;

            string sQuery;
            int iSurveyCalcTableVersion = GetTableVersion();
            int iAdditionalColumns = 0;

            if (iSurveyCalcTableVersion > 0)
            {
                //sQuery = String.Format("SELECT s.created, status, type, telemetry, bitDepth, bitDepth - dirToBit AS surveyDepth," +
                //                          "inc, azm, ax, ay, az, mx, my, mz, mTotal, gTotal, dipAngle, " +
                //                          "printf('%.2f', c.courseLength) AS courseLength, printf('%.2f', c.tvd) AS TVD, " +
                //                          "printf('%.2f', c.ns) AS NS, printf('%.2f', c.ew) AS EW, printf('%.2f', c.dls) AS DLS " +
                //                          "FROM tblSurvey s, tblSurveyCalc c " +
                //                          "WHERE s.created = c.createdSvy " +
                //                          (sStatus_ == "" ? "" : "AND status = '" + sStatus_ + "'") +
                //                          "ORDER BY datetime('s.created')");
                sQuery = String.Format("SELECT bitDepth, bitDepth - dirTobit AS SurveyDepth, s.created AS TimeDate, " +
                                          "inc, azm, ax, ay, az, " +
                                          "mx * {2} AS mx, my * {2} AS my, mz * {2} AS mz, mTotal * {2} AS mTotal, gTotal, dipAngle, " +
                                          "printf('%.2f', c.courseLength) AS courseLength, printf('%.2f', c.tvd) AS TVD, " +
                                          "printf('%.2f', c.ns) AS NS, printf('%.2f', c.ew) AS EW, printf('%.2f', c.dls) AS DLS " +
                                          "FROM tblSurvey s, tblSurveyCalc c " +
                                          "WHERE bitdepth - dirtobit >= {0} " +
                                          "AND s.created = c.createdSvy " +
                                          "AND bitdepth - dirtobit <= {1} " +
                                          (bIsAll_ ? "" : "AND status = 'ACCEPT' ") +
                                          "ORDER BY bitdepth - dirtobit ",
                                          fStartDepth_,
                                          fEndDepth_,
                                          dblMagneticFactor);

                iAdditionalColumns = 5;  // fields from tblSurveyCalc
            }
            else                
                sQuery = String.Format("SELECT bitDepth, bitDepth - dirTobit AS SurveyDepth, created AS TimeDate, " +
                                          "inc, azm, ax, ay, az, " +
                                          "mx * {2} AS mx, my * {2} AS my, mz * {2} AS mz, mTotal * {2} AS mTotal, gTotal, dipAngle " +
                                          "FROM tblSurvey " +
                                          "WHERE bitdepth - dirtobit >= {0} " +
                                          "AND bitdepth - dirtobit <= {1} " +
                                          (bIsAll_ ? "" : "AND status = 'ACCEPT' ") +
                                          "ORDER BY bitdepth - dirtobit ",
                                          fStartDepth_,
                                          fEndDepth_,
                                          dblMagneticFactor);
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                //DataColumn columnFirst = new DataColumn();
                //columnFirst.ColumnName = "#";
                //table.Columns.Add(columnFirst);
                for (int i = 0; i < 14 + iAdditionalColumns; i++)
                {
                    DataColumn column = new DataColumn();
                    column.DataType = reader.GetFieldType(i);
                    column.ColumnName = reader.GetName(i);
                    if (column.ColumnName == "TimeDate")
                        column.DataType = System.Type.GetType("System.DateTime");                    
                    else
                        column.DataType = System.Type.GetType("System.Decimal");
                    table.Columns.Add(column);
                }


                int iCount = 0;   // tracks number of records                   
                while (reader.Read())
                {
                    //Console.WriteLine("{0}, {1}, {2}, {3}, {4}", reader[0], reader[1], reader[2], reader[3], reader[4]);
                    DataRow row = table.NewRow();
                    //row[0] = iCount;
                    for (int i = 0; i < 14 + iAdditionalColumns; i++)
                        row[i] = reader[i];

                    table.Rows.Add(row);
                    iCount++;
                    //break;  // don't break, if you're going to do an insert afterwards because it will say that the datbase is locked
                    // let the while loop read through everything
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return table;
        }

        public float GetDirToBitLength(DateTime dtCreated_, out float fDirToBit_)
        {
            float fRetVal = 0.0f;
            fDirToBit_ = 0.0f;
            string sQuery = String.Format("SELECT bitdepth, dirTobit " +                                         
                                          "FROM tblSurvey " +
                                          "WHERE created = '{0}'",
                                          dtCreated_.ToString("yyyy-MM-dd HH:mm:ss.fff")); // depth can fall in without worrying about float precision
            try
            {
                // Create the command.
                DbCommand command = m_dbCnn.CreateCommand();
                command.CommandText = sQuery;
                command.CommandType = System.Data.CommandType.Text;
                // Retrieve the data.
                DbDataReader reader = command.ExecuteReader();

                reader.Read();
                fRetVal = System.Convert.ToSingle(reader[0]);
                fDirToBit_ = System.Convert.ToSingle(reader[1]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception.Message: {0}", ex.Message);
            }

            return fRetVal;
        }

        public void SetLengthUnit(string sVal_)
        {
            m_sLengthUnit = sVal_;
        }
    }
}

