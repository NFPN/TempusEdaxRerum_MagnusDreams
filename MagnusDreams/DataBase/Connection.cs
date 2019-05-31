using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagnusDrems.DAO
{
    class Connection
    {
        SqlConnection sqlConnection = new SqlConnection();

        public Connection()
        {
            sqlConnection.ConnectionString = $"Integrated Security=SSPI;Persist Security Info=True;User ID={Environment.MachineName};Initial Catalog=MagnusDreams;Data Source=NICOLAS-PC";
            //sqlConnection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\nico_\Documents\MagnusDreamsBD.mdf;Integrated Security=True;Connect Timeout=30";
        }

        public SqlConnection connect()
        {
            if(sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
            return sqlConnection;
        }

        public void desconnect()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
    }
}
