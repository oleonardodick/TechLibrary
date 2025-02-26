using TechLibrary.Application.Interfaces.Checkout;
using TechLibrary.Domain.Interfaces.Repositories;
using TechLibrary.Domain.Interfaces.Services;
using TechLibrary.Domain.Exceptions;

namespace TechLibrary.Application.UseCases.Checkout
{
    public class BookCheckoutUseCase : IBookCheckoutUseCase
    {
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICurrentUserService _currentUserService;

        public BookCheckoutUseCase(ICheckoutRepository checkoutRepository, IBookRepository bookRepository, ICurrentUserService currentUserService)
        {
            _checkoutRepository = checkoutRepository;
            _bookRepository = bookRepository;
            _currentUserService = currentUserService;
        }

        public async Task CheckoutBook(Guid bookId)
        {
            var user = _currentUserService.UserId;

            await Validate(bookId);

            await _checkoutRepository.CreateBookCheckoutAsync(bookId, user);
        }

        /// <summary>
        /// Validate the book provided by parameter and stops the process when an error occurred.
        /// </summary>
        /// <param name="bookId">Id of the book that will be checked out. The ID should be <see cref="Guid"/>. Example: <c>123e4567-e89b-12d3-a456-426614174000</c>.</param>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ConflictException"></exception>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task Validate(Guid bookId)
        {
            var book = await _bookRepository.GetBookAsync(bookId);

            if (book is null)
                throw new NotFoundException("Livro não encontrado.");

            var amountBooksNotReturned = await _checkoutRepository.GetAmountBooksNotReturnedAsync(bookId);

            if (amountBooksNotReturned == book.Amount)
                throw new ConflictException("Livro não disponível para empréstimo.");
        }
    }
}
