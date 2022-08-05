

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace MPM.Data
{
    public class CLogTransducerSetting
    {
        private int TRANSDUCER_LOG_COLUMNS = 6;
        private DbConnection m_dbCnn;

        public CLogTransducerSetting(ref DbConnection dbCnn_)
        {
            m_dbCnn = dbCnn_;
        }

        public bool Save(CLogTransducerSettingRecord rec_)
        {
            bool bRetVal = false;
            
            //4/23/22string sQuery = string.Format("INSERT INTO tblTransducer (date, time, type, unit, offset, gain) " +
            //4/23/22                              " VALUES ({0}, {1}, '{2}', '{3}', {4}, {5})", 
            //4/23/22                              rec_.iDate, rec_.iTime, rec_.sType, rec_.sUnit, rec_.fOffset, rec_.fGain);

            string sQuery = string.Format("INSERT INTO tblTransducer (Date, Time, Max Pressure, Unit, Offset, Gain) " +
                                  " VALUES ({0}, {1}, '{2}', '{3}', {4}, {5})",
                                  rec_.iDate, rec_.iTime, rec_.sType, rec_.sUnit, rec_.fOffset, rec_.fGain);
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
                Console.WriteLine("Exception.Message: {0}", ex.Message);
            }
                
            return bRetVal;
        }

        public DataTable Get()
        {
            DataTable table = new DataTable();
            
            string sQuery = String.Format("SELECT * " +
                                          "FROM tblTransducer " +                                
                                          "ORDER BY Date, Time");  //4/24/22
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
                for (int i = 0; i < TRANSDUCER_LOG_COLUMNS; i++)
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
                    for (int i = 1; i < TRANSDUCER_LOG_COLUMNS + 1; i++)
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
