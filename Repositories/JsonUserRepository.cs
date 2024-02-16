
using backend.Models;
using backend.Repositories.Interfaces;

namespace backend.Repositories
{
    internal class JsonUserRepository(string jsonPath)
    : JsonObjectsRepository<User>(jsonPath), IUserRepository
    {
        public User? GetUser(string email)
        {
            try
            {
                Acquire();
                return Load().Find(e => e.Email == email);
            }
            finally
            {
                Release();
            }
        }
    }
}