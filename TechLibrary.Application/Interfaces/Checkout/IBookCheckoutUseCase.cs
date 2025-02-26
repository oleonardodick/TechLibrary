namespace TechLibrary.Application.Interfaces.Checkout
{
    public interface IBookCheckoutUseCase
    {
        /// <summary>
        /// Executes an asynchronous operation to check out a book based on it's ID.
        /// </summary>
        /// <param name="bookId">Id of the book that will be checked out. The ID should be <see cref="Guid"/>. Example: <c>123e4567-e89b-12d3-a456-426614174000</c>.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CheckoutBook(Guid bookId);
    }
}