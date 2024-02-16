

using backend.Models;

namespace backend.DTO
{
    public class ProductoDTO : Producto
    {
        public ProductoDTO(Guid id, string name, TipoProducto kind) : base(id, name, kind)
        {
        }
    }
}