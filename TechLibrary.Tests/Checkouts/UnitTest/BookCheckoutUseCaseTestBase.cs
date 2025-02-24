using Moq;
using TechLibrary.Application.UseCases.Checkout;
using TechLibrary.Domain.Interfaces.Repositories;
using TechLibrary.Domain.Interfaces.Services;

namespace TechLibrary.Tests.Checkouts.UnitTest
{
    public abstract class BookCheckoutUseCaseTestBase
    {
        protected readonly Mock<ICheckoutRepository> CheckoutRepository;
        protected readonly Mock<IBookRepository> BookRepository;
        protected readonly Mock<ICurrentUserService> CurrentUserService;
        protected readonly BookCheckoutUseCase BookCheckoutUseCase;

        public BookCheckoutUseCaseTestBase()
        {
            CheckoutRepository = new Mock<ICheckoutRepository>();
            BookRepository = new Mock<IBookRepository>();
            CurrentUserService = new Mock<ICurrentUserService>();
            BookCheckoutUseCase = new BookCheckoutUseCase(CheckoutRepository.Object, BookRepository.Object, CurrentUserService.Object);
        }
    }
}
