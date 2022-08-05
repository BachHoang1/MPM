// author: hoan chau
// purpose: evaluate simple arithmetic expressions

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPM.Utilities
{
    class CEvaluate
    {
        public const double BAD_VALUE = -9999.9;  // 

        public double Do(string sExpression_)
        {
            double dblRetVal = BAD_VALUE;
            try
            {
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("expression", string.Empty.GetType(), sExpression_);
                System.Data.DataRow row = table.NewRow();
                table.Rows.Add(row);
                dblRetVal = double.Parse((string)row["expression"]);
            }
            catch(Exception e)
            {
                MessageBox.Show("Evaluate::Do error while trying to solve expression: " + sExpression_ + ". " + e.Message, "Formula", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            return dblRetVal;
        } 
        
        public Boolean DoBoolean(string sExpression_)
        {
            Boolean bRetVal = false;
            try
            {
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("", typeof(Boolean));
                table.Columns[0].Expression = sExpression_;  

                System.Data.DataRow r = table.NewRow();
                table.Rows.Add(r);
                bRetVal = (Boolean)r[0];
            }
            catch (Exception e)
            {
                MessageBox.Show("Evaluate::Do error while trying to solve expression: " + sExpression_ + ". " + e.Message, "Formula", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return bRetVal;
        }
    }
}
