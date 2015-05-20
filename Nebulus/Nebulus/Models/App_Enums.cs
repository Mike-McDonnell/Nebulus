using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nebulus.Models
{


    public enum MessageType
    {
        Marquee, Popup
    }

    public enum MessageLocation
    {
        Top, Bottom, Right, Left, FullScreen
    }

    public enum MessagePriorityType
    {
        Info, Warning, Severe
    }
}