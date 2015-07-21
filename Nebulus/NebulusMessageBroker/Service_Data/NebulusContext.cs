namespace NebulusMessageBroker
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class NebulusContext : DbContext
    {
        public NebulusContext()
            : base("name=NebulusContext")
        {
        }

        public NebulusContext(string ConnectionString)
            : base(ConnectionString)
        {
        }

        public virtual DbSet<MessageItem> MessageItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
