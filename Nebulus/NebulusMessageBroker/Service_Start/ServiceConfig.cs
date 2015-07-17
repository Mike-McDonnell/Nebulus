using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NebulusMessageBroker.Service_Start
{
    public class ServiceConfig
    {
        public string ServiceBUSConenctionString { get; set; }
        public string ServiceBUSQueueName { get; set; }
        public string DatabaseConnectionString { get; set; }
    }
}
