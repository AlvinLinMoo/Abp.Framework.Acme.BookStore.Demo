using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.DataSeedModels;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore
{
    public class BookStoreDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly AuthorManager _authorManager;

        private IConfiguration _config;
        private Mapper _mapper;

        public BookStoreDataSeederContributor(IRepository<Book, Guid> bookRepository, IAuthorRepository authorRepository, AuthorManager authorManager)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _authorManager = authorManager;

            //// Get DataSeed folder path in Domain layer
            //string baseDir = Directory.GetCurrentDirectory();
            //string binDirectory = Directory.GetParent(Directory.GetParent(baseDir).FullName).FullName;
            //string srcDirectory = Directory.GetParent(Directory.GetParent(binDirectory).FullName).FullName;
            //string dataSeedFolderPath = Path.Combine(srcDirectory, $"{GetType().Namespace}.Domain", "DataSeed");

            //// Create a configuration builder and add the json file
            //_config = new ConfigurationBuilder()
            //                    .SetBasePath(dataSeedFolderPath)
            //                    .AddJsonFile("Books.json", optional: true, reloadOnChange: true)
            //                    .Build();

            //// Create a mapper configuration
            //var mapperConfig = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<BookModel, Book>();
            //});

            //// Create a mapper
            //_mapper = new Mapper(mapperConfig);;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            //// Seed Books if Database is empty.
            //if (await _bookRepository.GetCountAsync() <= 0 && _config != null)
            //{
            //    // Get the BookData section from the json file
            //    BookData? bookData = _config.GetSection(nameof(BookData)).Get<BookData>();

            //    if (bookData != null && bookData.Books != null)
            //    {
            //        // Insert the books into the database   
            //        foreach (BookModel bookModel in bookData.Books)
            //        {
            //            // Map the BookModel to a Book entity
            //            Book book = _mapper.Map<Book>(bookModel);
            //            // Insert the Book entity into the database
            //            await _bookRepository.InsertAsync(book, autoSave: true);
            //        }
            //    }
            //}

            // Seed Books if Database is empty by hard code.
            if (await _bookRepository.GetCountAsync() > 0)
            {
                return;
            }

            var orwell = await _authorRepository.InsertAsync(
                await _authorManager.CreateAsync(
                    "George Orwell",
                    new DateTime(1903, 06, 25),
                    "Orwell produced literary criticism and poetry, fiction and polemical journalism; and is best known for the allegorical novella Animal Farm (1945) and the dystopian novel Nineteen Eighty-Four (1949)."
                )
            );

            var douglas = await _authorRepository.InsertAsync(
                await _authorManager.CreateAsync(
                    "Douglas Adams",
                    new DateTime(1952, 03, 11),
                    "Douglas Adams was an English author, screenwriter, essayist, humorist, satirist and dramatist. Adams was an advocate for environmentalism and conservation, a lover of fast cars, technological innovation and the Apple Macintosh, and a self-proclaimed 'radical atheist'."
                )
            );

            await _bookRepository.InsertAsync(
                new Book
                {
                    AuthorId = orwell.Id, // SET THE AUTHOR
                    Name = "1984",
                    Type = BookType.Dystopia,
                    PublishDate = new DateTime(1949, 6, 8),
                    Price = 19.84f
                },
                autoSave: true
            );

            await _bookRepository.InsertAsync(
                new Book
                {
                    AuthorId = douglas.Id, // SET THE AUTHOR
                    Name = "The Hitchhiker's Guide to the Galaxy",
                    Type = BookType.ScienceFiction,
                    PublishDate = new DateTime(1995, 9, 27),
                    Price = 42.0f
                },
                autoSave: true
            );
        }
    }
}
