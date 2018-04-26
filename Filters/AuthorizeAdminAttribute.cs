using Atut.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Atut.Filters
{
    public class AuthorizeAdminAttribute : TypeFilterAttribute
    {
        public AuthorizeAdminAttribute() : base(typeof(AuthorizeAdminFilter))
        {
        }
        
        private class AuthorizeAdminFilter : IAuthorizationFilter
        {
            private readonly RoleService _roleService;

            public AuthorizeAdminFilter(RoleService roleService)
            {
                _roleService = roleService;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (!_roleService.IsAdmin)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}