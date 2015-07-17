namespace NebulusMessageBroker
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MessageItem
    {
        public string MessageItemId { get; set; }

        [Required]
        public string MessageTitle { get; set; }

        [Required]
        [StringLength(2304)]
        public string MessageBody { get; set; }

        public int MessageType { get; set; }

        public int MessageLocation { get; set; }

        public int MessagePriority { get; set; }

        public DateTimeOffset ScheduleStart { get; set; }

        public int ScheduleInterval { get; set; }

        public double duration { get; set; }

        public DateTimeOffset Expiration { get; set; }

        public string TargetGroup { get; set; }

        public string MessageHeight { get; set; }

        public string MessageWidth { get; set; }

        public string MessageTop { get; set; }

        public string MessageLeft { get; set; }
    }
}
