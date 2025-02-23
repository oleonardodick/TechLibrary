using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLibrary.Domain.Entities;
using TechLibrary.Domain.Interfaces.Repositories;

namespace TechLibrary.Infrastructure.DataAccess.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly TechLibraryDbContext _dbContext;
        private const int MAX_LOAN_DAYS = 7;

        public CheckoutRepository(TechLibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateBookCheckoutAsync(Guid bookId, Guid userId)
        {
            var entity = new Checkout
            {
                UserId = userId,
                BookId = bookId,
                ExpectedReturnDate = DateTime.UtcNow.AddDays(MAX_LOAN_DAYS)
            };

            await _dbContext.Checkouts.AddAsync(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetAmountBooksNotReturnedAsync(Guid bookId)
        {
            var amountBooksNotReturned = await _dbContext
                .Checkouts
                .CountAsync(checkout => checkout.BookId == bookId && checkout.ReturnedDate == null);

            return amountBooksNotReturned;
        }
    }
}
