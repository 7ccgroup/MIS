using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Configuration;
using System.Data.SqlServerCe;
//Use this namespace to access sqlserver database .
namespace BaseAppData
{
  class SqlServerDataAccess
    {
        // This is for SQL Server Database
        private SqlConnection connection;
        public string DataAccessError;

        public SqlServerDataAccess()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            //Data Source=" + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\MyData.sdf;
            //Persist Security Info = False;
            string Cstring = "Data Source = DataDirectory\\posDB.sdf providerName=System.Data.SqlServerCE.4.0";
            connection = new SqlConnection(Cstring);
        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (OdbcException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (OdbcException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


  
        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query. 
        /// Additional columns or rows are ignored
        /// </summary>
        public bool execScalar(string txtSQL)
        {
            string Affected = "";
            try
            {
                //open connection
                if (this.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    SqlCommand cmd = new SqlCommand(txtSQL, connection);
                    //Execute command
                    Affected = cmd.ExecuteScalar().ToString();
                    DataAccessError = cmd.Notification.ToString();
                    //close connection
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (Affected == "")
                return false;
            else
                return true;
        }
        public long execSelect(string txtSQL)
        {
            long rtn_long = 0;
            try
            {
                //open connection
                if (this.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    SqlCommand cmd = new SqlCommand(txtSQL, connection);
                       //Execute command
                    object return_result;
                    return_result = cmd.ExecuteScalar();

                    if (return_result == System.DBNull.Value)
                        rtn_long = 0;
                    else
                        rtn_long = Convert.ToInt64(return_result.ToString());
                    //close connection
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return rtn_long;
        }

        public string execSelectString(string txtSQL)
        {
            String rtn_long = "No";
            try
            {
                //open connection
                if (this.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    SqlCommand cmd = new SqlCommand(txtSQL, connection);
                    //Execute command
                    object return_result;
                    return_result = cmd.ExecuteScalar();

                    if (return_result == System.DBNull.Value)
                        rtn_long = "No";
                    else
                        rtn_long = return_result.ToString();
                    //close connection
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return rtn_long;
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected
        /// </summary>
        public bool execNonQuery(string txtSQL)
        {
            int Affected = 0;
            try
            {
                //open connection
                if (this.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    SqlCommand cmd = new SqlCommand(txtSQL, connection);
                     //Execute command
                    Affected = cmd.ExecuteNonQuery();

                    //close connection
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {

                //if (NuOS.Properties.Settings.Default.debug == 1)
                //{
                 //   MessageBox.Show(ex.Message);
                  //  MessageBox.Show(ex.Data.ToString());
                //}
                Console.WriteLine(ex.Message);

            }

            if (Affected > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns a dataset depending on the SQL that is passed.
        /// </summary>
        public DataSet dsGetData(string txtSQL, string table)
        {
            DataSet dsAll = new DataSet();

            try
            {
                if (this.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    SqlCommand cmd = new SqlCommand(txtSQL, connection);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    //OleDbCommand cmd = new OleDbCommand(txtSQL, connection);
                    //OleDbDataAdapter adp = new OleDbDataAdapter(cmd);
                    adp.Fill(dsAll, table);

                    //close connection
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                //if (NuOS.Properties.Settings.Default.debug == 1)
                 //   MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
            }
            return dsAll;
        }

        public static string ServerName()
        {
            string returnresult = "";
            try
            {
                //Database db = null;
                //db = DatabaseFactory.CreateDatabase("ConnString");
                //string myconn = db.ConnectionString;
                //myconn = myconn.Substring(0, myconn.IndexOf(";"));
                //myconn = myconn.Substring(myconn.IndexOf("=") + 1);
                //returnresult = myconn;
                //db = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return returnresult;
        }
    }
   
}
