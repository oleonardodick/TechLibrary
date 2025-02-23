using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLibrary.Application.Interfaces.Checkout
{
    public interface IBookCheckoutUseCase
    {
        Task CheckoutBook(Guid bookId);
    }
}
