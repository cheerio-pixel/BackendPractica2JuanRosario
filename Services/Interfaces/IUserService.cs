

using backend.DTO;
using backend.Models;

namespace backend.Services.Interfaces
{
    public interface IUserService
    {
        User? ValidateUser(UserDTO user);
        User? GetUser(string email);
    }
}