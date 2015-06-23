using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NebulusClient
{
    public class MessageBopUp
    {
        public System.Windows.Window PopUpWindows { get; set; }

        public Nebulus.Models.MessageItem MessageItem { get; set; }

        public DateTime StartedTime {get; set;}
    }
}
