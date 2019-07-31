using System;
using System.Linq;
using Atut.Models;
using Microsoft.AspNetCore.Http;

namespace Atut.Services
{
    public class Authorizer
    {
        private readonly RoleService _roleService;
        private readonly DatabaseContext _databaseContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Authorizer(
            RoleService roleService, 
            DatabaseContext databaseContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _roleService = roleService;
            _databaseContext = databaseContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public void RequireVehicleAutorization(int vehicleId, string companyId)
        {
            if (string.IsNullOrWhiteSpace(companyId) || _roleService.IsAdmin)
            {
                return;
            }

            var vehicle = _databaseContext.Vehicles.SingleOrDefault(v => v.Id == vehicleId);

            if (vehicle != null && vehicle.UserId != companyId)
            {
                throw new UnauthorizedAccessException();
            }
        }

        public void RequireJourneyAutorization(int journeyId, string companyId)
        {
            if (string.IsNullOrWhiteSpace(companyId) || _roleService.IsAdmin)
            {
                return;
            }

            var journey = _databaseContext.Journeys.SingleOrDefault(v => v.Id == journeyId);

            if (journey != null && journey.UserId != companyId)
            {
                throw new UnauthorizedAccessException();
            }
        }

        public void RequireVatNumberAutorization(string companyId)
        {
            if (string.IsNullOrWhiteSpace(companyId) || _roleService.IsAdmin)
            {
                return;
            }

            var loggedUser = _databaseContext.Users.Single(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);

            if (loggedUser.Id != companyId)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
