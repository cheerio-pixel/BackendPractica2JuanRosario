

using backend.Models;

namespace backend.Repositories.Interfaces
{
    internal interface IUserRepository
    {
        User? GetUser(string email);
    }
}