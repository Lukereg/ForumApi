namespace ForumApi.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Value { get; set; } = null!;

        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
