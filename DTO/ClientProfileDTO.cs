
namespace backend.DTO
{
    public record ClientProfileDTO(string Name, string Surname,
                                   string Telefono, string Address,
                                   string Email, string Password,
                                   IEnumerable<ProductoDTO> Productos);
}