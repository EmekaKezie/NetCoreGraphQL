using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreGraphQL.Models
{
    public class Facility
    {
        public string FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string AccountName { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string FacilityEia { get; set; }
        //public List<FacilityPersonel> FacilityPersonel { get; set; }
        public string Remarks { get; set; }
        public string SectorId { get; set; }
        public string SectorName { get; set; }
        public string ZoneId { get; set; }
        public string ZoneName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string LgaId { get; set; }
        public string LgaName { get; set; }
        //public List<FacilityProduct> FacilityProduct { get; set; }
        //public List<FacilityRawMaterial> FacilityRawMaterial { get; set; }
        //public List<FacilityWaste> FacilityWaste { get; set; }
        public DateTime EntryDate { get; set; }
        public int SamplePointCount { get; set; }
        //public Coordinates Coordinates { get; set; }
        //public List<SamplePoint> SamplePoints { get; set; }
        //public List<FacilityConsultant> FacilityConsultant { get; set; }
        //public string EiaCert { get; set; }
    }
}
