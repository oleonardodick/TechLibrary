using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using TechLibrary.Domain.Interfaces.Services;
using TechLibrary.Infrastructure.Services.Security.Tokens.Configuration;

namespace TechLibrary.Infrastructure
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IJwtKeyProvider, JwtKeyProvider>();

            // Registrar o configurador de autenticação JWT como Singleton
            services.AddSingleton<JwtAuthenticationConfigurator>();

            // Configurar a autenticação JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // O JwtAuthenticationConfigurator é injetado diretamente pelo ASP.NET Core DI
                    services.BuildServiceProvider()
                        .GetRequiredService<JwtAuthenticationConfigurator>()
                        .ConfigureJwtBearerOptions(options);
                });

            return services;
        }
    }
}
