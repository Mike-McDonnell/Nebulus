namespace Nebulus.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MessageModel : DbContext
    {
        public DbSet<MessageItem> MessageItems { get; set; }

        public MessageModel()
            : base("name=MessageModel")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
     
        }
    }
}
