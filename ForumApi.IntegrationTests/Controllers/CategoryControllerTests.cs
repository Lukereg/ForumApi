using FluentAssertions;
using ForumApi.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.IntegrationTests.Controllers
{
    public class CategoryControllerTests :IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public CategoryControllerTests(WebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllCategories_ForAllUsers_ReturnsOkResult()
        {
            //act 
            var response = await _httpClient.GetAsync("/v1/categories");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
