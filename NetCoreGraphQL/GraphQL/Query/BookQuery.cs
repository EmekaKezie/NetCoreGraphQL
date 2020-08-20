using GraphQL.Types;
using NetCoreGraphQL.GraphQL.Type;
using NetCoreGraphQL.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreGraphQL.GraphQL.Query
{
    public class BookQuery : ObjectGraphType
    {
        public BookQuery()
        {
            FieldAsync<ListGraphType<BookType>>(
                name: "books",
                description: "get all books",
                resolve: async context => await BookUtil.BooksAsync());

            FieldAsync<BookType>(
                name: "book",
                description: "get book by id",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "id" }),
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    var model = await BookUtil.BookByIdAsync(id);
                    return model;
                });
        }
    }
}
