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

        public CategoryControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
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

        public String RandomString(int length)
        {
            var random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new String(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
