using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Linq;



namespace Nebulus.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual string RolesString { get {
            var AppUserDb = new ApplicationDbContext();
            return this.Roles != null ? string.Join(",", this.Roles.Select(role => AppUserDb.Roles.Find(role.RoleId).Name)) : string.Empty;
        
        } }

        public virtual System.Collections.Generic.IEnumerable<IdentityRole> RolesNames
        {
            get
            {
                var AppUserDb = new ApplicationDbContext();
                return this.Roles != null ? this.Roles.Select(role => AppUserDb.Roles.Find(role.RoleId)) : null;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("NebulusContext", throwIfV1Schema: false)
        {
        }

        public DbSet<SecurityRoleEntity> SecurityRoles { get; set; }

        static ApplicationDbContext()
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext(); 
        }
    }

    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext> //CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            Security.Roles.Initilize(db);
            
        }
    }
}