using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using RobotsInc.Inspections.API.I;
using Xunit;

namespace RobotsInc.Inspections.Host
{
    public class HealthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _fixture;
        public HealthControllerTests(WebApplicationFactory<Program> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CheckHealt_ReturnsResponse()
        {
            HttpClient client = _fixture.CreateClient();
            var response = await client.GetAsync(Routes.Health + "check-health");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
