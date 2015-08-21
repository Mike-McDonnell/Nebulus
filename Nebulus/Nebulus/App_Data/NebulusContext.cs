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

    public partial class NebulusContextInitializer : CreateDatabaseIfNotExists<NebulusContext> //DropCreateDatabaseIfModelChanges<NebulusContext>
    {
        protected override void Seed(NebulusContext context)
        {
            base.Seed(context);
        }
    }
}
