
using backend.Models;

namespace backend.Services.Interfaces
{
    public interface INameProduct
    {
        string GetNameOf(TipoProducto tipo);
    }
}