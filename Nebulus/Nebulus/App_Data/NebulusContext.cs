namespace Nebulus
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Nebulus.Models;

    public partial class NebulusContext : DbContext
    {
        public DbSet<MessageItem> MessageItems { get; set; }

        public DbSet<SecurityRoleEntity> SecurityRoles { get; set; }

        public DbSet<PrintServiceSettingsModel> PrintServiceConfiguration { get; set; }

        public NebulusContext()
            : base("name=NebulusContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            System.Data.Entity.Database.SetInitializer(new NebulusContextInitializer());
        }
        public static NebulusContext Create()
        {
            return new NebulusContext();
        }
    }

    public partial class NebulusContextInitializer : CreateDatabaseIfNotExists<NebulusContext>
    {
        protected override void Seed(NebulusContext context)
        {
            base.Seed(context);

            var initialRoleAdmin = new SecurityRoleEntity();
            initialRoleAdmin.Access = 0; initialRoleAdmin.Name = "User";

            var initialRoleMessage = new SecurityRoleEntity();
            initialRoleMessage.Access = 1; initialRoleMessage.Name = "User";

            context.SecurityRoles.Add(initialRoleAdmin);
            context.SecurityRoles.Add(initialRoleMessage);
            context.SaveChanges();
        }
    }
}
