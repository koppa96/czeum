using Czeum.Domain.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Extensions;

namespace Czeum.Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpContext httpContext;
        
        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
        
        public string GetCurrentUserName()
        {
            return httpContext.User.Identity.Name ?? throw new InvalidOperationException("Could not identify current user.");
        }

        public Guid GetCurrentUserId()
        {
            return Guid.Parse(httpContext.User.GetSubjectId());
        }
    }
}