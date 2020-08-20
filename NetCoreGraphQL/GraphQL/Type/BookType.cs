using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreGraphQL.GraphQL.Type
{
    public class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            Field(x => x.Id).Description("ID of the book");
            Field(x => x.Title).Description("Title of the book");
            Field(x => x.Pages, nullable: true);
            Field(x => x.Chapters, nullable: true);
        }
    }
}
