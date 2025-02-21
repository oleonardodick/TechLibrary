using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Checkouts
{
    public class BookCheckoutUseCase
    {
        private readonly TechLibraryDbContext _dbContext;
        private readonly LoggedUserService _loggedUserService;
        private const int MAX_LOAN_DAYS = 7;

        public BookCheckoutUseCase(TechLibraryDbContext dbContext, LoggedUserService loggedUserService)
        {
            _dbContext = dbContext;
            _loggedUserService = loggedUserService;
        }

        public void CheckoutBook(Guid bookId)
        {
            Validate(bookId);

            var user = _loggedUserService.User();

            var entity = new Checkout
            {
                UserId = user.Id,
                BookId = bookId,
                ExpectedReturnDate = DateTime.UtcNow.AddDays(MAX_LOAN_DAYS)
            };

            _dbContext.Checkouts.Add(entity);

            _dbContext.SaveChanges();
        }

        private void Validate(Guid bookId)
        {
            var book = _dbContext.Books.FirstOrDefault(book => book.Id.Equals(bookId));

            if(book is null)
                throw new NotFoundException("Livro não encontrado.");

            var amountBooksNotReturned = _dbContext
                .Checkouts
                .Count(checkout => checkout.BookId == bookId && checkout.ReturnedDate == null);
            if(amountBooksNotReturned == book.Amount)
                throw new ConflictException("Livro não disponível para empréstimo.");
        }
    }
}
