using Moq;
using TechLibrary.Domain.Entities;
using TechLibrary.Domain.Exceptions;

namespace TechLibrary.Tests.Checkout.UnitTest
{
    public class BookCheckoutUseCaseTest : BookCheckoutUseCaseTestBase
    {
        [Fact]
        public async Task ShouldNotFindTheBook()
        {
            //Arrange
            var bookId = Guid.NewGuid();

            BookRepository
                .Setup(x => x.GetBookAsync(bookId))
                .ReturnsAsync((Book?)null);

            CurrentUserService
                .Setup(x => x.UserId)
                .Returns(Guid.NewGuid());

            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => BookCheckoutUseCase.CheckoutBook(bookId));
            //Assert
            Assert.Equal("Livro não encontrado.", exception.Message);
        }

        [Fact]
        public async Task ShouldReturnBookNotAvailable()
        {
            //Arrange
            var bookId = Guid.NewGuid();

            var book = new Book
            {
                Id = bookId,
                Amount = 1
            };

            BookRepository
                .Setup(x => x.GetBookAsync(bookId))
                .ReturnsAsync(book);

            CheckoutRepository
                .Setup(x => x.GetAmountBooksNotReturnedAsync(bookId))
                .ReturnsAsync(1);

            CurrentUserService
                .Setup(x => x.UserId)
                .Returns(Guid.NewGuid());

            //Act
            var exception = await Assert.ThrowsAsync<ConflictException>(() => BookCheckoutUseCase.CheckoutBook(bookId));

            //Assert
            Assert.Equal("Livro não disponível para empréstimo.", exception.Message);
        }

        [Fact]
        public async Task ShouldCheckoutTheBook()
        {
            //Arrange
            var bookId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var book = new Book
            {
                Id = bookId,
                Amount = 1
            };

            BookRepository
                .Setup(x => x.GetBookAsync(bookId))
                .ReturnsAsync(book);

            CheckoutRepository
                .Setup(x => x.GetAmountBooksNotReturnedAsync(bookId))
                .ReturnsAsync(0);

            CurrentUserService
                .Setup(x => x.UserId)
                .Returns(userId);

            //Act
            await BookCheckoutUseCase.CheckoutBook(bookId);

            //Assert
            CheckoutRepository.Verify(c => c.CreateBookCheckoutAsync(bookId, userId), Times.Once);
        }
    }
}
