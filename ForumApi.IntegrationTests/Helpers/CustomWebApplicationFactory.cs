using ForumApi.Entities;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.IntegrationTests.Helpers
{
    public class CustomWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
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
        }
    }
}
