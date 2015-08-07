using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Nebulus.Models;

namespace Nebulus.Security
{
    public class Roles
    {
        internal static void Initilize()
        {
            var roleManager = HttpContext.Current
               .GetOwinContext()
               .Get<ApplicationRoleManager>();

            const string roleAdmin = "Admin";
            const string roleBroadcast = "BroadCastMessage";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleAdmin);
            if (role == null)
            {
                role = new IdentityRole(roleAdmin);
                var roleresult = roleManager.Create(role);
            }

            //Create Role BroadCast if it does not exist
            role = roleManager.FindByName(roleBroadcast);
            if (role == null)
            {
                role = new IdentityRole(roleBroadcast);
                var roleresult = roleManager.Create(role);
            }

            var userManager = HttpContext
              .Current.GetOwinContext()
              .GetUserManager<ApplicationUserManager>();

          
            const string name = "admin@admin.com";
            const string password = "ABcd1234%^&*()";

            var user = userManager.FindByName(name);

            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(roleAdmin))
            {
                var result = userManager.AddToRole(user.Id, roleAdmin);
            }
        }

        internal static void UpdateSaveRoles()
        {
            var roleManager = HttpContext.Current
              .GetOwinContext()
              .Get<ApplicationRoleManager>();

        }
    }
}