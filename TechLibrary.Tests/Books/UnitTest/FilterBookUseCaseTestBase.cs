using Moq;
using TechLibrary.Application.UseCases.Books;
using TechLibrary.Domain.Entities;
using TechLibrary.Domain.Interfaces.Repositories;

namespace TechLibrary.Tests.Books.UnitTest
{
    public abstract class FilterBookUseCaseTestBase
    {
        protected readonly Mock<IBookRepository> BookRepository;
        protected readonly FilterBookUseCase FilterBookUseCase;

        public FilterBookUseCaseTestBase()
        {
            BookRepository = new Mock<IBookRepository>();
            FilterBookUseCase = new FilterBookUseCase(BookRepository.Object);
        }
        protected List<Book> GetBooks()
        {
            var books = new List<Book> { };
            for(int i = 0; i < 15; i++)
            {
                books.Add(new Book
                {
                    Id = Guid.NewGuid(),
                    Title = $"Book {i}",
                    Author = $"Author {i}"
                });
            }
            return books;

        }
    }
}
