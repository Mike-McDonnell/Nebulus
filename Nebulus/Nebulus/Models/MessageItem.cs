using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Nebulus.Models
{
    public class MessageItem
    {
        public string MessageItemId { get; set; }
        [Required(ErrorMessage = "A message title is required")]
        public string MessageTitle { get; set; }
        [Required(ErrorMessage = "A message is required")]
        [StringLength(2304, ErrorMessage="Message body must be under 2048 characters")]
        [AllowHtml]
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
