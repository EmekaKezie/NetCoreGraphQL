using GraphQL;
using GraphQL.Types;
using NetCoreGraphQL.GraphQL.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreGraphQL.GraphQL
{
    public class GraphSchema : Schema, ISchema
    {
        public GraphSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<BookQuery>();
            Query = resolver.Resolve<FacilityQuery>();
        }
    }
}
