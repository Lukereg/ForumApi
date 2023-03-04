using Microsoft.EntityFrameworkCore;

namespace ForumApi.Entities
{
    public interface IForumDbContext: IDisposable
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}