using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Transactions;

namespace NetCoreGraphQL.ClassDB
{
    /// <summary>
    /// 
    /// </summary>
    public class DBFactory
    {
        private static string _ConnectString = "";
        private static string _DatabaseType = "";
        private static string _DBProvider = "";
        private static Logger logger = LogManager.GetCurrentClassLogger();

        DbConnection SQLConn = null;

        /// <summary>
        /// 
        /// </summary>
        public struct CDatabaseType
        {
            public const string Oracle = "ORACLE";
            public const string SQLServer = "SQLSERVER";
            public const string MySQL = "MYSQL";
            public const string PostgreSQL = "POSTGRES";
        }

        public struct CConnectString
        {
            public const string Oracle = "Data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));User Id={3};Password={4};Max Pool Size={5};Connection Timeout={6};";
            public const string SQLServer = "Server={0};Database={2};User ID={3};Password={4};Max Pool Size={5};Connection Timeout={6};";
            public const string MySQL = "Server={0};Port={1};Database={2};Uid={3};Pwd={4};maximumpoolsize={5};";
            public const string PostgreSQL = "Host={0};Port={1};Database={2};Username={3};Password={4};";
        }

        public struct CDatabaseBindMarker
        {
            public const string Oracle = ":";
            public const string SQLServer = "@";
            public const string MySQL = "?";
            public const string PostgreSQL = "@";
        }

        public struct CDatabaseProvider
        {
            public const string Oracle = "Oracle.DataAccess.Client";
            public const string SQLServer = "System.Data.SqlClient";
            public const string MySQL = "MySql.Data.MySqlClient";//"MySql.Data";//
            public const string PostgreSQL = "Npgsql";
        }

        public DBFactory(string sProviderName, string sDatabaseType, string sConnectString)
        {
            _ConnectString = sConnectString;
            _DatabaseType = sDatabaseType;
            _DBProvider = sProviderName;
        }

        public static string TransformSQL(string sSQL, string sDBType = "", string sPlaceHolder = "#")
        {

            int phLen = sPlaceHolder.Length;
            StringBuilder sb = new StringBuilder(sSQL);
            int iknt = 0;
            string bindXter = null;

            if (string.IsNullOrEmpty(sDBType))
                sDBType = _DatabaseType;

            try
            {
                dynamic Location = sb.ToString().IndexOf(sPlaceHolder);
                while (Location > -1)
                {
                    iknt += 1;
                    bindXter =
                        sDBType == CDatabaseType.MySQL ? CDatabaseBindMarker.MySQL :
                        sDBType == CDatabaseType.Oracle ? CDatabaseBindMarker.Oracle + iknt.ToString() :
                        sDBType == CDatabaseBindMarker.SQLServer ? CDatabaseBindMarker.SQLServer + iknt.ToString() :
                        CDatabaseBindMarker.PostgreSQL + iknt.ToString();


                    sb.Replace(sPlaceHolder, bindXter, Location, sPlaceHolder.Length);
                    Location = sb.ToString().IndexOf(sPlaceHolder);
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "[TransformSQL] " + ex.Message);
            }
            return sb.ToString();
        }

        public string GetProviderFactoryClasses()
        {
            // Retrieve the installed providers and factories.
            DataTable table = DbProviderFactories.GetFactoryClasses();
            string ret = null;
            int i = 0;
            int j = 0;
            try
            {
                j = 1;
                foreach (DataRow row in table.Rows)
                {
                    i = 0;
                    foreach (DataColumn column in table.Columns)
                    {
                        //column = column_loopVariable;
                        i = i + 1;
                        ret = ret + (i == 1 ? string.Format("[{0}] {1} ", j, row[column]) : i == 3 ? string.Format("  |  {0}", row[column]) : "");
                    }
                    ret = ret + Environment.NewLine;
                    j = j + 1;
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "[GetProviderFactoryClasses] " + ex.Message);
            }
            return ret;
        }


        public DbType GetDbTypeByName(string typeName)
        {
            DbType ColDbType = default(DbType);

            switch (typeName.ToUpper())
            {
                case "STRING":
                    ColDbType = DbType.String;
                    break;
                case "DECIMAL":
                    ColDbType = DbType.Decimal;
                    break;
                case "DOUBLE":
                    ColDbType = DbType.Double;
                    break;
                case "DATETIME":
                    ColDbType = DbType.DateTime;
                    break;
                case "DBNULL":
                    ColDbType = DbType.String;
                    break;
                case "INT32":
                    ColDbType = DbType.Int32;
                    break;
            }
            return ColDbType;
        }
        //public bool CheckDBConn(string cConnectString = "", string cDatabaseType = "", string cProviderName = "")
        //{
        //    bool bRet = false;
        //    string sql = string.Empty;
        //    if (string.IsNullOrEmpty(cConnectString))
        //        cConnectString = _ConnectString;
        //    if (string.IsNullOrEmpty(cDatabaseType))
        //        cDatabaseType = _DatabaseType;
        //    if (string.IsNullOrEmpty(cProviderName))
        //        cProviderName = _DBProvider;

        //    try
        //    {
        //        //2018-05-24 04:52:59.4375 INFO Host=10.132.0.3;Port=5432;Database=smshubdb;Username=smshub;Password=satellite1$; | POSTGRES| Npgsql
        //        //2018-05-24 04:52:59.6563 INFO Open 
        //        //2018-05-24 04:52:59.6719 INFO select count(1) as checkdb from Setting

        //        DbProviderFactory iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();

        //        using (SQLConn = iFactory.CreateConnection())
        //        {
        //            SQLConn.ConnectionString = cConnectString;
        //            SQLConn.Open();
        //            using (DbCommand SQLCmd = SQLConn.CreateCommand())
        //            {
        //                SQLCmd.CommandText = TransformSQL(SQLLib.GetCheckDBConn, Util._dbType);
        //                using (DbDataReader i = SQLCmd.ExecuteReader())
        //                {
        //                    if (i.HasRows)
        //                    {
        //                        while (i.Read())
        //                        {
        //                            bRet = true;
        //                        }
        //                    }
        //                }

        //            }
        //            SQLConn.Close();

        //        }
        //    }
        //    catch (DbException DBEx)
        //    {
        //        logger.Log(LogLevel.Error, $"[DBFactory][SQLExecuteCommand] { DBEx.Message } | {sql}");
        //    }
        //    catch (Exception ex)
        //    {
        //        var logStr = $"[DBFactory][SQLExecuteCommand] Error :: {ex.Message} | { ex.InnerException?.Message } | { ex.InnerException?.InnerException?.Message}| {sql}";
        //        logger.Log(LogLevel.Error, logStr);
        //        var logStr2 = $"[DBFactory][SQLExecuteCommand] StackTrace :: {ex.StackTrace}";
        //        logger.Log(LogLevel.Error, logStr2);
        //    }
        //    return bRet;
        //}

        public int SQLExecuteCommand(string sSQL, object[] strParam, string cConnectString = "", string cDatabaseType = "", string cProviderName = "")
        {
            int bRet = 0;
            int pknt = 0;
            string sql = string.Empty;
            if (string.IsNullOrEmpty(cConnectString))
                cConnectString = _ConnectString;
            if (string.IsNullOrEmpty(cDatabaseType))
                cDatabaseType = _DatabaseType;
            if (string.IsNullOrEmpty(cProviderName))
                cProviderName = _DBProvider;

            try
            {
                DbProviderFactory iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();
                StringBuilder sb = new StringBuilder();
                using (SQLConn = iFactory.CreateConnection())
                {
                    SQLConn.ConnectionString = cConnectString;
                    SQLConn.Open();
                    using (DbCommand SQLCmd = SQLConn.CreateCommand())
                    {

                        SQLCmd.CommandText = TransformSQL(sSQL, cDatabaseType);

                        sql = SQLCmd.CommandText;
                        pknt = strParam.Length;
                        DbParameter p = default(DbParameter);

                        for (int j = 0; j <= pknt - 1; j++)
                        {
                            if ((strParam[j] == null))
                                strParam[j] = "null";

                            p = iFactory.CreateParameter();
                            if (strParam[j].ToString() == "-9")
                            {
                                p.DbType = DbType.Int32;
                                p.Value = DBNull.Value;
                            }
                            else if (strParam[j].ToString() == "01-01-01")
                            {
                                p.DbType = DbType.DateTime;
                                p.Value = DBNull.Value;
                            }
                            else
                            {
                                //p.DbType = GetDbTypeByName(string.IsNullOrEmpty(strParam[j].ToString()) ? "DBNULL" : strParam[j].GetType().Name);
                                //p.Value = strParam[j] == null ? DBNull.Value : strParam[j];

                                p.DbType = GetDbTypeByName(strParam[j].ToString() == "null" ? "DBNULL" : strParam[j].GetType().Name);
                                p.Value = strParam[j].ToString() == "null" ? DBNull.Value : strParam[j];
                            }
                            p.ParameterName = (j + 1).ToString();
                            SQLCmd.Parameters.Add(p);
                            //sb.Append($"{p.Value.ToString()}:{p.DbType}, ");
                        }
                        //logger.Log(LogLevel.Info, sb.ToString());
                        bRet = SQLCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (DbException DBEx)
            {


                if (DBEx.Message.Contains("appindex_smsq"))
                {
                    bRet = -2;
                }
                else
                {
                    bRet = -1;
                    logger.Log(LogLevel.Error, $"[DBFactory][SQLExecuteCommand] { DBEx.Message } | {sql}");
                }
            }
            catch (Exception ex)
            {

                var logStr = $"[DBFactory][SQLExecuteCommand] Error :: {ex.Message} | { ex.InnerException?.Message } { ex.InnerException?.InnerException?.Message}| {sql}";
                logger.Log(LogLevel.Error, logStr);
                if (ex.Message.Contains("appindex_smsq"))
                    bRet = -2;
                else
                    bRet = -1;
            }
            //if (bRet == 0)
            //{
            //    string str = "";
            //    for (int j = 0; j <= strParam.Length - 1; j++)
            //    {
            //        str = str + strParam[j].ToString();
            //    }
            //    logger.Log(LogLevel.Error, $"{ str}");
            //    logger.Log(LogLevel.Error, $"{ sql}");
            //}           
            return bRet;
        }

        public int SQLExecuteCommand(string sSQL, string cConnectString = "", string cDatabaseType = "", string cProviderName = "")
        {
            int bRet = 0;

            if (string.IsNullOrEmpty(cConnectString))
                cConnectString = _ConnectString;
            if (string.IsNullOrEmpty(cDatabaseType))
                cDatabaseType = _DatabaseType;
            if (string.IsNullOrEmpty(cProviderName))
                cProviderName = _DBProvider;

            try
            {
                DbProviderFactory iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();

                using (SQLConn = iFactory.CreateConnection())
                {
                    SQLConn.ConnectionString = cConnectString;
                    SQLConn.Open();
                    using (DbCommand SQLCmd = SQLConn.CreateCommand())
                    {
                        SQLCmd.CommandText = TransformSQL(sSQL, cDatabaseType);
                        bRet = SQLCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (DbException DBEx)
            {

                logger.Log(LogLevel.Error, $"[DBFactory][SQLExecuteCommand] { DBEx.Message } | {sSQL}");
                if (DBEx.Message.Contains("appindex_smsq"))
                    bRet = -2;
                else
                    bRet = -1;
            }
            catch (Exception ex)
            {

                var logStr = $"[DBFactory][SQLExecuteCommand] Error :: {ex.Message} | { ex.InnerException?.Message } { ex.InnerException?.InnerException?.Message}| {sSQL}";
                logger.Log(LogLevel.Error, logStr);
                if (ex.Message.Contains("appindex_smsq"))
                    bRet = -2;
                else
                    bRet = -1;
            }

            return bRet;

        }

        public int SQLSelectCount(string sSQL, string cConnectString = "", string cDatabaseType = "", string cProviderName = "")
        {
            int bRet = 0;

            if (string.IsNullOrEmpty(cConnectString))
                cConnectString = _ConnectString;
            if (string.IsNullOrEmpty(cDatabaseType))
                cDatabaseType = _DatabaseType;
            if (string.IsNullOrEmpty(cProviderName))
                cProviderName = _DBProvider;

            try
            {
                DbProviderFactory iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();
                using (SQLConn = iFactory.CreateConnection())
                {
                    SQLConn.ConnectionString = cConnectString;
                    SQLConn.Open();
                    using (DbCommand SQLCmd = SQLConn.CreateCommand())
                    {
                        SQLCmd.CommandText = TransformSQL(sSQL, cDatabaseType);

                        using (DbDataReader RDR = SQLCmd.ExecuteReader())
                        {
                            if (RDR.HasRows)
                            {
                                while (RDR.Read())
                                {
                                    bRet = RDR.GetInt32(0);
                                }
                            }
                        }
                    }
                }
            }
            catch (DbException DBEx)
            {
                bRet = -1;
                logger.Log(LogLevel.Error, $"[DBFactory][SQLSelectCount] { DBEx.Message } | {sSQL}");
            }
            catch (Exception ex)
            {
                bRet = -1;
                var logStr = $"[DBFactory][SQLSelectCount] Error :: {ex.Message} | { ex.InnerException?.Message } { ex.InnerException?.InnerException?.Message}| {sSQL}";
                logger.Log(LogLevel.Error, logStr);
            }
            return bRet;
        }

        public int SQLSelectCount(string sSQL, object[] strParam, string cConnectString = "", string cDatabaseType = "", string cProviderName = "")
        {
            int bRet = 0;
            int pknt = 0;

            if (string.IsNullOrEmpty(cConnectString))
                cConnectString = _ConnectString;
            if (string.IsNullOrEmpty(cDatabaseType))
                cDatabaseType = _DatabaseType;
            if (string.IsNullOrEmpty(cProviderName))
                cProviderName = _DBProvider;

            try
            {
                DbProviderFactory iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();
                using (SQLConn = iFactory.CreateConnection())
                {
                    SQLConn.ConnectionString = cConnectString;
                    SQLConn.Open();
                    using (DbCommand SQLCmd = SQLConn.CreateCommand())
                    {
                        SQLCmd.CommandText = TransformSQL(sSQL, cDatabaseType);

                        pknt = strParam.Length;
                        DbParameter p = default(DbParameter);
                        for (int j = 0; j <= pknt - 1; j++)
                        {
                            p = iFactory.CreateParameter();
                            p.DbType = GetDbTypeByName(Convert.IsDBNull(strParam[j]) ? "DBNULL" : strParam[j].GetType().Name);
                            p.Value = strParam[j];
                            p.ParameterName = (j + 1).ToString();
                            SQLCmd.Parameters.Add(p);
                        }
                        using (DbDataReader RDR = SQLCmd.ExecuteReader())
                        {
                            if (RDR.HasRows)
                            {
                                while (RDR.Read())
                                {
                                    bRet = RDR.GetInt32(0);
                                }
                            }
                        }
                    }
                }
            }
            catch (DbException DBEx)
            {
                bRet = -1;
                logger.Log(LogLevel.Error, $"[DBFactory][SQLSelectCount] { DBEx.Message } | {sSQL}");
            }
            catch (Exception ex)
            {
                bRet = -1;
                var logStr = $"[DBFactory][SQLSelectCount] Error :: {ex.Message} | { ex.InnerException?.Message } { ex.InnerException?.InnerException?.Message}| {sSQL}";
                logger.Log(LogLevel.Error, logStr);
            }
            return bRet;
        }

        public int SQLSelectIntValue(string sSQL, object[] strParam, string cConnectString = "", string cDatabaseType = "", string cProviderName = "")
        {
            int bRet = 0;
            int pknt = 0;

            if (string.IsNullOrEmpty(cConnectString))
                cConnectString = _ConnectString;
            if (string.IsNullOrEmpty(cDatabaseType))
                cDatabaseType = _DatabaseType;
            if (string.IsNullOrEmpty(cProviderName))
                cProviderName = _DBProvider;

            try
            {
                DbProviderFactory iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();
                using (SQLConn = iFactory.CreateConnection())
                {
                    SQLConn.ConnectionString = cConnectString;
                    SQLConn.Open();
                    using (DbCommand SQLCmd = SQLConn.CreateCommand())
                    {
                        SQLCmd.CommandText = TransformSQL(sSQL, cDatabaseType);

                        pknt = strParam.Length;
                        DbParameter p = default(DbParameter);
                        for (int j = 0; j <= pknt - 1; j++)
                        {
                            p = iFactory.CreateParameter();
                            p.DbType = GetDbTypeByName(Convert.IsDBNull(strParam[j]) ? "DBNULL" : strParam[j].GetType().Name);
                            p.Value = strParam[j];
                            p.ParameterName = (j + 1).ToString();
                            SQLCmd.Parameters.Add(p);
                        }
                        using (DbDataReader i = SQLCmd.ExecuteReader())
                        {
                            if (i.HasRows)
                            {
                                while (i.Read())
                                {
                                    bRet = int.Parse(i.IsDBNull(0) ? "0" : i.GetString(0));
                                }
                            }
                        }
                    }
                }
            }
            catch (DbException DBEx)
            {
                bRet = -1;
                logger.Log(LogLevel.Error, $"[SMSCHubClient][SQLSelectIntValue] { DBEx.Message } | {sSQL}");
            }
            catch (Exception ex)
            {
                bRet = -1;
                var logStr = $"[SMSCHubClient][SQLSelectIntValue] Error :: {ex.Message} | { ex.InnerException?.Message } { ex.InnerException?.InnerException?.Message}| {sSQL}";
                logger.Log(LogLevel.Error, logStr);
            }
            return bRet;
        }

        public bool SQLSelectCountBool(string sSQL, object[] strParam, string cConnectString = "", string cDatabaseType = "", string cProviderName = "")
        {
            int bRet = 0;
            int pknt = 0;

            if (string.IsNullOrEmpty(cConnectString))
                cConnectString = _ConnectString;
            if (string.IsNullOrEmpty(cDatabaseType))
                cDatabaseType = _DatabaseType;
            if (string.IsNullOrEmpty(cProviderName))
                cProviderName = _DBProvider;

            try
            {
                DbProviderFactory iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();
                using (SQLConn = iFactory.CreateConnection())
                {
                    SQLConn.ConnectionString = cConnectString;
                    SQLConn.Open();
                    using (DbCommand SQLCmd = SQLConn.CreateCommand())
                    {
                        SQLCmd.CommandText = TransformSQL(sSQL, cDatabaseType);

                        pknt = strParam.Length;
                        DbParameter p = default(DbParameter);
                        for (int j = 0; j <= pknt - 1; j++)
                        {
                            p = iFactory.CreateParameter();
                            p.DbType = GetDbTypeByName(Convert.IsDBNull(strParam[j]) ? "DBNULL" : strParam[j].GetType().Name);
                            p.Value = strParam[j];
                            p.ParameterName = (j + 1).ToString();
                            SQLCmd.Parameters.Add(p);
                        }
                        using (DbDataReader i = SQLCmd.ExecuteReader())
                        {
                            if (i.HasRows)
                            {
                                while (i.Read())
                                {
                                    bRet = i.GetInt32(0);
                                }
                            }
                        }
                    }
                }
            }
            catch (DbException DBEx)
            {
                bRet = -1;
                logger.Log(LogLevel.Error, $"[SMSCHubClient][SQLSelectIntValue] { DBEx.Message } | {sSQL}");
            }
            catch (Exception ex)
            {
                bRet = -1;
                var logStr = $"[SMSCHubClient][SQLSelectIntValue] Error :: {ex.Message} | { ex.InnerException?.Message } { ex.InnerException?.InnerException?.Message}| {sSQL}";
                logger.Log(LogLevel.Error, logStr);
            }
            return bRet == 0 ? false : true;
        }

        public int SQLSelectSimple(string sSQL, object[] strParam, ref string[] strResult, string cConnectString = "", string cDatabaseType = "", string cProviderName = "")
        {
            int bRet = 0;
            int pknt = 0;

            if (string.IsNullOrEmpty(cConnectString))
                cConnectString = _ConnectString;
            if (string.IsNullOrEmpty(cDatabaseType))
                cDatabaseType = _DatabaseType;
            if (string.IsNullOrEmpty(cProviderName))
                cProviderName = _DBProvider;

            try
            {
                DbProviderFactory iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();
                using (SQLConn = iFactory.CreateConnection())
                {
                    SQLConn.ConnectionString = cConnectString;
                    SQLConn.Open();
                    using (DbCommand SQLCmd = SQLConn.CreateCommand())
                    {
                        SQLCmd.CommandText = TransformSQL(sSQL, cDatabaseType);

                        pknt = strParam.Length;
                        DbParameter p = default(DbParameter);
                        for (int j = 0; j <= pknt - 1; j++)
                        {
                            p = iFactory.CreateParameter();
                            p.DbType = GetDbTypeByName(Convert.IsDBNull(strParam[j]) ? "DBNULL" : strParam[j].GetType().Name);
                            p.Value = strParam[j];
                            p.ParameterName = (j + 1).ToString();
                            SQLCmd.Parameters.Add(p);
                        }
                        using (DbDataReader RDR = SQLCmd.ExecuteReader())
                        {
                            if (RDR.HasRows)
                            {
                                while (RDR.Read())
                                {
                                    int knt = 0;
                                    knt = RDR.FieldCount;
                                    for (int i = 0; i <= knt - 1; i++)
                                    {
                                        strResult[i] = RDR[i].ToString();
                                    }
                                    bRet += 1;
                                }
                            }
                        }
                    }
                }
            }
            catch (DbException DBEx)
            {
                
                logger.Log(LogLevel.Error, "[SQLSelectSimple-1] " + DBEx.Message + "{" + sSQL + "}");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "[SQLSelectSimple-1] " + ex.Message + "{" + sSQL + "}");
            }
            return bRet;
        }

        public string SQLSelectSimple(string sSQL, object[] strParam, string cConnectString = "", string cDatabaseType = "", string cProviderName = "")
        {
            int pknt = 0;
            string strResult = null;

            if (string.IsNullOrEmpty(cConnectString))
                cConnectString = _ConnectString;
            if (string.IsNullOrEmpty(cDatabaseType))
                cDatabaseType = _DatabaseType;
            if (string.IsNullOrEmpty(cProviderName))
                cProviderName = _DBProvider;

            try
            {
                DbProviderFactory iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();
                using (SQLConn = iFactory.CreateConnection())
                {
                    SQLConn.ConnectionString = cConnectString;
                    SQLConn.Open();
                    using (DbCommand SQLCmd = SQLConn.CreateCommand())
                    {
                        SQLCmd.CommandText = TransformSQL(sSQL, cDatabaseType);

                        pknt = strParam.Length;
                        DbParameter p = default(DbParameter);
                        for (int j = 0; j <= pknt - 1; j++)
                        {
                            p = iFactory.CreateParameter();
                            p.DbType = GetDbTypeByName(Convert.IsDBNull(strParam[j]) ? "DBNULL" : strParam[j].GetType().Name);
                            p.Value = strParam[j];
                            p.ParameterName = (j + 1).ToString();
                            SQLCmd.Parameters.Add(p);
                        }
                        using (DbDataReader RDR = SQLCmd.ExecuteReader())
                        {
                            if (RDR.HasRows)
                            {
                                RDR.Read();
                                strResult = RDR[0].ToString();
                            }
                        }
                    }
                }
            }
            catch (DbException DBEx)
            {
                strResult = "-1";
                logger.Log(LogLevel.Error, "[SQLSelectSimple-2] " + DBEx.Message + "{" + sSQL + "}");
            }
            catch (Exception ex)
            {
                strResult = "-1";
                logger.Log(LogLevel.Error, "[SQLSelectSimple-2] " + ex.Message + "{" + sSQL + "}");
            }
            return strResult;
        }
        public string SQLSelectSimpleTwo(string sSQL, object[] strParam, string cConnectString = "", string cDatabaseType = "", string cProviderName = "")
        {
            int pknt = 0;
            string strResult = null;

            if (string.IsNullOrEmpty(cConnectString))
                cConnectString = _ConnectString;
            if (string.IsNullOrEmpty(cDatabaseType))
                cDatabaseType = _DatabaseType;
            if (string.IsNullOrEmpty(cProviderName))
                cProviderName = _DBProvider;

            try
            {
                DbProviderFactory iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();
                using (SQLConn = iFactory.CreateConnection())
                {
                    SQLConn.ConnectionString = cConnectString;
                    SQLConn.Open();
                    using (DbCommand SQLCmd = SQLConn.CreateCommand())
                    {
                        SQLCmd.CommandText = TransformSQL(sSQL, cDatabaseType);

                        pknt = strParam.Length;
                        DbParameter p = default(DbParameter);
                        for (int j = 0; j <= pknt - 1; j++)
                        {
                            p = iFactory.CreateParameter();
                            p.DbType = GetDbTypeByName(Convert.IsDBNull(strParam[j]) ? "DBNULL" : strParam[j].GetType().Name);
                            p.Value = strParam[j];
                            p.ParameterName = (j + 1).ToString();
                            SQLCmd.Parameters.Add(p);
                        }
                        using (DbDataReader RDR = SQLCmd.ExecuteReader())
                        {
                            if (RDR.HasRows)
                            {
                                RDR.Read();
                                strResult = $"{RDR[0].ToString()},{RDR[1].ToString()}";
                            }
                        }
                    }
                }
            }
            catch (DbException DBEx)
            {
                strResult = "-1";
                logger.Log(LogLevel.Error, "[SQLSelectSimple-2] " + DBEx.Message + "{" + sSQL + "}");
            }
            catch (Exception ex)
            {
                strResult = "-1";
                logger.Log(LogLevel.Error, "[SQLSelectSimple-2] " + ex.Message + "{" + sSQL + "}");
            }
            return strResult;
        }

        public int SQLSelectMultiRow(string sSQL, object[] strParam, ref List<object> strResult, string cConnectString = "", string cDatabaseType = "", string cProviderName = "")
        {
            int bRet = 0;
            int pknt = 0;

            if (string.IsNullOrEmpty(cConnectString))
                cConnectString = _ConnectString;
            if (string.IsNullOrEmpty(cDatabaseType))
                cDatabaseType = _DatabaseType;
            if (string.IsNullOrEmpty(cProviderName))
                cProviderName = _DBProvider;

            try
            {
                DbProviderFactory iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();
                using (SQLConn = iFactory.CreateConnection())
                {
                    SQLConn.ConnectionString = cConnectString;
                    SQLConn.Open();
                    using (DbCommand SQLCmd = SQLConn.CreateCommand())
                    {
                        SQLCmd.CommandText = TransformSQL(sSQL, cDatabaseType);

                        pknt = strParam.Length;
                        DbParameter p = default(DbParameter);
                        for (int j = 0; j <= pknt - 1; j++)
                        {
                            p = iFactory.CreateParameter();
                            p.DbType = GetDbTypeByName(Convert.IsDBNull(strParam[j]) ? "DBNULL" : strParam[j].GetType().Name);
                            p.Value = strParam[j];
                            p.ParameterName = (j + 1).ToString();
                            SQLCmd.Parameters.Add(p);
                        }
                        using (DbDataReader RDR = SQLCmd.ExecuteReader())
                        {
                            if (RDR.HasRows)
                            {
                                while (RDR.Read())
                                {
                                    strResult[bRet] = RDR[0].ToString();
                                    bRet += 1;
                                }
                            }
                        }
                    }
                }
            }
            catch (DbException DBEx)
            {
                logger.Log(LogLevel.Error, "[SQLSelectSimple-1] " + DBEx.Message + "{" + sSQL + "}");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "[SQLSelectSimple-1] " + ex.Message + "{" + sSQL + "}");
            }
            return bRet;
        }



        private DbParameter[] ParamCollection(object[] strParam1, DbProviderFactory iFactory)
        {
            String sss = "";
            DbParameter p = default(DbParameter);
            int pknt = strParam1.Length;
            DbParameter[] pc = new DbParameter[pknt];
            for (int j = 0; j <= pknt - 1; j++)
            {
                if ((strParam1[j] == null))
                    strParam1[j] = "null";

                p = iFactory.CreateParameter();
                if (strParam1[j].ToString() == "-9")
                {
                    p.DbType = DbType.Int32;
                    p.Value = DBNull.Value;
                }
                else if (strParam1[j].ToString() == "01-01-01")
                {
                    p.DbType = DbType.DateTime;
                    p.Value = DBNull.Value;
                }
                else
                {
                    p.DbType = GetDbTypeByName(strParam1[j].ToString() == "null" ? "DBNULL" : strParam1[j].GetType().Name);
                    p.Value = strParam1[j].ToString() == "null" ? DBNull.Value : strParam1[j];
                }
                p.ParameterName = (j + 1).ToString();
                sss = sss + p.Value.ToString() + ",";
                pc[j] = p;

            }
            return pc;
        }



        public int SQLExecuteCommandTranxCommon(string sSQL, object[] strParam, string cConnectString = "", string cDatabaseType = "", string cProviderName = "")
        {
            int bRet = 0;
            string sql = string.Empty;
            if (string.IsNullOrEmpty(cConnectString))
                cConnectString = _ConnectString;
            if (string.IsNullOrEmpty(cDatabaseType))
                cDatabaseType = _DatabaseType;
            if (string.IsNullOrEmpty(cProviderName))
                cProviderName = _DBProvider;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    DbProviderFactory iFactory = DbProviderFactories.GetFactory(cProviderName);
                    StringBuilder sb = new StringBuilder();
                    using (SQLConn = iFactory.CreateConnection())
                    {
                        SQLConn.ConnectionString = cConnectString;

                        DbCommand[] SQLCmd = new DbCommand[strParam.Length];

                        for (Int32 i = 0; i < strParam.Length; i++)
                        {
                            if (strParam[i] != null)
                            {
                                SQLCmd[i] = SQLConn.CreateCommand();
                                SQLCmd[i].CommandText = TransformSQL(sSQL, cDatabaseType);
                                object[] param = strParam[i] as object[];
                                SQLCmd[i].Parameters.AddRange(ParamCollection(param, iFactory));
                            }
                        }
                        SQLConn.Open();
                        try
                        {
                            for (Int32 i = 0; i < strParam.Length; i++)
                            {
                                if (SQLCmd[i] != null)
                                    SQLCmd[i].ExecuteNonQuery();
                            }
                            ts.Complete();
                            bRet = 1;
                        }
                        catch (InvalidCastException)
                        {
                            ts.Dispose();
                            SQLConn.Close();
                            throw;
                        }
                        catch (Exception)
                        {
                            ts.Dispose();
                            SQLConn.Close();
                            throw;
                        }

                    }

                }
                catch (DbException ex)
                {
                    bRet = -1;
                    var logStr = $"[DBFactory][SQLExecuteCommandTranxCommon] Error :: {ex.Message} | { ex.InnerException?.Message } { ex.InnerException?.InnerException?.Message}| {sql}";
                    logger.Log(LogLevel.Error, logStr);
                    SQLConn.Close();
                    throw;
                }
                catch (Exception ex)
                {
                    bRet = -1;
                    var logStr = $"[DBFactory][SQLExecuteCommandTranxCommon] Error :: {ex.Message} | { ex.InnerException?.Message } { ex.InnerException?.InnerException?.Message}| {sql}";
                    logger.Log(LogLevel.Error, logStr);
                    SQLConn.Close();
                    throw;
                }
            }
            return bRet;
        }
        public int SQLExecuteCommandTranx(string[] sSQL, object[] strParam, string cConnectString = "", string cDatabaseType = "", string cProviderName = "")
        {
            int bRet = 0;
            string sql = string.Empty;
            if (string.IsNullOrEmpty(cConnectString))
                cConnectString = _ConnectString;
            if (string.IsNullOrEmpty(cDatabaseType))
                cDatabaseType = _DatabaseType;
            if (string.IsNullOrEmpty(cProviderName))
                cProviderName = _DBProvider;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    /// DbProviderFactory iFactory = DbProviderFactories.GetFactory(cProviderName);
                    DbProviderFactory iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();
                    StringBuilder sb = new StringBuilder();
                    using (SQLConn = iFactory.CreateConnection())
                    {
                        SQLConn.ConnectionString = cConnectString;

                        DbCommand[] SQLCmd = new DbCommand[strParam.Length];

                        for (Int32 i = 0; i < strParam.Length; i++)
                        {

                            SQLCmd[i] = SQLConn.CreateCommand();
                            SQLCmd[i].CommandText = TransformSQL(sSQL[i], cDatabaseType);
                            if (strParam[i] != null)
                            {
                                object[] param = strParam[i] as object[];
                                SQLCmd[i].Parameters.AddRange(ParamCollection(param, iFactory));

                            }
                        }
                        SQLConn.Open();
                        try
                        {
                            for (Int32 i = 0; i < strParam.Length; i++)
                            {
                                if (SQLCmd[i] != null)
                                    SQLCmd[i].ExecuteNonQuery();
                            }
                            ts.Complete();
                            bRet = 1;
                        }
                        catch (InvalidCastException)
                        {
                            ts.Dispose();
                            SQLConn.Close();
                            throw;
                        }
                        catch (Exception)
                        {
                            ts.Dispose();
                            SQLConn.Close();
                            throw;
                        }

                    }

                }
                catch (DbException ex)
                {
                    bRet = -1;
                    var logStr = $"[DBFactory][SQLExecuteCommandTranx] Error :: {ex.Message} | { ex.InnerException?.Message } { ex.InnerException?.InnerException?.Message}| {sql}";
                    logger.Log(LogLevel.Error, logStr);
                    SQLConn.Close();
                    throw;
                }
                catch (Exception ex)
                {
                    bRet = -1;
                    var logStr = $"[DBFactory][SQLExecuteCommandTranx] Error :: {ex.Message} | { ex.InnerException?.Message } { ex.InnerException?.InnerException?.Message}| {sql}";
                    logger.Log(LogLevel.Error, logStr);
                    SQLConn.Close();
                    throw;
                }
            }
            return bRet;
        }


    }
}
