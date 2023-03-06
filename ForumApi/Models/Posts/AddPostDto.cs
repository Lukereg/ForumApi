namespace ForumApi.Models.Posts
{
    public class AddPostDto
    {
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public List<int>? TagsIds { get; set; }
    }
}
