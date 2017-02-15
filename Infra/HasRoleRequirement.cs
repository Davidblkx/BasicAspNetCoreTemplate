using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace OwnAspNetCore.Infra
{
    public class HasRoleRequirement : AuthorizationHandler<HasRoleRequirement>, IAuthorizationRequirement
    {
        private string _role;

        public HasRoleRequirement(string roleToEnforce)
        {
            _role = roleToEnforce ?? UserRoles.Basic;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            HasRoleRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
                return Task.CompletedTask;

            foreach (var claim in context.User.Claims.Where(c => c.Type == ClaimTypes.Role))
                if (claim.Value == _role)
                    context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}