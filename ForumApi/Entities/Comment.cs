using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumApi.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Post Post { get; set; } = null!;
        public int PostId { get; set; }
        public User Author { get; set; } = null!;
        public int AuthorId { get; set; }
    }
}
