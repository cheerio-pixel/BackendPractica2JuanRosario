

using backend.Models;

namespace backend.Repositories.Interfaces
{
    public interface IProductStatusRepository
    {
        IEnumerable<ProductStatus> PullStatuses(string rol);
        IEnumerable<ProductStatus> PullStatuses();
        ResultValue<bool> SaveStatuses(IEnumerable<ProductStatus> status);
    }
}