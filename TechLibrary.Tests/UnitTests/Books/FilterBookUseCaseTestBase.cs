using Moq;
using TechLibrary.Application.UseCases.Books;
using TechLibrary.Domain.Interfaces.Repositories;

namespace TechLibrary.Tests.UnitTests.Books
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
    }
}
