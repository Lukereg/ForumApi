using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumApi.Entities
{
    public class Post
    {
        public int Id { get; set; } 
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime CreatedDate { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
        public User Author { get; set; } = null!;
        public int AuthorId { get; set; }
        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
