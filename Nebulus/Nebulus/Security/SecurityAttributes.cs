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

            System.Security.Claims.ClaimsPrincipal user = (System.Security.Claims.ClaimsPrincipal)httpContext.User;
            
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }

            if(user.IsInAnyRole("Admin"))
            {
                return true;
            }

            return false;

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

            System.Security.Claims.ClaimsPrincipal user = (System.Security.Claims.ClaimsPrincipal)httpContext.User;

            
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }

            if (user.IsInRole("BroadCastMessage"))
            {
                return true;
            }

            return false;

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
        public static bool IsInAnyRole(this System.Security.Principal.IPrincipal user, string Role)
        {
            try
            {
                if (user.IsInRole(Role))
                {
                    return true;
                }

                if (Nebulus.AppConfiguration.Settings.SecurityRoles == null)
                {
                    var AppUserDb = new ApplicationDbContext();
                    Nebulus.AppConfiguration.Settings.SecurityRoles = AppUserDb.SecurityRoles.ToList();
                }
                foreach (var role in Nebulus.AppConfiguration.Settings.SecurityRoles.Where(role => role.IdentityRole == Role))
                {
                    if (user.IsInRole(role.Name))
                    {
                        return true;
                    }
                }


            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error Checking Users Role: ", ex);
            }

            return false;
        }
    }
}