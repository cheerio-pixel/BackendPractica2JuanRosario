
using System.Security.Claims;
using backend.Authentication.RoleAccess;
using Microsoft.AspNetCore.Authorization;

namespace backend.Authentication
{
    internal class AccessHandler
    : AuthorizationHandler<AccessRequirement>
    {
        private readonly IRoleAccessFactory _roleAccessFactory;

        public AccessHandler(IRoleAccessFactory roleAccessFactory)
        {
            _roleAccessFactory = roleAccessFactory;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       AccessRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                return Task.CompletedTask;
            }

            Claim claim = context.User.Claims.First(c => c.Type == ClaimTypes.Role);
            if (_roleAccessFactory.GetRoleAccess(claim.Value).Accesses.Contains(requirement.RequiredAccess))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail(new(this, $"Role doesn't have access rights for {claim.Value}"));
            }

            return Task.CompletedTask;
        }
    }
}