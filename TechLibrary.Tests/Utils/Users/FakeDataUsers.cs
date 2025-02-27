
using Bogus;
using TechLibrary.Domain.Entities;

namespace TechLibrary.Tests.Utils.Users
{
    public static class FakeDataUsers
    {
        public static List<User> FakeListUsers(int qtdToGenerate)
        {
            var userFaker = new Faker<User>()
                .RuleFor(c => c.Id, f => f.Random.Guid())
                .RuleFor(c => c.Name, f => f.Name.FullName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Password, f => f.Internet.Password(6));
            var users = userFaker.Generate(qtdToGenerate);
            return users;
        }
    }
}
