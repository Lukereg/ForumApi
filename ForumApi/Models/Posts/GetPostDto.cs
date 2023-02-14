using ForumApi.Entities;

namespace ForumApi.Models.Posts
{
    public class GetPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }
}
