using Microsoft.EntityFrameworkCore;

namespace ForumApi.Entities
{
    public class ForumContext : DbContext
    {
        public ForumContext(DbContextOptions<ForumContext> options) :base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("posts");

                entity.Property(p => p.Id)
                .HasColumnName("id");

                entity.Property(p => p.Message)
                .HasColumnName("message");

                entity.Property(p => p.Title)
                .HasColumnName("title")
                .HasColumnType("varchar(200)");

                entity.Property(p => p.CreatedDate)
                .HasColumnName("created_date")
                .HasDefaultValueSql("getutcdate()");

                entity.HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId);

                entity.HasOne(p => p.Author)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.AuthorId);

                entity.HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId);

                entity.HasMany(p => p.Tags)
                .WithMany(t => t.Posts);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.HasIndex(c => c.Name, "name_UNIQUE")
                .IsUnique();

                entity.Property(c => c.Id)
                .HasColumnName("id");

                entity.Property(c => c.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(200)");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comments");

                entity.Property(c => c.Id)
                .HasColumnName("id");

                entity.Property(c => c.Message)
                .HasColumnName("message");

                entity.Property(c => c.CreatedDate)
                .HasColumnName("created_date")
                .HasDefaultValueSql("getutcdate()");

                entity.Property(c => c.UpdatedDate)
                .HasColumnName("updated_date")
                .ValueGeneratedOnUpdate();

                entity.HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasKey(r => r.Name)
                .HasName("pk_name");

                entity.Property(r => r.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tags");

                entity.Property(t => t.Value)
                .HasColumnName("value")
                .HasMaxLength(40);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(u => u.Login, "login_UNIQUE")
                .IsUnique();

                entity.HasIndex(u => u.Email, "email_UNIQUE")
                .IsUnique();

                entity.Property(u => u.Login)
                .HasColumnName("login")
                .HasMaxLength(80);

                entity.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(80);

                entity.Property(u => u.Password)          
                .HasColumnName("password")
                .HasColumnType("varchar(255)");

                entity.Property(u => u.Name)
                .HasColumnName("name")
                .HasMaxLength(60);

                entity.Property(u => u.Surname)
                .HasColumnName("surname")
                .HasMaxLength(60);

                entity.HasMany(u => u.Roles)
                .WithMany(r => r.Users);
            });

        }
    }
}
