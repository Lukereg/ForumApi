namespace ForumApi.Models.Posts
{
    public class AddPostDto
    {
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public int AuthorId { get; set; }
    }
}
