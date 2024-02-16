
using backend.DTO;
using backend.Models;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services
{
    internal class DefaultUserService
    : IUserService
    {
        private readonly IUserRepository _userRepo;

        public DefaultUserService(IUserRepository authRepo)
        {
            _userRepo = authRepo;
        }

        public User? GetUser(string email)
        {
            return _userRepo.GetUser(email);
        }

        public User? ValidateUser(UserDTO user)
        {
            User? resultUser = _userRepo.GetUser(user.Email);
            if (resultUser != null && resultUser.Password == user.Password)
            {
                return resultUser;
            }

            return null;

        }
    }
}