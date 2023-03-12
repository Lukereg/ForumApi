using FluentAssertions;
using ForumApi.Entities;
using ForumApi.IntegrationTests.Helpers;
using ForumApi.Models.Categories;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Drawing.Text;
using System.Security.Policy;
using System.Text;

namespace ForumApi.IntegrationTests.Controllers
{
    public class CategoryControllerTests :IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public CategoryControllerTests(WebApplicationFactory<Program> factory)
        {
            _httpClient = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContext = services
                            .SingleOrDefault(dbContext => dbContext.ServiceType == typeof(DbContextOptions<ForumDbContext>));
                        
                        if (dbContext is not null)
                            services.Remove(dbContext);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                        services.AddDbContext<ForumDbContext>(options => options.UseInMemoryDatabase("ForumDb"));
                    });
                })
                .CreateClient();
        }

        [Fact]
        public async Task GetAllCategories_ForAllUsers_ReturnsOkResult()
        {
            //act 
            var response = await _httpClient.GetAsync("/v1/categories");
            var temp = await response.Content.ReadAsStringAsync();

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddCategory_WithValidModel_ReturnsCreatedStatus()
        {
            //arrange
            var model = new AddCategoryDto()
            {
                Name = "TestCategory"
            };

            var json = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //act
            var response = await _httpClient.PostAsync("/v1/categories", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }

        [Fact]
        public async Task AddCategory_WithInvalidModel_ReturnsBadRequest()
        {
            //arrange
            var randomString = RandomString(201);

            var model = new AddCategoryDto()
            {
                Name = randomString
            };

            var json = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //act
            var response = await _httpClient.PostAsync("/v1/categories", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        public String RandomString(int length)
        {
            var random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new String(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
