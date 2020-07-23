using NetCoreGraphQL.GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreGraphQL.Util
{
    public class BookUtil
    {
        public static List<Book> GetBooks()
        {
            List<Book> books = new List<Book>();
            books.Add(new Book
            {
                Id = 1,
                Title = "Understanding GraphQL",
                Pages = 260
            });

            books.Add(new Book
            {
                Id = 2,
                Title = "Understanding .Net Core",
                Pages = 500,
                Chapters = 24
            });

            return books;
        }
    }
}
