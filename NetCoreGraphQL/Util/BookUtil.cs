using NetCoreGraphQL.GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreGraphQL.Util
{
    public class BookUtil
    {
        public static async Task<List<Book>> BooksAsync()
        {
            List<Book> list = new List<Book>();
            var i = await GetBooks();

            foreach (var a in i)
            {
                list.Add(new Book
                {
                    Id = a.Id,
                    Title = a.Title,
                    Chapters = a.Chapters,
                    Pages = a.Pages,
                });
            }

            return list;
        }


        public static async Task<Book> BookByIdAsync(int id)
        {
            Book data = null;
            var i = await GetBooks();
            var j = i.Where(x => x.Id == id).FirstOrDefault();
            data = new Book
            {
                Id = j.Id,
                Title = j.Title,
                Chapters = j.Chapters,
                Pages = j.Pages,
            };

            return data;
        }


        public static async Task<List<Book>> GetBooks()
        {
            List<Book> list = new List<Book>();

            list.Add(new Book
            {
                Id = 1,
                Title = "Understanding GraphQL",
                Pages = 260,
                Chapters = 36
            });

            list.Add(new Book
            {
                Id = 2,
                Title = "Understanding .Net Core",
                Pages = 500,
                Chapters = 24
            });

            list.Add(new Book
            {
                Id = 3,
                Title = "The new way of writing business proposal",
                Pages = 400,
                Chapters = 10
            });

            return list;
        }
    }
}
