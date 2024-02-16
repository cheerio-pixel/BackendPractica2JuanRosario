
using Microsoft.AspNetCore.Authorization;

namespace backend.Authentication
{
    internal class AccessRequirement
    : IAuthorizationRequirement
    {
        public Access RequiredAccess { get; set; }

        public AccessRequirement(Access requiredAccess)
        {
            RequiredAccess = requiredAccess;
        }
    }
}