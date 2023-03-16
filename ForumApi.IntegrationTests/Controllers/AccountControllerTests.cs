using FluentAssertions;
using ForumApi.Entities;
using ForumApi.IntegrationTests.Helpers;
using ForumApi.Models.Accounts;
using ForumApi.Services.AccountService;
using Moq;
using System.Runtime.Intrinsics;

namespace ForumApi.IntegrationTests.Controllers
{
    public class AccountControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        private Mock<IAccountService> _accountServiceMock = new Mock<IAccountService>();

        public AccountControllerTests(CustomWebApplicationFactory<Program> factory) 
        {
            factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IAccountService>(_accountServiceMock.Object);
                });
            });
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsOk()
        {
            //arrange
            var registerUser = new RegisterUserDto()
            {
                Name = "Test",
                Surname = "Testtest",
                Email = "testemail@testemail.com",
                Password = "password",
                ConfirmPassword = "password",
                Login = "testlogin"
            };

            var role = new Role() 
            {
                Name = "User"
            };

            var httpContent = registerUser.ToJsonHttpContent();

            //seed
            await SeedRole(role);

            //act
            var response = await _client.PostAsync("/v1/accounts/register", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterUser_ForInvalidModel_ReturnsBadRequest()
        {
            //arrange
            var registerUser = new RegisterUserDto()
            {
                Name = "Test",
                Surname = "Testtest",
                Email = "testemail@testemail.com",
                Password = "password123",
                ConfirmPassword = "password",
                Login = "testlogin"
            };

            var role = new Role()
            {
                Name = "User"
            };

            var httpContent = registerUser.ToJsonHttpContent();

            //seed
            await SeedRole(role);

            //act
            var response = await _client.PostAsync("/v1/accounts/register", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginUser_ForRegisteredUser_ReturnsOk()
        {
            //arrange
            _accountServiceMock
                .Setup(e => e.LoginUser(It.IsAny<LoginUserDto>()))
                .ReturnsAsync("jwt");

            var loginDto = new LoginUserDto() 
            {
                LoginOrEmail = "testlogin",
                Password = "password"
            };

            var httpContent = loginDto.ToJsonHttpContent();

            //act
            var response = await _client.PostAsync("/v1/accounts/login", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        private async Task SeedRole(Role role)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _forumDbContext = scope.ServiceProvider.GetService<ForumDbContext>();

            if (!_forumDbContext.Roles.Contains(role))
                await _forumDbContext.Roles.AddAsync(role);
            await _forumDbContext.SaveChangesAsync();
        }
    }
}
