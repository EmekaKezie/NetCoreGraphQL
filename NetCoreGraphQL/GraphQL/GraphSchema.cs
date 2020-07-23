using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreGraphQL.GraphQL
{
    public class GraphSchema : Schema
    {
        public GraphSchema(IDependencyResolver resolver) :
        base(resolver)
        {
            Query = resolver.Resolve<RootQuery>();
        }
    }
}
