using System.ComponentModel.DataAnnotations;

namespace ForumApi.Entities
{
    public class Role
    {
        public string Name { get; set; } = null!;

        public List<User> Users { get; set; } = new List<User>();
    }
}
