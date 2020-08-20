using GraphQL.Types;
using NetCoreGraphQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreGraphQL.GraphQL.Type
{
    public class FacilityType : ObjectGraphType<Facility>
    {
        public FacilityType()
        {
            Field(x => x.FacilityId);
            Field(x => x.FacilityName);
            Field(x => x.AccountName);
            Field(x => x.Address);
            Field(x => x.Status);
            Field(x => x.FacilityEia);
            Field(x => x.Remarks);
            Field(x => x.SectorId);
            Field(x => x.SectorName);
            Field(x => x.ZoneId);
            Field(x => x.ZoneName);
            Field(x => x.StateId);
            Field(x => x.StateName);
            Field(x => x.LgaId);
            Field(x => x.LgaName);
            Field(x => x.EntryDate);
            Field(x => x.SamplePointCount);
            //Field(x => x.EiaCert);
        }
    }
}
