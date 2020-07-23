using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreGraphQL.ClassDB
{
    public class SQL
    {
        public const string GetFacility = "select a.facilityid, a.facilityname, a.address, a.coordinates, a.status, a.remarks, a.entrydate, a.eia, a.eiacert, b.stateid, b.statename, d.sectorid, d.sectorname, f.zoneid, f.zonename, g.lgaid, g.lganame, count(e.samplepointid) spcount  from facility a left join state b on a.stateid=b.stateid left join sector d on a.sectorid=d.sectorid left join samplepoint e on a.facilityid=e.facilityid left join zone f on a.zoneid=f.zoneid left join lga g on a.lgaid=g.lgaid group by a.facilityid, a.facilityname, a.address, a.coordinates, a.status, a.remarks, a.entrydate, a.eia, a.eiacert, b.stateid, b.statename, d.sectorid, d.sectorname, f.zoneid, f.zonename, g.lgaid, g.lganame order by entrydate desc limit # offset #";
    }
}
