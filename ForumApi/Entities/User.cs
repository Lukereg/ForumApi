using Microsoft.Identity.Client;

namespace ForumApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!; 
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!; 

        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
