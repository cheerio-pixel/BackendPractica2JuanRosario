
namespace backend.Authentication.RoleAccess
{
    internal interface IRoleAccessFactory
    {
        IRoleAccess GetRoleAccess(string RoleName);
    }
}