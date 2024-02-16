

namespace backend.Authentication.RoleAccess
{
    internal class AgenteServiciosRoleAccess
    : IRoleAccess
    {
        private static AgenteServiciosRoleAccess? s_instance;

        private AgenteServiciosRoleAccess() { }

        public static AgenteServiciosRoleAccess Instance { get => s_instance ?? (s_instance = new AgenteServiciosRoleAccess()); }

        public IEnumerable<Access> Accesses { get; } = new List<Access>() {
            Access.AgregarProducto,
            Access.BorrarProducto,
            Access.EditarProducto,
            Access.MostrarProductos,
            Access.CrearCliente,
            Access.MostrarClientes,
            Access.BorrarCliente,
            Access.EditarCliente,
            Access.VerEstadoProductos,
        }.AsReadOnly();

        public string RoleName => "Agente De Servicios";
    }
}