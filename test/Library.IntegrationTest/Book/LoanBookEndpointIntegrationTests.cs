using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Library.Core.Features;
using Library.Features.Response;
using Library.IntegrationTest.Book.Mocks;
using Library.Persistence.Features.Book;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Library.IntegrationTest.Book
{
    public class LoanBookEndpointIntegrationTests : HttpClientFixture
    {
        public LoanBookEndpointIntegrationTests(WebApplicationFactory<Startup> webApplicationFactory) : base(webApplicationFactory)
        {
        }

        [Fact]
        public async Task Should_ReturnOk_When_RequestIsValid()
        {
            var httpClient = CreateDefaultHttpClient();

            var url = "api/library/books/1";

            var response = await httpClient.PutAsync(url, new StringContent(JsonConvert.SerializeObject(1)));

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_ReturnNotFound_When_LoanCommandReturnsNotFoundError()
        {
            var httpClient = CreateDefaultHttpClient(collection => 
                collection.AddScoped<ILoanBookCommand>(_ => new LoanBookCommandMock(Result.Fail(new []{new Error("", ErrorType.NotFound)}))));

            var url = "api/library/books/1";

            var response = await httpClient.PutAsync(url, new StringContent(JsonConvert.SerializeObject(1)));

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var responseBody = JsonConvert.DeserializeObject<NotFoundProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.ShouldNotBeNull();
            responseBody.Title.ShouldBe("Not Found");
            responseBody.Detail.ShouldBe("The requested resource could not be found");
            responseBody.Status.ShouldBe(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_When_BookIDIsDefaultInteger()
        {
            var httpClient = CreateDefaultHttpClient();
            var url = "api/library/books/0";

            var response = await httpClient.PutAsync(url, new StringContent(""));

            var responseBody = JsonConvert.DeserializeObject<BadRequestProblemDetails>(await response.Content.ReadAsStringAsync());

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            responseBody.ShouldNotBeNull();
            responseBody.Title.ShouldBe("Bad Request");
            responseBody.Detail.ShouldBe("The request produced one or more errors");
            responseBody.Status.ShouldBe(StatusCodes.Status400BadRequest);
            responseBody.Errors.ShouldNotBeEmpty();
            responseBody.Errors.Count().ShouldBe(1);
            responseBody.Errors.ShouldAllBe(error => error.Equals("'Book Id' must not be empty."));
        }

        [Fact]
        public async Task Should_ReturnInternalServerError_When_PersistenceCallFails()
        {
            var httpClient = CreateDefaultHttpClient(collection =>
                collection.AddScoped<ILoanBookCommand>(_ =>
                    new LoanBookCommandMock(Result.Fail(new[] { new Error("", ErrorType.Unspecified) }))));

            var url = "api/library/books/1";

            var response = await httpClient.PutAsync(url, new StringContent("1"));


            var responseBody = JsonConvert.DeserializeObject<InternalServerErrorProblemDetails>(await response.Content.ReadAsStringAsync());

            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            responseBody.ShouldNotBeNull();
            responseBody.Title.ShouldBe("Internal Server Error");
            responseBody.Detail.ShouldBe("An unexpected error occurred on the server and has been logged");
            responseBody.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        }
    }
}
