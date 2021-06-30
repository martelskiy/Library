using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Library.Core.Features;
using Library.Features.Book.Fetch;
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
    public class GetBooksEndpointIntegrationTests : HttpClientFixture
    {
        public GetBooksEndpointIntegrationTests(WebApplicationFactory<Startup> webApplicationFactory) 
            : base(webApplicationFactory)
        {
        }

        [Theory]
        [AutoData]
        public async Task Should_ReturnOkResultWithData_When_RequestIsValidAndDatabaseCallSucceeds(IReadOnlyList<Core.Features.Book.Book> booksFromDatabase)
        {
            var httpClient = CreateDefaultHttpClient(collection => 
                collection.AddScoped<IGetBooksQuery, GetBooksQueryMock>(_ => 
                    new GetBooksQueryMock(Result<IReadOnlyList<Core.Features.Book.Book>>.Ok(booksFromDatabase))));

            var url = "api/library/books";

            var response = await httpClient.GetAsync(url);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var responseBody = JsonConvert.DeserializeObject<IList<GetBookResponseDto>>(await response.Content.ReadAsStringAsync());
            
            responseBody.ShouldNotBeNull();
            responseBody.Count.ShouldBe(booksFromDatabase.Count);
            foreach (var book in booksFromDatabase)
            {
                var matchingEntity = responseBody.FirstOrDefault(dto => dto.Name == book.Name);
                matchingEntity.ShouldNotBeNull();
                matchingEntity.Name.ShouldBe(book.Name);
                matchingEntity.IsAvailable.ShouldBe(book.IsAvailable);
                matchingEntity.Author.ShouldBe(book.Author);
            }
        }

        [Fact]
        public async Task Should_ReturnNoContent_When_DatabaseReturnsEmpty()
        {
            var httpClient = CreateDefaultHttpClient();

            var url = "api/library/books";

            var response = await httpClient.GetAsync(url);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnInternalServerError_When_PersistenceCallFails()
        {
            var httpClient = CreateDefaultHttpClient(collection =>
                collection.AddScoped<IGetBooksQuery, GetBooksQueryMock>(_ =>
                    new GetBooksQueryMock(Result<IReadOnlyList<Core.Features.Book.Book>>.Fail(new[] { new Error("", ErrorType.Unspecified) }))));

            var url = "api/library/books";

            var response = await httpClient.GetAsync(url);

            var responseBody = JsonConvert.DeserializeObject<InternalServerErrorProblemDetails>(await response.Content.ReadAsStringAsync());

            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            responseBody.ShouldNotBeNull();
            responseBody.Title.ShouldBe("Internal Server Error");
            responseBody.Detail.ShouldBe("An unexpected error occurred on the server and has been logged");
            responseBody.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        }
    }
}
