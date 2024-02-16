namespace backend.Authentication.RoleAccess
{
    internal class GerenteRoleAccess
    : IRoleAccess
    {
        private static GerenteRoleAccess? s_instance;

        private GerenteRoleAccess() { }

        public static GerenteRoleAccess Instance { get => s_instance ?? (s_instance = new GerenteRoleAccess()); }


        public IEnumerable<Access> Accesses { get; }
        = new List<Access>(AgenteServiciosRoleAccess.Instance.Accesses)
        {
            Access.VerEstadoProductos,
            Access.ModificarEstadoProductos,
        }.AsReadOnly();

        public string RoleName => "Gerente";
    }
}