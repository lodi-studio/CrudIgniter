using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CrudIgniter.Helper
{
    public class CRUD
    {

        #region Constructor
        public CRUD(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        #endregion

        #region "Property"
        public string ConnectionString { get; set; }
        public string Table { get; set; }
        public string[] Columns { get; set; }
        public string[] Rows { get; set; }
        public object[] Values { get; set; }
        public string[] Where { get; set; }
        public object[] WhereValues { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Getdata without parameters
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetData(string sql)
        {
            DataTable result = new DataTable();

            using (MySqlConnection cn = new MySqlConnection(this.ConnectionString))
            {
                cn.Open();

                using (MySqlDataAdapter da = new MySqlDataAdapter(sql, cn))
                {
                    da.Fill(result);
                }
            }

            return result;
        }

        /// <summary>
        /// GetData with parameters required
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable GetData(string sql, object[] parameters)
        {
            DataTable result = new DataTable();

            using (MySqlConnection cn = new MySqlConnection(this.ConnectionString))
            {
                cn.Open();

                using (MySqlDataAdapter da = new MySqlDataAdapter(sql, cn))
                {

                    int counter = 0;
                    foreach (object obj in parameters)
                    {
                        da.SelectCommand.Parameters.Add("@" + counter, ToMysqlDbType(obj));

                        da.SelectCommand.Parameters[counter].Value = obj;

                        counter++;
                    }

                    da.SelectCommand.Prepare();

                    da.Fill(result);

                }
            }

            return result;
        }

        public DataTable Select(string tableName) 
        {
            DataTable result = new DataTable();

            string sql = "SELECT * FROM " + tableName;  

            return this.GetData(sql);
        }

        public DataTable Select(string tableName, string[] columns)
        {
            DataTable result = new DataTable();

            string cols = "";
            foreach (string col in columns) 
            {
                cols += col + ",";
            }

            cols = cols.TrimEnd(new char[] { ',' });

            string sql = "SELECT "+ cols +" FROM " + tableName;

            return this.GetData(sql);
        }

        public void ExecuteQuery(string sql)
        {
            using (MySqlConnection cn = new MySqlConnection(this.ConnectionString))
            {
                cn.Open();

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {
                    //Execute command
                    cmd.ExecuteNonQuery();

                }
            }
        }

        public void ExecuteQuery(string sql, object[] parameters) 
        {
            using (MySqlConnection cn = new MySqlConnection(this.ConnectionString))
            {
                cn.Open();

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {

                    int counter = 0;

                    //Set values parameters value
                    foreach (object obj in parameters)
                    {
                        cmd.Parameters.Add("@" + counter, ToMysqlDbType(obj));
                        cmd.Parameters[counter].Value = obj;
                        counter++;
                    }

                    //Prepare command
                    cmd.Prepare();

                    //Execute command
                    cmd.ExecuteNonQuery();

                }
            }
        }

        public void Insert()
        {
            using (MySqlConnection cn = new MySqlConnection(this.ConnectionString))
            {
                cn.Open();

                string cols = "";
                string values = "";
                int colCounter = 0;

                //Looping the this.Columns
                foreach (string column in this.Columns)
                {
                    cols += column + ",";
                    values += "@" + colCounter + ",";
                    colCounter++;
                }

                //Trim the last char ',' in the string
                cols = cols.TrimEnd(new char[] { ',' });
                values = values.TrimEnd(new char[] { ',' });

                //Output will be INSERT INTO TableName(col1,col2,col3) VALUES(@0,@1,@2) 
                //cols are based on this.Columns
                //@values are based this.Values
                string sql = "INSERT INTO " + this.Table + "(" + cols + ") VALUES(" + values + ")";

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {

                    int counter = 0;

                    //Set values parameters value
                    foreach (object obj in this.Values)
                    {
                        cmd.Parameters.AddWithValue("@" + counter, obj);
                        counter++;
                    }

                    //Prepare command
                    cmd.Prepare();

                    //Execute command
                    cmd.ExecuteNonQuery();

                }
            }
        }

        public void Update()
        {
            using (MySqlConnection cn = new MySqlConnection(this.ConnectionString))
            {
                cn.Open();


                string setClause = "";
                int colCounter = 0;

                //Output : col1=@0,col2=@1,col3=@2
                //Looping the this.Columns and set the as numbers e.g @0,@1,@2
                foreach (string column in this.Columns)
                {
                    setClause += column + "=@" + colCounter + ",";
                    colCounter++;
                }

                string whereClause = "";

                //Output ; where1=@where1 AND where2=@where2
                foreach (string where in this.Where)
                {
                    whereClause += where + "=@" + where + " AND ";
                }

                //Trim the last char ',' in the string
                setClause = setClause.TrimEnd(new char[] { ',' });

                //Remove the last 'AND ' string in the string
                whereClause = whereClause.Substring(0, whereClause.Length - 4);

                //Output will be UPDATE TableName SET col1=@0, col2=@1, col3=@2 WHERE whereCol1=@whereCol1 AND whereCol2=@whereCol2
                //setClause is based on how many this.Values
                string sql = "UPDATE " + this.Table + " SET " + setClause + " WHERE " + whereClause;

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {
                    //Set setClause parameters value
                    int counter = 0;
                    foreach (object obj in this.Values)
                    {
                        cmd.Parameters.AddWithValue("@" + counter, obj);
                        counter++;
                    }

                    //Set whereClause parameters value
                    int whereCounter = 0;
                    foreach (object obj in this.WhereValues)
                    {
                        cmd.Parameters.AddWithValue("@" + this.Where[whereCounter].ToString(), obj);

                        whereCounter++;
                    }

                    //Prepare command
                    cmd.Prepare();

                    //Execute command
                    cmd.ExecuteNonQuery();

                }
            }
        }

        public void Delete()
        {
            using (MySqlConnection cn = new MySqlConnection(this.ConnectionString))
            {
                cn.Open();

                string whereClause = "";

                //Output ; where1=@where1 AND where2=@where2
                foreach (string where in this.Where)
                {
                    whereClause += where + "=@" + where + " AND ";
                }

                //Remove the last 'AND ' string in the string
                whereClause = whereClause.Substring(0, whereClause.Length - 4);

                //Combine sql query
                string sql = "DELETE FROM " + this.Table + " WHERE " + whereClause;

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {

                    int whereCounter = 0;

                    //Set whereClause parameters value
                    foreach (object obj in this.WhereValues)
                    {
                        cmd.Parameters.AddWithValue("@" + this.Where[whereCounter].ToString(), obj);

                        whereCounter++;
                    }

                    //Prepare command
                    cmd.Prepare();

                    //Execute command
                    cmd.ExecuteNonQuery();
                }

            }
        }

        #endregion

        #region Helper

        public MySqlDbType ToMysqlDbType(object obj)
        {
            MySqlDbType result = new MySqlDbType();

            switch (obj.GetType().ToString())
            {
                case "System.Boolean":
                    result = MySqlDbType.Int16;
                    break;

                case "System.Byte":
                    result = MySqlDbType.Byte;
                    break;

                case "System.SByte":
                    result = MySqlDbType.Byte;
                    break;

                case "System.Char":
                    result = MySqlDbType.VarChar;
                    break;

                case "System.Decimal":
                    result = MySqlDbType.Decimal;
                    break;

                case "System.Double":
                    result = MySqlDbType.Double;
                    break;

                case "System.Single":
                    result = MySqlDbType.Float;
                    break;

                case "System.Int32":
                    result = MySqlDbType.Int32;
                    break;

                case "System.UInt32":
                    result = MySqlDbType.UInt32;
                    break;

                case "System.Int64":
                    result = MySqlDbType.Int64;
                    break;

                case "System.UInt64":
                    result = MySqlDbType.UInt64;
                    break;

                case "System.Int16":
                    result = MySqlDbType.Int16;
                    break;

                case "System.UInt16":
                    result = MySqlDbType.UInt16;
                    break;

                case "System.String":
                    result = MySqlDbType.String;
                    break;

                case "System.DateTime":
                    result = MySqlDbType.DateTime;
                    break;

                case "System.Date":
                    result = MySqlDbType.Date;
                    break;
            }

            return result;
        }

        #endregion

    }
}
