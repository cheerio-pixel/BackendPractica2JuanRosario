

namespace backend.Models
{
    public interface IProductFactory
    {
        Producto CreateTemplate(TipoProducto tipo);
    }
}