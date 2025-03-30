
namespace core.Models
{
    public class Users
    {
        public int Id { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public UserRole Role { get; set; } 
            public string Phone { get; set; }
            public string Status { get; set; }
            public string Description { get; set; }
        

    }
}
