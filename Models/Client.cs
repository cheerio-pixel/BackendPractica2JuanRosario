
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Client
    {
        [JsonInclude]
        public Guid Id { get; set; }
        [JsonInclude]
        public string Name { get; set; } = string.Empty;
        [JsonInclude]
        public string Surname { get; set; } = string.Empty;
        [JsonInclude]
        public string Telefono { get; set; } = string.Empty;
        [JsonInclude]
        public string Address { get; set; } = string.Empty;
        [JsonInclude]
        public string Email { get; set; } = string.Empty;
        [JsonInclude]
        public string Password { get; set; } = string.Empty;

        [JsonInclude]
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();

        [JsonIgnore]
        private string? _fullName;
        [JsonIgnore]
        public string FullName => _fullName ??= Name + " " + Surname;

        public Client(Guid id, string name, string surname, string telefono, string address, string email, string password, ICollection<Producto> productos)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Telefono = telefono;
            Address = address;
            Email = email;
            Password = password;
            Productos = productos;
        }
    }
}