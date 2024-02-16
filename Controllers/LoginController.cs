
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.DTO;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers
{
    [Authorize]
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IConfiguration _config;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IUserService authService, IConfiguration config, ILogger<LoginController> logger)
        {
            _userService = authService;
            _config = config;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserDTO user)
        {
            User? result = _userService.ValidateUser(user);
            if (result == null)
            {
                _logger.LogInformation("Se fallo el inicio de sesion con email " + user.Email);
                return Unauthorized(new ErrorDTO(ErrorDTO.Errors.NotAuthorized, "Email o contraseña invalida."));
            }

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, result.Email),
                new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
                new Claim(ClaimTypes.Role, result.Role),
            };

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken secToken = new(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(secToken);
            _logger.LogInformation("Usuario con email " + user.Email + " ha inciado sesion");

            return Ok(token);
        }

        [HttpGet]
        public IActionResult CheckProfile(string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new ErrorDTO(ErrorDTO.Errors.BadRequest, "Parametros introducidos incorrectos"));
            }
            if (!HttpContext.User.HasClaim(c =>
            {
                return c.Type == ClaimTypes.Name
                       && c.Value == email;
            }))
            {
                return Forbid();
            }
            User? user = _userService.GetUser(email);
            if (user == null)
            {
                return NotFound(new ErrorDTO(ErrorDTO.Errors.NotFound, "No hay un perfil de usuario con ese correo."));
            }
            return Ok(
                new UserProfileDTO(user.FirstName, user.LastName, user.Email, user.Role)
            );
        }
    }
}