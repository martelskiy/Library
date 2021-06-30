using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;

namespace Library.IntegrationTest.HealthCheck
{
    public class HealthCheckIntegrationTests : HttpClientFixture
    {
        public HealthCheckIntegrationTests(WebApplicationFactory<Startup> webApplicationFactory) 
            : base(webApplicationFactory)
        {
        }

        [Fact]
        public async Task Should_ReturnStatusOk()
        {
            var httpClient = CreateDefaultHttpClient();
            var url = "health";

            var response = await httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            responseBody.ShouldBe("Healthy");
        }
    }
}
