using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Data.Odbc;
using System.Configuration;
using System.Reflection;
using System.Windows.Input;
using System.Collections;
using Dapper;
using System.Collections.Generic;
//Use this namespace to access sqlserver database .
namespace BaseAppData
{
    public static class POSConvert
    {
        public static IList<T> ConvertSqlQueryToIList<T>(this SqlCeConnection SqlConn, T domainClass, string sqlQueryCommand)
        {
            ConnectionState _ConnectionState = ConnectionState.Closed;
            try
            {
                if (SqlConn.State != ConnectionState.Open)
                {
                    SqlConn.Open();
                    _ConnectionState = SqlConn.State;
                }
                IList<T> result = SqlConn.Query<T>(sqlQueryCommand).AsList();
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_ConnectionState == ConnectionState.Open)
                    SqlConn.Close();
            }
        }

       
    }
  class SqlCEDataAccess
    {
        // This is for SQLce Server Database
        public SqlCeConnection connection;
        public string DataAccessError;

        public SqlCEDataAccess()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            //Data Source=" + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\MyData.sdf;
            //Persist Security Info = False;
            string DbLocation = BaseAppUI.Properties.Settings.Default.DatabaseLocation;
            string Cstring = "Data Source = posDB.sdf";

            if (DbLocation != "C:")
            {
                DbLocation = DbLocation + "\\posDB.sdf";
                Cstring = string.Format("Data Source = {0} ", DbLocation);   //posDB.sdf";
                
            }//    AppDomain.CurrentDomain.SetData("DataDirectory", DbLocation); //this was causing error in register if you go from item to register.
            
           
            connection = new SqlCeConnection(Cstring);
           
            
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
                    SqlCeCommand cmd = new SqlCeCommand(txtSQL, connection);
                    //Execute command
                    Affected = cmd.ExecuteScalar().ToString();
                    //DataAccessError = cmd.CommandText(); //. cmd.Notification.ToString();
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
                    SqlCeCommand cmd = new SqlCeCommand(txtSQL, connection);
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
                    SqlCeCommand cmd = new SqlCeCommand(txtSQL, connection);
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
                    SqlCeCommand cmd = new SqlCeCommand(txtSQL, connection);
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
        ////Convert request to IList
        ////
        

        /// <summary>
        /// 
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
                    SqlCeCommand cmd = new SqlCeCommand(txtSQL, connection);
                    
                    SqlCeDataAdapter adp = new SqlCeDataAdapter(cmd);
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
