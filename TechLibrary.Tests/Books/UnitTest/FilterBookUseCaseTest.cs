using FluentAssertions;
using Moq;
using TechLibrary.Application.DTOs.Books.Request;
using TechLibrary.Tests.Books.Utils;

namespace TechLibrary.Tests.Books.UnitTest
{
    public class FilterBookUseCaseTest : FilterBookUseCaseTestBase
    {
        [Fact]
        public async Task ShouldReturnAllBooks()
        {
            //Arrange
            var request = new RequestFilterBooksDTO
            {
                PageNumber = 1
            };
            var qtdBooks = 15;
            var books = FakeDataBooks.FakeListBooks(qtdBooks);

            BookRepository
                .Setup(x => x.FilterBooksAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((books, books.Count));

            //Act
            var response = await FilterBookUseCase.ExecuteAsync(request);


            //Assert
            response.Should().NotBeNull();
            response.Books.Should().NotBeNull().And.HaveCount(books.Count);
            response.Pagination.PageNumber.Should().Be(1);
            response.Pagination.TotalPages.Should().Be(2);
            response.Pagination.TotalCount.Should().Be(qtdBooks);
        }

        [Fact]
        public async Task ShouldReturnFilteredBook()
        {
            //Arrange
            var books = FakeDataBooks.FakeListBooks(5);

            var request = new RequestFilterBooksDTO
            {
                Title = books[0].Title,
                PageNumber = 1
            };

            var filteredBooks = books.Where(x => x.Title == request.Title).ToList();

            BookRepository
                .Setup(x => x.FilterBooksAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((filteredBooks, 1));

            //Act
            var response = await FilterBookUseCase.ExecuteAsync(request);

            //Assert
            response.Should().NotBeNull();
            response.Books.Should().NotBeNull().And.HaveCount(1);
            response.Books[0].Title.Should().Be(request.Title);
            response.Pagination.PageNumber.Should().Be(1);
            response.Pagination.TotalPages.Should().Be(1);
            response.Pagination.TotalCount.Should().Be(1);
        }

        [Fact]
        public async Task ShouldReturnEmpty()
        {
            //Arrange
            var request = new RequestFilterBooksDTO
            {
                PageNumber = 3
            };

            BookRepository
                .Setup(x => x.FilterBooksAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(([],15));

            //Act
            var response = await FilterBookUseCase.ExecuteAsync(request);

            //Assert
            response.Should().NotBeNull();
            response.Books.Should().BeEmpty();
            response.Pagination.PageNumber.Should().Be(3);
            response.Pagination.TotalPages.Should().Be(2);
            response.Pagination.TotalCount.Should().Be(15);
        }
    }
}
