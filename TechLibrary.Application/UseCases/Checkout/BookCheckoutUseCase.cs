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
