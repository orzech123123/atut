using System;
using System.Linq;
using Atut.Identity;
using Microsoft.AspNetCore.Http;

namespace Atut.Services
{
    public class RoleService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAdmin => Convert.ToBoolean(_httpContextAccessor.HttpContext.User.Claims.Single(c => c.Type == UserClaimTypes.IsAdmin).Value);
    }
}
