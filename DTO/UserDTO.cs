

namespace backend.DTO
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public UserDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}