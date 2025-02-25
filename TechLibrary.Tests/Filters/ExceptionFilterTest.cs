using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TechLibrary.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using TechLibrary.Api.Filters;
using FluentAssertions;
using TechLibrary.Application.DTOs.Error.Response;
using System;

namespace TechLibrary.Tests.Filters
{
    public class ExceptionFilterTest
    {
        private readonly ActionContext _actionContext;
        private ExceptionContext _exceptionContext;
        private ExceptionFilter _filter;

        public ExceptionFilterTest()
        {
            _actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
            };

            _exceptionContext = new ExceptionContext(_actionContext, new List<IFilterMetadata>());
            _filter = new ExceptionFilter();
        }

        [Fact]
        public void ShouldReturn401Unauthorized()
        {
            //Arrange
            _exceptionContext.Exception = new InvalidLoginException();

            //Act
            _filter.OnException(_exceptionContext);

            //Assert
            var result = _exceptionContext.Result as ObjectResult;
            result.Should().NotBeNull();

            var errorResponse = result.Value as ResponseErrorMessagesDTO;
            errorResponse.Should().NotBeNull();
            errorResponse.Errors.Should().HaveCount(1).And.Contain("E-mail e/ou senha inválidos.");

            var httpContextResponse = _exceptionContext.HttpContext.Response;
            httpContextResponse.Should().NotBeNull();
            httpContextResponse.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public void ShouldReturn404NotFoundWhenConflictException()
        {
            //Arrange
            _exceptionContext.Exception = new ConflictException("Mensagem erro");

            //Act
            _filter.OnException(_exceptionContext);

            //Assert
            var result = _exceptionContext.Result as ObjectResult;
            result.Should().NotBeNull();

            var errorResponse = result.Value as ResponseErrorMessagesDTO;
            errorResponse.Should().NotBeNull();
            errorResponse.Errors.Should().HaveCount(1).And.Contain("Mensagem erro");

            var httpContextResponse = _exceptionContext.HttpContext.Response;
            httpContextResponse.Should().NotBeNull();
            httpContextResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public void ShouldReturn400BadRequest()
        {
            //Arrange
            var errors = new List<string> { "error 1", "error 2" };
            _exceptionContext.Exception = new ErrorOnValidationException(errors);

            //Act
            _filter.OnException(_exceptionContext);

            //Assert
            var result = _exceptionContext.Result as ObjectResult;
            result.Should().NotBeNull();

            var errorResponse = result.Value as ResponseErrorMessagesDTO;
            errorResponse.Should().NotBeNull();
            errorResponse.Errors.Should().HaveCount(2).And.Contain("error 1").And.Contain("error 2");

            var httpContextResponse = _exceptionContext.HttpContext.Response;
            httpContextResponse.Should().NotBeNull();
            httpContextResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void ShouldReturn404NotFoundWhenNotFoundException()
        {
            //Arrange
            _exceptionContext.Exception = new NotFoundException("error message");

            //Act
            _filter.OnException(_exceptionContext);

            //Assert
            var result = _exceptionContext.Result as ObjectResult;
            result.Should().NotBeNull();

            var errorResponse = result.Value as ResponseErrorMessagesDTO;
            errorResponse.Should().NotBeNull();
            errorResponse.Errors.Should().HaveCount(1).And.Contain("error message");

            var httpContextResponse = _exceptionContext.HttpContext.Response;
            httpContextResponse.Should().NotBeNull();
            httpContextResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public void ShouldReturn500InternalServerErrorWhenAnyOtherException()
        {
            //Arrange
            _exceptionContext.Exception = new Exception("error message");

            //Act
            _filter.OnException(_exceptionContext);

            //Assert
            var result = _exceptionContext.Result as ObjectResult;
            result.Should().NotBeNull();

            var errorResponse = result.Value as ResponseErrorMessagesDTO;
            errorResponse.Should().NotBeNull();
            errorResponse.Errors.Should().HaveCount(1).And.Contain("Erro desconhecido.");

            var httpContextResponse = _exceptionContext.HttpContext.Response;
            httpContextResponse.Should().NotBeNull();
            httpContextResponse.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
