using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRP
{
    public class db_cdb
    {
        public SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=PPL;Persist Security Info=True;User ID=dev;Password=ppl@123#");  //Local
        //-------------------------------------//
        public DataTable dt = new DataTable();
        public SqlCommand cmd = new SqlCommand();
        public SqlDataAdapter da = new SqlDataAdapter();

        public SqlConnection getcon
        {
            get { return conn; }
        }

        public DataTable SqlDataTable(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public int insert(dynamic qurry)
        {
            int newId = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand(qurry, conn))
                {
                    conn.Open();
                    //cmd.ExecuteNonQuery();
                    newId = (int)cmd.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                string msg = "Insert Error:";
                msg += ex.Message;
                newId = 0;
            }
            return newId;
        }

        public void insert_details(dynamic qurry)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(qurry, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                string msg = "Insert Error:";
                msg += ex.Message;
            }
        }

        public void CallProcedure(Array sqlpra, string procidurname)  // call Procedur whith Paramiter and Procidure Name
        {
            try
            {
                cmd = new SqlCommand(procidurname, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                if (sqlpra != null)
                {
                    cmd.Parameters.AddRange(sqlpra);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
            }

        }

    }
}
