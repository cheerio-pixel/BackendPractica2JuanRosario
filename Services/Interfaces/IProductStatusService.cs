
using backend.Models;

namespace backend.Services.Interfaces
{
    public interface IProductStatusService
    {
        IEnumerable<ProductStatus> PullStatuses(string rol);
        ResultValue<bool> SaveStatuses(IEnumerable<ProductStatus> status);
        bool CanCreateOfType(TipoProducto tipo);
    }
}