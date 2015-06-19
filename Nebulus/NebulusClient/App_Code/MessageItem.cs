using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nebulus.Models
{
    public class MessageItem
    {
        public string MessageItemId { get; set; }
        public string MessageTitle { get; set; }
        public string MessageBody { get; set; }
        public MessageType MessageType { get; set; }
        public MessageLocation MessageLocation { get; set; }
        public MessagePriorityType MessagePriority { get; set; }
        public DateTimeOffset ScheduleStart { get; set; }
        public ScheduleIntervalType ScheduleInterval { get; set; }
        public double duration { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public string TargetGroup { get; set; }
        public string MessageHeight { get; set; }
        public string MessageWidth { get; set; }
        public string MessageTop { get; set; }
        public string MessageLeft { get; set; }
    }
}
