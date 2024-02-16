

namespace backend.Authentication.RoleAccess
{
    public interface IRoleAccess
    {
        IEnumerable<Access> Accesses { get; }
        string RoleName { get; }
    }
}