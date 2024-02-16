

using backend.Models;

namespace backend.Services.Interfaces
{
    public interface IProductService
    {
        ResultValue<Guid> AddProductToClient(Guid clientId, Producto product);
        void DeleteProductFromClient(Guid clientId, Guid productId);
        ResultValue<bool> UpdateProductFromClient(Guid clientId, Producto product);
        Result<Producto> ShowProductOfClient(Guid clientId, Guid productId);
    }
}