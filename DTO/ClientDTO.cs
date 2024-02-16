
namespace backend.DTO
{
    public record ClientDTO(Guid Id, string Name, string Surname,
                            string Telefono, string Address, string Email);
}