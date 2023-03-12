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
    public class CategoryControllerTests :IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public CategoryControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
            _factory = factory;
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

            var httpContent = model.ToJsonHttpContent();

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

            var httpContent = model.ToJsonHttpContent();

            //act
            var response = await _httpClient.PostAsync("/v1/categories", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteCategory_ForNonExistingCategory_ReturnsNotFound()
        {
            //act
            var response = await _httpClient.DeleteAsync("/v1/categories/9999");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ForExistingCategory_ReturnsNotFound()
        {
            //arrange
            var category = new Category()
            {
                Id = 1,
                Name = "Automotive"
            };

            //seed
            SeedCategory(category);

            //act
            var response = await _httpClient.DeleteAsync("/v1/categories/" + category.Id);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateCategory_ForExistingCategoryAndWithValidModel_ReturnsOkResult()
        {
            //arrange
            var category = new Category()
            {
                Id = 1,
                Name = "Automotive"
            };

            var model = new UpdateCategoryDto()
            {
                Name = "Automotive - cars"
            };

            var httpContent = model.ToJsonHttpContent();

            //seed
            SeedCategory(category);

            //act
            var response = await _httpClient.PutAsync("/v1/categories/" + category.Id, httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        public String RandomString(int length)
        {
            var random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new String(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void SeedCategory(Category category)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _forumDbContext = scope.ServiceProvider.GetService<ForumDbContext>();

            _forumDbContext.Categories.Add(category);
            _forumDbContext.SaveChanges();
        }
    }
}
