
namespace backend.Authentication.RoleAccess
{
    internal class DefaultRoleAccessFactory
    : IRoleAccessFactory
    {
        private readonly Dictionary<string, IRoleAccess> _roles;

        public DefaultRoleAccessFactory(Dictionary<string, IRoleAccess> roles)
        {
            _roles = roles;
        }

        public IRoleAccess GetRoleAccess(string RoleName)
        {
            try
            {
                return _roles[RoleName];
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException($"No se ha registrado el rol {RoleName}.", e);
            }
        }
    }
}