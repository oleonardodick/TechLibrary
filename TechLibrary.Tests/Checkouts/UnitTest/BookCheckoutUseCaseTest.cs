using FluentAssertions;
using Moq;
using TechLibrary.Domain.Entities;
using TechLibrary.Domain.Exceptions;
using TechLibrary.Tests.Books.Utils;

namespace TechLibrary.Tests.Checkouts.UnitTest
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

            //Act & Assert
            await FluentActions
                .Awaiting(() => BookCheckoutUseCase.CheckoutBook(bookId))
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Livro não encontrado.");
        }

        [Fact]
        public async Task ShouldReturnBookNotAvailable()
        {
            //Arrange

            var books = FakeDataBooks.FakeListBooks(1);
            var bookAmount = books[0].Amount;

            BookRepository
                .Setup(x => x.GetBookAsync(books[0].Id))
                .ReturnsAsync(books[0]);

            CheckoutRepository
                .Setup(x => x.GetAmountBooksNotReturnedAsync(books[0].Id))
                .ReturnsAsync(bookAmount);

            CurrentUserService
                .Setup(x => x.UserId)
                .Returns(Guid.NewGuid());


            //Act & Assert
            await FluentActions
                .Awaiting(() => BookCheckoutUseCase.CheckoutBook(books[0].Id))
                .Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Livro não disponível para empréstimo.");
        }

        [Fact]
        public async Task ShouldCheckoutTheBook()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var books = FakeDataBooks.FakeListBooks(3);

            Checkout checkoutMade = default!;

            BookRepository
                .Setup(x => x.GetBookAsync(books[0].Id))
                .ReturnsAsync(books[0]);

            CheckoutRepository
                .Setup(x => x.GetAmountBooksNotReturnedAsync(books[0].Id))
                .ReturnsAsync(0);

            CurrentUserService
                .Setup(x => x.UserId)
                .Returns(userId);

            CheckoutRepository
                .Setup(x => x.CreateBookCheckoutAsync(books[0].Id, userId))
                .Callback<Guid, Guid>((id, usrId) =>
                {
                    checkoutMade = new Checkout
                    {
                        BookId = books[0].Id,
                        UserId = userId,
                    };
                });

            //Act
            await BookCheckoutUseCase.CheckoutBook(books[0].Id);

            //Assert
            CheckoutRepository.Verify(c => c.CreateBookCheckoutAsync(books[0].Id, userId), Times.Once);

            checkoutMade.Should().NotBeNull();
            checkoutMade.BookId.Should().Be(books[0].Id);
            checkoutMade.UserId.Should().Be(userId);
            checkoutMade.CheckoutDate.Should().BeCloseTo(DateTime.UtcNow,TimeSpan.FromSeconds(1));
        }
    }
}
