using PRP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRP
{
    public class MethodOrFunction
    {
        public static string GetInvoice(string fieldName, string tableName)
        {
            //****************** How to Use **********************//
            // string No = MethodOrFunction.GetInvoice("No", "Test1");          // No: field Name; Test1: Table Name 
            // label1.Text = No;                                                // Show Labe/Textbox 

            db_ppl Connstring = new db_ppl();
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string query = @"SELECT MAX(" + fieldName + ") AS Max FROM " + tableName + " GROUP BY MONTH(AddDate) HAVING (MONTH(AddDate) = '" + month + "')";
            DataTable dt = Connstring.SqlDataTable(query);
            if (dt.Rows.Count >= 1)
            {
                string sqldata = dt.Rows[0]["MAX"].ToString();
                string[] tokens = sqldata.Split('-');
                string middle = tokens[tokens.Length - 2];
                string increment = Convert.ToString(Convert.ToInt32(middle.ToString()) + 1);
                string zero = "00000";
                int get_lengt = zero.Length - increment.Length;
                string lenght = zero.Substring(0, get_lengt);
                string cash_memo = lenght + increment;
                string No = year + "-" + cash_memo + "-" + month;
                return No;
            }
            else
            {
                string No = year + "-" + "00001" + "-" + month;
                return No;
            }
        }

        internal static DataSet GetData(string tableName)
        {
            //****************** How to Use **********************//
            //DataSet ds = MethodOrFunction.GetData("Test1");                          // Test1: Table Name 
            //dataGridView1.DataSource = ds.Tables["Data"].DefaultView;                // Show DataGridview

            db_ppl Connstring = new db_ppl();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * from " + tableName + " order by ID desc", Connstring.conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "Data");
            return ds;
        }
    }
}
