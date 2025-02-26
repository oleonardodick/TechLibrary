using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using TechLibrary.Application.DTOs.Books.Response;
using TechLibrary.Tests.Utils.Books;

namespace TechLibrary.Tests.IntegrationTests.Books
{
    public class BooksControllerTest : IntegrationTestBase
    {

        public BooksControllerTest(IntegrationTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData(1,5)]
        [InlineData(2, 15)]
        public async Task ShouldReturnAListOfBooks(int pageNumber, int totalBooks)
        {
            var booksPerPage = 10;
            var totalPages = (int)Math.Ceiling(totalBooks / (double)booksPerPage);
            int qtdBooksOnPage = booksPerPage;

            if(pageNumber == totalPages)
            {
                qtdBooksOnPage = totalBooks % booksPerPage == 0 ? booksPerPage : totalBooks % booksPerPage;
            };

            var booksToInsert = FakeDataBooks.FakeListBooks(totalBooks);

            _dbContext.Books.AddRange(booksToInsert);
            _dbContext.SaveChanges();

            var response = await _client.GetAsync($"/books/filter?pageNumber={pageNumber}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseBooksDTO>(content);

            result.Should().NotBeNull();
            result.Pagination.PageNumber.Should().Be(pageNumber);
            result.Pagination.TotalPages.Should().Be(totalPages);
            result.Pagination.TotalCount.Should().Be(totalBooks);

            result.Books.Should().HaveCount(qtdBooksOnPage);
            if(pageNumber > 1)
                result.Books[0].Title.Should().NotBe(booksToInsert[0].Title);
        }

        [Fact]
        public async Task ShouldReturnABookByItsTitle()
        {
            var booksToInsert = FakeDataBooks.FakeListBooks(10);
            _dbContext.Books.AddRange(booksToInsert);
            _dbContext.SaveChanges();

            var titleToFind = booksToInsert[5].Title;

            var response = await _client.GetAsync($"/books/filter?pageNumber=1&title={titleToFind}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseBooksDTO>(content);

            result.Should().NotBeNull();
            result.Books.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Books[0].Title.Should().Be(titleToFind);
        }

        [Fact]
        public async Task ShouldReturnEmpty()
        {
            var response = await _client.GetAsync("/books/filter?pageNumber=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseBooksDTO>(content);
            result.Should().NotBeNull();
            result.Books.Should().BeEmpty();
        }
    }
}
