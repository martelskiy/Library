using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Library.Core.Features;
using Library.IntegrationTest.Book.Mocks;
using Library.Persistence.Features.Book;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Library.IntegrationTest
{
    public class HttpClientFixture : IClassFixture<WebApplicationFactory<Startup>>
    {
        internal readonly WebApplicationFactory<Startup> WebApplicationFactory;

        public HttpClientFixture(WebApplicationFactory<Startup> webApplicationFactory)
        {
            WebApplicationFactory = webApplicationFactory;
        }

        public HttpClient CreateDefaultHttpClient(Action<IServiceCollection> configureServices = null)
        {
            var httpClient = WebApplicationFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(collection =>
                {
                    ConfigureDefaultServices(collection);
                    configureServices?.Invoke(collection);
                });
            }).CreateClient();

            httpClient.Timeout = TimeSpan.FromSeconds(10);

            return httpClient;
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddScoped<ICreateBookCommand, CreateBookCommandMock>(_ => new CreateBookCommandMock(Result.Ok()));
            services.AddScoped<ILoanBookCommand, LoanBookCommandMock>(_ => new LoanBookCommandMock(Result.Ok()));
            services.AddScoped<IGetBooksQuery, GetBooksQueryMock>(_ => new GetBooksQueryMock(
                Result<IReadOnlyList<Core.Features.Book.Book>>.Ok(Enumerable.Empty<Core.Features.Book.Book>().ToList())));
        }
    }
}