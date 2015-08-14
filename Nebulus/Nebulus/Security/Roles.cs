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
        internal static void Initilize(ApplicationDbContext db)
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

          
            const string AdminName = "admin@admin.com";
            const string AdminPassword = "ABcd1234%^&*()";

            const string BroadCastMessageName = "user@user.com";
            const string BroadCastMessagePassword = "ABcd1234%^&*()";

            var user = userManager.FindByName(AdminName);

            if (user == null)
            {
                user = new ApplicationUser { UserName = AdminName, Email = AdminName };
                var result = userManager.Create(user, AdminPassword);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(roleAdmin))
            {
                var result = userManager.AddToRole(user.Id, roleAdmin);
            }

            user = userManager.FindByName(BroadCastMessageName);

            if (user == null)
            {
                user = new ApplicationUser { UserName = BroadCastMessageName, Email = BroadCastMessageName };
                var result = userManager.Create(user, BroadCastMessagePassword);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user BroadCastMessage to Role BroadCastMessage if not already added
            rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(roleBroadcast))
            {
                var result = userManager.AddToRole(user.Id, roleBroadcast);
            }

            SecurityRoleEntity sRole = new SecurityRoleEntity();
            sRole.Name = "BUILTIN\\Users";
            sRole.IdentityRole = roleAdmin;

            db.SecurityRoles.Add(sRole);
            
        }

        internal static void UpdateSaveRoles()
        {
            var roleManager = HttpContext.Current
              .GetOwinContext()
              .Get<ApplicationRoleManager>();

        }
    }
}