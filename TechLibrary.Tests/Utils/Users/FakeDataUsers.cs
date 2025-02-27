
using Bogus;
using TechLibrary.Domain.Entities;

namespace TechLibrary.Tests.Utils.Users
{
    public static class FakeDataUsers
    {
        public static List<User> FakeListUsers(int qtdToGenerate)
        {
            var userFaker = new Faker<User>()
                .RuleFor(c => c.Name, f => f.Name.FullName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Password, f => f.Internet.Password(6));
            var users = userFaker.Generate(qtdToGenerate);
            return users;
        }

        public static User FakeUserWithSpecificValues(string name = "", string email = "", string password = "")
        {
            var userFaker = new Faker<User>()
                .RuleFor(c => c.Name, f => string.IsNullOrEmpty(name) ? f.Name.FullName() : name)
                .RuleFor(c => c.Email, f => string.IsNullOrEmpty(email) ? f.Internet.Email() : email)
                .RuleFor(c => c.Password, f => string.IsNullOrEmpty(password) ? f.Internet.Password(6) : password);
            var user = userFaker.Generate();
            return user;
        }
    }
}
