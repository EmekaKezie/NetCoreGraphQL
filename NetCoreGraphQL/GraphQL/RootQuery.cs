using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreGraphQL.Util;

namespace NetCoreGraphQL.GraphQL
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            Field<ListGraphType<BookType>>("book", resolve: context => BookUtil.GetBooks());
            Field<BookType>("books", arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "id" }), resolve: context =>
            {
                var id = context.GetArgument<int>("id");
                return BookUtil.GetBooks().FirstOrDefault(x => x.Id == id);
            });


            Field<ListGraphType<FacilityType>>("facility", resolve: context => FacilityUtil.GetFacility(0, 20));
            Field<FacilityType>("facilitys", arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "facilityId" }), resolve: context =>
            {
                var id = context.GetArgument<string>("facilityId");
                return FacilityUtil.GetFacility(0, 20).FirstOrDefault(x => x.FacilityId == id);
            });
        }


    }
}
