
using backend.Models;

namespace backend.Repositories.Interfaces
{
    public interface IProductRepository
    {
        ResultValue<bool> Save(Guid clientId, Producto product);
        ResultValue<Guid> Create(Guid clientId, Producto product);
        void Delete(Guid clientId, Guid productId);
        Result<Producto> Show(Guid clientId, Guid productId);
    }
}