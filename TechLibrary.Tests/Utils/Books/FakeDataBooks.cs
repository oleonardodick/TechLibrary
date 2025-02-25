using Bogus;
using TechLibrary.Domain.Entities;

namespace TechLibrary.Tests.Utils.Books
{
    public static class FakeDataBooks
    {
        public static List<Book> FakeListBooks(int qtdToGenerate)
        {
            var bookFaker = new Faker<Book>()
                .RuleFor(c => c.Id, f => f.Random.Guid())
                .RuleFor(c => c.Title, f => f.Lorem.Sentence(3))
                .RuleFor(c => c.Author, f => f.Name.FullName())
                .RuleFor(c => c.Amount, f => f.Random.Int(1, 50));
            var books = bookFaker.Generate(qtdToGenerate);
            return books;
        }
    }
}
