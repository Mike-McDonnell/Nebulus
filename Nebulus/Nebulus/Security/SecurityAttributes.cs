using Nebulus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nebulus.Security
{
    public class ADAdminAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            System.Security.Principal.IPrincipal user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }

            if (!user.IsInAnyRole(Nebulus.AppConfiguration.Settings.SecurityRoles, 0))
            {
                return false;
            }

            return true;

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

        }
    }

    public class BroadCastAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            System.Security.Principal.IPrincipal user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }

            if (!user.IsInAnyRole(Nebulus.AppConfiguration.Settings.SecurityRoles, 1))
            {
                return false;
            }

            return true;

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

        }
    }
}

namespace Nebulus
{
    public static class UserRoleHelper
    {
        public static bool IsInAnyRole(this System.Security.Principal.IPrincipal user, IEnumerable<SecurityRoleEntity> roles, int Mode)
        {
            
            var userRoles = System.Web.Security.Roles.GetRolesForUser(user.Identity.Name);

            foreach (var role in userRoles)
            {
                return roles.Any(sRole => sRole.Name.Contains(role) || sRole.Access == Mode);
            }

            return false;
        }
    }
}