
using System.Text.Json.Serialization;

namespace core.Models
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; }
    }

}
