using GraphQL.Types;
using NetCoreGraphQL.GraphQL.Type;
using NetCoreGraphQL.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreGraphQL.GraphQL.Query
{
    public class FacilityQuery : ObjectGraphType
    {
        public FacilityQuery()
        {
            Field<ListGraphType<FacilityType>>(
               name: "facilitys",
               description: "get all facilities",
               resolve: context => FacilityUtil.GetFacility());

            FieldAsync<FacilityType>(
                name: "facility",
                description: "get facility by id",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "id" }),
                resolve: async context =>
                {
                    var id = context.GetArgument<string>("id");
                    var model = await FacilityUtil.FacilityByIdAsync(id);
                    return model;
                });
        }
    }
}
