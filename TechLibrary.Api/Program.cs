using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TechLibrary.Api.Filters;
using TechLibrary.Application.DTOs.Users.Request;
using TechLibrary.Application.Interfaces.Books;
using TechLibrary.Application.Interfaces.Checkout;
using TechLibrary.Application.Interfaces.Login;
using TechLibrary.Application.Interfaces.Users;
using TechLibrary.Application.UseCases.Books;
using TechLibrary.Application.UseCases.Checkout;
using TechLibrary.Application.UseCases.Login;
using TechLibrary.Application.UseCases.Users;
using TechLibrary.Application.Validators;
using TechLibrary.Domain.Interfaces.Repositories;
using TechLibrary.Domain.Interfaces.Services;
using TechLibrary.Infrastructure;
using TechLibrary.Infrastructure.DataAccess;
using TechLibrary.Infrastructure.DataAccess.Repositories;
using TechLibrary.Infrastructure.Services.CurrentUserService;
using TechLibrary.Infrastructure.Services.Security.Cryptography;
using TechLibrary.Infrastructure.Services.Security.Tokens.Access;

const string AUTHENTICATION_TYPE = "Bearer";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. Example: ""Authorization: Bearer.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = AUTHENTICATION_TYPE
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = AUTHENTICATION_TYPE
                },
                Scheme = "oauth2",
                Name = AUTHENTICATION_TYPE,
                In = ParameterLocation.Header
            },
            new List<string> ()
        }
    });
});

builder.Services.AddSingleton<JwtService>();
builder.Services.AddSingleton<EncryptionService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
builder.Services.AddScoped<IValidator<RequestRegisterUserDTO>, RegisterUserValidator>();
builder.Services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IFilterBooksUseCase, FilterBookUseCase>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookCheckoutUseCase, BookCheckoutUseCase>();
builder.Services.AddScoped<ICheckoutRepository, CheckoutRepository>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddDbContext<TechLibraryDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = false,
//            ValidateAudience = false,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = JwtSecurityHelper.GetSecurityKey(builder.Configuration)
//        };
//    });

//using (var scope = builder.Services.BuildServiceProvider().CreateScope())
//{
//    var keyProvider = scope.ServiceProvider.GetRequiredService<IJwtKeyProvider>();
//    var key = keyProvider.GetSigningKey();

//    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//        .AddJwtBearer(options =>
//        {
//            options.TokenValidationParameters = new TokenValidationParameters
//            {
//                ValidateIssuer = false,
//                ValidateAudience = false,
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,
//                IssuerSigningKey = key
//            };
//        });
//};

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        // O JwtKeyProvider ser� injetado automaticamente aqui
//        builder.Services.BuildServiceProvider().GetService<IJwtKeyProvider>().ConfigureJwtBearerOptions(options);
//    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }