using NetCoreGraphQL.ClassDB;
using NetCoreGraphQL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreGraphQL.Util
{
    public class FacilityUtil
    {
       

        public static List<Facility> GetFacility()
        {
            List<Facility> list = new List<Facility>();
            //ResponseStatus status = new ResponseStatus();

            try
            {
                DBFacType db = DBFactoryUtil.DBSettings();
                DbConnection SQLConn;

                var iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();
                using (SQLConn = iFactory.CreateConnection())
                {
                    SQLConn.ConnectionString = db.ConnStr;
                    using (DbCommand SQLCmd = SQLConn.CreateCommand())
                    {
                        SQLConn.Open();
                        SQLCmd.CommandText = DBFactory.TransformSQL(SQL.GetFacility, db.DBType);
                        DbParameter p;


                        //p = iFactory.CreateParameter();
                        //p.DbType = DbType.Int32;
                        //p.Value = stop;
                        //p.ParameterName = "1";
                        //SQLCmd.Parameters.Add(p);

                        //p = iFactory.CreateParameter();
                        //p.DbType = DbType.Int32;
                        //p.Value = start;
                        //p.ParameterName = "2";
                        //SQLCmd.Parameters.Add(p);


                        using (DbDataReader i = SQLCmd.ExecuteReader())
                        {
                            if (i.HasRows)
                            {
                                while (i.Read())
                                {
                                    list.Add(new Facility
                                    {
                                        FacilityId = i["facilityid"].ToString(),
                                        FacilityName = i["facilityname"].ToString(),
                                        Address = i["address"].ToString(),
                                        Status = i["status"].ToString(),
                                        FacilityEia = i["eia"].ToString(),
                                        Remarks = i["remarks"].ToString(),
                                        SectorId = i["sectorid"].ToString(),
                                        SectorName = i["sectorname"].ToString(),
                                        ZoneId = i["zoneid"].ToString(),
                                        ZoneName = i["zonename"].ToString(),
                                        StateId = i["stateid"].ToString(),
                                        StateName = i["statename"].ToString(),
                                        LgaId = i["lgaid"].ToString(),
                                        LgaName = i["lganame"].ToString(),
                                        EntryDate = Convert.ToDateTime(i["entrydate"]),
                                        SamplePointCount = Convert.ToInt32(i["spcount"]),
                                        //Coordinates = Newtonsoft.Json.JsonConvert.DeserializeObject<Coordinates>(i["coordinates"].ToString()),
                                        //SamplePoints = GetSamplePointsByFacilityId(i["facilityid"].ToString()),
                                        //FacilityPersonel = GetFacilityPersonel(i["facilityid"].ToString()),
                                        //FacilityConsultant = GetFaciltyOperator(i["facilityid"].ToString()),
                                        //FacilityProduct = GetFacilityProductByFacilityId(i["facilityid"].ToString()),
                                        //FacilityRawMaterial = GetFacilityRawMaterialByFacilityId(i["facilityid"].ToString()),
                                        //FacilityWaste = GetFacilityWasteByFacilityId(i["facilityid"].ToString()),
                                        //EiaCert = i["eiacert"].ToString(),
                                    });
                                }
                            }
                        }
                        SQLConn.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return list;
        }

        public static async Task<Facility> FacilityByIdAsync(string id)
        {
            Facility data = null;

            try
            {
                DBFacType db = DBFactoryUtil.DBSettings();
                DbConnection SQLConn;

                var iFactory = DBFactoryUtil.GetDbProviderFactoryFromConfigRow();
                using (SQLConn = iFactory.CreateConnection())
                {
                    SQLConn.ConnectionString = db.ConnStr;
                    using (DbCommand SQLCmd = SQLConn.CreateCommand())
                    {
                        SQLConn.Open();
                        SQLCmd.CommandText = DBFactory.TransformSQL(SQL.GetFacilityById, db.DBType);
                        DbParameter p;


                        p = iFactory.CreateParameter();
                        p.DbType = DbType.String;
                        p.Value = id;
                        p.ParameterName = "1";
                        SQLCmd.Parameters.Add(p);

                        //p = iFactory.CreateParameter();
                        //p.DbType = DbType.Int32;
                        //p.Value = start;
                        //p.ParameterName = "2";
                        //SQLCmd.Parameters.Add(p);


                        using (DbDataReader i = SQLCmd.ExecuteReader())
                        {
                            if (i.HasRows)
                            {
                                while (i.Read())
                                {
                                    data = new Facility
                                    {
                                        FacilityId = i["facilityid"].ToString(),
                                        FacilityName = i["facilityname"].ToString(),
                                        Address = i["address"].ToString(),
                                        Status = i["status"].ToString(),
                                        FacilityEia = i["eia"].ToString(),
                                        Remarks = i["remarks"].ToString(),
                                        SectorId = i["sectorid"].ToString(),
                                        SectorName = i["sectorname"].ToString(),
                                        ZoneId = i["zoneid"].ToString(),
                                        ZoneName = i["zonename"].ToString(),
                                        StateId = i["stateid"].ToString(),
                                        StateName = i["statename"].ToString(),
                                        LgaId = i["lgaid"].ToString(),
                                        LgaName = i["lganame"].ToString(),
                                        EntryDate = Convert.ToDateTime(i["entrydate"]),
                                        SamplePointCount = Convert.ToInt32(i["spcount"]),
                                        //Coordinates = Newtonsoft.Json.JsonConvert.DeserializeObject<Coordinates>(i["coordinates"].ToString()),
                                        //SamplePoints = GetSamplePointsByFacilityId(i["facilityid"].ToString()),
                                        //FacilityPersonel = GetFacilityPersonel(i["facilityid"].ToString()),
                                        //FacilityConsultant = GetFaciltyOperator(i["facilityid"].ToString()),
                                        //FacilityProduct = GetFacilityProductByFacilityId(i["facilityid"].ToString()),
                                        //FacilityRawMaterial = GetFacilityRawMaterialByFacilityId(i["facilityid"].ToString()),
                                        //FacilityWaste = GetFacilityWasteByFacilityId(i["facilityid"].ToString()),
                                        //EiaCert = i["eiacert"].ToString(),
                                    };
                                }
                            }
                        }
                        SQLConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return data;
        }
    }
}
