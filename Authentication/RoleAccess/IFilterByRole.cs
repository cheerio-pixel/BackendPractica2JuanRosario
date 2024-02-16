

namespace backend.Authentication.RoleAccess
{
    interface IFilterByRole<T> where T : class
    {
        IEnumerable<T> Filter(string rol, IEnumerable<T> @base);
    }
}