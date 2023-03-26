using Microsoft.AspNetCore.Authorization;

namespace AspNetcoreIdentity.Extensions
{
    public class PermissaoNecesaria : IAuthorizationRequirement
    {
        public string Permissao { get; }
        public PermissaoNecesaria(string permissao)
        {
            Permissao = permissao;
        }

    }

    public class PermissaoNecessariaHandler : AuthorizationHandler<PermissaoNecesaria>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissaoNecesaria requisito)
        {
            if(context.User.HasClaim(c => c.Type == "Permissao" && c.Value.Contains(requisito.Permissao)))
            {
                context.Succeed(requisito);
            }
            return Task.CompletedTask;
        }
    }
}
