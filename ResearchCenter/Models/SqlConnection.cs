using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ResearchCenter.Models
{
    public class SqlConnectionClass
    {
        public static string ConnectionString;
        public SqlConnectionClass()
        {
            //ConnectionString = "Data Source=DESKTOP-NJBSP6T;Initial Catalog=ResearchCenterDB;Integrated Security=True";
            ConnectionString = "Data Source=191.96.52.2;Initial Catalog=mandiga2_KURC;Persist Security Info=true;User ID=mandiga2KURC;Password=3s0raV7~3";
        }

        public DataTable Select(string query)
        {
            DataTable dt = new DataTable();
            using(SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
            }
            return dt;
        }

        public void Insert(string query)
        {
            using(SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(string query)
        {
            using(SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(string query)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
        }
    }
}