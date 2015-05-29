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
        }
    }
}
