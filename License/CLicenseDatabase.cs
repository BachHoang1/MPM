// author: hoan chau
// purpose: database layer for saving, checking, and looking up data in the license table

using System;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Linq;
using System.Collections.Generic;

namespace License
{
    public class CLicenseDatabase
    {
        private const string DB_PATH = "C:\\APSLicenseGenerator\\Data\\";
        private const string REPORT_PATH = "C:\\APSLicenseGenerator\\Reports\\";
        private const string DB_FILE = DB_PATH + "License.db";
        private const string CONNECTION_DB = "Data Source=" + DB_FILE;
        private int LICENSE_LOG_COLUMNS = 10;  // excludes the days_valid column which isn't used yet so doesn't need to be displayed

        public void Create()
        {            
            if (!File.Exists(DB_FILE))
            {
                SQLiteConnection con;
                SQLiteCommand cmd;
                Directory.CreateDirectory(DB_PATH);
                SQLiteConnection.CreateFile(DB_FILE);
                string sql = @"create table tblLicense(date integer, time integer, removeDate integer, removeTime integer, active int, computer text, user text, organization text, mac_address text, license text, days_valid integer);";
                con = new SQLiteConnection(CONNECTION_DB);
                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();

                string sqlTblVersion = @"create table tblVersion(version integer);";
                cmd = new SQLiteCommand(sqlTblVersion, con);
                cmd.ExecuteNonQuery();
                con.Close();                
            }      
            else  // which version is the database table
            {
                // check existence of tblVersion
                int iDBVersion = CheckTblVersionExists();
                if (iDBVersion == -1)
                {
                    if (UpgradeVersion0to1())
                    {
                        MessageBox.Show("Your License database has been successfully upgraded from version 0 to 1!", "Upgrade Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("The License database failed to upgrade from version 0 to 1.  Please contact the owner of the application for support.", "Upgrade Version", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (iDBVersion == 1)
                {
                    // everything is good
                }
                else
                {
                    MessageBox.Show("Unknown version of the License database.", "Invalid Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        public int CheckTblVersionExists()
        {
            int iRetVal = -1;

            DataTable table = new DataTable();
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (DbConnection cnn = fact.CreateConnection())
            {
                cnn.ConnectionString = CONNECTION_DB;
                // Open the connection.
                cnn.Open();

                string sQuery = String.Format("SELECT version FROM tblVersion");
                try
                {
                    // Create the command.
                    DbCommand command = cnn.CreateCommand();
                    command.CommandText = sQuery;
                    command.CommandType = System.Data.CommandType.Text;
                    // Retrieve the data.
                    DbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                        iRetVal = System.Convert.ToInt32(reader["version"]);

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception.Message: {0}", ex.Message);
                }

                cnn.Close();
            }

            return iRetVal;
        }

        public bool UpgradeVersion0to1()
        {
            bool bRetVal = false;            

            // alter the table to add the following fields
            // alter table tblLicense add column computer text
            DataTable table = new DataTable();
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (DbConnection cnn = fact.CreateConnection())
            {
                cnn.ConnectionString = CONNECTION_DB;                
                cnn.Open();

                int []iArrQueryRes = new int[6] { -1, -1, -1, -1, -1, -1 };
                string[] sArrQuery = new string[6];
                sArrQuery[0] = String.Format("ALTER TABLE tblLicense ADD COLUMN removeDate int");
                sArrQuery[1] = String.Format("ALTER TABLE tblLicense ADD COLUMN removeTime int");
                sArrQuery[2] = String.Format("ALTER TABLE tblLicense ADD COLUMN computer text DEFAULT unspecified");
                sArrQuery[3] = String.Format("ALTER TABLE tblLicense ADD COLUMN active int DEFAULT 1");                
                sArrQuery[4] = String.Format("CREATE TABLE tblVersion (version int)");
                sArrQuery[5] = String.Format("INSERT INTO tblVersion (version) VALUES (1)");

                bRetVal = true;
                for (int i = 0; i < sArrQuery.Length; i++)
                {
                    try
                    {
                        // Create the command.                    
                        DbCommand command = cnn.CreateCommand();
                        command.CommandText = sArrQuery[i];
                        command.CommandType = System.Data.CommandType.Text;
                        iArrQueryRes[i] = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)  // at first sign of failure, get out
                    {
                        MessageBox.Show(sArrQuery[i] + " Exception.Message: " + ex.Message, "Upgrade", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }

                cnn.Close();

                for (int i = 0; i < iArrQueryRes.Length; i++)
                {
                    if (iArrQueryRes[i] < 0)
                    {
                        bRetVal = false;
                        break;
                    }
                }
            }
                            
            return bRetVal;
        }

        public bool Save(CLicenseRecord rec_)
        {
            bool bRetVal = false;
            if (!File.Exists(DB_FILE))
            {
                MessageBox.Show("License database " + DB_FILE + "does not exist. Data can't be logged.", "Save Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
                using (DbConnection cnn = fact.CreateConnection())
                {
                    cnn.ConnectionString = CONNECTION_DB;
                    cnn.Open();
                    string sQuery = string.Format("INSERT INTO tblLicense (date, time, active, computer, user, organization, mac_address, license, days_valid) " +
                        " VALUES ({0}, {1}, {2}, '{3}', '{4}', '{5}', '{6}', '{7}', {8})", rec_.iDate, rec_.iTime, rec_.iActive, rec_.sComputer, rec_.sUser, rec_.sOrganization, rec_.sMACAddress, rec_.sLicense, rec_.iDaysValid);
                    try
                    {
                        // Create the command.
                        DbCommand command = cnn.CreateCommand();
                        command.CommandText = sQuery;
                        command.CommandType = System.Data.CommandType.Text;

                        // Retrieve the data.
                        int iRowsAffected = command.ExecuteNonQuery();

                        //Console.WriteLine("Inserted {0} rows.", iRowsAffected);
                        bRetVal = iRowsAffected == 1 ? true : false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed save of data. Exception: " + ex.Message + ".", "Save Record", MessageBoxButtons.OK, MessageBoxIcon.Error);                        
                    }

                    cnn.Close();
                }

            }

            return bRetVal;
        }

        public CLicenseRecord Find(string sMACAddress_)
        {
            CLicenseRecord recReturn = new CLicenseRecord();
            recReturn.iDate = 0;  // flag for empty record

            DataTable table = new DataTable();
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (DbConnection cnn = fact.CreateConnection())
            {
                cnn.ConnectionString = CONNECTION_DB;
                // Open the connection.
                cnn.Open();

                string sQuery = String.Format("SELECT * " +
                                "FROM tblLicense " +
                                "WHERE mac_address = '{0}' " +
                                "AND active = 1 " +
                                "ORDER BY date, time", sMACAddress_);
                try
                {
                    // Create the command.
                    DbCommand command = cnn.CreateCommand();
                    command.CommandText = sQuery;
                    command.CommandType = System.Data.CommandType.Text;
                    // Retrieve the data.
                    DbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        recReturn.iDate = System.Convert.ToInt32(reader["date"]);
                        recReturn.iTime = System.Convert.ToInt32(reader["time"]);
                        recReturn.sUser = reader["user"].ToString();
                        recReturn.sOrganization = reader["organization"].ToString();
                        break;                                                
                    }
                    reader.Close();                                        
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Find failed to locate record with MAC address " + sMACAddress_ + ". Exception: " + ex.Message + ".", "Find Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                cnn.Close();
            }

            return recReturn;
        }

        public DataTable Get(bool bOrderByCustomer_ = false)
        {
            DataTable table = new DataTable();
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (DbConnection cnn = fact.CreateConnection())
            {
                cnn.ConnectionString = CONNECTION_DB;
                // Open the connection.
                cnn.Open();

                string sQuery = String.Format("SELECT date AS activeDate, " +
                                                     "time AS activeTime, " +
                                                     "removeDate AS removeDate, " +
                                                     "removeTime AS removeTime, " +
                                                     "active, computer, user AS customer, organization AS company, mac_address, license " +
                                             "FROM tblLicense "); // +
                if (!bOrderByCustomer_)
                     sQuery += "ORDER BY date, time";
                else
                    sQuery += "ORDER BY customer";
                try
                {
                    // Create the command.
                    DbCommand command = cnn.CreateCommand();
                    command.CommandText = sQuery;
                    command.CommandType = System.Data.CommandType.Text;
                    // Retrieve the data.
                    DbDataReader reader = command.ExecuteReader();

                    DataColumn columnFirst = new DataColumn();
                    columnFirst.ColumnName = "#";
                    table.Columns.Add(columnFirst);
                    for (int i = 0; i < LICENSE_LOG_COLUMNS; i++)
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
                        DataRow row = table.NewRow();
                        row[0] = iCount;
                        for (int i = 1; i < LICENSE_LOG_COLUMNS + 1; i++)
                            row[i] = reader[i - 1];

                        table.Rows.Add(row);
                        iCount++;                        
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not get records. Exception: " + ex.Message + ".", "Get Records", MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                }

                cnn.Close();
            }

            return table;
        }

        public bool Update(CLicenseRecord rec_)
        {
            bool bRetVal = false;            

            DataTable table = new DataTable();
            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (DbConnection cnn = fact.CreateConnection())
            {
                cnn.ConnectionString = CONNECTION_DB;
                
                cnn.Open();

                string sQuery = String.Format("UPDATE tblLicense " +
                                "SET active = {0}, removeDate = {1}, removeTime = {2}, computer = '{3}' " +
                                "WHERE mac_address = '{4}' " +
                                "AND organization = '{5}' " +
                                "AND user = '{6}' ",
                                rec_.iActive, rec_.iRemovedDate, rec_.iRemovedTime, rec_.sComputer, rec_.sMACAddress, rec_.sOrganization, rec_.sUser);
                try
                {
                    // Create the command.
                    DbCommand command = cnn.CreateCommand();
                    command.CommandText = sQuery;
                    command.CommandType = System.Data.CommandType.Text;
                    
                    int iRowsAffected = command.ExecuteNonQuery();
                    if (iRowsAffected == 1)
                        bRetVal = true;                    
                }
                catch (Exception ex)
                {                    
                    MessageBox.Show("Error updating record with MAC Address " + rec_.sMACAddress + ". " + "Exception: " + ex.Message, "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                cnn.Close();
            }

            return bRetVal;
        }

        public void Report()
        {
            CLicenseDatabase db = new CLicenseDatabase();
            DataTable dt = db.Get(true);
            try
            {
                string sReportName = REPORT_PATH + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + " report.csv";
                StreamWriter sw = new StreamWriter(sReportName, false);
                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName);
                
                sw.WriteLine(string.Join(",", columnNames));
                foreach (DataRow row in dt.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                    sw.WriteLine(string.Join(",", fields));                    
                }
                sw.Close();
                MessageBox.Show("Report was successfully generated. Please get it from here: " + sReportName + ".", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {                
                MessageBox.Show("Failed to create report. Exception: " + ex.Message, "Report", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
