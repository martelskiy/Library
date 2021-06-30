using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Library.Core.Features;
using Library.Features.Book.Create;
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
    public class CreateBookEndpointIntegrationTests : HttpClientFixture
    {
        public CreateBookEndpointIntegrationTests(WebApplicationFactory<Startup> webApplicationFactory) 
            : base(webApplicationFactory)
        {
        }

        [Fact]
        public async Task Should_ReturnOkResult_When_RequestIsValidAndDatabaseCallSucceeds()
        {
            var httpClient = CreateDefaultHttpClient();
            var url = "api/library/books";

            var response = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(new CreateBookRequestDto
            {
                Author = "Author",
                Name = "Name",
                IsAvailable = true
            }), Encoding.UTF8, "application/json"));

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_When_RequestIsNotValid()
        {
            var httpClient = CreateDefaultHttpClient();
            var url = "api/library/books";

            var response = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(new CreateBookRequestDto
            {
                Author = null,
                Name = "Name",
                IsAvailable = true
            }), Encoding.UTF8, "application/json"));

            var responseBody = JsonConvert.DeserializeObject<BadRequestProblemDetails>(await response.Content.ReadAsStringAsync());

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            responseBody.ShouldNotBeNull();
            responseBody.Title.ShouldBe("Bad Request");
            responseBody.Detail.ShouldBe("The request produced one or more errors");
            responseBody.Status.ShouldBe(StatusCodes.Status400BadRequest);
            responseBody.Errors.ShouldNotBeEmpty();
            responseBody.Errors.Count().ShouldBe(1);
            responseBody.Errors.ShouldAllBe(error => error.Equals("'Author' must not be empty."));
        }

        [Fact]
        public async Task Should_ReturnInternalServerError_When_PersistenceCallFails()
        {
            var httpClient = CreateDefaultHttpClient(collection => 
                collection.AddScoped<ICreateBookCommand, CreateBookCommandMock>(_ => 
                    new CreateBookCommandMock(Result.Fail(new []{new Error("", ErrorType.Unspecified)}))));

            var url = "api/library/books";

            var response = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(new CreateBookRequestDto
            {
                Author = "Author",
                Name = "Name",
                IsAvailable = true
            }), Encoding.UTF8, "application/json"));

            var responseBody = JsonConvert.DeserializeObject<InternalServerErrorProblemDetails>(await response.Content.ReadAsStringAsync());

            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            responseBody.ShouldNotBeNull();
            responseBody.Title.ShouldBe("Internal Server Error");
            responseBody.Detail.ShouldBe("An unexpected error occurred on the server and has been logged");
            responseBody.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        }
    }
}
