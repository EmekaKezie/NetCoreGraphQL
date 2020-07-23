using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreGraphQL.ClassDB
{
    public class DBFactoryUtil
    {
        //public static string _dbtyp = ConfigurationManager.AppSettings["DatabaseType"].ToString();

        public static DBFacType DBSettings()
        {
            DBFacType db = new DBFacType();

            string dbname = string.Empty;
            string password = string.Empty;
            string userid = string.Empty;
            string server = string.Empty;
            string port = string.Empty;
            string maxpoolsize = string.Empty;
            string conntimeout = string.Empty;
            string cmdtimeout = string.Empty;
            string fName = string.Empty;

            fName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.INI");
            CreateINIFile(fName);

            db.DBProvider = GetIni(fName, "ProviderName", DBFactory.CDatabaseProvider.PostgreSQL);
            db.DBType = GetIni(fName, "DatabaseType", DBFactory.CDatabaseType.PostgreSQL);
            dbname = GetIni(fName, "DatabaseName", String.Empty);
            password = GetIni(fName, "Password", String.Empty);
            userid = GetIni(fName, "UserId", String.Empty);
            server = GetIni(fName, "Server", String.Empty);
            port = GetIni(fName, "Port", String.Empty);
            maxpoolsize = GetIni(fName, "MaxPoolSize", "100");
            conntimeout = GetIni(fName, "ConnectionTimeout", "60");

            db.ConnStr = string.Format(db.DBType.ToUpper() == DBFactory.CDatabaseType.Oracle ?
            DBFactory.CConnectString.Oracle : db.DBType.ToUpper() == DBFactory.CDatabaseType.MySQL ?
            DBFactory.CConnectString.MySQL : db.DBType.ToUpper() == DBFactory.CDatabaseType.PostgreSQL ?
            DBFactory.CConnectString.PostgreSQL : DBFactory.CConnectString.SQLServer, server, port, dbname, userid, password, maxpoolsize, conntimeout, cmdtimeout);

            return db;
        }

        public static DBFacType GetDBSettingsMVC(IOptions<DatabaseSettings> appSettings)
        {
            DBFacType db = new DBFacType();

            string dbname = string.Empty;
            string password = string.Empty;
            string userid = string.Empty;
            string server = string.Empty;
            string port = string.Empty;
            string maxpoolsize = string.Empty;
            string conntimeout = string.Empty;
            string cmdtimeout = string.Empty;
            string fName = string.Empty;

            /*
            db.DBProvider = appSettings.Value.ProviderName; //Config.GetSection("DatabaseSettings").GetSection("ProviderName").Value; //  ConfigurationManager.AppSettings[0].ToString();//"ProviderName"
            db.DBType = appSettings.Value.DatabaseType;
            dbname = appSettings.Value.DatabaseName;
            //  dbname = ConfigurationManager.AppSettings["DatabaseName"].ToString();
            password = appSettings.Value.Password;
            //password = ConfigurationManager.AppSettings["Password"].ToString();
            // userid = ConfigurationManager.AppSettings["UserId"].ToString();
            userid = appSettings.Value.UserId;

            //server = ConfigurationManager.AppSettings["Server"].ToString();
            server = appSettings.Value.Server;
            //port = ConfigurationManager.AppSettings["Port"].ToString();
            port = appSettings.Value.Port.ToString();

            //maxpoolsize = ConfigurationManager.AppSettings["MaxPoolSize"].ToString();
            maxpoolsize = appSettings.Value.MaxPoolSize.ToString();
            //conntimeout = ConfigurationManager.AppSettings["ConnectionTimeout"].ToString();
            conntimeout = appSettings.Value.ConnectionTimeout.ToString();
            cmdtimeout = appSettings.Value.CommandTimeout.ToString();
            //cmdtimeout = ConfigurationManager.AppSettings["CommandTimeout"].ToString();
            */

            fName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.INI");
            CreateINIFile(fName);

            db.DBProvider = GetIni(fName, "ProviderName", DBFactory.CDatabaseProvider.PostgreSQL);
            db.DBType = GetIni(fName, "DatabaseType", DBFactory.CDatabaseType.PostgreSQL);
            dbname = GetIni(fName, "DatabaseName", String.Empty);
            password = GetIni(fName, "Password", String.Empty);
            userid = GetIni(fName, "UserId", String.Empty);
            server = GetIni(fName, "Server", String.Empty);
            port = GetIni(fName, "Port", String.Empty);
            maxpoolsize = GetIni(fName, "MaxPoolSize", "100");
            conntimeout = GetIni(fName, "ConnectionTimeout", "60");

            db.ConnStr = string.Format(db.DBType.ToUpper() == DBFactory.CDatabaseType.Oracle ?
            DBFactory.CConnectString.Oracle : db.DBType.ToUpper() == DBFactory.CDatabaseType.MySQL ?
            DBFactory.CConnectString.MySQL : db.DBType.ToUpper() == DBFactory.CDatabaseType.PostgreSQL ?
            DBFactory.CConnectString.PostgreSQL : DBFactory.CConnectString.SQLServer, server, port, dbname, userid, password, maxpoolsize, conntimeout, cmdtimeout);
            
            return db;
        }


        private static void CreateINIFile(string asFile)
        {
            if (!File.Exists(asFile))
            {
                using (StreamWriter ofile = new StreamWriter(asFile, false))
                {
                    ofile.Write("[SETTINGS]" + Environment.NewLine);
                    ofile.Write("###########################################################################" + Environment.NewLine);
                    ofile.Write("# Supported Databases & their Providers ORACLE, SQLSERVER, MYSQL, POSTGRES" + Environment.NewLine);
                    ofile.Write("# Their providers are Oracle.DataAccess.Client, System.Data.SqlClient, " + Environment.NewLine);
                    ofile.Write("# MySql.Data.MySqlClient, Npgsql" + Environment.NewLine);
                    ofile.Write("###########################################################################" + Environment.NewLine);
                    ofile.Write("#" + Environment.NewLine);
                    ofile.Write("#DatabaseType=ORACLE" + Environment.NewLine);
                    ofile.Write("DatabaseType=" + Environment.NewLine);
                    ofile.Write("#ProviderName=Oracle.DataAccess.Client" + Environment.NewLine);
                    ofile.Write("ProviderName=" + Environment.NewLine);
                    ofile.Write("#" + Environment.NewLine);
                    ofile.Write("[DB CONNECTION]" + Environment.NewLine);
                    ofile.Write("###########################################################################" + Environment.NewLine);
                    ofile.Write("# Database connection parameters" + Environment.NewLine);
                    ofile.Write("Server=" + Environment.NewLine);
                    ofile.Write("Port=" + Environment.NewLine);
                    ofile.Write("DatabaseName=" + Environment.NewLine);
                    ofile.Write("MaxPoolSize=" + Environment.NewLine);
                    ofile.Write("ConnectionTimeout=" + Environment.NewLine);
                    ofile.Write("UserId=" + Environment.NewLine);
                    ofile.Write("Password=" + Environment.NewLine);
                }
            }
        }


        public static string GetIni(string sIniFile, string sKey, string sDefaultValue)
        {
            try
            {
                if (File.Exists(sIniFile))
                {
                    string Line = null;
                    StreamReader sr = new StreamReader(sIniFile, Encoding.Default);
                    while (sr.Peek() >= 0)
                    {
                        Line = sr.ReadLine().Trim();
                        if (Line.Substring(0, 1) != "#")
                        {
                            if (Line.ToUpper().Contains(sKey.ToUpper() + "="))
                            {
                                sDefaultValue = string.IsNullOrEmpty(Line.Split('=')[1]) ? sDefaultValue : Line.Split('=')[1];
                                break; // TODO: might not be correct. Was : Exit Do
                            }
                        }
                    }
                    sr.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return sDefaultValue;
        }


        public static DbProviderFactory GetDbProviderFactoryFromConfigRow()
        {
            DbProviderFactory loDbProviderFactoryByRow = null;

            DataRow loConfig = null;
            DataSet loDataSet = new DataSet();

            DataTable tb = new DataTable("system.data");
            tb.Columns.Add("InvariantName");
            tb.Columns.Add("Description");
            tb.Columns.Add("Name");
            tb.Columns.Add("AssemblyQualifiedName");
            loDataSet.Tables.Add(tb);
            loConfig = loDataSet.Tables[0].NewRow();

            loConfig["InvariantName"] = "Npgsql";
            loConfig["Description"] = ".Net Data Provider for PostgreSQL";
            loConfig["Name"] = "Npgsql Data Provider";
            loConfig["AssemblyQualifiedName"] = "Npgsql.NpgsqlFactory, Npgsql, Culture=neutral, PublicKeyToken=5d8b90d52f46fda7";
            // loDataSet.Tables[0].Rows.Add(loConfig);
            //}

            try
            {
                loDbProviderFactoryByRow = DbProviderFactories.GetFactory(loConfig);

            }
            catch (Exception loE)
            {
                throw loE;
                //// Handled exception if needed, otherwise, null is returned and another method can be tried.
            }
            //}
            return loDbProviderFactoryByRow;
        }
    }
}
