using ForumApi.Entities;

namespace ForumApi.Models.Comments
{
    public class GetCommentDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; }
    }
}
