
using backend.Models;

namespace backend.Authentication.RoleAccess
{
    internal class FilterByRoleProductStatus
    : IFilterByRole<ProductStatus>
    {
        public IEnumerable<ProductStatus> Filter(string rol, IEnumerable<ProductStatus> @base)
        {
            return rol == AgenteServiciosRoleAccess.Instance.RoleName ?
                   @base.Where(ps => ps.IsEnabled) :
                   rol == GerenteRoleAccess.Instance.RoleName ?
                   @base :
                   throw new KeyNotFoundException("Don't know how to filter statuses for role: " + rol);
        }
    }
}