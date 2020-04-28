using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace PRP
{
    public class db_ppl
    {
        public SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UserManagement;Persist Security Info=True;User ID=sa;Password=ppl@123#"); //Local

        public SqlConnection getcon
        {
            get { return conn; }
        }

        public DataTable SqlDataTable(string sql)
        {
            //try
            //{
                conn.Close();
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                return dt;
            //}
            //catch
            //{
               

            //    SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=PPL;Persist Security Info=True;User ID=sa;Password=ppl@123#");

            //    conn.Close();
            //    SqlCommand cmd = new SqlCommand(sql, conn);
            //    conn.Open();
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    DataTable dt = new DataTable();
            //    da.Fill(dt);
            //    conn.Close();
            //    return dt;

            //}
            
        }

        public void insert(dynamic qurry)
        {
            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(qurry, conn))
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                conn.Close();
            }
            catch (SqlException ex)
            {
                conn.Close();
                string msg = "Insert Error:";
                msg += ex.Message;
            }
        }

    }
}