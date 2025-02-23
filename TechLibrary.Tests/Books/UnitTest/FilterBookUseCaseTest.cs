using Moq;
using TechLibrary.Application.DTOs.Books.Request;
using TechLibrary.Domain.Entities;

namespace TechLibrary.Tests.Books.UnitTest
{
    public class FilterBookUseCaseTest : FilterBookUseCaseTestBase
    {
        [Fact]
        public async Task ShouldReturnBooksAllBooks()
        {
            //Arrange
            var request = new RequestFilterBooksDTO
            {
                PageNumber = 1
            };
            var books = GetBooks();

            BookRepository
                .Setup(x => x.FilterBooksAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((books, books.Count));

            //Act
            var response = await FilterBookUseCase.ExecuteAsync(request);

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Books);
            Assert.Equal(15, response.Books.Count);
            Assert.Equal(1, response.Pagination.PageNumber);
            Assert.Equal(2, response.Pagination.TotalPages);
            Assert.Equal(15, response.Pagination.TotalCount);
        }

        [Fact]
        public async Task ShouldReturnFilteredBook()
        {
            //Arrange
            var request = new RequestFilterBooksDTO
            {
                Title = "Book 1",
                PageNumber = 1
            };
            var books = GetBooks();

            var filteredBooks = books.Where(x => x.Title == request.Title).ToList();

            BookRepository
                .Setup(x => x.FilterBooksAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((filteredBooks, 1));

            //Act
            var response = await FilterBookUseCase.ExecuteAsync(request);

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Books);
            Assert.Single(response.Books);
            Assert.Equal(1, response.Pagination.PageNumber);
            Assert.Equal(1, response.Pagination.TotalPages);
            Assert.Equal(1, response.Pagination.TotalCount);
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
            Assert.NotNull(response);
            Assert.Empty(response.Books);
            Assert.Equal(3, response.Pagination.PageNumber);
            Assert.Equal(2, response.Pagination.TotalPages);
            Assert.Equal(15, response.Pagination.TotalCount);
        }
    }
}
