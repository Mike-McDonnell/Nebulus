using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NebulusClient
{
    public class ClientConfig
    {
        public string ServiceBUSConenctionString { get; set; }

        public string ServiceBUSQueueName { get; set; }

        public bool GroupTAGsEnabled { get; set; }

        public bool UserTAGsEnabled { get; set; }

        public bool ComputerTAGsEnabled { get; set; }

        public bool SubNetTAGsEnabled { get; set; }

    }
}
