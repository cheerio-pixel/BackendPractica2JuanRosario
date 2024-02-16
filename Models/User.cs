

using System.Text.Json.Serialization;
using backend.Authentication.RoleAccess;

namespace backend.Models
{
    public class User
    {
        [JsonInclude]
        public Guid Id { get; set; }
        [JsonInclude]
        public string FirstName { get; set; }
        [JsonInclude]
        public string LastName { get; set; }
        [JsonInclude]
        public string Email { get; set; }
        // Could make this a real password by saving the hash and the salt,
        // but who has the time of making that when we have no requirement
        // of registering and after spending 3 days making a js framework.
        // Also the teacher is going to read the JSON file so being
        // *pratical* is going to be unpractical here.
        [JsonInclude]
        public string Password { get; set; }
        // RoleName as specified in Authentication.RoleAccess.IRoleAccess
        [JsonInclude]
        public string Role { get; set; }

        public User(Guid id, string email, string password, string role, string firstName, string lastName)
        {
            Id = id;
            Email = email;
            Password = password;
            Role = role;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}